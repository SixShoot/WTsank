using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace WorldOfTanks {

	class ConfigService {

		public static ConfigService Instance = ConfigDao.Instance.Load ();

		public int NetworkRequestInterval { get; set; }
		public int MaxQueryHistoryCount { get; set; } = 10;
		public List<string> BoxCombatQueryHistoryPlayerNames { get; set; } = new List<string> ();
		public List<string> BoxClanQueryHistoryNames { get; set; } = new List<string> ();
		public List<string> BoxCombatQueryTankListColumns { get; set; }

		public static void Load () {
			Instance = ConfigDao.Instance.Load ();
		}

		public static void Save () {
			ConfigDao.Instance.Save (Instance);
		}

		public bool AddBoxCombatQueryPlayerName (string text) {
			return AddName (BoxCombatQueryHistoryPlayerNames, text);
		}

		public bool AddBoxClanQueryName (string text) {
			return AddName (BoxClanQueryHistoryNames, text);
		}

		bool AddName (List<string> names, string text) {
			int index = names.IndexOf (text);
			if (index > -1) {
				names.RemoveAt (index);
			}
			while (names.Count > MaxQueryHistoryCount) {
				names.RemoveAt (names.Count - 1);
			}
			names.Insert (0, text);
			return index == -1;
		}

	}

}