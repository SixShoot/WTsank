using Eruru.Http;
using Eruru.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace WorldOfTanks {

	class WarGamingNetService {

		public static readonly WarGamingNetService Instance = new WarGamingNetService ();

		readonly Http Http = new Http () {
			OnRequest = httpWebRequest => {
				httpWebRequest.Proxy = null;
				return true;
			}
		};

		public int QueryClanID (string name) {
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

		public List<ClanMember> GetClanMembers (int id) {
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

	}

}