using Eruru.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class OujBoxClanQueryForm : Form {

		readonly ListViewComparer MemberListViewComparer;

		string[] Modes;
		ListViewItem.ListViewSubItem CurrentSubItem;

		public OujBoxClanQueryForm () {
			InitializeComponent ();
			MemberListViewComparer = new ListViewComparer (MemberResultListView) {
				OnGetColumnType = column => {
					if (column == NameColumnHeader.Index) {
						return typeof (string);
					}
					if (column == PositionColumnHeader.Index) {
						return typeof (ClanMemberPosition);
					}
					return typeof (float);
				},
				OnSorted = () => {
					int serialNumber = 0;
					foreach (ListViewItem item in MemberListViewComparer.ListView.Items) {
						serialNumber++;
						item.Text = serialNumber.ToString ();
					}
				}
			};
			MemberResultListView.ListViewItemSorter = MemberListViewComparer;
			ModeComboBox.SelectedIndex = 0;
			NameTextBox.Text = Config.Instance.BoxClanQueryName;
			AutoResizeResultListViewColumns ();
			API.AutoResizeListViewColumns (MemberResultListView);
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

		private void ExportButton_Click (object sender, EventArgs e) {
			if (saveFileDialog1.ShowDialog () == DialogResult.OK) {
				API.ExportCSV (ResultListView, MemberResultListView, saveFileDialog1.FileName);
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

		private void MemberResultListView_ColumnClick (object sender, ColumnClickEventArgs e) {
			MemberListViewComparer.OnClickColumn (e.Column);
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

		void AutoResizeResultListViewColumns () {
			API.AutoResizeListViewColumns (ResultListView, true);
			MemberResultListView.Left = ResultListView.Right + 5;
			MemberResultListView.Width = AttendanceButton.Right - MemberResultListView.Left;
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
					int clanID = WarGamingNetService.Instance.QueryClanID (name);
					List<float> combats = new List<float> ();
					List<float> winRates = new List<float> ();
					List<float> hitRates = new List<float> ();
					List<float> combatLevels = new List<float> ();
					List<float> damages = new List<float> ();
					float totalCombat = 0;
					float totalWinRate = 0;
					float totalHitRate = 0;
					float totalCombatLevel = 0;
					float totalDamage = 0;
					List<ClanMember> clanMembers = WarGamingNetService.Instance.GetClanMembers (clanID);
					count = clanMembers.Count;
					SetState ($"进度：0/{clanMembers.Count}");
					foreach (ClanMember clanMember in clanMembers) {
						ThreadPool.QueueUserWorkItem (state => {
							ClanMember innerClanMember = (ClanMember)state;
							try {
								try {
									innerClanMember.Player = BoxService.Instance.CreatePlayer (innerClanMember.Name);
									innerClanMember.Player.CombatText = innerClanMember.Player.Combat.ToString ();
								} catch {
									innerClanMember.Player = new CombatRecordPlayer () {
										Name = innerClanMember.Name
									};
									throw;
								}
								combats.Add (innerClanMember.Player.Combat);
								winRates.Add (innerClanMember.Player.WinRate);
								hitRates.Add (innerClanMember.Player.HitRate);
								combatLevels.Add (innerClanMember.Player.AverageCombatLevel);
								damages.Add (innerClanMember.Player.AverageDamage);
								totalCombat += innerClanMember.Player.Combat;
								totalWinRate += innerClanMember.Player.WinRate;
								totalHitRate += innerClanMember.Player.HitRate;
								totalCombatLevel += innerClanMember.Player.AverageCombatLevel;
								totalDamage += innerClanMember.Player.AverageDamage;
								if (isAttendance) {
									try {
										innerClanMember.AttendanceDays = QueryAttendance (innerClanMember, out int attendanceCount);
										innerClanMember.AttendanceDaysText = innerClanMember.AttendanceDays.ToString ();
										innerClanMember.AttendanceCount = attendanceCount;
										innerClanMember.AttendanceCountText = innerClanMember.AttendanceCount.ToString ();
									} catch (Exception e) {
										innerClanMember.AttendanceDaysText = e.ToString ();
										innerClanMember.AttendanceDaysText = "0";
									}
								}
							} catch (Exception combatException) {
								innerClanMember.Player.CombatText = combatException.Message;
							} finally {
								lock (summaryLock) {
									count--;
									SetState ($"进度：{clanMembers.Count - count}/{clanMembers.Count}");
									if (count <= 0) {
										autoResetEvent.Set ();
									}
								}
							}
						}, clanMember);
					}
					if (count <= 0) {
						autoResetEvent.Set ();
					}
					autoResetEvent.WaitOne ();
					float averageCombat = API.Divide (totalCombat, combats.Count);
					float averageWinRate = API.Divide (totalWinRate, combats.Count);
					float averageHitRate = API.Divide (totalHitRate, combats.Count);
					float averageCombatLevel = API.Divide (totalCombatLevel, combats.Count);
					float averageDamage = API.Divide (totalDamage, combats.Count);
					combats.Sort ();
					float medianCombat = API.GetMedian (combats);
					winRates.Sort ();
					float medianWinRate = API.GetMedian (winRates);
					hitRates.Sort ();
					float medianHitRate = API.GetMedian (hitRates);
					combatLevels.Sort ();
					float medianCombatLevel = API.GetMedian (combatLevels);
					damages.Sort ();
					float medianDamage = API.GetMedian (damages);
					Invoke (new Action (() => {
						ResultListView.BeginUpdate ();
						ResultListView.Items.Add ("军团").SubItems.Add ($"{name}");
						if (!isAttendance || StartDateTimePicker.Value.Date == EndDateTimePicker.Value.Date) {
							ResultListView.Items.Add ($"查询日期").SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
						} else {
							ResultListView.Items.Add ($"查询范围：从").SubItems.Add ($"{StartDateTimePicker.Value:yyyy年MM月dd日}");
							ResultListView.Items.Add ($"至").SubItems.Add ($"{EndDateTimePicker.Value:yyyy年MM月dd日}");
						}
						ResultListView.Items.Add ("成员数").SubItems.Add ($"{clanMembers.Count}");
						ResultListView.Items.Add ("平均效率").SubItems.Add (new ListViewItem.ListViewSubItem () {
							Text = $"{averageCombat:F2}",
							Tag = API.GetCombatColor (averageCombat)
						});
						ResultListView.Items.Add ("效率中位数").SubItems.Add (new ListViewItem.ListViewSubItem () {
							Text = $"{medianCombat:F2}",
							Tag = API.GetCombatColor (medianCombat)
						});
						ResultListView.Items.Add ("平均胜率").SubItems.Add (new ListViewItem.ListViewSubItem () { Text = $"{averageWinRate:P2}" });
						ResultListView.Items.Add ("胜率中位数").SubItems.Add (new ListViewItem.ListViewSubItem () { Text = $"{medianWinRate:P2}" });
						ResultListView.Items.Add ("平均命中率").SubItems.Add (new ListViewItem.ListViewSubItem () { Text = $"{averageHitRate:P2}" });
						ResultListView.Items.Add ("命中率中位数").SubItems.Add (new ListViewItem.ListViewSubItem () { Text = $"{medianHitRate:P2}" });
						ResultListView.Items.Add ("平均出战等级").SubItems.Add (new ListViewItem.ListViewSubItem () { Text = $"{averageCombatLevel:F2}" });
						ResultListView.Items.Add ("出战等级中位数").SubItems.Add (new ListViewItem.ListViewSubItem () { Text = $"{medianCombatLevel:F2}" });
						ResultListView.Items.Add ("平均均伤").SubItems.Add (new ListViewItem.ListViewSubItem () { Text = $"{averageDamage:F2}" });
						ResultListView.Items.Add ("均伤中位数").SubItems.Add (new ListViewItem.ListViewSubItem () { Text = $"{medianDamage:F2}" });
						AutoResizeResultListViewColumns ();
						ResultListView.EndUpdate ();
						MemberResultListView.BeginUpdate ();
						int serialNumber = 0;
						foreach (ClanMember clanMember in clanMembers) {
							serialNumber++;
							ListViewItem listViewItem = new ListViewItem (serialNumber.ToString ()) {
								Tag = clanMember
							};
							listViewItem.SubItems.Insert (NameColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = clanMember.Name });
							listViewItem.SubItems.Insert (PositionColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = clanMember.Position });
							listViewItem.SubItems.Insert (CombatColumnHeader.Index, new ListViewItem.ListViewSubItem () {
								Text = clanMember.Player.CombatText,
								Tag = API.GetCombatColor (clanMember.Player.Combat)
							});
							listViewItem.SubItems.Insert (WinRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{clanMember.Player.WinRate:P0}" });
							listViewItem.SubItems.Insert (HitRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{clanMember.Player.HitRate:P0}" });
							listViewItem.SubItems.Insert (AverageCombatLevelColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{clanMember.Player.AverageCombatLevel}" });
							listViewItem.SubItems.Insert (AverageDamageColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{clanMember.Player.AverageDamage}" });
							listViewItem.SubItems.Insert (AttendanceDaysColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = clanMember.AttendanceDaysText });
							listViewItem.SubItems.Insert (AttendanceCountColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = clanMember.AttendanceCountText });
							MemberResultListView.Items.Add (listViewItem);
						}
						API.AutoResizeListViewColumns (MemberResultListView);
						MemberListViewComparer.SortColumn (CombatColumnHeader.Index, SortOrder.Descending);
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

		int QueryAttendance (ClanMember clanMember, out int count) {
			List<CombatRecord> combatRecords = BoxService.Instance.GetCombatRecords (clanMember.Player, StartDateTimePicker.Value, EndDateTimePicker.Value, false);
			DateTime dateTime = DateTime.MinValue;
			List<int> days = new List<int> ();
			int dayIndex = 0;
			foreach (CombatRecord combatRecord in combatRecords) {
				if (!Modes.Contains (combatRecord.Mode)) {
					continue;
				}
				if (dateTime.Date == combatRecord.DateTime.Date) {
					if (days[dayIndex] > 0) {
						//continue;
					}
				} else {
					dateTime = combatRecord.DateTime;
					dayIndex = days.Count;
					days.Add (0);
				}
				BoxService.Instance.FillCombatRecord (combatRecord);
				int clanMemberNumber = 0;
				foreach (JsonValue playerJsonObject in combatRecord.PlayerTeamPlayers) {
					if (playerJsonObject["clanDBID"] == clanMember.ClanID) {
						clanMemberNumber++;
					}
				}
				if (clanMemberNumber >= 5) {
					days[days.Count - 1]++;
				}
			}
			count = days.Sum ();
			return days.Sum (value => value > 0 ? 1 : 0);
		}

	}

}