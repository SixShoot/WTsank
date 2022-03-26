using Eruru.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class OujBoxClanQueryForm : Form {

		readonly OujBoxService OujBoxService = new OujBoxService ();
		readonly WarGamingNetService WarGamingNetService = new WarGamingNetService ();
		readonly ListViewComparer MemberListViewComparer;

		string[] Modes;
		ListViewItem.ListViewSubItem CurrentSubItem;

		public OujBoxClanQueryForm () {
			InitializeComponent ();
			MemberListViewComparer = new ListViewComparer (MemberResultListView);
			MemberResultListView.ListViewItemSorter = MemberListViewComparer;
			ModeComboBox.SelectedIndex = 0;
			NameTextBox.Text = Config.Instance.BoxClanQueryName;
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			Query (NameTextBox.Text, false);
		}

		private void AttendanceButton_Click (object sender, EventArgs e) {
			if (MessageBox.Show ("使用此功能会发送大量请求，由于限制了频率，处理时间会很久，需要耐心等待，是否继续？", "注意", MessageBoxButtons.YesNo) != DialogResult.Yes) {
				return;
			}
			if (API.CheckDateTime (StartDateTimePicker.Value, EndDateTimePicker.Value)) {
				Query (NameTextBox.Text, true);
			}
		}
		private void MemberResultListView_MouseClick (object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				CurrentSubItem = MemberResultListView.HitTest (e.X, e.Y).SubItem;
				if (CurrentSubItem == null) {
					return;
				}
				contextMenuStrip1.Show (MemberResultListView, e.Location);
			}
		}

		private void CopyToolStripMenuItem_Click (object sender, EventArgs e) {
			Clipboard.SetText (CurrentSubItem.Text);
		}

		void SetState (string text) {
			Invoke (new Action (() => StateLabel.Text = text));
		}

		void Query (string name, bool isAttendance) {
			NameTextBox.Enabled = false;
			StartDateTimePicker.Enabled = false;
			EndDateTimePicker.Enabled = false;
			QueryButton.Enabled = false;
			AttendanceButton.Enabled = false;
			ExportButton.Enabled = false;
			ModeComboBox.Enabled = false;
			Modes = new string[] { ModeComboBox.Text };
			ResultListView.Items.Clear ();
			MemberResultListView.Items.Clear ();
			Config.Instance.BoxClanQueryName = NameTextBox.Text;
			ConfigDao.Instance.Save (Config.Instance);
			int count = 0;
			object summaryLock = new object ();
			AutoResetEvent autoResetEvent = new AutoResetEvent (false);
			new Thread (() => {
				try {
					int id = WarGamingNetService.QueryClanID (name);
					List<string> names = WarGamingNetService.GetClanMemberNames (id);
					List<float> combats = new List<float> ();
					float totalCombat = 0;
					List<ClanMember> clanMembers = new List<ClanMember> ();
					if (isAttendance) {
						count = names.Count;
					}
					SetState ($"进度：{clanMembers.Count}/{names.Count}");
					foreach (string memberName in names) {
						ClanMember clanMember = new ClanMember (memberName, id);
						try {
							clanMember.Combat = OujBoxService.GetCombat (memberName);
							clanMember.CombatText = $"{clanMember.Combat:F2}";
							combats.Add (clanMember.Combat);
							if (isAttendance) {
								ThreadPool.QueueUserWorkItem (state => {
									ClanMember innerClanMember = (ClanMember)state;
									try {
										innerClanMember.Attendance = QueryAttendance (innerClanMember);
										innerClanMember.AttendanceText = innerClanMember.Attendance.ToString ();
									} catch (Exception e) {
										innerClanMember.AttendanceText = e.ToString ();
									} finally {
										lock (summaryLock) {
											count--;
											SetState ($"进度：{names.Count - count}/{names.Count}");
											if (count <= 0) {
												autoResetEvent.Set ();
											}
										}
									}
								}, clanMember);
							}
							totalCombat += clanMember.Combat;
						} catch (Exception e) {
							clanMember.CombatText = e.Message;
							if (isAttendance) {
								count--;
							}
						}
						clanMembers.Add (clanMember);
						if (!isAttendance) {
							SetState ($"进度：{clanMembers.Count}/{names.Count}");
						}
					}
					if (count <= 0) {
						autoResetEvent.Set ();
					}
					autoResetEvent.WaitOne ();
					float averageCombat = totalCombat / clanMembers.Count;
					combats.Sort ();
					float medianCombat = API.GetMedian (combats);
					Invoke (new Action (() => {
						ResultListView.BeginUpdate ();
						ResultListView.Items.Add ("军团").SubItems.Add ($"{name}");
						if (isAttendance) {
							ResultListView.Items.Add ($"查询范围：从").SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
							ResultListView.Items.Add ($"至").SubItems.Add ($"{EndDateTimePicker.Value:yyyy年MM月dd日}");
						} else {
							ResultListView.Items.Add ($"查询日期").SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
						}
						ResultListView.Items.Add ("成员数").SubItems.Add ($"{names.Count}");
						ResultListView.Items.Add ("平均效率").SubItems.Add ($"{averageCombat:F2}");
						ResultListView.Items.Add ("效率中位数").SubItems.Add ($"{medianCombat:F2}");
						API.AutoResizeListViewColumns (ResultListView);
						ResultListView.EndUpdate ();
						MemberResultListView.BeginUpdate ();
						foreach (ClanMember clanMember in clanMembers) {
							ListViewItem listViewItem = new ListViewItem (clanMember.Name);
							listViewItem.SubItems.Add (clanMember.CombatText);
							listViewItem.SubItems.Add ($"{clanMember.AttendanceText}");
							MemberResultListView.Items.Add (listViewItem);
						}
						API.AutoResizeListViewColumns (MemberResultListView);
						SortMemberResultListViewColumn (CombatColumnHeader.Index, SortOrder.Descending);
						MemberResultListView.EndUpdate ();
					}));
				} catch (Exception e) {
					Invoke (new Action (() => {
						MessageBox.Show (this, e.ToString ());
					}));
				} finally {
					Invoke (new Action (() => {
						NameTextBox.Enabled = true;
						StartDateTimePicker.Enabled = true;
						EndDateTimePicker.Enabled = true;
						QueryButton.Enabled = true;
						AttendanceButton.Enabled = true;
						ExportButton.Enabled = true;
						StateLabel.Text = string.Empty;
						ModeComboBox.Enabled = true;
					}));
				}
			}) {
				IsBackground = true
			}.Start ();
		}

		int QueryAttendance (ClanMember clanMember) {
			List<CombatRecord> combatRecords = OujBoxService.GetCombatRecords (clanMember.Name, StartDateTimePicker.Value, EndDateTimePicker.Value, false);
			List<CombatRecord> filteredCombatRecords = new List<CombatRecord> ();
			DateTime dateTime = DateTime.MinValue;
			List<int> days = new List<int> ();
			int count = 0;
			int dayIndex = 0;
			object summaryLock = new object ();
			Exception exception = null;
			AutoResetEvent autoResetEvent = new AutoResetEvent (false);
			foreach (CombatRecord combatRecord in combatRecords) {
				if (!Modes.Contains (combatRecord.Mode)) {
					continue;
				}
				if (dateTime == combatRecord.DateTime) {
					if (days[dayIndex] > 0) {
						continue;
					}
				} else {
					dateTime = combatRecord.DateTime;
					dayIndex = days.Count;
					days.Add (0);
					//combatRecord.Tag = days.Count - 1;
				}
				OujBoxService.FillCombatRecord (combatRecord);
				int clanMemberNumber = 0;
				foreach (JsonValue playerJsonObject in combatRecord.TeamA) {
					if (playerJsonObject["clanDBID"] == clanMember.ClanID) {
						clanMemberNumber++;
					}
				}
				if (clanMemberNumber >= 5) {
					days[days.Count - 1]++;
					filteredCombatRecords.Add (combatRecord);
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

		private void EndDateTimePicker_ValueChanged (object sender, EventArgs e) {
			if (StartDateTimePicker.Value > EndDateTimePicker.Value) {
				StartDateTimePicker.Value = EndDateTimePicker.Value;
			}
		}

		private void MemberResultListView_ColumnClick (object sender, ColumnClickEventArgs e) {
			if (MemberListViewComparer.SelectedColumnIndex == e.Column) {
				MemberListViewComparer.ToggleSortOrder ();
			} else {
				MemberListViewComparer.ListView.Sorting = SortOrder.Descending;
			}
			SortMemberResultListViewColumn (e.Column, MemberListViewComparer.ListView.Sorting);
		}

		void SortMemberResultListViewColumn (int column, SortOrder sortOrder) {
			MemberListViewComparer.SelectedColumnIndex = column;
			MemberListViewComparer.ListView.Sorting = sortOrder;
			if (column == NameColumnHeader.Index) {
				MemberListViewComparer.SelectedColumnType = typeof (string);
			} else {
				MemberListViewComparer.SelectedColumnType = typeof (float);
			}
			MemberListViewComparer.ListView.Sort ();
		}

	}

}