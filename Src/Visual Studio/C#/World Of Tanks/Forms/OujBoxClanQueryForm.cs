using Eruru.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class OujBoxClanQueryForm : Form {

		readonly OujBoxService OujBoxService = new OujBoxService ();
		readonly WarGamingNetService WarGamingNetService = new WarGamingNetService ();
		readonly string[] Modes = { "要塞" };

		public OujBoxClanQueryForm () {
			InitializeComponent ();
			MemberResultListView.ListViewItemSorter = new MemberResultListViewComparer ();
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			Query (NameTextBox.Text, false);
		}

		private void AttendanceButton_Click (object sender, EventArgs e) {
			if (MessageBox.Show ("使用此功能会发送大量请求，由于限制了频率，处理时间会很久，需要耐心等待，是否继续？", "注意", MessageBoxButtons.YesNo) != DialogResult.Yes) {
				return;
			}
			if (CheckDateTime ()) {
				Query (NameTextBox.Text, true);
			}
		}

		bool CheckDateTime () {
			if (DateTimePicker.Value > DateTime.Now) {
				MessageBox.Show ("不能查询未来的战绩");
				return false;
			}
			return true;
		}

		void Query (string name, bool isAttendance) {
			NameTextBox.Enabled = false;
			DateTimePicker.Enabled = false;
			QueryButton.Enabled = false;
			AttendanceButton.Enabled = false;
			ExportButton.Enabled = false;
			ResultListView.Items.Clear ();
			MemberResultListView.Items.Clear ();
			int count = 0;
			object summaryLock = new object ();
			AutoResetEvent autoResetEvent = new AutoResetEvent (false);
			new Thread (() => {
				try {
					int id = WarGamingNetService.QueryClanID (name);
					List<string> names = WarGamingNetService.GetClanMembers (id);
					List<float> combats = new List<float> ();
					float totalCombat = 0;
					List<Player> players = new List<Player> ();
					if (isAttendance) {
						count = names.Count;
					}
					Invoke (new Action (() => {
						StateLabel.Text = $"进度：{players.Count}/{names.Count}";
					}));
					foreach (string memberName in names) {
						Player player = new Player (memberName, id);
						try {
							player.Combat = OujBoxService.GetCombat (memberName);
							player.CombatText = $"{player.Combat:F2}";
							combats.Add (player.Combat);
							if (isAttendance) {
								ThreadPool.QueueUserWorkItem (state => {
									Player innerPlayer = (Player)state;
									try {
										innerPlayer.Attendance = QueryAttendance (innerPlayer);
										innerPlayer.AttendanceText = innerPlayer.Attendance.ToString ();
									} catch (Exception e) {
										innerPlayer.AttendanceText = e.ToString ();
									} finally {
										lock (summaryLock) {
											count--;
											Invoke (new Action (() => {
												StateLabel.Text = $"进度：{names.Count - count}/{names.Count}";
											}));
											if (count <= 0) {
												autoResetEvent.Set ();
											}
										}
									}
								}, player);
							}
							totalCombat += player.Combat;
						} catch (Exception e) {
							player.CombatText = e.Message;
							if (isAttendance) {
								count--;
							}
						}
						players.Add (player);
						if (!isAttendance) {
							Invoke (new Action (() => {
								StateLabel.Text = $"进度：{players.Count}/{names.Count}";
							}));
						}
					}
					if (count <= 0) {
						autoResetEvent.Set ();
					}
					autoResetEvent.WaitOne ();
					float averageCombat = totalCombat / players.Count;
					combats.Sort ();
					float medianCombat = API.GetMedian (combats);
					Invoke (new Action (() => {
						ResultListView.BeginUpdate ();
						ResultListView.Items.Add ("军团").SubItems.Add ($"{name}");
						if (isAttendance) {
							ResultListView.Items.Add ($"查询范围：从").SubItems.Add ($"{DateTimePicker.Value:yyyy年MM月dd日}");
							ResultListView.Items.Add ($"至").SubItems.Add ($"{DateTime.Now:yyyy年MM月dd日}");
						} else {
							ResultListView.Items.Add ($"查询日期").SubItems.Add ($"{DateTimePicker.Value:yyyy年MM月dd日}");
						}
						ResultListView.Items.Add ("成员数").SubItems.Add ($"{names.Count}");
						ResultListView.Items.Add ("平均效率").SubItems.Add ($"{averageCombat:F2}");
						ResultListView.Items.Add ("效率中位数").SubItems.Add ($"{medianCombat:F2}");
						API.AutoResizeListViewColumns (ResultListView);
						ResultListView.EndUpdate ();
						MemberResultListView.BeginUpdate ();
						foreach (Player player in players) {
							ListViewItem listViewItem = new ListViewItem (player.Name);
							listViewItem.SubItems.Add (player.CombatText);
							listViewItem.SubItems.Add ($"{player.AttendanceText}");
							MemberResultListView.Items.Add (listViewItem);
						}
						API.AutoResizeListViewColumns (MemberResultListView);
						MemberResultListView.EndUpdate ();
					}));
				} catch (Exception e) {
					Invoke (new Action (() => {
						MessageBox.Show (this, e.ToString ());
					}));
				} finally {
					Invoke (new Action (() => {
						NameTextBox.Enabled = true;
						DateTimePicker.Enabled = true;
						QueryButton.Enabled = true;
						AttendanceButton.Enabled = true;
						ExportButton.Enabled = true;
						StateLabel.Text = string.Empty;
						MessageBox.Show (this, "查询完毕");
					}));
				}
			}) {
				IsBackground = true
			}.Start ();
		}

		int QueryAttendance (Player player) {
			List<CombatRecord> combatRecords = OujBoxService.GetCombatRecords (player.Name, DateTimePicker.Value, false);
			List<CombatRecord> filteredCombatRecords = new List<CombatRecord> ();
			DateTime dateTime = DateTime.MinValue;
			List<int> days = new List<int> ();
			int count = 0;
			object summaryLock = new object ();
			Exception exception = null;
			AutoResetEvent autoResetEvent = new AutoResetEvent (false);
			foreach (CombatRecord combatRecord in combatRecords) {
				if (Modes.Contains (combatRecord.Mode)) {
					if (dateTime != combatRecord.DateTime) {
						dateTime = combatRecord.DateTime;
						days.Add (0);
					}
					count++;
					ThreadPool.QueueUserWorkItem (state => {
						try {
							CombatRecord innerCombatRecord = (CombatRecord)state;
							OujBoxService.FillCombatRecord (innerCombatRecord);
							lock (summaryLock) {
								int clanMemberNumber = 0;
								foreach (JsonValue playerJsonObject in innerCombatRecord.TeamA) {
									if (playerJsonObject["clanDBID"] == player.ClanID) {
										clanMemberNumber++;
									}
								}
								if (clanMemberNumber > (innerCombatRecord.TeamA.Count - clanMemberNumber)) {
									days[days.Count - 1]++;
									filteredCombatRecords.Add (innerCombatRecord);
								}
							}
						} catch (Exception e) {
							exception = e;
						} finally {
							lock (summaryLock) {
								count--;
								if (count <= 0) {
									autoResetEvent.Set ();
								}
							}
						}
					}, combatRecord);
				}
			}
			if (count <= 0) {
				autoResetEvent.Set ();
			}
			autoResetEvent.WaitOne ();
			if (exception != null) {
				throw exception;
			}
			return days.Sum (value => value > 0 ? 1 : 0);
		}

		class MemberResultListViewComparer : IComparer {

			public int Compare (object x, object y) {
				ListViewItem a = (ListViewItem)x;
				ListViewItem b = (ListViewItem)y;
				if (!float.TryParse (a.SubItems[1].Text, out float combatA)) {
					combatA = -1;
				}
				if (!float.TryParse (b.SubItems[1].Text, out float combatB)) {
					combatB = -1;
				}
				return combatA.CompareTo (combatB) * -1;
			}

		}

		private void ExportButton_Click (object sender, EventArgs e) {
			if (saveFileDialog1.ShowDialog () == DialogResult.OK) {
				API.ExportCSV (ResultListView, MemberResultListView, saveFileDialog1.FileName);
			}
		}

		private void NameTextBox_KeyUp (object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				QueryButton.PerformClick ();
			}
		}
	}

}