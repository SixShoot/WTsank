using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Eruru.Http;
using Eruru.Json;

namespace WorldOfTanks {

	class WarGamingNetService {

		public static readonly WarGamingNetService Instance = new WarGamingNetService ();

		readonly Http Http = new Http () {
			OnRequest = httpWebRequest => {
				httpWebRequest.Proxy = null;
				return true;
			}
		};
		readonly object RequestLock = new object ();
		readonly Stopwatch Stopwatch = new Stopwatch ();

		long RequestTime = 0;

		Dictionary<string, Tank> Tanks = null;

		public WarGamingNetService () {
			Stopwatch.Start ();
		}

		public int GetClanIdByName (string name) {
			BeginHttpRequest ();
			string response = Http.Request (new HttpRequestInformation () {
				Type = HttpRequestType.Get,
				Url = "https://wgn.wggames.cn/clans/wot/search/api/autocomplete/",
				QueryStringParameters = {
					{ "search", HttpAPI.UrlEncode (name) },
					{ "type", "clans" }
				},
				OnResponseError = (httpWebResponse, webException) => {
					if (httpWebResponse.StatusCode == HttpStatusCode.Conflict) {
						return true;
					}
					throw webException;
				}
			});
			JsonObject jsonObject = JsonObject.Parse (response);
			if (jsonObject.ContainsKey ("error")) {
				throw new Exception (jsonObject["error"]);
			}
			JsonArray resultsJsonArray = jsonObject["search_autocomplete_result"];
			if (resultsJsonArray.Count == 0) {
				throw new Exception ("没有找到与名称或标签相匹配的军团");
			}
			int id = resultsJsonArray[0]["id"];
			return id;
		}

		public List<ClanMember> GetPlayersByClanId (int id) {
			BeginHttpRequest ();
			string response = Http.Request (new HttpRequestInformation () {
				Type = HttpRequestType.Get,
				Url = $"https://wgn.wggames.cn/clans/wot/{id}/api/players/",
				OnRequest = httpWebRequest => {
					httpWebRequest.Headers.Set ("x-requested-with", "XMLHttpRequest");
					return true;
				}
			});
			JsonObject jsonObject = JsonObject.Parse (response);
			JsonArray membersJsonArray = jsonObject["items"];
			List<ClanMember> clanMembers = new List<ClanMember> ();
			foreach (JsonValue memberJsonArray in membersJsonArray) {
				JsonObject role = memberJsonArray["role"];
				clanMembers.Add (new ClanMember (memberJsonArray["name"], id) {
					Position = role["localized_name"],
					PositionRank = role["rank"]
				});
			}
			return clanMembers;
		}

		public string GetClanNameByPlayerName (string name) {
			BeginHttpRequest ();
			string response = Http.Request (new HttpRequestInformation () {
				Type = HttpRequestType.Get,
				Url = "https://wgn.wggames.cn/clans/wot/search/api/autocomplete/",
				QueryStringParameters = {
					{ "search", HttpAPI.UrlEncode (name) },
					{ "type", "accounts" }
				}
			});
			JsonObject jsonObject = JsonObject.Parse (response);
			if (jsonObject.ContainsKey ("error")) {
				throw new Exception (jsonObject["error"]);
			}
			JsonArray resultsJsonArray = jsonObject["search_autocomplete_result"];
			if (resultsJsonArray.Count == 0) {
				throw new Exception ("没有找到使用该名字的玩家");
			}
			return resultsJsonArray[0]["clan"]["tag"];
		}

		public Dictionary<string, Tank> GetTanks () {
			BeginHttpRequest ();
			string json = Http.Request (new HttpRequestInformation () {
				Url = "https://wotgame.cn/wotpbe/tankopedia/api/vehicles/by_filters/",
				Type = HttpRequestType.Get,
				QueryStringParameters = {
					{ "filter[nation]", null },
					{ "filter[type]", null },
					{ "filter[role]", null },
					{ "filter[tier]", "1,2,3,4,5,6,7,8,9,10" },
					{ "filter[language]", "zh-cn" },
					{ "filter[premium]", "0,1" }
				}
			});
			JsonObject jsonObject = JsonObject.Parse (json)["data"];
			JsonArray parametersJsonArray = jsonObject["parameters"];
			JsonArray tanksJsonArray = jsonObject["data"];
			int nameIndex = parametersJsonArray.FindIndex (item => item == "name");
			int tierIndex = parametersJsonArray.FindIndex (item => item == "tier");
			int typeIndex = parametersJsonArray.FindIndex (item => item == "type");
			Dictionary<string, Tank> tanks = new Dictionary<string, Tank> ();
			void AddTankWithName (string name, Tank newTank) {
				try {
					tanks.Add (name, newTank);
				} catch (ArgumentException) {
					Console.WriteLine ($"坦克名：{name} 重复");
				}
			}
			void AddTank (Tank newTank) {
				AddTankWithName (newTank.Name, newTank);
			}
			for (int i = 0; i < tanksJsonArray.Count; i++) {
				JsonArray tankJsonArray = tanksJsonArray[i];
				string name = Regex.Unescape (tankJsonArray[nameIndex]);
				AddTankWithName (name, new Tank () {
					Name = name,
					Tier = int.Parse (tankJsonArray[tierIndex]),
					Type = Api.ParseTankType (tankJsonArray[typeIndex])
				});
			}
			Tank tank = new Tank () {
				Name = "AMX M449L",
				Tier = 8,
				Type = TankType.HeavyTank
			};
			AddTank (tank);
			tank = new Tank () {
				Name = "VK2801 105L/28",
				Tier = 6,
				Type = TankType.LightTank
			};
			AddTank (tank);
			tank = new Tank () {
				Name = "A46",
				Tier = 6,
				Type = TankType.LightTank
			};
			AddTank (tank);
			tank = new Tank () {
				Name = "T3战斗车",
				Tier = 3,
				Type = TankType.MediumTank
			};
			AddTank (tank);
			tank = new Tank () {
				Name = "T26E5 P",
				Tier = 8,
				Type = TankType.HeavyTank
			};
			AddTank (tank);
			tank = new Tank () {
				Name = "T71 CMCD",
				Tier = 7,
				Type = TankType.LightTank
			};
			AddTank (tank);
			tank = new Tank () {
				Name = "\"虎王\"(H)",
				Tier = 7,
				Type = TankType.HeavyTank
			};
			AddTank (tank);
			tank = new Tank () {
				Name = "T-34 1941型",
				Tier = 4,
				Type = TankType.MediumTank
			};
			AddTank (tank);
			tank = new Tank () {
				Name = "酋长 MK.VI",
				Tier = 10,
				Type = TankType.HeavyTank
			};
			AddTank (tank);
			return tanks;
		}

		public Tank GetTankByName (string name, bool allowNotFound = true) {
			if (Tanks == null) {
				Tanks = GetTanks ();
			}
			if (!Tanks.TryGetValue (name, out Tank tank)) {
				string message = $"未找到名为：{name} 的坦克";
				Console.WriteLine (message);
				if (!allowNotFound) {
					throw new Exception (message);
				}
			}
			return tank;
		}

		void BeginHttpRequest () {
			if (ConfigService.Instance.NetworkRequestInterval <= 0) {
				return;
			}
			lock (RequestLock) {
				int elapsed = (int)(Stopwatch.ElapsedMilliseconds - RequestTime);
				if (elapsed < ConfigService.Instance.NetworkRequestInterval) {
					Thread.Sleep (ConfigService.Instance.NetworkRequestInterval - elapsed);
				}
				RequestTime = Stopwatch.ElapsedMilliseconds;
			}
		}

	}

}