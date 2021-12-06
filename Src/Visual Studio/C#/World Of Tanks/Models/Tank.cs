namespace WorldOfTanks {

	class Tank {

		public string Name { get; set; }
		public float AimTime { get; set; }
		public float Dispersion { get; set; }
		public float MoveDispersionFactor { get; set; }
		public float HullTraverseDispersionFactor { get; set; }
		public float TurretTraverseDispersionFactor { get; set; }
		public float FireDispersionFactor { get; set; }
		public float DamagedDispersionFactor { get; set; }
		public float MoveSpeedByHard { get; set; }
		public float MoveSpeedByMedium { get; set; }
		public float MoveSpeedBySoft { get; set; }
		public float HullTraverseSpeedByHard { get; set; }
		public float HullTraverseSpeedByMedium { get; set; }
		public float HullTraverseSpeedBySoft { get; set; }
		public float TurretTraverseSpeed { get; set; }

	}

}