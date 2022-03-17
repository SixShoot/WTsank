using System;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class SpottingDistanceCalculatorForm : Form {

		readonly SpottingPlayer PlayerA = new SpottingPlayer ();
		readonly SpottingPlayer PlayerB = new SpottingPlayer ();

		bool BlockCalculate = true;

		public SpottingDistanceCalculatorForm () {
			InitializeComponent ();
		}

		private void SpottingDistanceCalculatorForm_Load (object sender, EventArgs e) {
			InitializeCommanderVisionSystemComboBox (PlayerACommanderVisionSystemComboBox);
			InitializeCommanderVisionSystemComboBox (PlayerBCommanderVisionSystemComboBox);
			PlayerAViewRangeTextBox.Text = "450";
			PlayerAStaticConcealmentTextBox.Text = "30";
			PlayerAMoveConcealmentTextBox.Text = "25";
			PlayerAIsMovingCheckBox.Checked = true;
			PlayerACommanderVisionSystemComboBox.SelectedIndex = (int)CommanderVisionSystemType.SpecialSlot;
			PlayerASmallFoliageNumberTextBox.Text = "0";
			PlayerALargeFoliageNumberTextBox.Text = "0";
			PlayerBViewRangeTextBox.Text = "500";
			PlayerBStaticConcealmentTextBox.Text = "40";
			PlayerBMoveConcealmentTextBox.Text = "30";
			PlayerBCommanderVisionSystemComboBox.SelectedIndex = (int)CommanderVisionSystemType.NonSpecialSlot;
			PlayerBSmallFoliageNumberTextBox.Text = "0";
			PlayerBLargeFoliageNumberTextBox.Text = "1";
			BlockCalculate = false;
			Calculate ();
		}

		private void PlayerAViewRangeTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerAViewRangeTextBox.Text)) {
					PlayerA.ViewRange = 0;
				} else {
					PlayerA.ViewRange = double.Parse (PlayerAViewRangeTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerAStaticConcealmentTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerAStaticConcealmentTextBox.Text)) {
					PlayerA.StaticConcealment = 0;
				} else {
					PlayerA.StaticConcealment = double.Parse (PlayerAStaticConcealmentTextBox.Text) / 100;
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerAMoveConcealmentTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerAMoveConcealmentTextBox.Text)) {
					PlayerA.MoveConcealment = 0;
				} else {
					PlayerA.MoveConcealment = double.Parse (PlayerAMoveConcealmentTextBox.Text) / 100;
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerAIsMovingCheckBox_CheckedChanged (object sender, EventArgs e) {
			try {
				PlayerA.IsMoving = PlayerAIsMovingCheckBox.Checked;
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerACommanderVisionSystemComboBox_SelectedIndexChanged (object sender, EventArgs e) {
			PlayerA.CommanderVisionSystemType = (CommanderVisionSystemType)PlayerACommanderVisionSystemComboBox.SelectedIndex;
			OnSelectedCommanderVisionSystemType (PlayerA, PlayerACommanderVisionSystemValueByFoliageTextBox, PlayerACommanderVisionSystemValueByMoveTextBox);
		}

		private void PlayerACommanderVisionSystemValueByFoliageTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerACommanderVisionSystemValueByFoliageTextBox.Text)) {
					PlayerA.CommanderVisionSystemValueByFoliage = 0;
				} else {
					PlayerA.CommanderVisionSystemValueByFoliage = double.Parse (PlayerACommanderVisionSystemValueByFoliageTextBox.Text) / 100;
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerACommanderVisionSystemValueByMoveTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerACommanderVisionSystemValueByMoveTextBox.Text)) {
					PlayerA.CommanderVisionSystemValueByMove = 0;
				} else {
					PlayerA.CommanderVisionSystemValueByMove = double.Parse (PlayerACommanderVisionSystemValueByMoveTextBox.Text) / 100;
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerASmallFoliageNumberTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerASmallFoliageNumberTextBox.Text)) {
					PlayerA.SmallFoliageNumber = 0;
				} else {
					PlayerA.SmallFoliageNumber = int.Parse (PlayerASmallFoliageNumberTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerALargeFoliageNumberTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerALargeFoliageNumberTextBox.Text)) {
					PlayerA.LargeFoliageNumber = 0;
				} else {
					PlayerA.LargeFoliageNumber = int.Parse (PlayerALargeFoliageNumberTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerBViewRangeTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerBViewRangeTextBox.Text)) {
					PlayerB.ViewRange = 0;
				} else {
					PlayerB.ViewRange = double.Parse (PlayerBViewRangeTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerBStaticConcealmentTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerBStaticConcealmentTextBox.Text)) {
					PlayerB.StaticConcealment = 0;
				} else {
					PlayerB.StaticConcealment = double.Parse (PlayerBStaticConcealmentTextBox.Text) / 100;
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerBMoveConcealmentTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerBMoveConcealmentTextBox.Text)) {
					PlayerB.MoveConcealment = 0;
				} else {
					PlayerB.MoveConcealment = double.Parse (PlayerBMoveConcealmentTextBox.Text) / 100;
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerBIsMovingCheckBox_CheckedChanged (object sender, EventArgs e) {
			PlayerB.IsMoving = PlayerBIsMovingCheckBox.Checked;
			Calculate ();
		}

		private void PlayerBCommanderVisionSystemComboBox_SelectedIndexChanged (object sender, EventArgs e) {
			PlayerB.CommanderVisionSystemType = (CommanderVisionSystemType)PlayerBCommanderVisionSystemComboBox.SelectedIndex;
			OnSelectedCommanderVisionSystemType (PlayerB, PlayerBCommanderVisionSystemValueByFoliageTextBox, PlayerBCommanderVisionSystemValueByMoveTextBox);
		}

		private void PlayerBCommanderVisionSystemValueByFoliageTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerBCommanderVisionSystemValueByFoliageTextBox.Text)) {
					PlayerB.CommanderVisionSystemValueByFoliage = 0;
				} else {
					PlayerB.CommanderVisionSystemValueByFoliage = double.Parse (PlayerBCommanderVisionSystemValueByFoliageTextBox.Text) / 100;
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerBCommanderVisionSystemValueByMoveTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerBCommanderVisionSystemValueByMoveTextBox.Text)) {
					PlayerB.CommanderVisionSystemValueByMove = 0;
				} else {
					PlayerB.CommanderVisionSystemValueByMove = double.Parse (PlayerBCommanderVisionSystemValueByMoveTextBox.Text) / 100;
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerBSmallFoliageNumberTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerBSmallFoliageNumberTextBox.Text)) {
					PlayerB.SmallFoliageNumber = 0;
				} else {
					PlayerB.SmallFoliageNumber = int.Parse (PlayerBSmallFoliageNumberTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void PlayerBLargeFoliageNumberTextBox_TextChanged (object sender, EventArgs e) {
			try {
				if (string.IsNullOrWhiteSpace (PlayerBLargeFoliageNumberTextBox.Text)) {
					PlayerB.LargeFoliageNumber = 0;
				} else {
					PlayerB.LargeFoliageNumber = int.Parse (PlayerBLargeFoliageNumberTextBox.Text);
				}
				Calculate ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		void InitializeCommanderVisionSystemComboBox (ComboBox comboBox) {
			comboBox.Items.Add ("加成位");
			comboBox.Items.Add ("非加成位");
			comboBox.Items.Add ("自定义");
			comboBox.Items.Add ("无");
		}

		void OnSelectedCommanderVisionSystemType (SpottingPlayer player, TextBox commanderVisionSystemValueByFoliageTextBox,
			TextBox commanderVisionSystemValueByMoveTextBox
		) {
			BlockCalculate = true;
			try {
				switch (player.CommanderVisionSystemType) {
					case CommanderVisionSystemType.SpecialSlot:
						commanderVisionSystemValueByFoliageTextBox.Text = "20";
						commanderVisionSystemValueByMoveTextBox.Text = "12.5";
						break;
					case CommanderVisionSystemType.NonSpecialSlot:
						commanderVisionSystemValueByFoliageTextBox.Text = "15";
						commanderVisionSystemValueByMoveTextBox.Text = "10";
						break;
					case CommanderVisionSystemType.Custom:
						commanderVisionSystemValueByFoliageTextBox.Text = (player.CustomCommanderVisionSystemValueByFoliage * 100).ToString ();
						commanderVisionSystemValueByMoveTextBox.Text = (player.CustomCommanderVisionSystemValueByMove * 100).ToString ();
						break;
					case CommanderVisionSystemType.None:
						commanderVisionSystemValueByFoliageTextBox.Text = "0";
						commanderVisionSystemValueByMoveTextBox.Text = "0";
						break;
					default:
						throw new NotImplementedException (player.CommanderVisionSystemType.ToString ());
				}
				if (player.CanInput ()) {
					commanderVisionSystemValueByFoliageTextBox.Enabled = true;
					commanderVisionSystemValueByMoveTextBox.Enabled = true;
				} else {
					commanderVisionSystemValueByFoliageTextBox.Enabled = false;
					commanderVisionSystemValueByMoveTextBox.Enabled = false;
				}
			} finally {
				BlockCalculate = false;
			}
			Calculate ();
		}

		void Calculate () {
			if (BlockCalculate) {
				return;
			}
			double spottingDistanceA = Calculate (PlayerA, PlayerB);
			double spottingDistanceB = Calculate (PlayerB, PlayerA);
			PlayerAToBSpottingDistanceTextBox.Text = spottingDistanceA.ToString ("F2");
			PlayerBToASpottingDistanceTextBox.Text = spottingDistanceB.ToString ("F2");
			API.SetCompareColor (PlayerAToBSpottingDistanceTextBox, PlayerBToASpottingDistanceTextBox, spottingDistanceA.CompareTo (spottingDistanceB));
		}

		double Calculate (SpottingPlayer a, SpottingPlayer b) {
			double foliageConcealment = Math.Min (b.SmallFoliageNumber * 0.25 + b.LargeFoliageNumber * 0.5, 0.8);
			double actualFoliageConcealment = foliageConcealment * (1 - a.CommanderVisionSystemValueByFoliage);
			double tankConcealment = b.IsMoving ? b.MoveConcealment : b.StaticConcealment;
			double actualTankConcealment = tankConcealment * (b.IsMoving ? (1 - a.CommanderVisionSystemValueByMove) : 1);
			double actualConcealment = actualFoliageConcealment + actualTankConcealment;
			return Math.Min (Math.Max (a.ViewRange - (a.ViewRange - 50) * actualConcealment, 50), 445);
		}

	}

}