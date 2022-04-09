using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {


	public partial class OujBoxCombatRecordQueryForm : Form {

		readonly OujBoxService OujBoxService = new OujBoxService ();
		readonly string[] Modes = { "标准赛" };
		readonly ListViewComparer TankListViewComparer;
		readonly ListViewGroup BoxListViewGroup = new ListViewGroup ("偶游盒子");
		readonly ListViewGroup ResultListViewGroup = new ListViewGroup ("查询结果");

		public OujBoxCombatRecordQueryForm () {
			InitializeComponent ();
			TankListViewComparer = new ListViewComparer (TankResultListView);
			TankResultListView.ListViewItemSorter = TankListViewComparer;
			NameTextBox.Text = Config.Instance.BoxCombatQueryPlayerName;
			ResultListView.HideSelection = true;
			TankResultListView.HideSelection = true;
			ResultListView.Groups.Add (BoxListViewGroup);
			ResultListView.Groups.Add (ResultListViewGroup);
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			if (API.CheckDateTime (StartDateTimePicker.Value, EndDateTimePicker.Value)) {
				Query (NameTextBox.Text, StartDateTimePicker.Value.Date == EndDateTimePicker.Value.Date);
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

		private void StartDateTimePicker_KeyUp (object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				QueryButton.PerformClick ();
			}
		}

		private void EndDateTimePicker_KeyUp (object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				QueryButton.PerformClick ();
			}
		}

		private void StartDateTimePicker_ValueChanged (object sender, EventArgs e) {
			if (EndDateTimePicker.Value < StartDateTimePicker.Value) {
				EndDateTimePicker.Value = StartDateTimePicker.Value;
			}
		}

		private void EndDateTimePicker_ValueChanged (object sender, EventArgs e) {
			if (StartDateTimePicker.Value > EndDateTimePicker.Value) {
				StartDateTimePicker.Value = EndDateTimePicker.Value;
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

		private void ResultListView_DrawColumnHeader (object sender, DrawListViewColumnHeaderEventArgs e) {
			e.DrawDefault = true;
		}

		private void ResultListView_DrawSubItem (object sender, DrawListViewSubItemEventArgs e) {
			e.DrawDefault = true;
			if (e.SubItem.Tag != null) {
				DoubleColor doubleColor = (DoubleColor)e.SubItem.Tag;
				e.SubItem.ForeColor = doubleColor.ForeColor;
				e.SubItem.BackColor = doubleColor.BackColor;
			}
		}

		private void TankResultListView_DrawColumnHeader (object sender, DrawListViewColumnHeaderEventArgs e) {
			e.DrawDefault = true;
		}

		private void TankResultListView_DrawSubItem (object sender, DrawListViewSubItemEventArgs e) {
			e.DrawDefault = true;
			if (e.SubItem.Tag != null) {
				DoubleColor doubleColor = (DoubleColor)e.SubItem.Tag;
				e.SubItem.ForeColor = doubleColor.ForeColor;
				e.SubItem.BackColor = doubleColor.BackColor;
			}
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

		void Query (string name, bool isSameDay = true) {
			NameTextBox.Enabled = false;
			StartDateTimePicker.Enabled = false;
			EndDateTimePicker.Enabled = false;
			QueryButton.Enabled = false;
			ExportButton.Enabled = false;
			ResultListView.Items.Clear ();
			TankResultListView.Items.Clear ();
			Config.Instance.BoxCombatQueryPlayerName = NameTextBox.Text;
			ConfigDao.Instance.Save (Config.Instance);
			new Thread (() => {
				try {
					CombatRecordPlayer player = OujBoxService.CreatePlayer (name);
					List<CombatRecord> combatRecords = OujBoxService.GetCombatRecords (player, StartDateTimePicker.Value, EndDateTimePicker.Value, isSameDay);
					CombatRecordSummary combatRecordSummary = OujBoxService.Summary (combatRecords, combatRecord => {
						return Modes.Contains (combatRecord.Mode) ? FilterResult.Execute : FilterResult.Continue;
					});
					float maxTankAverageCombat = 0, minTankAverageCombat = 0;
					int i = -1;
					foreach (var tank in combatRecordSummary.Tanks.Values) {
						i++;
						if (i == 0) {
							maxTankAverageCombat = tank.AverageCombat;
							minTankAverageCombat = tank.AverageCombat;
							continue;
						}
						if (maxTankAverageCombat < tank.AverageCombat) {
							maxTankAverageCombat = tank.AverageCombat;
						}
						if (minTankAverageCombat > tank.AverageCombat) {
							minTankAverageCombat = tank.AverageCombat;
						}
					}
					float tankAverageCombatDifference = maxTankAverageCombat - minTankAverageCombat;
					Invoke (new Action (() => {
						ResultListView.BeginUpdate ();
						ResultListView.Items.Add (new ListViewItem ("更新时间", BoxListViewGroup)).SubItems.Add ($"{player.UpdateTime:yyyy年MM月dd日 HH时mm分}");
						ResultListView.Items.Add (new ListViewItem ("昵称", BoxListViewGroup)).SubItems.Add (player.Name);
						ResultListView.Items.Add (new ListViewItem ("千场效率", BoxListViewGroup)).SubItems.Add (new ListViewItem.ListViewSubItem () {
							Text = $"{player.Combat}",
							Tag = API.GetCombatColor (player.Combat)
						});
						ResultListView.Items.Add (new ListViewItem ("千场胜率", BoxListViewGroup)).SubItems.Add ($"{player.WinRate:P0}");
						ResultListView.Items.Add (new ListViewItem ("千场命中率", BoxListViewGroup)).SubItems.Add ($"{player.HitRate:P0}");
						ResultListView.Items.Add (new ListViewItem ("千场出战等级", BoxListViewGroup)).SubItems.Add ($"{player.AverageCombatLevel}");
						ResultListView.Items.Add (new ListViewItem ("千场均伤", BoxListViewGroup)).SubItems.Add ($"{player.AverageDamage}");
						if (combatRecordSummary.CombatNumber > 0) {
							if (isSameDay) {
								ResultListView.Items.Add (new ListViewItem ($"查询日期", ResultListViewGroup)).SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
							} else {
								ResultListView.Items.Add (new ListViewItem ($"查询范围：从", ResultListViewGroup)).SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
								ResultListView.Items.Add (new ListViewItem ($"至", ResultListViewGroup)).SubItems.Add ($"{EndDateTimePicker.Value:yyyy年MM月dd日}");
							}
							ResultListView.Items.Add (new ListViewItem ("战斗次数", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.CombatNumber}");
							ResultListView.Items.Add (new ListViewItem ("胜率", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.VictoryRate:P2}");
							ResultListView.Items.Add (new ListViewItem ("胜利次数", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.VictoryNumber}");
							ResultListView.Items.Add (new ListViewItem ("平局次数", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.EvenNumber}");
							ResultListView.Items.Add (new ListViewItem ("失败次数", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.FailNumber}");
							ResultListView.Items.Add (new ListViewItem ("平均对局时间", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageDuration / 60:F2} 分钟");
							ResultListView.Items.Add (new ListViewItem ("平均存活时间", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageSurvivalTime / 60:F2} 分钟");
							ResultListView.Items.Add (new ListViewItem ("平均经验", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageXP:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均效率", ResultListViewGroup)).SubItems.Add (new ListViewItem.ListViewSubItem () {
								Text = $"{combatRecordSummary.AverageCombat:F2}",
								Tag = API.GetCombatColor (combatRecordSummary.AverageCombat)
							});
							ResultListView.Items.Add (new ListViewItem ("平均伤害", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageDamage:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均协助", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageAssist:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均抵挡伤害", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageArmorResistence:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均命中率", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageHitRate:P2}");
							ResultListView.Items.Add (new ListViewItem ("平均击穿率", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AveragePenetrationRate:P2}");
							ResultListView.Items.Add (new ListViewItem ("平均击穿率(含未命中)", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AveragePenetrationRateIncludeNoHit:P2}");
						}

						API.AutoResizeListViewColumns (ResultListView);
						ResultListView.EndUpdate ();
						if (combatRecordSummary.CombatNumber == 0) {
							MessageBox.Show (this, "没有战斗数据");
							return;
						}
						TankResultListView.BeginUpdate ();
						foreach (KeyValuePair<string, CombatRecordSummary> tank in combatRecordSummary.Tanks) {
							ListViewItem listViewItem = new ListViewItem (tank.Key);
							listViewItem.SubItems.Insert (CombatNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.CombatNumber}"));
							listViewItem.SubItems.Insert (VictoryRateColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.VictoryRate:P2}"));
							listViewItem.SubItems.Insert (VictoryNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.VictoryNumber}"));
							listViewItem.SubItems.Insert (EvenNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.EvenNumber}"));
							listViewItem.SubItems.Insert (FailNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.FailNumber}"));
							listViewItem.SubItems.Insert (AverageDurationColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageDuration / 60:F2} 分"));
							listViewItem.SubItems.Insert (AverageSurvivalTimeColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageSurvivalTime / 60:F2} 分"));
							listViewItem.SubItems.Insert (AverageXPColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageXP:F2}"));
							listViewItem.SubItems.Insert (AverageCombatColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageCombat:F2}") {
								Tag = API.GetCombatColor (tank.Value.AverageCombat)
							});
							listViewItem.SubItems.Insert (AverageDamageColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageDamage:F2}"));
							listViewItem.SubItems.Insert (AverageAssistColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageAssist:F2}"));
							listViewItem.SubItems.Insert (AverageArmorResistanceColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageArmorResistence:F2}"));
							listViewItem.SubItems.Insert (AverageHitRateColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageHitRate:P2}"));
							listViewItem.SubItems.Insert (AveragePenetrationRateColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AveragePenetrationRate:P2}"));
							listViewItem.SubItems.Insert (AveragePenetrationRateIncludeNoHitColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AveragePenetrationRateIncludeNoHit:P2}"));
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