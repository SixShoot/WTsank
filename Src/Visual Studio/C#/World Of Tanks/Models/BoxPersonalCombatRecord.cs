using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldOfTanks {

	class BoxPersonalCombatRecord {

		public string ID { get; set; }
		public string Name { get; set; }
		public int Combat { get; set; }
		public DateTime UpdateTime { get; set; }
		public float WinRate { get; set; }
		public float HitRate { get; set; }
		public float CombatLevel { get; set; }
		public float Damage { get; set; }
		public string CombatText { get; set; }
		public Exception Exception { get; set; }

	}

}