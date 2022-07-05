using Eruru.Html;
using Eruru.Http;
using Eruru.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace WorldOfTanks {

	class BoxService {

		public static readonly BoxService Instance = new BoxService ();

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

		public BoxService () {
			Stopwatch.Start ();
		}

		public BoxPersonalCombatRecord GetPersonalCombatRecord (string name) {
			BeginHttpRequest ();
			string html = Http.Request (new HttpRequestInformation () {
				Url = "https://wotbox.ouj.com/wotbox/index.php",
				QueryStringParameters = {
					{ "r", HttpAPI.UrlEncode ("default/index") },
					{ "pn", HttpAPI.UrlEncode (name) }
				},
				OnResponseError = (httpWebResponse, webException) => {
					throw webException;
				}
			});
			CheckCombatRecordHtml (html);
			HtmlDocument htmlDocument = HtmlDocument.Parse (html);
			string id = htmlDocument.GetElementById ("p_id").GetAttribute ("value");
			int.TryParse (htmlDocument.QuerySelector (".power .num").TextContent, out int combat);
			DateTime.TryParse (htmlDocument.QuerySelector (".userRecord-history__title p").ChildNodes[0].NodeValue.Substring ("更新时间：".Length), out DateTime updateTime);
			float.TryParse (htmlDocument.GetElementByClassName ("win-rate-1k").GetAttribute ("win-rate"), out float winRate);
			float.TryParse (htmlDocument.GetElementByClassName ("hit-rate-1k").GetAttribute ("hit-rate"), out float hitRate);
			float.TryParse (htmlDocument.GetElementByClassName ("avg-lv-1k").NextElementSibling.TextContent.TrimEnd ('级'), out float combatLevel);
			float.TryParse (htmlDocument.QuerySelector (".userRecord-data li .num").TextContent, out float damage);
			return new BoxPersonalCombatRecord () {
				ID = id,
				Name = name,
				Combat = combat,
				UpdateTime = updateTime,
				WinRate = winRate / 100,
				HitRate = hitRate / 100,
				CombatLevel = combatLevel,
				Damage = damage,
				CombatText = combat.ToString ()
			};
		}

		public List<BoxCombatRecord> GetCombatRecords (BoxPersonalCombatRecord boxPersonalCombatRecord, int page, ref DateTime dateTime, out bool hasPreviousPage, out bool hasNextPage) {
			BeginHttpRequest ();
			string html = Http.Request (new HttpRequestInformation () {
				Type = HttpRequestType.Get,
				Url = "http://wotbox.ouj.com/wotbox/index.php",
				QueryStringParameters = {
					{ "r", HttpAPI.UrlEncode ("default/battleLog") },
					{ "pn", HttpAPI.UrlEncode (boxPersonalCombatRecord.Name) },
					{ "p", page }
				},
				OnResponseError = (httpWebResponse, webException) => {
					throw webException;
				}
			});
			CheckCombatRecordHtml (html);
			HtmlDocument htmlDocument = HtmlDocument.Parse (html);
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
			List<BoxCombatRecord> combatRecords = new List<BoxCombatRecord> ();
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
				BoxCombatRecord combatRecord = new BoxCombatRecord () {
					BoxPersonalCombatRecord = boxPersonalCombatRecord,
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
		public List<BoxCombatRecord> GetCombatRecords (
			BoxPersonalCombatRecord player, Func<int, bool> pageFilter, Func<BoxCombatRecord, LoopAction> combatRecordFilter, Action<int, DateTime> onPage = null, Action OnPageOut = null
		) {
			int page = 1;
			DateTime dateTime = DateTime.Now.Date;
			List<BoxCombatRecord> combatRecords = new List<BoxCombatRecord> ();
			while (true) {
				if (page > 215) {
					OnPageOut?.Invoke ();
					break;
				}
				if (!pageFilter?.Invoke (page) ?? false) {
					break;
				}
				List<BoxCombatRecord> pageCombatRecords = GetCombatRecords (player, page, ref dateTime, out bool _, out bool hasNextPage);
				onPage?.Invoke (page, dateTime);
				bool needBreak = false;
				foreach (BoxCombatRecord combatRecord in pageCombatRecords) {
					LoopAction loopAction = combatRecordFilter?.Invoke (combatRecord) ?? LoopAction.Execute;
					switch (loopAction) {
						case LoopAction.Execute:
							combatRecords.Add (combatRecord);
							break;
						case LoopAction.Continue:
							break;
						case LoopAction.Break:
							needBreak = true;
							break;
						default:
							throw new NotImplementedException (loopAction.ToString ());
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
		public List<BoxCombatRecord> GetCombatRecords (
			BoxPersonalCombatRecord player, DateTime startDateTime, DateTime endDateTime, bool isSameDay = true, Action<int, DateTime> onPage = null, Action OnPageOut = null
		) {
			startDateTime = startDateTime.Date;
			endDateTime = endDateTime.Date;
			return GetCombatRecords (player, null, battleRecord => {
				if (battleRecord.DateTime > endDateTime || (isSameDay && battleRecord.DateTime > startDateTime)) {
					return LoopAction.Continue;
				}
				if (battleRecord.DateTime < startDateTime) {
					return LoopAction.Break;
				}
				return LoopAction.Execute;
			}, onPage, OnPageOut);
		}
		public List<BoxCombatRecord> GetCombatRecords (
			BoxPersonalCombatRecord player, DateTime dateTime, bool isSameDay = true, Action<int, DateTime> onPage = null, Action OnPageOut = null
		) {
			return GetCombatRecords (player, dateTime, DateTime.Now, isSameDay, onPage, OnPageOut);
		}

		public void FillCombatRecord (BoxCombatRecord combatRecord) {
			if (BoxDao.Instance.TryGetArenaJson (combatRecord.ArenaID, out string response)) {

			} else {
				BeginHttpRequest ();
				response = Http.Request (new HttpRequestInformation () {
					Type = HttpRequestType.Get,
					Url = "http://wotapp.ouj.com/",
					QueryStringParameters = {
						{ "r", "wotboxapi/battledetail" },
						{ "pn", HttpAPI.UrlEncode (combatRecord.BoxPersonalCombatRecord.Name) },
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
			combatRecord.TeamAPlayers = resultJsonObject["team_a"];
			combatRecord.TeamBPlayers = resultJsonObject["team_b"];
			combatRecord.WinTeamPlayers = resultJsonObject[resultJsonObject["win_team"]];
			combatRecord.FailTeamPlayers = resultJsonObject["win_team"] == "team_a" ? combatRecord.TeamBPlayers : combatRecord.TeamAPlayers;
			float duration = 0;
			foreach (JsonObject player in combatRecord.WinTeamPlayers) {
				float lifeTime = player["vehicle"]["lifeTime"];
				if (duration < lifeTime) {
					duration = lifeTime;
				}
			}
			DateTime dateTime = resultJsonObject["end_time"];
			combatRecord.DateTime = combatRecord.DateTime.AddHours (dateTime.Hour);
			combatRecord.DateTime = combatRecord.DateTime.AddMinutes (dateTime.Minute);
			combatRecord.Duration = duration;
			bool findPlayer (JsonValue item) => combatRecord.BoxPersonalCombatRecord.ID == item["vehicle"]["accountDBID"];
			JsonObject playerJsonObject = combatRecord.TeamAPlayers.Find (findPlayer);
			if (playerJsonObject == null) {
				combatRecord.PlayerTeamPlayers = combatRecord.TeamBPlayers;
				combatRecord.EnemyTeamPlayers = combatRecord.TeamAPlayers;
				playerJsonObject = combatRecord.TeamBPlayers.Find (findPlayer);
			} else {
				combatRecord.PlayerTeamPlayers = combatRecord.TeamAPlayers;
				combatRecord.EnemyTeamPlayers = combatRecord.TeamBPlayers;
			}
			combatRecord.TeamPlayer = new CombatRecordTeamPlayer { };
			FillCombatRecordTeamPlayer (combatRecord.TeamPlayer, playerJsonObject);
		}

		public void FillCombatRecordTeamPlayer (CombatRecordTeamPlayer player, JsonObject jsonObject) {
			JsonObject vehicleJsonObject = jsonObject["vehicle"];
			player.Name = jsonObject["realName"];
			player.ClanAbbrev = jsonObject["clanAbbrev"];
			player.Combat = jsonObject["combat"];
			player.Damage = vehicleJsonObject["damageDealt"];
			player.Assist += vehicleJsonObject["damageAssistedInspire"];
			player.Assist += vehicleJsonObject["damageAssistedRadio"];
			player.Assist += vehicleJsonObject["damageAssistedSmoke"];
			player.Assist += vehicleJsonObject["damageAssistedStun"];
			player.Assist += vehicleJsonObject["damageAssistedTrack"];
			player.ShootCount = vehicleJsonObject["shots"];
			player.HitCount = vehicleJsonObject["directEnemyHits"];
			player.PenetrationCount = vehicleJsonObject["piercingEnemyHits"];
			player.ArmorResistence = vehicleJsonObject["damageBlockedByArmor"];
			player.SurvivalTime = vehicleJsonObject["lifeTime"];
			player.XP = vehicleJsonObject["xp"];
			player.TankName = jsonObject["tank_title"];
		}

		public BoxCombatRecordSummary SummaryCombatRecords (List<BoxCombatRecord> combatRecords, Func<BoxCombatRecord, LoopAction> filter, Action onFill = null) {
			BoxCombatRecordSummary combatRecordSummary = new BoxCombatRecordSummary ();
			int count = 0;
			AutoResetEvent autoResetEvent = new AutoResetEvent (false);
			Exception exception = null;
			object summaryLock = new object ();
			foreach (BoxCombatRecord combatRecord in combatRecords) {
				bool needBreak = false;
				LoopAction loopAction = filter?.Invoke (combatRecord) ?? LoopAction.Execute;
				switch (loopAction) {
					case LoopAction.Execute:
						count++;
						ThreadPool.QueueUserWorkItem (state => {
							try {
								BoxCombatRecord innerCombatRecord = (BoxCombatRecord)state;
								FillCombatRecord (innerCombatRecord);
								lock (summaryLock) {
									onFill?.Invoke ();
									combatRecordSummary.AddCombatRecord (innerCombatRecord);
									if (!combatRecordSummary.Tanks.TryGetValue (innerCombatRecord.TeamPlayer.TankName, out BoxCombatRecordSummary tank)) {
										tank = new BoxCombatRecordSummary ();
										combatRecordSummary.Tanks[innerCombatRecord.TeamPlayer.TankName] = tank;
										tank.TankLevel = WarGamingNetService.Instance.GetTankByName (innerCombatRecord.TeamPlayer.TankName).Tier;
									}
									tank.AddCombatRecord (innerCombatRecord);
									if (tank.TankLevel > 0) {
										combatRecordSummary.TankLevelCount++;
										combatRecordSummary.TotalTankLevel += tank.TankLevel;
									}
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
					case LoopAction.Continue:
						break;
					case LoopAction.Break:
						needBreak = true;
						break;
					default:
						throw new NotImplementedException (loopAction.ToString ());
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
			foreach (var nameTank in combatRecordSummary.Tanks) {
				nameTank.Value.Summary (nameTank.Key);
			}
			return combatRecordSummary;
		}

		void CheckCombatRecordHtml (string html) {
			if (html == "您的访问过于频繁，请稍后再试") {
				throw new Exception ("您的访问过于频繁，请稍后再试");
			}
			if (html == "NOT FOUND USER" || html.Contains ("没有找到您搜索的玩家！")) {
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

		void BeginHttpRequest () {
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