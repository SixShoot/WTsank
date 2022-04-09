using Eruru.Json;
using System;
using System.Collections.Generic;

namespace WorldOfTanks {

	class CombatRecord {

		public CombatRecordPlayer Player { get; set; }
		public int Page { get; set; }
		public int IndexInPage { get; set; }
		public string Name { get; set; }
		public string ArenaID { get; set; }
		public CombatResult Result { get; set; }
		public string Mode { get; set; }
		public DateTime DateTime { get; set; }
		public float Duration { get; set; }
		public float Combat { get; set; }
		public float Damage { get; set; }
		public float Assist { get; set; }
		public int ShootCount { get; set; }
		public int HitCount { get; set; }
		public int PenetrationCount { get; set; }
		public float ArmorResistence { get; set; }
		public float SurvivalTime { get; set; }
		public float XP { get; set; }
		public string TankName { get; set; }
		public JsonArray WinTeamPlayers { get; set; }
		public JsonArray TeamAPlayers { get; set; }
		public JsonArray TeamBPlayers { get; set; }
		public JsonArray PlayerTeamPlayers { get; set; }
		public object Tag { get; set; }

	}

	class CombatRecordPlayer {

		public string Name { get; set; }
		public string ID { get; set; }
		public int Combat { get; set; }
		public DateTime UpdateTime { get; set; }
		public float WinRate { get; set; }
		public float HitRate { get; set; }
		public float AverageCombatLevel { get; set; }
		public float AverageDamage { get; set; }

	}

}