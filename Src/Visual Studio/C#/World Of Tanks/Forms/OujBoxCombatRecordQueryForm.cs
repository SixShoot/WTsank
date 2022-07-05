using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {


	public partial class OujBoxCombatRecordQueryForm : Form {

		readonly string[] Modes = { "标准赛" };
		readonly ListViewComparer TankListViewComparer;
		readonly ListViewGroup QueryListViewGroup = new ListViewGroup ("查询");
		readonly ListViewGroup BoxListViewGroup = new ListViewGroup ("偶游盒子");
		readonly ListViewGroup ResultListViewGroup = new ListViewGroup ("结果");
		readonly List<ColumnHeader> Columns = new List<ColumnHeader> ();

		public OujBoxCombatRecordQueryForm () {
			InitializeComponent ();
			TankListViewComparer = new ListViewComparer (TankResultListView) {
				OnGetColumnType = column => {
					if (column == NameColumnHeader.Index) {
						return typeof (string);
					}
					return typeof (float);
				}
			};
			TankResultListView.ListViewItemSorter = TankListViewComparer;
			NameTextBox.Text = Config.Instance.BoxCombatQueryPlayerName;
			ResultListView.Groups.Add (QueryListViewGroup);
			ResultListView.Groups.Add (BoxListViewGroup);
			ResultListView.Groups.Add (ResultListViewGroup);
			AutoResizeResultListViewColumns ();
			foreach (ColumnHeader columnHeader in TankResultListView.Columns) {
				Columns.Add (columnHeader);
			}
			if (Config.Instance.BoxCombatQueryTankListColumns == null) {
				Config.Instance.BoxCombatQueryTankListColumns = new List<string> () {
					NameColumnHeader.Text,
					CombatNumberColumnHeader.Text,
					VictoryRateColumnHeader.Text,
					EvenNumberColumnHeader.Text,
					AverageCombatColumnHeader.Text,
					AverageDamageColumnHeader.Text,
					AverageAssistColumnHeader.Text,
					AverageArmorResistanceColumnHeader.Text
				};
				ConfigDao.Instance.Save (Config.Instance);
			}
			for (int i = 0; i < TankResultListView.Columns.Count; i++) {
				TankResultListView.Columns[i].Tag = Config.Instance.BoxCombatQueryTankListColumns.Contains (TankResultListView.Columns[i].Text) ? null : (object)0;
			}
			ApplyColumnVisible ();
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			if (Api.CheckDateTime (StartDateTimePicker.Value, EndDateTimePicker.Value)) {
				Query (NameTextBox.Text, StartDateTimePicker.Value.Date == EndDateTimePicker.Value.Date);
			}
		}

		private void ExportButton_Click (object sender, EventArgs e) {
			if (saveFileDialog1.ShowDialog () == DialogResult.OK) {
				Api.ExportCSV (ResultListView, TankResultListView, saveFileDialog1.FileName);
			}
		}

		private void Control_KeyUp (object sender, KeyEventArgs e) {
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
			TankListViewComparer.OnClickColumn (e.Column);
		}

		private void ListView_DrawColumnHeader (object sender, DrawListViewColumnHeaderEventArgs e) {
			e.DrawDefault = true;
		}

		private void ListView_DrawSubItem (object sender, DrawListViewSubItemEventArgs e) {
			e.DrawDefault = true;
			if (e.SubItem.Tag != null) {
				DoubleColor doubleColor = (DoubleColor)e.SubItem.Tag;
				e.SubItem.ForeColor = doubleColor.ForeColor;
				e.SubItem.BackColor = doubleColor.BackColor;
			}
		}

		private void SetColumnButton_Click (object sender, EventArgs e) {
			if (new SetColumnForm (Columns).ShowDialog () == DialogResult.OK) {
				ApplyColumnVisible ();
				Config.Instance.BoxCombatQueryTankListColumns.Clear ();
				for (int i = 0; i < Columns.Count; i++) {
					if (Columns[i].Tag == null) {
						Config.Instance.BoxCombatQueryTankListColumns.Add (Columns[i].Text);
					}
				}
				ConfigDao.Instance.Save (Config.Instance);
			}
		}

		void AutoResizeResultListViewColumns () {
			Api.AutoResizeListViewColumns (ResultListView, true);
			TankResultListView.Left = ResultListView.Right + 5;
			TankResultListView.Width = QueryButton.Right - TankResultListView.Left;
		}

		void SetState (string text) {
			Invoke (new Action (() => StateLabel.Text = text));
		}

		void Query (string name, bool isSameDay = true) {
			NameTextBox.Enabled = false;
			StartDateTimePicker.Enabled = false;
			EndDateTimePicker.Enabled = false;
			QueryButton.Enabled = false;
			ExportButton.Enabled = false;
			SetColumnButton.Enabled = false;
			ResultListView.Items.Clear ();
			TankResultListView.Items.Clear ();
			Config.Instance.BoxCombatQueryPlayerName = NameTextBox.Text;
			ConfigDao.Instance.Save (Config.Instance);
			new Thread (() => {
				try {
					BoxPersonalCombatRecord player = BoxService.Instance.GetPersonalCombatRecord (name);
					double dayDifference = (DateTime.Now - StartDateTimePicker.Value).TotalDays;
					List<BoxCombatRecord> combatRecords = BoxService.Instance.GetCombatRecords (
						player,
						StartDateTimePicker.Value,
						EndDateTimePicker.Value,
						isSameDay,
						(page, dateTime) => {
							SetState ($"进度：{Api.Divide ((DateTime.Now.Date - dateTime).TotalDays, dayDifference):P0} 页：{page} 时间：{dateTime:yyyy年MM月dd日}");
						}
					);
					combatRecords.RemoveAll (item => !Modes.Contains (item.Mode));
					int count = 0;
					SetState ($"进度：0% 0/{combatRecords.Count}");
					BoxCombatRecordSummary combatRecordSummary = BoxService.Instance.SummaryCombatRecords (combatRecords, combatRecord => {
						return Modes.Contains (combatRecord.Mode) ? LoopAction.Execute : LoopAction.Continue;
					}, () => {
						count++;
						SetState ($"进度：{Api.Divide (count, combatRecords.Count):P0} {count}/{combatRecords.Count}");
					});
					Invoke (new Action (() => {
						ResultListView.BeginUpdate ();
						ResultListView.Items.Add (new ListViewItem ("昵称", QueryListViewGroup)).SubItems.Add (player.Name);
						string clanName;
						try {
							clanName = WarGamingNetService.Instance.GetClanNameByPlayerName (player.Name);
						} catch (Exception e) {
							clanName = e.Message;
						}
						ResultListView.Items.Add (new ListViewItem ("军团", QueryListViewGroup)).SubItems.Add (clanName);
						ResultListView.Items.Add (new ListViewItem ("更新时间", BoxListViewGroup)).SubItems.Add ($"{player.UpdateTime:yyyy年MM月dd日 HH时mm分}");
						ResultListView.Items.Add (new ListViewItem ("千场效率", BoxListViewGroup)).SubItems.Add (new ListViewItem.ListViewSubItem () {
							Text = $"{player.Combat}",
							Tag = Api.GetCombatColor (player.Combat)
						});
						ResultListView.Items.Add (new ListViewItem ("千场胜率", BoxListViewGroup)).SubItems.Add ($"{player.WinRate:P0}");
						ResultListView.Items.Add (new ListViewItem ("千场命中率", BoxListViewGroup)).SubItems.Add ($"{player.HitRate:P0}");
						ResultListView.Items.Add (new ListViewItem ("千场出战等级", BoxListViewGroup)).SubItems.Add ($"{player.CombatLevel}");
						ResultListView.Items.Add (new ListViewItem ("千场均伤", BoxListViewGroup)).SubItems.Add ($"{player.Damage}");
						if (combatRecordSummary.CombatNumber > 0) {
							if (isSameDay) {
								ResultListView.Items.Add (new ListViewItem ($"查询日期", ResultListViewGroup)).SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
							} else {
								ResultListView.Items.Add (new ListViewItem ($"查询范围：从", ResultListViewGroup)).SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
								ResultListView.Items.Add (new ListViewItem ($"至", ResultListViewGroup)).SubItems.Add ($"{EndDateTimePicker.Value:yyyy年MM月dd日}");
							}
							ResultListView.Items.Add (new ListViewItem ("战斗次数", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.CombatNumber}");
							ResultListView.Items.Add (new ListViewItem ("胜率", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.WinRate:P2}");
							ResultListView.Items.Add (new ListViewItem ("胜利次数", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.WinNumber}");
							ResultListView.Items.Add (new ListViewItem ("平局次数", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.EvenNumber}");
							ResultListView.Items.Add (new ListViewItem ("失败次数", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.FailNumber}");
							ResultListView.Items.Add (new ListViewItem ("平均出战等级", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageTankLevel:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均对局时间", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageDuration / 60:F2} 分钟");
							ResultListView.Items.Add (new ListViewItem ("平均存活时间", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageSurvivalTime / 60:F2} 分钟");
							ResultListView.Items.Add (new ListViewItem ("平均经验", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageXP:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均效率", ResultListViewGroup)).SubItems.Add (new ListViewItem.ListViewSubItem () {
								Text = $"{combatRecordSummary.AverageCombat:F2}",
								Tag = Api.GetCombatColor (combatRecordSummary.AverageCombat)
							});
							ResultListView.Items.Add (new ListViewItem ("效率中位数", ResultListViewGroup)).SubItems.Add (new ListViewItem.ListViewSubItem () {
								Text = $"{combatRecordSummary.MedianCombat:F2}",
								Tag = Api.GetCombatColor (combatRecordSummary.MedianCombat)
							});
							ResultListView.Items.Add (new ListViewItem ("预测效率", ResultListViewGroup)).SubItems.Add (new ListViewItem.ListViewSubItem () {
								Text = $"{combatRecordSummary.PredictCombat:F2}",
								Tag = Api.GetCombatColor (combatRecordSummary.PredictCombat)
							});
							ResultListView.Items.Add (new ListViewItem ("平均伤害", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageDamage:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均协助", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageAssist:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均抵挡伤害", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageArmorResistence:F2}");
							ResultListView.Items.Add (new ListViewItem ("平均命中率", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AverageHitRate:P2}");
							ResultListView.Items.Add (new ListViewItem ("平均击穿率", ResultListViewGroup)).SubItems.Add ($"{combatRecordSummary.AveragePenetrationRate:P2}");
							ResultListView.Items.Add (new ListViewItem ("平均击穿率(含未命中)", ResultListViewGroup)).SubItems.Add (
								$"{combatRecordSummary.AveragePenetrationRateIncludeNoHit:P2}"
							);
						}
						AutoResizeResultListViewColumns ();
						ResultListView.EndUpdate ();
						if (combatRecordSummary.CombatNumber == 0) {
							MessageBox.Show (this, "没有战斗数据");
							return;
						}
						TankResultListView.BeginUpdate ();
						foreach (KeyValuePair<string, BoxCombatRecordSummary> tank in combatRecordSummary.Tanks) {
							ListViewItem listViewItem = new ListViewItem (tank.Key);
							listViewItem.SubItems.Insert (TankLevelColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.TankLevel}"));
							listViewItem.SubItems.Insert (CombatNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.CombatNumber}"));
							listViewItem.SubItems.Insert (VictoryRateColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.WinRate:P2}"));
							listViewItem.SubItems.Insert (VictoryNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.WinNumber}"));
							listViewItem.SubItems.Insert (EvenNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.EvenNumber}"));
							listViewItem.SubItems.Insert (FailNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.FailNumber}"));
							listViewItem.SubItems.Insert (AverageDurationColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.AverageDuration / 60:F2} 分"
							));
							listViewItem.SubItems.Insert (AverageSurvivalTimeColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.AverageSurvivalTime / 60:F2} 分"
							));
							listViewItem.SubItems.Insert (AverageXPColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageXP:F2}"));
							listViewItem.SubItems.Insert (AverageCombatColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageCombat:F2}") {
								Tag = Api.GetCombatColor (tank.Value.AverageCombat)
							});
							listViewItem.SubItems.Insert (MedianCombatColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.MedianCombat:F2}") {
								Tag = Api.GetCombatColor (tank.Value.MedianCombat)
							});
							listViewItem.SubItems.Insert (PredictCombatColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.PredictCombat:F2}") {
								Tag = Api.GetCombatColor (tank.Value.PredictCombat)
							});
							listViewItem.SubItems.Insert (AverageDamageColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageDamage:F2}"));
							listViewItem.SubItems.Insert (AverageAssistColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageAssist:F2}"));
							listViewItem.SubItems.Insert (AverageArmorResistanceColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.AverageArmorResistence:F2}"
							));
							listViewItem.SubItems.Insert (AverageHitRateColumnHeader.Index, new ListViewItem.ListViewSubItem (listViewItem, $"{tank.Value.AverageHitRate:P2}"));
							listViewItem.SubItems.Insert (AveragePenetrationRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.AveragePenetrationRate:P2}"
							));
							listViewItem.SubItems.Insert (AveragePenetrationRateIncludeNoHitColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.AveragePenetrationRateIncludeNoHit:P2}"
							));
							listViewItem.SubItems.Insert (MeetSPGNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MeetSPGNumber}"
							));
							listViewItem.SubItems.Insert (MeetSPGRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MeetSPGRate:P2}"
							));
							listViewItem.SubItems.Insert (OneSPGNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MeetOneSPGNumber}"
							));
							listViewItem.SubItems.Insert (OneSPGRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MeetOneSPGRate:P2}"
							));
							listViewItem.SubItems.Insert (TwoSPGNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MeetTwoSPGNumber}"
							));
							listViewItem.SubItems.Insert (TwoSPGRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MeetTwoSPGRate:P2}"
							));
							listViewItem.SubItems.Insert (ThreeSPGNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MeetThreeSPGNumber}"
							));
							listViewItem.SubItems.Insert (ThreeSPGRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MeetThreeSPGRate:P2}"
							));
							listViewItem.SubItems.Insert (AverageSPGNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.AverageSPGNumber}"
							));
							listViewItem.SubItems.Insert (MinusTwoLevelNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MinusTwoLevelNumber}"
							));
							listViewItem.SubItems.Insert (MinusTwoLevelRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MinusTwoLevelRate:P2}"
							));
							listViewItem.SubItems.Insert (MinusOneLevelNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MinusOneLevelNumber}"
							));
							listViewItem.SubItems.Insert (MinusOneLevelRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MinusOneLevelRate:P2}"
							));
							listViewItem.SubItems.Insert (SameLevelNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.SameLevelNumber}"
							));
							listViewItem.SubItems.Insert (SameLevelRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.SameLevelRate:P2}"
							));
							listViewItem.SubItems.Insert (MiddleLevelNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MiddleLevelNumber}"
							));
							listViewItem.SubItems.Insert (MiddleLevelRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.MiddleLevelRate:P2}"
							));
							listViewItem.SubItems.Insert (PlusOneLevelNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.PlusOneLevelNumber}"
							));
							listViewItem.SubItems.Insert (PlusOneLevelRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.PlusOneLevelRate:P2}"
							));
							listViewItem.SubItems.Insert (PlusTwoLevelNumberColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.PlusTwoLevelNumber}"
							));
							listViewItem.SubItems.Insert (PlusTwoLevelRateColumnHeader.Index, new ListViewItem.ListViewSubItem (
								listViewItem,
								$"{tank.Value.PlusTwoLevelRate:P2}"
							));
							TankResultListView.Items.Add (listViewItem);
						}
						TankListViewComparer.SortColumn (AverageCombatColumnHeader.Index, SortOrder.Descending);
						ApplyColumnVisible ();
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
						SetColumnButton.Enabled = true;
						StateLabel.Text = string.Empty;
					}));
				}
			}) {
				IsBackground = true
			}.Start ();
		}

		void ApplyColumnVisible () {
			TankResultListView.BeginUpdate ();
			Api.AutoResizeListViewColumns (TankResultListView);
			for (int i = 0; i < TankResultListView.Columns.Count; i++) {
				if (TankResultListView.Columns[i].Tag != null) {
					TankResultListView.Columns[i].Width = 0;
				}
			}
			TankResultListView.EndUpdate ();
		}

	}

}