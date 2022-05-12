namespace WorldOfTanks {

	class ClanMember {

		public BoxPersonalCombatRecord BoxPersonalCombatRecord;
		public string Name;
		public string Position;
		public int PositionRank;
		public int ClanID;
		public int AttendanceDays;
		public string AttendanceDaysText;
		public int AttendanceCount;
		public string AttendanceCountText;

		public ClanMember (string name, int clanID) {
			Name = name;
			ClanID = clanID;
		}

	}

}