using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldOfTanks {

	class CombatRecordTeamPlayer {

		public string Name { get; set; }
		public string ClanAbbrev { get; set; }
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

	}

}