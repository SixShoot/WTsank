namespace WorldOfTanks {

	class ClanMember {

		public string Name;
		public float Combat;
		public string CombatText;
		public int ClanID;
		public int Attendance;
		public string AttendanceText;

		public ClanMember (string name, int clanID) {
			Name = name;
			ClanID = clanID;
		}

	}

}