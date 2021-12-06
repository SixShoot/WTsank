using System;

namespace WorldOfTanks {

	class CombatRecord {

		public string Name { get; set; }
		public string ArenaID { get; set; }
		public CombatResult Result { get; set; }
		public string Mode { get; set; }
		public DateTime DateTime { get; set; }
		public float Duration { get; set; }
		public float Combat { get; set; }
		public float Damage { get; set; }
		public float Assist { get; set; }
		public float SurvivalTime { get; set; }
		public float XP { get; set; }
		public string TankName { get; set; }

	}

}