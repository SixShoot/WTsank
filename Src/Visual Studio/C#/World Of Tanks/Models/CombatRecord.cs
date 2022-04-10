using Eruru.Json;
using System;

namespace WorldOfTanks {

	class CombatRecord {

		public CombatRecordPlayer Player { get; set; }
		public int Page { get; set; }
		public int IndexInPage { get; set; }
		public string ArenaID { get; set; }
		public CombatResult Result { get; set; }
		public string Mode { get; set; }
		public DateTime DateTime { get; set; }
		public float Duration { get; set; }
		public CombatRecordTeamPlayer TeamPlayer { get; set; }
		public JsonArray WinTeamPlayers { get; set; }
		public JsonArray TeamAPlayers { get; set; }
		public JsonArray TeamBPlayers { get; set; }
		public JsonArray PlayerTeamPlayers { get; set; }
		public object Tag { get; set; }

	}

}