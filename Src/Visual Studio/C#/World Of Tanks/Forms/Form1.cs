﻿using System;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class Form1 : Form {

		readonly PageChanger<Page> PageChanger = new PageChanger<Page> ();

		public Form1 () {
			InitializeComponent ();
		}

		private void Form1_Load (object sender, EventArgs e) {
			PageChanger.MDIForm = this;
			PageChanger.Parent = Panel;
			PageChanger.Add (Page.OujBoxCombatRecordQuery, new OujBoxCombatRecordQueryForm ());
			PageChanger.Add (Page.SpottingDistanceCalculator, new SpottingDistanceCalculatorForm ());
			PageChanger.Add (Page.AimTimeCalculator, new AimTimeCalculatorForm ());
#if DEBUG
			AimTimeCalculatorRadioButton.Checked = true;
#else
			OujBoxCombatRecordQueryRadioButton.Checked = true;
#endif
			WindowState = FormWindowState.Maximized;
		}

		private void OujBoxCombatRecordQueryRadioButton_CheckedChanged (object sender, EventArgs e) {
			if (OujBoxCombatRecordQueryRadioButton.Checked) {
				PageChanger.Change (Page.OujBoxCombatRecordQuery);
			}
		}

		private void SpottingDistanceCalculatorRadioButton_CheckedChanged (object sender, EventArgs e) {
			if (SpottingDistanceCalculatorRadioButton.Checked) {
				PageChanger.Change (Page.SpottingDistanceCalculator);
			}
		}

		private void AimTimeCalculatorRadioButton_CheckedChanged (object sender, EventArgs e) {
			if (AimTimeCalculatorRadioButton.Checked) {
				PageChanger.Change (Page.AimTimeCalculator);
			}
		}

		private void Panel_Resize (object sender, EventArgs e) {
			PageChanger.ResizeCurrentPage ();
		}

	}

}