using Eruru.Html;
using Eruru.Http;
using Eruru.Json;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace WorldOfTanks {

	class OujBoxService {

		readonly Http Http = new Http () {
			OnRequest = httpWebRequest => {
				httpWebRequest.Proxy = null;
				return true;
			}
		};
		readonly object RequestLock = new object ();
		readonly int RequestInterval = 50;
		readonly Stopwatch Stopwatch = new Stopwatch ();

		long RequestTime = 0;

		public OujBoxService () {
			Stopwatch.Start ();
		}

		public string GetID (string name, out int combat) {
			Request ();
			string response = Http.Request (new HttpRequestInformation () {
				Url = "https://wotbox.ouj.com/wotbox/index.php",
				QueryStringParameters = {
					{ "r", "default/index" },
					{ "pn", name }
				},
				OnResponseError = (httpWebResponse, webException) => {
					throw webException;
				}
			});
			CheckSearch (response);
			HtmlDocument htmlDocument = HtmlDocument.Parse (response);
			HtmlElement powerHtmlElement = htmlDocument.GetElementByClassName ("power");
			HtmlElement numHtmlElement = powerHtmlElement.GetElementByClassName ("num");
			combat = int.Parse (numHtmlElement.TextContent);
			return htmlDocument.GetElementById ("p_id").GetAttribute ("value");
		}

		public int GetCombat (string name) {
			GetID (name, out int combat);
			return combat;
		}

		public List<CombatRecord> GetCombatRecords (string name, int page, ref DateTime dateTime, ref CombatRecordPlayer player, out bool hasPreviousPage, out bool hasNextPage) {
			Request ();
			if (player == null) {
				player = CreatePlayer (name);
			}
			string response = Http.Request (new HttpRequestInformation () {
				Type = HttpRequestType.Get,
				Url = "http://wotbox.ouj.com/wotbox/index.php",
				QueryStringParameters = {
					{ "r", "default/battleLog" },
					{ "pn", name },
					{ "p", page }
				},
				OnResponseError = (httpWebResponse, webException) => {
					throw webException;
				}
			});
			CheckSearch (response);
			HtmlDocument htmlDocument = HtmlDocument.Parse (response);
			HtmlElement battleLogHtmlElement = htmlDocument.GetElementById ("battle-log");
			HtmlElement jNavHtmlElement = battleLogHtmlElement?.GetElementByClassName ("J_nav");
			List<HtmlElement> htmlElements = jNavHtmlElement?.GetElementsByTagName ("li");
			if ((htmlElements?.Count ?? 0) == 0) {
				throw new Exception ("没有战斗日志");
			}
			HtmlElement modPageHtmlElement = battleLogHtmlElement?.GetElementByClassName ("mod-page");
			HtmlElement previousPageHtmlElement = modPageHtmlElement?.GetElementByClassName ("prev");
			HtmlElement nextPageHtmlElement = modPageHtmlElement?.GetElementByClassName ("next");
			hasPreviousPage = previousPageHtmlElement != null;
			hasNextPage = page < 215 && nextPageHtmlElement != null;
			List<CombatRecord> combatRecords = new List<CombatRecord> ();
			int i = -1;
			foreach (HtmlElement htmlElement in htmlElements) {
				i++;
				string arenaID = htmlElement.GetAttribute ("arena-id");
				CombatResult combatResult = ParseCombatResult (htmlElement.GetElementByClassName ("state").TextContent);
				string[] datas = htmlElement.GetElementByClassName ("game").TextContent.Split (' ');
				if (datas.Length < 2) {
					continue;
				}
				string[] dates = datas[1].Split ('-');
				int month = int.Parse (dates[0]);
				int day = int.Parse (dates[1]);
				string mode = datas[0];
				CombatRecord combatRecord = new CombatRecord () {
					Player = player,
					Name = name,
					Page = page,
					IndexInPage = i,
					ArenaID = arenaID,
					Result = combatResult,
					Mode = mode,
					DateTime = new DateTime (dateTime.Year, month, day)
				};
				if (combatRecord.DateTime.Month > dateTime.Month || (combatRecord.DateTime.Month == dateTime.Month && combatRecord.DateTime.Day > dateTime.Day)) {
					combatRecord.DateTime = combatRecord.DateTime.AddYears (-1);
				}
				dateTime = combatRecord.DateTime;
				combatRecords.Add (combatRecord);
			}
			return combatRecords;
		}

		public List<CombatRecord> GetCombatRecords (string name, Func<int, bool> pageFilter, Func<CombatRecord, FilterResult> combatRecordFilter) {
			int page = 1;
			DateTime dateTime = DateTime.Now.Date;
			List<CombatRecord> combatRecords = new List<CombatRecord> ();
			CombatRecordPlayer player = CreatePlayer (name);
			while (true) {
				if (!pageFilter?.Invoke (page) ?? false) {
					break;
				}
				List<CombatRecord> pageCombatRecords = GetCombatRecords (name, page, ref dateTime, ref player, out bool _, out bool hasNextPage);
				bool needBreak = false;
				foreach (CombatRecord combatRecord in pageCombatRecords) {
					FilterResult filterResult = combatRecordFilter?.Invoke (combatRecord) ?? FilterResult.Execute;
					switch (filterResult) {
						case FilterResult.Execute:
							combatRecords.Add (combatRecord);
							break;
						case FilterResult.Continue:
							break;
						case FilterResult.Break:
							needBreak = true;
							break;
						default:
							throw new NotImplementedException (filterResult.ToString ());
					}
					if (needBreak) {
						break;
					}
				}
				if (needBreak) {
					break;
				}
				if (!hasNextPage) {
					break;
				}
				page++;
			}
			return combatRecords;
		}

		public List<CombatRecord> GetCombatRecords (string name, DateTime startDateTime, DateTime endDateTime, bool isSameDay = true) {
			startDateTime = startDateTime.Date;
			endDateTime = endDateTime.Date;
			return GetCombatRecords (name, null, battleRecord => {
				if (battleRecord.DateTime > endDateTime || (isSameDay && battleRecord.DateTime > startDateTime)) {
					return FilterResult.Continue;
				}
				if (battleRecord.DateTime < startDateTime) {
					return FilterResult.Break;
				}
				return FilterResult.Execute;
			});
		}
		public List<CombatRecord> GetCombatRecords (string name, DateTime dateTime, bool isSameDay = true) {
			return GetCombatRecords (name, dateTime, DateTime.Now, isSameDay);
		}

		public void FillCombatRecord (CombatRecord combatRecord) {
			Console.WriteLine ($"请求 {combatRecord.ArenaID}");
			if (BoxDao.Instance.TryGetArenaJson (combatRecord.ArenaID, out string response)) {
				Console.WriteLine ($"已缓存 {combatRecord.ArenaID}");
			} else {
				Console.WriteLine ($"拉取 {combatRecord.ArenaID}");
				Request ();
				response = Http.Request (new HttpRequestInformation () {
					Type = HttpRequestType.Get,
					Url = "http://wotapp.ouj.com/",
					QueryStringParameters = {
						{ "r", "wotboxapi/battledetail" },
						{ "pn", combatRecord.Name },
						{ "arena_id", combatRecord.ArenaID }
					},
					OnResponseError = (httpWebResponse, webException) => {
						throw webException;
					},
				}).Trim ('(', ')');
				BoxDao.Instance.SaveArenaJson (combatRecord.ArenaID, response);
			}
			JsonObject jsonObject = JsonObject.Parse (response);
			if (jsonObject["code"] != 200) {
				throw new Exception ("获取战斗记录失败");
			}
			JsonObject resultJsonObject = jsonObject["result"];
			JsonArray winTeamPlayers = resultJsonObject[resultJsonObject["win_team"]];
			float duration = 0;
			foreach (JsonObject player in winTeamPlayers) {
				float lifeTime = player["vehicle"]["lifeTime"];
				if (duration < lifeTime) {
					duration = lifeTime;
				}
			}
			DateTime dateTime = resultJsonObject["end_time"];
			combatRecord.DateTime.AddHours (dateTime.Hour);
			combatRecord.DateTime.AddMinutes (dateTime.Minute);
			combatRecord.Duration = duration;
			combatRecord.TeamA = resultJsonObject["team_a"];
			combatRecord.TeamA.AddRange (resultJsonObject["team_b"].Array);
			JsonObject playerJsonObject = combatRecord.TeamA.Find (item => combatRecord.Player.ID == item["vehicle"]["accountDBID"]);
			JsonObject vehicleJsonObject = playerJsonObject["vehicle"];
			combatRecord.Combat = playerJsonObject["combat"];
			combatRecord.Damage = vehicleJsonObject["damageDealt"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedInspire"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedRadio"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedSmoke"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedStun"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedTrack"];
			combatRecord.ShootCount = vehicleJsonObject["shots"];
			combatRecord.HitCount = vehicleJsonObject["directEnemyHits"];
			combatRecord.PenetrationCount = vehicleJsonObject["piercingEnemyHits"];
			combatRecord.ArmorResistence = vehicleJsonObject["damageBlockedByArmor"];
			combatRecord.SurvivalTime = vehicleJsonObject["lifeTime"];
			combatRecord.XP = vehicleJsonObject["xp"];
			combatRecord.TankName = playerJsonObject["tank_title"];
		}

		public CombatRecordSummary Summary (List<CombatRecord> combatRecords, Func<CombatRecord, FilterResult> filter) {
			CombatRecordSummary combatRecordSummary = new CombatRecordSummary ();
			int count = 0;
			AutoResetEvent autoResetEvent = new AutoResetEvent (false);
			Exception exception = null;
			object summaryLock = new object ();
			foreach (CombatRecord combatRecord in combatRecords) {
				bool needBreak = false;
				FilterResult filterResult = filter?.Invoke (combatRecord) ?? FilterResult.Execute;
				switch (filterResult) {
					case FilterResult.Execute:
						count++;
						ThreadPool.QueueUserWorkItem (state => {
							try {
								CombatRecord innerCombatRecord = (CombatRecord)state;
								FillCombatRecord (innerCombatRecord);
								lock (summaryLock) {
									combatRecordSummary.Append (innerCombatRecord);
									if (!combatRecordSummary.Tanks.TryGetValue (innerCombatRecord.TankName, out CombatRecordSummary tank)) {
										tank = new CombatRecordSummary ();
										combatRecordSummary.Tanks[innerCombatRecord.TankName] = tank;
									}
									tank.Append (innerCombatRecord);
								}
							} catch (Exception e) {
								exception = e;
							} finally {
								lock (summaryLock) {
									count--;
									if (count <= 0) {
										autoResetEvent.Set ();
									}
								}
							}
						}, combatRecord);
						break;
					case FilterResult.Continue:
						break;
					case FilterResult.Break:
						needBreak = true;
						break;
					default:
						throw new NotImplementedException (filterResult.ToString ());
				}
				if (needBreak) {
					break;
				}
			}
			if (count <= 0) {
				autoResetEvent.Set ();
			}
			autoResetEvent.WaitOne ();
			if (exception != null) {
				throw exception;
			}
			combatRecordSummary.Summary ();
			foreach (CombatRecordSummary tank in combatRecordSummary.Tanks.Values) {
				tank.Summary ();
			}
			return combatRecordSummary;
		}

		void CheckSearch (string response) {
			if (response == "您的访问过于频繁，请稍后再试") {
				throw new Exception ("您的访问过于频繁，请稍后再试");
			}
			if (response == "NOT FOUND USER" || response.Contains ("没有找到您搜索的玩家！")) {
				throw new Exception ("没有找到您搜索的玩家！");
			}
		}

		CombatResult ParseCombatResult (string text) {
			switch (text) {
				case "胜利":
					return CombatResult.Victory;
				case "失败":
					return CombatResult.Fail;
				case "平局":
					return CombatResult.Even;
				default:
					throw new NotImplementedException (text);
			}
		}

		CombatRecordPlayer CreatePlayer (string name) {
			return new CombatRecordPlayer () {
				ID = GetID (name, out int combat),
				Combat = combat
			};
		}

		void Request () {
			if (RequestInterval <= 0) {
				return;
			}
			lock (RequestLock) {
				int elapsed = (int)(Stopwatch.ElapsedMilliseconds - RequestTime);
				if (elapsed < RequestInterval) {
					Thread.Sleep (RequestInterval - elapsed);
				}
				RequestTime = Stopwatch.ElapsedMilliseconds;
			}
		}

	}

}