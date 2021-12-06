using Eruru.Html;
using Eruru.Http;
using Eruru.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorldOfTanks {

	class OujBoxService {

		readonly Http Http = new Http ();
		readonly Encoding Big5 = Encoding.GetEncoding ("Big5");
		readonly Encoding GBK = Encoding.GetEncoding ("GBK");

		public float GetCombat (string name) {
			string response = Http.Request (new HttpRequestInformation () {
				Url = "https://wotbox.ouj.com/wotbox/index.php?r=default%2Findex&pn=E+ru+ru",
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
			int combat = int.Parse (numHtmlElement.TextContent);
			return combat;
		}

		public List<CombatRecord> GetCombatRecords (string name, int page, out bool hasPreviousPage, out bool hasNextPage) {
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
			foreach (HtmlElement htmlElement in htmlElements) {
				string arenaID = htmlElement.GetAttribute ("arena-id");
				CombatResult combatResult = ParseCombatResult (htmlElement.GetElementByClassName ("state").TextContent);
				string[] datas = htmlElement.GetElementByClassName ("game").TextContent.Split (' ');
				string[] dates = datas[1].Split ('-');
				int month = int.Parse (dates[0]);
				int day = int.Parse (dates[1]);
				string mode = datas[0];
				CombatRecord combatRecord = new CombatRecord () {
					Name = name,
					ArenaID = arenaID,
					Result = combatResult,
					Mode = mode,
					DateTime = new DateTime (DateTime.Now.Year, month, day)
				};
				combatRecords.Add (combatRecord);
			}
			return combatRecords;
		}

		public List<CombatRecord> GetCombatRecords (string name, Func<int, bool> pageFilter, Func<CombatRecord, FilterResult> combatRecordFilter) {
			int page = 1;
			List<CombatRecord> combatRecords = new List<CombatRecord> ();
			while (true) {
				if (!pageFilter?.Invoke (page) ?? false) {
					break;
				}
				List<CombatRecord> pageCombatRecords = GetCombatRecords (name, page, out bool _, out bool hasNextPage);
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

		public List<CombatRecord> GetCombatRecords (string name, int month, int day) {
			return GetCombatRecords (name, null, battleRecord => {
				if (battleRecord.DateTime.Month < month || (battleRecord.DateTime.Month == month && battleRecord.DateTime.Day < day)) {
					return FilterResult.Break;
				}
				if (battleRecord.DateTime.Month > month || (battleRecord.DateTime.Month == month && battleRecord.DateTime.Day > day)) {
					return FilterResult.Continue;
				}
				return FilterResult.Execute;
			});
		}

		public void FillCombatRecord (CombatRecord combatRecord) {
			string response = Http.Request (new HttpRequestInformation () {
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
			JsonObject jsonObject = JsonObject.Parse (response);
			if (jsonObject["code"] != 200) {
				throw new Exception ("获取战斗记录失败");
			}
			JsonObject resultJsonObject = jsonObject["result"];
			JsonObject playerJsonObject = resultJsonObject["team_a"].Array.Find (item => item["name"] == combatRecord.Name);
			if (playerJsonObject == null) {
				playerJsonObject = resultJsonObject["team_a"].Array.Find (item => Big5.GetString (GBK.GetBytes (item["name"])) == combatRecord.Name);
			}
			JsonArray winTeamPlayers = resultJsonObject[resultJsonObject["win_team"]];
			float duration = 0;
			foreach (JsonObject player in winTeamPlayers) {
				float lifeTime = player["vehicle"]["lifeTime"];
				if (duration < lifeTime) {
					duration = lifeTime;
				}
			}
			Console.WriteLine (response);
			JsonObject vehicleJsonObject = playerJsonObject["vehicle"];
			DateTime dateTime = resultJsonObject["end_time"];
			combatRecord.DateTime.AddHours (dateTime.Hour);
			combatRecord.DateTime.AddMinutes (dateTime.Minute);
			combatRecord.Duration = duration;
			combatRecord.Combat = playerJsonObject["combat"];
			combatRecord.Damage = vehicleJsonObject["damageDealt"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedInspire"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedRadio"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedSmoke"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedStun"];
			combatRecord.Assist += vehicleJsonObject["damageAssistedTrack"];
			combatRecord.SurvivalTime = vehicleJsonObject["lifeTime"];
			combatRecord.XP = vehicleJsonObject["xp"];
			combatRecord.TankName = playerJsonObject["tank_title"];
		}

		public CombatRecordSummary Summary (List<CombatRecord> combatRecords, Func<CombatRecord, FilterResult> filter) {
			CombatRecordSummary combatRecordSummary = new CombatRecordSummary ();
			foreach (CombatRecord combatRecord in combatRecords) {
				bool needBreak = false;
				FilterResult filterResult = filter?.Invoke (combatRecord) ?? FilterResult.Execute;
				switch (filterResult) {
					case FilterResult.Execute:
						FillCombatRecord (combatRecord);
						combatRecordSummary.CombatNumber++;
						combatRecordSummary.AddCombatResult (combatRecord.Result);
						combatRecordSummary.TotalDuration += combatRecord.Duration;
						combatRecordSummary.TotalCombat += combatRecord.Combat;
						combatRecordSummary.TotalDamage += combatRecord.Damage;
						combatRecordSummary.TotalAssist += combatRecord.Assist;
						combatRecordSummary.TotalSurvivalTime += combatRecord.SurvivalTime;
						combatRecordSummary.TotalXP += combatRecord.XP;
						if (!combatRecordSummary.Tanks.TryGetValue (combatRecord.TankName, out CombatRecordSummary tank)) {
							tank = new CombatRecordSummary ();
							combatRecordSummary.Tanks[combatRecord.TankName] = tank;
						}
						tank.CombatNumber++;
						tank.AddCombatResult (combatRecord.Result);
						tank.TotalDuration += combatRecord.Duration;
						tank.TotalCombat += combatRecord.Combat;
						tank.TotalDamage += combatRecord.Damage;
						tank.TotalAssist += combatRecord.Assist;
						tank.TotalSurvivalTime += combatRecord.SurvivalTime;
						tank.TotalXP += combatRecord.XP;
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

	}

}