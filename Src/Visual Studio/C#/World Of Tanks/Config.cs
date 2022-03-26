using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldOfTanks {

	class Config {

		public static Config Instance = ConfigDao.Instance.Load ();

		public string BoxCombatQueryPlayerName { get; set; }
		public string BoxClanQueryName { get; set; }

	}

}