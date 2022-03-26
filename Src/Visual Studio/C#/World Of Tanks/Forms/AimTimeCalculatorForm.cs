using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class AimTimeCalculatorForm : Form {

		const float AimCircleLineWidth = 1;
		const float AimTimeTrackBarScale = 1000;
		const float AimCircleScale = 100;

		readonly AimTimeCalculatorTank TankA = new AimTimeCalculatorTank ();
		readonly AimTimeCalculatorTank TankB = new AimTimeCalculatorTank ();
		readonly Stopwatch Stopwatch = new Stopwatch ();

		double MaxAimTime {

			get {
				return _MaxAimTime;
			}

			set {
				_MaxAimTime = value;
				TimeTrackBar.Maximum = (int)(value * AimTimeTrackBarScale);
				if (CurrentAimTime > MaxAimTime) {
					CurrentAimTime = MaxAimTime;
				}
			}

		}
		double _MaxAimTime;
		double CurrentAimTime {

			get {
				return _CurrentAimTime;
			}

			set {
				if (value < 0) {
					value = 0;
				} else if (value > MaxAimTime) {
					value = MaxAimTime;
				}
				_CurrentAimTime = value;
				TimeTrackBar.Value = (int)(value * AimTimeTrackBarScale);
				if (!BlockCurrentTimeTextBox) {
					CurrentTimeTextBox.Text = value.ToString ("F4");
				}
			}

		}
		double _CurrentAimTime;
		bool BlockCalculate = true;
		bool BlockCurrentTimeTextBox;

		public AimTimeCalculatorForm () {
			InitializeComponent ();
		}

		private void AimTimeCalculatorForm_Load (object sender, EventArgs e) {
			InitializeTerrainComboBox (TankATerrainComboBox);
			InitializeTerrainComboBox (TankBTerrainComboBox);
			TankA.NameTextBox = TankANameTextBox;
			TankA.AimTimeTextBox = TankAAimTimeTextBox;
			TankA.DispersionTextBox = TankADispersionTextBox;
			TankA.MoveDispersionFactorTextBox = TankAMoveDispersionFactorTextBox;
			TankA.HullTraverseDispersionFactorTextBox = TankAHullTraverseDispersionFactorTextBox;
			TankA.TurretTraverseDispersionFactorTextBox = TankATurretTraverseDispersionFactorTextBox;
			TankA.FireDispersionFactorTextBox = TankAFireDispersionFactorTextBox;
			TankA.DamagedDispersionFactorTextBox = TankADamagedDispersionFactorTextBox;
			TankA.MoveSpeedByHardTextBox = TankAMoveSpeedByHardTextBox;
			TankA.MoveSpeedByMediumTextBox = TankAMoveSpeedByMediumTextBox;
			TankA.MoveSpeedBySoftTextBox = TankAMoveSpeedBySoftTextBox;
			TankA.HullTraverseSpeedByHardTextBox = TankAHullTraverseSpeedByHardTextBox;
			TankA.HullTraverseSpeedByMediumTextBox = TankAHullTraverseSpeedByMediumTextBox;
			TankA.HullTraverseSpeedBySoftTextBox = TankAHullTraverseSpeedBySoftTextBox;
			TankA.TurretTraverseSpeedTextBox = TankATurretTraverseSpeedTextBox;
			TankA.ActualAimTimeTextBox = TankAActualAimTimeTextBox;
			TankA.ActualDispersionTextBox = TankAActualDispersionTextBox;
			TankA.AutoInputTanksGGTextBox = TankAAutoInputTanksGGTextBox;
			TankA.Pen = new Pen (Color.Green, AimCircleLineWidth);
			TankANameTextBox.Text = "277工程";
			TankAAimTimeTextBox.Text = "2.5887";
			TankADispersionTextBox.Text = "0.3643";
			TankAMoveDispersionFactorTextBox.Text = "0.16";
			TankAHullTraverseDispersionFactorTextBox.Text = "0.16";
			TankATurretTraverseDispersionFactorTextBox.Text = "0.08";
			TankAFireDispersionFactorTextBox.Text = "4";
			TankADamagedDispersionFactorTextBox.Text = "2";
			TankAMoveSpeedByHardTextBox.Text = "55";
			TankAMoveSpeedByMediumTextBox.Text = "55";
			TankAMoveSpeedBySoftTextBox.Text = "34.5992";
			TankAHullTraverseSpeedByHardTextBox.Text = "33.376";
			TankAHullTraverseSpeedByMediumTextBox.Text = "30.3418";
			TankAHullTraverseSpeedBySoftTextBox.Text = "17.5663";
			TankATurretTraverseSpeedTextBox.Text = "28.161";
			TankATerrainComboBox.SelectedIndex = (int)TerrainType.Medium;
			TankAIsTraversingHullCheckBox.Checked = true;
			TankAIsTraversingTurretCheckBox.Checked = true;
			TankB.NameTextBox = TankBNameTextBox;
			TankB.AimTimeTextBox = TankBAimTimeTextBox;
			TankB.DispersionTextBox = TankBDispersionTextBox;
			TankB.MoveDispersionFactorTextBox = TankBMoveDispersionFactorTextBox;
			TankB.HullTraverseDispersionFactorTextBox = TankBHullTraverseDispersionFactorTextBox;
			TankB.TurretTraverseDispersionFactorTextBox = TankBTurretTraverseDispersionFactorTextBox;
			TankB.FireDispersionFactorTextBox = TankBFireDispersionFactorTextBox;
			TankB.DamagedDispersionFactorTextBox = TankBDamagedDispersionFactorTextBox;
			TankB.MoveSpeedByHardTextBox = TankBMoveSpeedByHardTextBox;
			TankB.MoveSpeedByMediumTextBox = TankBMoveSpeedByMediumTextBox;
			TankB.MoveSpeedBySoftTextBox = TankBMoveSpeedBySoftTextBox;
			TankB.HullTraverseSpeedByHardTextBox = TankBHullTraverseSpeedByHardTextBox;
			TankB.HullTraverseSpeedByMediumTextBox = TankBHullTraverseSpeedByMediumTextBox;
			TankB.HullTraverseSpeedBySoftTextBox = TankBHullTraverseSpeedBySoftTextBox;
			TankB.TurretTraverseSpeedTextBox = TankBTurretTraverseSpeedTextBox;
			TankB.ActualAimTimeTextBox = TankBActualAimTimeTextBox;
			TankB.ActualDispersionTextBox = TankBActualDispersionTextBox;
			TankB.AutoInputTanksGGTextBox = TankBAutoInputTanksGGTextBox;
			TankB.Pen = new Pen (Color.Red, AimCircleLineWidth);
			TankBNameTextBox.Text = "蟋蟀 15";
			TankBAimTimeTextBox.Text = "1.4382";
			TankBDispersionTextBox.Text = "0.2589";
			TankBMoveDispersionFactorTextBox.Text = "0.26";
			TankBHullTraverseDispersionFactorTextBox.Text = "0.3";
			TankBTurretTraverseDispersionFactorTextBox.Text = "0.4";
			TankBFireDispersionFactorTextBox.Text = "4";
			TankBDamagedDispersionFactorTextBox.Text = "2";
			TankBMoveSpeedByHardTextBox.Text = "60";
			TankBMoveSpeedByMediumTextBox.Text = "54.254";
			TankBMoveSpeedBySoftTextBox.Text = "18.6014";
			TankBHullTraverseSpeedByHardTextBox.Text = "23.7804";
			TankBHullTraverseSpeedByMediumTextBox.Text = "19.817";
			TankBHullTraverseSpeedBySoftTextBox.Text = "6.7944";
			TankBTurretTraverseSpeedTextBox.Text = "20.86";
			TankBTerrainComboBox.SelectedIndex = (int)TerrainType.Medium;
			TankBIsTraversingHullCheckBox.Checked = true;
			TankBIsTraversingTurretCheckBox.Checked = true;
			BlockCalculate = false;
			Calculate ();
			CurrentTimeTextBox.Text = "0";
		}

		private void TankANameTextBox_TextChanged (object sender, EventArgs e) {
			try {
				TankA.Name = TankANameTextBox.Text;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAAimTimeTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAAimTimeTextBox.Text)) {
					TankA.AimTime = 0;
				} else {
					TankA.AimTime = double.Parse (TankAAimTimeTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankADispersionTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankADispersionTextBox.Text)) {
					TankA.Dispersion = 0;
				} else {
					TankA.Dispersion = double.Parse (TankADispersionTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAMoveDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAMoveDispersionFactorTextBox.Text)) {
					TankA.MoveDispersionFactor = 0;
				} else {
					TankA.MoveDispersionFactor = double.Parse (TankAMoveDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAHullTraverseDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAHullTraverseDispersionFactorTextBox.Text)) {
					TankA.HullTraverseDispersionFactor = 0;
				} else {
					TankA.HullTraverseDispersionFactor = double.Parse (TankAHullTraverseDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankATurretTraverseDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankATurretTraverseDispersionFactorTextBox.Text)) {
					TankA.TurretTraverseDispersionFactor = 0;
				} else {
					TankA.TurretTraverseDispersionFactor = double.Parse (TankATurretTraverseDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAFireDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAFireDispersionFactorTextBox.Text)) {
					TankA.FireDispersionFactor = 0;
				} else {
					TankA.FireDispersionFactor = double.Parse (TankAFireDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankADamagedDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankADamagedDispersionFactorTextBox.Text)) {
					TankA.DamagedDispersionFactor = 0;
				} else {
					TankA.DamagedDispersionFactor = double.Parse (TankADamagedDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAMoveSpeedByHardTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAMoveSpeedByHardTextBox.Text)) {
					TankA.MoveSpeedByHard = 0;
				} else {
					TankA.MoveSpeedByHard = double.Parse (TankAMoveSpeedByHardTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAMoveSpeedByMediumTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAMoveSpeedByMediumTextBox.Text)) {
					TankA.MoveSpeedByMedium = 0;
				} else {
					TankA.MoveSpeedByMedium = double.Parse (TankAMoveSpeedByMediumTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAMoveSpeedBySoftTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAMoveSpeedBySoftTextBox.Text)) {
					TankA.MoveSpeedBySoft = 0;
				} else {
					TankA.MoveSpeedBySoft = double.Parse (TankAMoveSpeedBySoftTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAHullTraverseSpeedByHardTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAHullTraverseSpeedByHardTextBox.Text)) {
					TankA.HullTraverseSpeedByHard = 0;
				} else {
					TankA.HullTraverseSpeedByHard = double.Parse (TankAHullTraverseSpeedByHardTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAHullTraverseSpeedByMediumTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAHullTraverseSpeedByMediumTextBox.Text)) {
					TankA.HullTraverseSpeedByMedium = 0;
				} else {
					TankA.HullTraverseSpeedByMedium = double.Parse (TankAHullTraverseSpeedByMediumTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAHullTraverseSpeedBySoftTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankAHullTraverseSpeedBySoftTextBox.Text)) {
					TankA.HullTraverseSpeedBySoft = 0;
				} else {
					TankA.HullTraverseSpeedBySoft = double.Parse (TankAHullTraverseSpeedBySoftTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankATurretTraverseSpeedTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankATurretTraverseSpeedTextBox.Text)) {
					TankA.TurretTraverseSpeed = 0;
				} else {
					TankA.TurretTraverseSpeed = double.Parse (TankATurretTraverseSpeedTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankATerrainComboBox_SelectedIndexChanged (object sender, EventArgs e) {
			TankA.TerrainType = (TerrainType)TankATerrainComboBox.SelectedIndex;
			OnSelectedTerrainType (TankA);
			Calculate ();
		}

		private void TankAIsMovingCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankA.IsMoving = TankAIsMovingCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAIsTraversingHullCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankA.IsTraversingHull = TankAIsTraversingHullCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAIsTraversingTurretCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankA.IsTraversingTurret = TankAIsTraversingTurretCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAIsFiringCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankA.IsFireing = TankAIsFiringCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAIsDamagedCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankA.IsDamaged = TankAIsDamagedCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankAAutoInputTanksGGTextBox_TextChanged (object sender, EventArgs e) {
			AutoInputTanksGG (TankA);
		}

		private void TankAAutoInputTanksGGTextBox_Click (object sender, EventArgs e) {
			TankAAutoInputTanksGGTextBox.Text = Clipboard.GetText ();
		}

		private void TankBNameTextBox_TextChanged (object sender, EventArgs e) {
			try {
				TankB.Name = TankBNameTextBox.Text;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBAimTimeTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBAimTimeTextBox.Text)) {
					TankB.AimTime = 0;
				} else {
					TankB.AimTime = double.Parse (TankBAimTimeTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBDispersionTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBDispersionTextBox.Text)) {
					TankB.Dispersion = 0;
				} else {
					TankB.Dispersion = double.Parse (TankBDispersionTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBMoveDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBMoveDispersionFactorTextBox.Text)) {
					TankB.MoveDispersionFactor = 0;
				} else {
					TankB.MoveDispersionFactor = double.Parse (TankBMoveDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBHullTraverseDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBHullTraverseDispersionFactorTextBox.Text)) {
					TankB.HullTraverseDispersionFactor = 0;
				} else {
					TankB.HullTraverseDispersionFactor = double.Parse (TankBHullTraverseDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBTurretTraverseDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBTurretTraverseDispersionFactorTextBox.Text)) {
					TankB.TurretTraverseDispersionFactor = 0;
				} else {
					TankB.TurretTraverseDispersionFactor = double.Parse (TankBTurretTraverseDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBFireDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBFireDispersionFactorTextBox.Text)) {
					TankB.FireDispersionFactor = 0;
				} else {
					TankB.FireDispersionFactor = double.Parse (TankBFireDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBDamagedDispersionFactorTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBDamagedDispersionFactorTextBox.Text)) {
					TankB.DamagedDispersionFactor = 0;
				} else {
					TankB.DamagedDispersionFactor = double.Parse (TankBDamagedDispersionFactorTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBMoveSpeedByHardTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBMoveSpeedByHardTextBox.Text)) {
					TankB.MoveSpeedByHard = 0;
				} else {
					TankB.MoveSpeedByHard = double.Parse (TankBMoveSpeedByHardTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBMoveSpeedByMediumTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBMoveSpeedByMediumTextBox.Text)) {
					TankB.MoveSpeedByMedium = 0;
				} else {
					TankB.MoveSpeedByMedium = double.Parse (TankBMoveSpeedByMediumTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBMoveSpeedBySoftTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBMoveSpeedBySoftTextBox.Text)) {
					TankB.MoveSpeedBySoft = 0;
				} else {
					TankB.MoveSpeedBySoft = double.Parse (TankBMoveSpeedBySoftTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBHullTraverseSpeedByHardTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBHullTraverseSpeedByHardTextBox.Text)) {
					TankB.HullTraverseSpeedByHard = 0;
				} else {
					TankB.HullTraverseSpeedByHard = double.Parse (TankBHullTraverseSpeedByHardTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBHullTraverseSpeedByMediumTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBHullTraverseSpeedByMediumTextBox.Text)) {
					TankB.HullTraverseSpeedByMedium = 0;
				} else {
					TankB.HullTraverseSpeedByMedium = double.Parse (TankBHullTraverseSpeedByMediumTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBHullTraverseSpeedBySoftTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBHullTraverseSpeedBySoftTextBox.Text)) {
					TankB.HullTraverseSpeedBySoft = 0;
				} else {
					TankB.HullTraverseSpeedBySoft = double.Parse (TankBHullTraverseSpeedBySoftTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBTurretTraverseSpeedTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (TankBTurretTraverseSpeedTextBox.Text)) {
					TankB.TurretTraverseSpeed = 0;
				} else {
					TankB.TurretTraverseSpeed = double.Parse (TankBTurretTraverseSpeedTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBTerrainComboBox_SelectedIndexChanged (object sender, EventArgs e) {
			TankB.TerrainType = (TerrainType)TankBTerrainComboBox.SelectedIndex;
			OnSelectedTerrainType (TankB);
			Calculate ();
		}

		private void TankBIsMovingCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankB.IsMoving = TankBIsMovingCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBIsTraversingHullCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankB.IsTraversingHull = TankBIsTraversingHullCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBIsTraversingTurretCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankB.IsTraversingTurret = TankBIsTraversingTurretCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBIsFiringCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankB.IsFireing = TankBIsFiringCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBIsDamagedCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				TankB.IsDamaged = TankBIsDamagedCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void TankBAutoInputTanksGGTextBox_TextChanged (object sender, EventArgs e) {
			AutoInputTanksGG (TankB);
		}

		private void TankBAutoInputTanksGGTextBox_Click (object sender, EventArgs e) {
			TankBAutoInputTanksGGTextBox.Text = Clipboard.GetText ();
		}

		private void Panel_Paint (object sender, PaintEventArgs e) {
			DrawAimCircle (TankA, e.Graphics);
			DrawAimCircle (TankB, e.Graphics);
			CompareResult (TankA, TankB);
		}

		private void TimeTrackBar_Scroll (object sender, EventArgs e) {
			CurrentAimTime = (double)TimeTrackBar.Value / TimeTrackBar.Maximum * MaxAimTime;
			Panel.Refresh ();
		}

		private void CurrentTimeTextBox_TextChanged (object sender, EventArgs e) {
			try {
				BlockCurrentTimeTextBox = true;
				if (string.IsNullOrWhiteSpace (CurrentTimeTextBox.Text)) {
					CurrentAimTime = 0;
				} else {
					CurrentAimTime = double.Parse (CurrentTimeTextBox.Text);
				}
				Panel.Refresh ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			} finally {
				BlockCurrentTimeTextBox = false;
			}
		}

		private void PlayButton_Click (object sender, EventArgs e) {
			Stopwatch.Restart ();
			Timer.Enabled = true;
		}

		private void Timer_Tick (object sender, EventArgs e) {
			CurrentAimTime = Stopwatch.Elapsed.TotalSeconds;
			Panel.Refresh ();
			if (CurrentAimTime >= MaxAimTime) {
				Timer.Enabled = false;
			}
		}

		protected override CreateParams CreateParams {

			get {
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 0x02000000;
				return createParams;
			}

		}

		void InitializeTerrainComboBox (ComboBox comboBox) {
			comboBox.Items.Add ("硬");
			comboBox.Items.Add ("中");
			comboBox.Items.Add ("软");
		}

		void OnSelectedTerrainType (AimTimeCalculatorTank tank) {
			switch (tank.TerrainType) {
				case TerrainType.Hard:
					tank.MoveSpeed = tank.MoveSpeedByHard;
					tank.HullTraverseSpeed = tank.HullTraverseSpeedByHard;
					break;
				case TerrainType.Medium:
					tank.MoveSpeed = tank.MoveSpeedByMedium;
					tank.HullTraverseSpeed = tank.HullTraverseSpeedByMedium;
					break;
				case TerrainType.Soft:
					tank.MoveSpeed = tank.MoveSpeedBySoft;
					tank.HullTraverseSpeed = tank.HullTraverseSpeedBySoft;
					break;
				default:
					throw new NotImplementedException (tank.TerrainType.ToString ());
			}
		}

		void Calculate () {
			if (BlockCalculate) {
				return;
			}
			Calculate (TankA);
			Calculate (TankB);
			SetResult (TankA);
			SetResult (TankB);
			Compare (TankA, TankB);
			MaxAimTime = Math.Max (TankA.ActualAimTime, TankB.ActualAimTime);
			Panel.Refresh ();
		}

		void Calculate (AimTimeCalculatorTank tank) {
			OnSelectedTerrainType (tank);
			double movePenalty = tank.IsMoving ? Math.Pow (tank.MoveSpeed * tank.MoveDispersionFactor, 2) : 0;
			double hullTraversePenalty = tank.IsTraversingHull ? Math.Pow (tank.HullTraverseSpeed * tank.HullTraverseDispersionFactor, 2) : 0;
			double turretTraversePenalty = tank.IsTraversingTurret ? Math.Pow (tank.TurretTraverseSpeed * tank.TurretTraverseDispersionFactor, 2) : 0;
			double firePenalty = tank.IsFireing ? Math.Pow (tank.FireDispersionFactor, 2) : 0;
			double dispersionFactor = Math.Sqrt (1 + movePenalty + hullTraversePenalty + turretTraversePenalty + firePenalty);
			double aimTime = Math.Log (dispersionFactor) * tank.AimTime;
			double dispersion = (tank.IsDamaged ? (tank.Dispersion * tank.DamagedDispersionFactor) : tank.Dispersion) * dispersionFactor;
			tank.ActualAimTime = aimTime;
			tank.ActualDispersion = dispersion;
			tank.CurrentAimTime = aimTime;
			tank.CurrentDispersion = dispersion;
		}

		void SetResult (AimTimeCalculatorTank tank) {
			tank.ActualAimTimeTextBox.Text = $"{tank.CurrentAimTime:F4}";
			tank.ActualDispersionTextBox.Text = $"{tank.CurrentDispersion:F4}";
		}

		void AutoInputTanksGG (AimTimeCalculatorTank tank) {
			if (tank.AutoInputTanksGGTextBox.TextLength == 0) {
				return;
			}
			BlockCalculate = true;
			try {
				tank.NameTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, "<h1>(.*?)<");
				tank.AimTimeTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"Aim time \(sec\)");
				tank.DispersionTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, "Dispersion");
				tank.MoveDispersionFactorTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… moving \(\+\)");
				tank.HullTraverseDispersionFactorTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… tank traverse \(\+\)");
				tank.TurretTraverseDispersionFactorTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… turret traverse \(\+\)");
				tank.FireDispersionFactorTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… after firing \(\+\)");
				tank.DamagedDispersionFactorTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… damaged \(\×\)");
				tank.MoveSpeedByHardTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… hard \(km/h\)");
				tank.MoveSpeedByMediumTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… medium \(km/h\)");
				tank.MoveSpeedBySoftTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… soft \(km/h\)");
				tank.HullTraverseSpeedByHardTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… hard \(°/sec\)");
				tank.HullTraverseSpeedByMediumTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… medium \(°/sec\)");
				tank.HullTraverseSpeedBySoftTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"… soft \(°/sec\)");
				tank.TurretTraverseSpeedTextBox.Text = GetTanksGGValue (tank.AutoInputTanksGGTextBox.Text, @"Turret traverse \(°/sec\)");
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
			BlockCalculate = false;
			tank.AutoInputTanksGGTextBox.Clear ();
			OnSelectedTerrainType (tank);
			Calculate ();
		}

		string GetTanksGGValue (string html, string label) {
			Group group = Regex.Match (html, $@"{label}.*?([0-9\.]*?)</span>").Groups[1];
			if (!group.Success) {
				throw new Exception ($"未能解析出相应数据{Environment.NewLine}" +
					$"请使用浏览器访问tanks.gg，例如：https://tanks.gg/tank/obj-277 {Environment.NewLine}" +
					$"然后打开浏览器的开发者工具复制整个页面的html代码");
			}
			return group.Value;
		}

		void Compare (AimTimeCalculatorTank a, AimTimeCalculatorTank b) {
			API.SetCompareColor (a.AimTimeTextBox, b.AimTimeTextBox, a.AimTime.CompareTo (b.AimTime) * -1);
			API.SetCompareColor (a.DispersionTextBox, b.DispersionTextBox, a.Dispersion.CompareTo (b.Dispersion) * -1);
			API.SetCompareColor (a.MoveDispersionFactorTextBox, b.MoveDispersionFactorTextBox, a.MoveDispersionFactor.CompareTo (b.MoveDispersionFactor) * -1);
			API.SetCompareColor (a.HullTraverseDispersionFactorTextBox, b.HullTraverseDispersionFactorTextBox,
				a.HullTraverseDispersionFactor.CompareTo (b.HullTraverseDispersionFactor) * -1
			);
			API.SetCompareColor (a.TurretTraverseDispersionFactorTextBox, b.TurretTraverseDispersionFactorTextBox,
				a.TurretTraverseDispersionFactor.CompareTo (b.TurretTraverseDispersionFactor) * -1
			);
			API.SetCompareColor (a.FireDispersionFactorTextBox, b.FireDispersionFactorTextBox, a.FireDispersionFactor.CompareTo (b.FireDispersionFactor) * -1);
			API.SetCompareColor (a.DamagedDispersionFactorTextBox, b.DamagedDispersionFactorTextBox,
				a.DamagedDispersionFactor.CompareTo (b.DamagedDispersionFactor) * -1
			);
			API.SetCompareColor (a.MoveSpeedByHardTextBox, b.MoveSpeedByHardTextBox, a.MoveSpeedByHard.CompareTo (b.MoveSpeedByHard));
			API.SetCompareColor (a.MoveSpeedByMediumTextBox, b.MoveSpeedByMediumTextBox, a.MoveSpeedByMedium.CompareTo (b.MoveSpeedByMedium));
			API.SetCompareColor (a.MoveSpeedBySoftTextBox, b.MoveSpeedBySoftTextBox, a.MoveSpeedBySoft.CompareTo (b.MoveSpeedBySoft));
			API.SetCompareColor (a.HullTraverseSpeedByHardTextBox, b.HullTraverseSpeedByHardTextBox,
				a.HullTraverseSpeedByHard.CompareTo (b.HullTraverseSpeedByHard)
			);
			API.SetCompareColor (a.HullTraverseSpeedByMediumTextBox, b.HullTraverseSpeedByMediumTextBox,
				a.HullTraverseSpeedByMedium.CompareTo (b.HullTraverseSpeedByMedium)
			);
			API.SetCompareColor (a.HullTraverseSpeedBySoftTextBox, b.HullTraverseSpeedBySoftTextBox,
				a.HullTraverseSpeedBySoft.CompareTo (b.HullTraverseSpeedBySoft)
			);
			API.SetCompareColor (a.TurretTraverseSpeedTextBox, b.TurretTraverseSpeedTextBox, a.TurretTraverseSpeed.CompareTo (b.TurretTraverseSpeed));
			CompareResult (a, b);
		}

		void CompareResult (AimTimeCalculatorTank a, AimTimeCalculatorTank b) {
			API.SetCompareColor (a.ActualAimTimeTextBox, b.ActualAimTimeTextBox, a.CurrentAimTime.CompareTo (b.CurrentAimTime) * -1);
			API.SetCompareColor (a.ActualDispersionTextBox, b.ActualDispersionTextBox, a.CurrentDispersion.CompareTo (b.CurrentDispersion) * -1);
		}

		void DrawAimCircle (AimTimeCalculatorTank tank, Graphics graphics) {
			float centerX = Panel.Width / 2F;
			float centerY = Panel.Height / 2F;
			double current = 1 - Math.Min (CurrentAimTime / tank.ActualAimTime, 1);
			if (double.IsNaN (current)) {
				current = 1;
			}
			tank.CurrentAimTime = tank.ActualAimTime - tank.ActualAimTime * (1 - current);
			tank.CurrentDispersion = tank.Dispersion + (tank.ActualDispersion - tank.Dispersion) * current;
			double diameter = tank.CurrentDispersion * AimCircleScale;
			double radius = diameter / 2;
			double startX = centerX - radius;
			double startY = centerY - radius;
			graphics.DrawEllipse (tank.Pen, (float)startX, (float)startY, (float)diameter, (float)diameter);
			SetResult (tank);
		}

	}

}