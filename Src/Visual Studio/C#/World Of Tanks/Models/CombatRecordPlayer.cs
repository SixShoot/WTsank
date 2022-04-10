using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldOfTanks {

	class CombatRecordPlayer {

		public string Name { get; set; }
		public string ID { get; set; }
		public int Combat { get; set; }
		public DateTime UpdateTime { get; set; }
		public float WinRate { get; set; }
		public float HitRate { get; set; }
		public float AverageCombatLevel { get; set; }
		public float AverageDamage { get; set; }
		public string CombatText { get; set; }
		public Exception Exception { get; set; }

	}

}