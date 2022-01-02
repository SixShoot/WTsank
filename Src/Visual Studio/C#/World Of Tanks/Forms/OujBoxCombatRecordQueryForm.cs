using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {


	public partial class OujBoxCombatRecordQueryForm : Form {

		readonly OujBoxService OujBoxService = new OujBoxService ();
		readonly string[] Modes = { "标准赛" };

		public OujBoxCombatRecordQueryForm () {
			InitializeComponent ();
			TankResultListView.ListViewItemSorter = new TankResultListViewComparer ();
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			if (CheckDateTime ()) {
				Query (NameTextBox.Text);
			}
		}

		private void ToTodayButton_Click (object sender, EventArgs e) {
			if (CheckDateTime ()) {
				Query (NameTextBox.Text, false);
			}
		}

		bool CheckDateTime () {
			if (DateTimePicker.Value > DateTime.Now) {
				MessageBox.Show ("不能查询未来的战绩");
				return false;
			}
			return true;
		}

		void Query (string name, bool isSameDay = true) {
			NameTextBox.Enabled = false;
			DateTimePicker.Enabled = false;
			QueryButton.Enabled = false;
			ToTodayButton.Enabled = false;
			ExportButton.Enabled = false;
			ResultListView.Items.Clear ();
			TankResultListView.Items.Clear ();
			new Thread (() => {
				try {
					List<CombatRecord> combatRecords = OujBoxService.GetCombatRecords (name, DateTimePicker.Value, isSameDay);
					CombatRecordSummary combatRecordSummary = OujBoxService.Summary (combatRecords, combatRecord => {
						return Modes.Contains (combatRecord.Mode) ? FilterResult.Execute : FilterResult.Continue;
					});
					if (combatRecordSummary.CombatNumber == 0) {
						MessageBox.Show (this, "没有战斗数据");
						return;
					}
					Invoke (new Action (() => {
						ResultListView.BeginUpdate ();
						ResultListView.Items.Add ("玩家").SubItems.Add ($"{name}");
						ResultListView.Items.Add ("千场效率").SubItems.Add ($"{OujBoxService.GetCombat (name)}");
						if (isSameDay) {
							ResultListView.Items.Add ($"查询日期").SubItems.Add ($"{DateTimePicker.Value:yyyy年MM月dd日}");
						} else {
							ResultListView.Items.Add ($"查询范围：从").SubItems.Add ($"{DateTimePicker.Value:yyyy年MM月dd日}");
							ResultListView.Items.Add ($"至").SubItems.Add ($"{DateTime.Now:yyyy年MM月dd日}");
						}
						ResultListView.Items.Add ("战斗次数").SubItems.Add ($"{combatRecordSummary.CombatNumber}");
						ResultListView.Items.Add ("胜率").SubItems.Add ($"{combatRecordSummary.VictoryRate:P2}");
						ResultListView.Items.Add ("胜利次数").SubItems.Add ($"{combatRecordSummary.VictoryNumber}");
						ResultListView.Items.Add ("平局次数").SubItems.Add ($"{combatRecordSummary.EvenNumber}");
						ResultListView.Items.Add ("失败次数").SubItems.Add ($"{combatRecordSummary.FailNumber}");
						ResultListView.Items.Add ("平均对局时间").SubItems.Add ($"{combatRecordSummary.AverageDuration / 60:F2} 分钟");
						ResultListView.Items.Add ("平均存活时间").SubItems.Add ($"{combatRecordSummary.AverageSurvivalTime / 60:F2} 分钟");
						ResultListView.Items.Add ("平均效率").SubItems.Add ($"{combatRecordSummary.AverageCombat:F2}");
						ResultListView.Items.Add ("平均经验").SubItems.Add ($"{combatRecordSummary.AverageXP:F2}");
						ResultListView.Items.Add ("平均伤害").SubItems.Add ($"{combatRecordSummary.AverageDamage:F2}");
						ResultListView.Items.Add ("平均协助").SubItems.Add ($"{combatRecordSummary.AverageAssist:F2}");
						API.AutoResizeListViewColumns (ResultListView);
						ResultListView.EndUpdate ();
						TankResultListView.BeginUpdate ();
						foreach (KeyValuePair<string, CombatRecordSummary> tank in combatRecordSummary.Tanks) {
							ListViewItem listViewItem = new ListViewItem (tank.Key);
							listViewItem.SubItems.Add ($"{tank.Value.CombatNumber}");
							listViewItem.SubItems.Add ($"{tank.Value.VictoryRate:P2}");
							listViewItem.SubItems.Add ($"{tank.Value.VictoryNumber}");
							listViewItem.SubItems.Add ($"{tank.Value.EvenNumber}");
							listViewItem.SubItems.Add ($"{tank.Value.FailNumber}");
							listViewItem.SubItems.Add ($"{tank.Value.AverageDuration / 60:F2} 分");
							listViewItem.SubItems.Add ($"{tank.Value.AverageSurvivalTime / 60:F2} 分");
							listViewItem.SubItems.Add ($"{tank.Value.AverageCombat:F2}");
							listViewItem.SubItems.Add ($"{tank.Value.AverageXP:F2}");
							listViewItem.SubItems.Add ($"{tank.Value.AverageDamage:F2}");
							listViewItem.SubItems.Add ($"{tank.Value.AverageAssist:F2}");
							TankResultListView.Items.Add (listViewItem);
						}
						API.AutoResizeListViewColumns (TankResultListView);
						TankResultListView.EndUpdate ();
					}));
				} catch (Exception exception) {
					Invoke (new Action (() => {
						MessageBox.Show (this, exception.ToString ());
					}));
				} finally {
					Invoke (new Action (() => {
						NameTextBox.Enabled = true;
						DateTimePicker.Enabled = true;
						QueryButton.Enabled = true;
						ToTodayButton.Enabled = true;
						ExportButton.Enabled = true;
						MessageBox.Show (this, "查询完毕");
					}));
				}
			}) {
				IsBackground = true
			}.Start ();
		}

		class TankResultListViewComparer : IComparer {

			public int Compare (object x, object y) {
				ListViewItem a = (ListViewItem)x;
				ListViewItem b = (ListViewItem)y;
				if (!float.TryParse (a.SubItems[8].Text, out float combatA)) {
					combatA = -1;
				}
				if (!float.TryParse (b.SubItems[8].Text, out float combatB)) {
					combatB = -1;
				}
				return combatA.CompareTo (combatB) * -1;
			}

		}

		private void ExportButton_Click (object sender, EventArgs e) {
			if (saveFileDialog1.ShowDialog () == DialogResult.OK) {
				API.ExportCSV (ResultListView, TankResultListView, saveFileDialog1.FileName);
			}
		}

		private void NameTextBox_KeyUp (object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				QueryButton.PerformClick ();
			}
		}
	}

}