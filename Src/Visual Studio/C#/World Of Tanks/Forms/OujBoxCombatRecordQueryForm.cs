using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {


	public partial class OujBoxCombatRecordQueryForm : Form {

		readonly OujBoxService OujBoxService = new OujBoxService ();
		readonly string[] Modes = { "标准赛" };
		readonly ListViewComparer TankListViewComparer;

		public OujBoxCombatRecordQueryForm () {
			InitializeComponent ();
			TankListViewComparer = new ListViewComparer (TankResultListView);
			TankResultListView.ListViewItemSorter = TankListViewComparer;
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			if (CheckDateTime ()) {
				Query (NameTextBox.Text, false);
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

		private void TankResultListView_ColumnClick (object sender, ColumnClickEventArgs e) {
			if (TankListViewComparer.SelectedColumnIndex == e.Column) {
				TankListViewComparer.ToggleSortOrder ();
			} else {
				TankListViewComparer.ListView.Sorting = SortOrder.Descending;
			}
			SortTankResultListViewColumn (e.Column, TankListViewComparer.ListView.Sorting);
		}

		void SortTankResultListViewColumn (int column, SortOrder sortOrder) {
			TankListViewComparer.SelectedColumnIndex = column;
			TankListViewComparer.ListView.Sorting = sortOrder;
			if (column == NameColumnHeader.Index) {
				TankListViewComparer.SelectedColumnType = typeof (string);
			} else {
				TankListViewComparer.SelectedColumnType = typeof (float);
			}
			TankListViewComparer.ListView.Sort ();
		}

		bool CheckDateTime () {
			if (StartDateTimePicker.Value.Date > DateTime.Now.Date) {
				MessageBox.Show ("不能查询未来的战绩");
				return false;
			}
			if (StartDateTimePicker.Value.Date > EndDateTimePicker.Value.Date) {
				MessageBox.Show ("开始日期不能大于结束日期");
				return false;
			}
			return true;
		}

		void Query (string name, bool isSameDay = true) {
			NameTextBox.Enabled = false;
			StartDateTimePicker.Enabled = false;
			EndDateTimePicker.Enabled = false;
			QueryButton.Enabled = false;
			ExportButton.Enabled = false;
			ResultListView.Items.Clear ();
			TankResultListView.Items.Clear ();
			new Thread (() => {
				try {
					List<CombatRecord> combatRecords = OujBoxService.GetCombatRecords (name, StartDateTimePicker.Value, EndDateTimePicker.Value, isSameDay);
					CombatRecordSummary combatRecordSummary = OujBoxService.Summary (combatRecords, combatRecord => {
						return Modes.Contains (combatRecord.Mode) ? FilterResult.Execute : FilterResult.Continue;
					});
					Invoke (new Action (() => {
						if (combatRecordSummary.CombatNumber == 0) {
							MessageBox.Show (this, "没有战斗数据");
							return;
						}
						ResultListView.BeginUpdate ();
						ResultListView.Items.Add ("玩家").SubItems.Add ($"{name}");
						ResultListView.Items.Add ("千场效率").SubItems.Add ($"{OujBoxService.GetCombat (name)}");
						if (isSameDay) {
							ResultListView.Items.Add ($"查询日期").SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
						} else {
							ResultListView.Items.Add ($"查询范围：从").SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
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
						SortTankResultListViewColumn (AverageCombatColumnHeader.Index, SortOrder.Descending);
						TankResultListView.EndUpdate ();
					}));
				} catch (Exception exception) {
					Invoke (new Action (() => {
						MessageBox.Show (this, exception.ToString ());
					}));
				} finally {
					Invoke (new Action (() => {
						NameTextBox.Enabled = true;
						StartDateTimePicker.Enabled = true;
						EndDateTimePicker.Enabled = true;
						QueryButton.Enabled = true;
						ExportButton.Enabled = true;
					}));
				}
			}) {
				IsBackground = true
			}.Start ();
		}

	}

}