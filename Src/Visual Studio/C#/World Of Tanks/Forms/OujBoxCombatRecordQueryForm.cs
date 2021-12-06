using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {


	public partial class OujBoxCombatRecordQueryForm : Form {

		readonly PageChanger<Page> PageChanger = new PageChanger<Page> ();
		readonly CombatRecordQueryResultForm CombatRecordQueryResultForm = new CombatRecordQueryResultForm ();
		readonly ClanQueryResultForm ClanQueryResultForm = new ClanQueryResultForm ();
		readonly OujBoxService OujBoxService = new OujBoxService ();
		readonly WarGamingNetService WarGamingNetService = new WarGamingNetService ();
		readonly string[] Modes = { "标准赛" };

		public OujBoxCombatRecordQueryForm () {
			InitializeComponent ();
		}

		private void OujBoxCombatRecordQueryForm_Load (object sender, EventArgs e) {
			PageChanger.Parent = Panel;
			PageChanger.Add (Page.CombatRecordQueryResult, CombatRecordQueryResultForm);
			PageChanger.Add (Page.ClanQueryResult, ClanQueryResultForm);
			PageChanger.Change (Page.None);
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			if (DateTimePicker.Value.Month > DateTime.Now.Month ||
				(DateTimePicker.Value.Month == DateTime.Now.Month && DateTimePicker.Value.Day > DateTime.Now.Day)
			) {
				MessageBox.Show ("不能查询未来的战绩");
				return;
			}
			Query (NameTextBox.Text);
		}

		private void ClanButton_Click (object sender, EventArgs e) {
			QueryClan (NameTextBox.Text);
		}

		private void OujBoxCombatRecordQueryForm_Resize (object sender, EventArgs e) {
			PageChanger.ResizeCurrentPage ();
		}

		void BeginQuery () {
			QueryButton.Enabled = false;
			ClanButton.Enabled = false;
			PageChanger.Change (Page.None);
			SetSummaryLabel ("查询中");
		}

		void EndQuery () {
			if (InvokeRequired) {
				Invoke (new Action (EndQuery));
				return;
			}
			QueryButton.Enabled = true;
			ClanButton.Enabled = true;
		}

		void SetSummaryLabel (string text) {
			if (InvokeRequired) {
				Invoke (new Action<string> (SetSummaryLabel), text);
				return;
			}
			SummaryLabel.Text = text;
		}

		void Query (string name) {
			BeginQuery ();
			PageChanger.Change (Page.CombatRecordQueryResult);
			CombatRecordQueryResultForm.ClearAllListViewItems ();
			new Thread (() => {
				try {
					List<CombatRecord> combatRecords = OujBoxService.GetCombatRecords (name, DateTimePicker.Value.Month, DateTimePicker.Value.Day);
					CombatRecordSummary combatRecordSummary = OujBoxService.Summary (combatRecords, combatRecord => {
						return Modes.Contains (combatRecord.Mode) ? FilterResult.Execute : FilterResult.Continue;
					});
					if (combatRecordSummary.CombatNumber == 0) {
						throw new Exception ("没有战斗数据");
					}
					CombatRecordQueryResultForm.AddResultListViewItem (listView => {
						listView.Items.Add ("千场效率").SubItems.Add ($"{OujBoxService.GetCombat (name)}");
						listView.Items.Add ("战斗次数").SubItems.Add ($"{combatRecordSummary.CombatNumber}");
						listView.Items.Add ("胜率").SubItems.Add ($"{combatRecordSummary.VictoryRate:P2}");
						listView.Items.Add ("胜利次数").SubItems.Add ($"{combatRecordSummary.VictoryNumber}");
						listView.Items.Add ("平局次数").SubItems.Add ($"{combatRecordSummary.EvenNumber}");
						listView.Items.Add ("失败次数").SubItems.Add ($"{combatRecordSummary.FailNumber}");
						listView.Items.Add ("平均对局持续时间").SubItems.Add ($"{combatRecordSummary.AverageDuration / 60:F2} 分钟");
						listView.Items.Add ("平均存活时间").SubItems.Add ($"{combatRecordSummary.AverageSurvivalTime / 60:F2} 分钟");
						listView.Items.Add ("平均效率").SubItems.Add ($"{combatRecordSummary.AverageCombat:F2}");
						listView.Items.Add ("平均经验").SubItems.Add ($"{combatRecordSummary.AverageXP:F2}");
						listView.Items.Add ("平均伤害").SubItems.Add ($"{combatRecordSummary.AverageDamage:F2}");
						listView.Items.Add ("平均协助").SubItems.Add ($"{combatRecordSummary.AverageAssist:F2}");
					});
					CombatRecordQueryResultForm.AddTankResultListViewItem (listView => {
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
							listView.Items.Add (listViewItem);
						}
					});
					SetSummaryLabel ("查询完毕");
				} catch (Exception exception) {
					SetSummaryLabel (exception.Message);
					Invoke (new Action (() => {
						MessageBox.Show (this, exception.ToString ());
					}));
				} finally {
					EndQuery ();
				}
			}) {
				IsBackground = true
			}.Start ();
		}

		void QueryClan (string name) {
			BeginQuery ();
			PageChanger.Change (Page.ClanQueryResult);
			ClanQueryResultForm.ClearResultListViewItems ();
			new Thread (() => {
				try {
					int id = WarGamingNetService.QueryClanID (name);
					List<string> names = WarGamingNetService.GetClanMembers (id);
					float totalCombat = 0;
					List<float> combats = new List<float> ();
					foreach (string memberName in names) {
						float memberCombat = 0;
						string combatText;
						try {
							memberCombat = OujBoxService.GetCombat (memberName);
							combatText = $"{memberCombat:F2}";
							totalCombat += memberCombat;
							combats.Add (memberCombat);
						} catch (Exception exception) {
							combatText = exception.Message;
						}
						ClanQueryResultForm.AddResultListViewItem (listView => {
							ListViewItem listViewItem = new ListViewItem (memberName);
							listViewItem.SubItems.Add (combatText);
							listView.Items.Add (listViewItem);
						});
					}
					float averageCombat = totalCombat / combats.Count;
					combats.Sort ();
					float medianCombat = API.GetMedian (combats);
					SetSummaryLabel ($"成员数：{names.Count} 平均效率：{averageCombat:F2} 效率中位数：{medianCombat}");
				} catch (Exception exception) {
					SetSummaryLabel (exception.Message);
					Invoke (new Action (() => {
						MessageBox.Show (this, exception.ToString ());
					}));
				} finally {
					EndQuery ();
				}
			}) {
				IsBackground = true
			}.Start ();
		}

	}

}