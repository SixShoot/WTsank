namespace WorldOfTanks {

	class Config {

		public static Config Instance = ConfigDao.Instance.Load ();

		public string BoxCombatQueryPlayerName { get; set; }
		public string BoxClanQueryName { get; set; }
		public string BoxCombatAnalysisPlayerName { get; set; }

	}

}