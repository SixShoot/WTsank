using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class AimTimeCalculatorForm : Form {

		const float AimCircleLineWidth = 1;
		const float AimTimeTrackBarScale = 1000;

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
		int ShootNumber;
		bool BlockCalculate = true;
		bool BlockCurrentTimeTextBox;
		float AimCircleScale;
		float TankDistanceScale;
		float TankAimScale;
		Image TankImage;
		bool NeedRefreshTankImage;

		public AimTimeCalculatorForm () {
			InitializeComponent ();
		}

		private void AimTimeCalculatorForm_Load (object sender, EventArgs e) {
			Panel.BackColor = Color.Gray;
			ScaleComboBox.Items.Add (new KeyValuePair<string, float> ("无倍镜", 1));
			ScaleComboBox.Items.Add (new KeyValuePair<string, float> ("2倍镜", 2));
			ScaleComboBox.Items.Add (new KeyValuePair<string, float> ("4倍镜", 4));
			ScaleComboBox.Items.Add (new KeyValuePair<string, float> ("8倍镜", 8));
			ScaleComboBox.Items.Add (new KeyValuePair<string, float> ("16倍镜", 16));
			ScaleComboBox.Items.Add (new KeyValuePair<string, float> ("25倍镜", 25));
			ScaleComboBox.DisplayMember = "Key";
			ScaleComboBox.ValueMember = "Value";
			ScaleComboBox.SelectedIndex = 3;
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("50米", 50));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("100米", 100));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("150米", 150));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("200米", 200));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("250米", 250));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("300米", 300));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("350米", 350));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("400米", 400));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("450米", 450));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("500米", 500));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("550米", 550));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("600米", 600));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("650米", 650));
			DistanceComboBox.Items.Add (new KeyValuePair<string, float> ("700米", 700));
			DistanceComboBox.DisplayMember = "Key";
			DistanceComboBox.ValueMember = "Value";
			DistanceComboBox.SelectedIndex = 3;
			Image LoadImage (string path) {
				Type type = GetType ();
				return Image.FromStream (type.Assembly.GetManifestResourceStream ($"WorldOfTanks.Images.{path}"));
			}
			ShootSceneComboBox.Items.Add (new KeyValuePair<string, Image> ("E5头包", LoadImage ("100_25_0_E5_CommanderTower.jpg")));
			ShootSceneComboBox.Items.Add (new KeyValuePair<string, Image> ("279头包", LoadImage ("100_25_0_279_CommanderTower.jpg")));
			ShootSceneComboBox.Items.Add (new KeyValuePair<string, Image> ("E5整体", LoadImage ("100_25_0_E5_Center.jpg")));
			ShootSceneComboBox.DisplayMember = "Key";
			ShootSceneComboBox.ValueMember = "Value";
			ShootSceneComboBox.SelectedIndex = 0;
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
			TankA.Pen = new Pen (Color.LightGreen, AimCircleLineWidth);
			TankA.Brush = Brushes.LightGreen;
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
			TankB.Brush = Brushes.Red;
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
			ShootNumberTextBox.Text = "1";
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
			if (NeedRefreshTankImage) {
				//NeedRefreshTankImage = false;
				float tankWidth = TankImage.Width * TankDistanceScale * TankAimScale;
				float tankHeight = TankImage.Height * TankDistanceScale * TankAimScale;
				float tankHalfWidth = tankWidth / 2F;
				float tankHalfHeight = tankHeight / 2F;
				float panelHalfWidth = Panel.ClientSize.Width / 2F;
				float panelHalfHeight = Panel.ClientSize.Height / 2F;
				e.Graphics.DrawImage (
					TankImage,
					(int)(-tankHalfWidth + panelHalfWidth),
					(int)(-tankHalfHeight + panelHalfHeight),
					(int)tankWidth,
					(int)tankHeight
				);
			}
			DrawImpactPoints (TankA, e.Graphics);
			DrawImpactPoints (TankB, e.Graphics);
			DrawAimCircle (TankA, e.Graphics);
			DrawAimCircle (TankB, e.Graphics);
			CompareResult (TankA, TankB);
		}

		private void TimeTrackBar_Scroll (object sender, EventArgs e) {
			CurrentAimTime = Api.Divide ((double)TimeTrackBar.Value, TimeTrackBar.Maximum) * MaxAimTime;
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

		private void ShootNumberTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (ShootNumberTextBox.Text)) {
					ShootNumber = 0;
				} else {
					ShootNumber = int.Parse (ShootNumberTextBox.Text);
				}
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayButton_Click (object sender, EventArgs e) {
			Stopwatch.Restart ();
			Timer.Enabled = true;
		}

		private void Timer_Tick (object sender, EventArgs e) {
			CurrentAimTime = Stopwatch.Elapsed.TotalSeconds;
			Panel.Refresh ();
			CurrentAimTime = Stopwatch.Elapsed.TotalSeconds;
			if (CurrentAimTime >= MaxAimTime) {
				Stopwatch.Stop ();
				Timer.Enabled = false;
				Console.WriteLine (Stopwatch.ElapsedMilliseconds);
			}
		}

		private void ScaleComboBox_SelectedIndexChanged (object sender, EventArgs e) {
			float value = ((KeyValuePair<string, float>)ScaleComboBox.SelectedItem).Value;
			AimCircleScale = 40 * value;
			TankAimScale = 0.04F * value;
			NeedRefreshTankImage = true;
			Panel.Refresh ();
		}

		private void DistanceComboBox_SelectedIndexChanged (object sender, EventArgs e) {
			float value = ((KeyValuePair<string, float>)DistanceComboBox.SelectedItem).Value;
			TankDistanceScale = 1 * (100 / value);
			NeedRefreshTankImage = true;
			Panel.Refresh ();
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
			TankA.ImpactPoints.Clear ();
			TankB.ImpactPoints.Clear ();
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
					$"然后按下F12键打开浏览器的开发者工具复制整个页面的html代码");
			}
			return group.Value;
		}

		void Compare (AimTimeCalculatorTank a, AimTimeCalculatorTank b) {
			Api.SetCompareColor (a.AimTimeTextBox, b.AimTimeTextBox, a.AimTime.CompareTo (b.AimTime) * -1);
			Api.SetCompareColor (a.DispersionTextBox, b.DispersionTextBox, a.Dispersion.CompareTo (b.Dispersion) * -1);
			Api.SetCompareColor (a.MoveDispersionFactorTextBox, b.MoveDispersionFactorTextBox, a.MoveDispersionFactor.CompareTo (b.MoveDispersionFactor) * -1);
			Api.SetCompareColor (a.HullTraverseDispersionFactorTextBox, b.HullTraverseDispersionFactorTextBox,
				a.HullTraverseDispersionFactor.CompareTo (b.HullTraverseDispersionFactor) * -1
			);
			Api.SetCompareColor (a.TurretTraverseDispersionFactorTextBox, b.TurretTraverseDispersionFactorTextBox,
				a.TurretTraverseDispersionFactor.CompareTo (b.TurretTraverseDispersionFactor) * -1
			);
			Api.SetCompareColor (a.FireDispersionFactorTextBox, b.FireDispersionFactorTextBox, a.FireDispersionFactor.CompareTo (b.FireDispersionFactor) * -1);
			Api.SetCompareColor (a.DamagedDispersionFactorTextBox, b.DamagedDispersionFactorTextBox,
				a.DamagedDispersionFactor.CompareTo (b.DamagedDispersionFactor) * -1
			);
			Api.SetCompareColor (a.MoveSpeedByHardTextBox, b.MoveSpeedByHardTextBox, a.MoveSpeedByHard.CompareTo (b.MoveSpeedByHard));
			Api.SetCompareColor (a.MoveSpeedByMediumTextBox, b.MoveSpeedByMediumTextBox, a.MoveSpeedByMedium.CompareTo (b.MoveSpeedByMedium));
			Api.SetCompareColor (a.MoveSpeedBySoftTextBox, b.MoveSpeedBySoftTextBox, a.MoveSpeedBySoft.CompareTo (b.MoveSpeedBySoft));
			Api.SetCompareColor (a.HullTraverseSpeedByHardTextBox, b.HullTraverseSpeedByHardTextBox,
				a.HullTraverseSpeedByHard.CompareTo (b.HullTraverseSpeedByHard)
			);
			Api.SetCompareColor (a.HullTraverseSpeedByMediumTextBox, b.HullTraverseSpeedByMediumTextBox,
				a.HullTraverseSpeedByMedium.CompareTo (b.HullTraverseSpeedByMedium)
			);
			Api.SetCompareColor (a.HullTraverseSpeedBySoftTextBox, b.HullTraverseSpeedBySoftTextBox,
				a.HullTraverseSpeedBySoft.CompareTo (b.HullTraverseSpeedBySoft)
			);
			Api.SetCompareColor (a.TurretTraverseSpeedTextBox, b.TurretTraverseSpeedTextBox, a.TurretTraverseSpeed.CompareTo (b.TurretTraverseSpeed));
			CompareResult (a, b);
		}

		void CompareResult (AimTimeCalculatorTank a, AimTimeCalculatorTank b) {
			Api.SetCompareColor (a.ActualAimTimeTextBox, b.ActualAimTimeTextBox, a.CurrentAimTime.CompareTo (b.CurrentAimTime) * -1);
			Api.SetCompareColor (a.ActualDispersionTextBox, b.ActualDispersionTextBox, a.CurrentDispersion.CompareTo (b.CurrentDispersion) * -1);
		}

		void DrawAimCircle (AimTimeCalculatorTank tank, Graphics graphics) {
			float centerX = Panel.ClientSize.Width / 2F;
			float centerY = Panel.ClientSize.Height / 2F;
			double current = 1 - Math.Min (Api.Divide (CurrentAimTime, tank.ActualAimTime), 1);
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
			diameter = tank.Dispersion * AimCircleScale;
			radius = diameter / 2;
			startX = centerX - radius;
			startY = centerY - radius;
			graphics.DrawEllipse (tank.Pen, (float)startX, (float)startY, (float)diameter, (float)diameter);
			SetResult (tank);
		}

		void DrawImpactPoints (AimTimeCalculatorTank tank, Graphics graphics) {
			float width = 10 * TankAimScale;
			float radius = width / 2;
			double x;
			double y;
			float offsetX = Panel.ClientSize.Width / 2F;
			float offsetY = Panel.ClientSize.Height / 2F;
			for (int i = 0; i < tank.ImpactPoints.Count; i++) {
				x = tank.ImpactPoints[i].X * AimCircleScale;
				y = tank.ImpactPoints[i].Y * AimCircleScale;
				graphics.FillEllipse (tank.Brush, (float)(x - radius + offsetX), (float)(y - radius + offsetY), width, width);
			}
		}

		private void AimTimeCalculatorForm_Resize (object sender, EventArgs e) {
			NeedRefreshTankImage = true;
		}

		private void Panel_Click (object sender, EventArgs e) {
			ShootButton.PerformClick ();
		}

		void Shoot (AimTimeCalculatorTank tank) {
			double x;
			double y;
			double radius = tank.CurrentDispersion / 2;
			double standardDeviation = 1 / 3F;
			do {
				x = Api.NextGaussian (0, standardDeviation);
				y = Api.NextGaussian (0, standardDeviation);
			} while (!Api.PointInsideCircle (x, y, 1));
			tank.ImpactPoints.Add (new ImpactPoint () { X = x * radius, Y = y * radius });
		}

		static double[] ayZTFB = null;

		/// <summary>
		/// 计算标准正态分布表
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static double CalcN (double input) {
			if (ayZTFB == null) {
				//从0.00到3.09的标准正态分布表
				string ss = "0.5,0.504,0.508,0.512,0.516,0.5199,0.5239,0.5279,0.5319,0.5359,0.5398,0.5438,0.5478,0.5517,0.5557,0.5596,0.5636,0.5675,0.5714,0.5753,0.5793,0.5832,0.5871,0.591,0.5948,0.5987,0.6026,0.6064,0.6103,0.6141,0.6179,0.6217,0.6255,0.6293,0.6331,0.6368,0.6406,0.6443,0.648,0.6517,0.6554,0.6591,0.6628,0.6664,0.67,0.6736,0.6772,0.6808,0.6844,0.6879,0.6915,0.695,0.6985,0.7019,0.7054,0.7088,0.7123,0.7157,0.719,0.7224,0.7257,0.7291,0.7324,0.7357,0.7389,0.7422,0.7454,0.7486,0.7517,0.7549,0.758,0.7611,0.7642,0.7673,0.7703,0.7734,0.7764,0.7794,0.7823,0.7852,0.7881,0.791,0.7939,0.7967,0.7995,0.8023,0.8051,0.8078,0.8106,0.8133,0.8159,0.8186,0.8212,0.8238,0.8264,0.8289,0.8315,0.834,0.8365,0.8389,0.8413,0.8438,0.8461,0.8485,0.8508,0.8531,0.8554,0.8577,0.8599,0.8621,0.8643,0.8665,0.8686,0.8708,0.8729,0.8749,0.877,0.879,0.881,0.883,0.8849,0.8869,0.8888,0.8907,0.8925,0.8944,0.8962,0.898,0.8997,0.9015,0.9032,0.9049,0.9066,0.9082,0.9099,0.9115,0.9131,0.9147,0.9162,0.9177,0.9192,0.9207,0.9222,0.9236,0.9251,0.9265,0.9278,0.9292,0.9306,0.9319,0.9332,0.9345,0.9357,0.937,0.9382,0.9394,0.9406,0.9418,0.943,0.9441,0.9452,0.9463,0.9474,0.9484,0.9495,0.9505,0.9515,0.9525,0.9535,0.9545,0.9554,0.9564,0.9573,0.9582,0.9591,0.9599,0.9608,0.9616,0.9625,0.9633,0.9641,0.9648,0.9656,0.9664,0.9671,0.9678,0.9686,0.9693,0.97,0.9706,0.9713,0.9719,0.9726,0.9732,0.9738,0.9744,0.975,0.9756,0.9762,0.9767,0.9772,0.9778,0.9783,0.9788,0.9793,0.9798,0.9803,0.9808,0.9812,0.9817,0.9821,0.9826,0.983,0.9834,0.9838,0.9842,0.9846,0.985,0.9854,0.9857,0.9861,0.9864,0.9868,0.9871,0.9874,0.9878,0.9881,0.9884,0.9887,0.989,0.9893,0.9896,0.9898,0.9901,0.9904,0.9906,0.9909,0.9911,0.9913,0.9916,0.9918,0.992,0.9922,0.9925,0.9927,0.9929,0.9931,0.9932,0.9934,0.9936,0.9938,0.994,0.9941,0.9943,0.9945,0.9946,0.9948,0.9949,0.9951,0.9952,0.9953,0.9955,0.9956,0.9957,0.9959,0.996,0.9961,0.9962,0.9963,0.9964,0.9965,0.9966,0.9967,0.9968,0.9969,0.997,0.9971,0.9972,0.9973,0.9974,0.9974,0.9975,0.9976,0.9977,0.9977,0.9978,0.9979,0.9979,0.998,0.9981,0.9981,0.9982,0.9982,0.9983,0.9984,0.9984,0.9985,0.9985,0.9986,0.9986,0.9987,0.999,0.9993,0.9995,0.9997,0.9998,0.9998,0.9999,0.9999,1";
				var ays = ss.Split (',');

				var temp = new double[310];
				for (int i = 0; i < 310; i++) {
					temp[i] = double.Parse (ays[i]);
				}
				ayZTFB = temp;
			}

			int idx = (int)(Math.Abs (input * 100) + 0.5);
			if (idx < 0) {
				idx = 0;
			}
			if (idx >= ayZTFB.Length) {
				idx = ayZTFB.Length - 1;
			}

			var val = ayZTFB[idx];
			if (input < 0) {
				val = 1 - val;
			}
			return val;
		}

		private void ShootSceneComboBox_SelectedIndexChanged (object sender, EventArgs e) {
			Image value = ((KeyValuePair<string, Image>)ShootSceneComboBox.SelectedItem).Value;
			TankImage = value;
			Panel.Refresh ();
		}

		private void ShootButton_Click (object sender, EventArgs e) {
			for (int i = 0; i < ShootNumber; i++) {
				Shoot (TankA);
				Shoot (TankB);
			}
			Panel.Refresh ();
		}

		private void ClearButton_Click (object sender, EventArgs e) {
			TankA.ImpactPoints.Clear ();
			TankB.ImpactPoints.Clear ();
			Panel.Refresh ();
		}

	}

}