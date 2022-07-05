using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WorldOfTanks {

	class AimTimeCalculatorTank : TanksGGTank {

		public TextBox NameTextBox { get; set; }
		public TextBox AimTimeTextBox { get; set; }
		public TextBox DispersionTextBox { get; set; }
		public TextBox MoveDispersionFactorTextBox { get; set; }
		public TextBox HullTraverseDispersionFactorTextBox { get; set; }
		public TextBox TurretTraverseDispersionFactorTextBox { get; set; }
		public TextBox FireDispersionFactorTextBox { get; set; }
		public TextBox DamagedDispersionFactorTextBox { get; set; }
		public TextBox MoveSpeedByHardTextBox { get; set; }
		public TextBox MoveSpeedByMediumTextBox { get; set; }
		public TextBox MoveSpeedBySoftTextBox { get; set; }
		public TextBox HullTraverseSpeedByHardTextBox { get; set; }
		public TextBox HullTraverseSpeedByMediumTextBox { get; set; }
		public TextBox HullTraverseSpeedBySoftTextBox { get; set; }
		public TextBox TurretTraverseSpeedTextBox { get; set; }
		public TextBox ActualAimTimeTextBox { get; set; }
		public TextBox ActualDispersionTextBox { get; set; }
		public TextBox AutoInputTanksGGTextBox { get; set; }
		public TerrainType TerrainType { get; set; }
		public bool IsMoving { get; set; }
		public bool IsTraversingHull { get; set; }
		public bool IsTraversingTurret { get; set; }
		public bool IsFireing { get; set; }
		public bool IsDamaged { get; set; }
		public double ActualAimTime { get; set; }
		public double ActualDispersion { get; set; }
		public double MoveSpeed { get; set; }
		public double HullTraverseSpeed { get; set; }
		public double CurrentAimTime { get; set; }
		public double CurrentDispersion { get; set; }
		public Pen Pen { get; set; }
		public Brush Brush { get; set; }
		public List<ImpactPoint> ImpactPoints { get; set; } = new List<ImpactPoint> ();

	}

	struct ImpactPoint {

		public double X { get; set; }
		public double Y { get; set; }

	}

}