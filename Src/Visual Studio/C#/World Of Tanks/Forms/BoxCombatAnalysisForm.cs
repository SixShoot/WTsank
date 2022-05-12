using Eruru.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class BoxCombatAnalysisForm : Form {

		readonly Dictionary<JsonValue, CachedPlayer> CachedPlayers = new Dictionary<JsonValue, CachedPlayer> ();

		ListViewItem.ListViewSubItem CurrentSubItem;
		BoxCombatRecord CurrentCombatRecord;

		public BoxCombatAnalysisForm () {
			InitializeComponent ();
			TeamAListView.ListViewItemSorter = new ListViewComparer (TeamAListView) {
				OnGetColumnType = column => {
					if (column == NameColumnHeader.Index || column == TankColumnHeader.Index) {
						return typeof (string);
					}
					return typeof (float);
				}
			};
			TeamBListView.ListViewItemSorter = new ListViewComparer (TeamBListView) {
				OnGetColumnType = ((ListViewComparer)TeamAListView.ListViewItemSorter).OnGetColumnType
			};
			NameTextBox.Text = Config.Instance.BoxCombatAnalysisPlayerName;
			TeamBListView.Columns.Clear ();
			foreach (ColumnHeader column in TeamAListView.Columns) {
				TeamBListView.Columns.Add (column.Text, column.Width);
			}
			AutoResizeResultListViewColumns ();
			API.AutoResizeListViewColumns (TeamAListView);
			API.AutoResizeListViewColumns (TeamBListView);
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			if (API.CheckDateTime (StartDateTimePicker.Value, EndDateTimePicker.Value)) {
				Query (NameTextBox.Text, StartDateTimePicker.Value.Date == EndDateTimePicker.Value.Date);
			}
		}

		private void ListView_MouseClick (object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				ListView listView = (ListView)sender;
				CurrentSubItem = listView.HitTest (e.X, e.Y).SubItem;
				if (CurrentSubItem == null) {
					return;
				}
				contextMenuStrip1.Show (listView, e.Location);
			}
		}

		private void CopyToolStripMenuItem_Click (object sender, EventArgs e) {
			Clipboard.SetText (CurrentSubItem.Text);
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

		private void CombatListView_SelectedIndexChanged (object sender, EventArgs e) {
			if (CombatListView.SelectedItems.Count == 0) {
				return;
			}
			OnSelectedCombatRecord ((BoxCombatRecord)CombatListView.SelectedItems[0].Tag);
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

		private void ListView_ColumnClick (object sender, ColumnClickEventArgs e) {
			ListView listView = (ListView)sender;
			ListViewComparer listViewComparer = (ListViewComparer)listView.ListViewItemSorter;
			listViewComparer.OnClickColumn (e.Column);
		}

		private void BoxCombatAnalysisForm_Resize (object sender, EventArgs e) {
			int height = (int)((CombatListView.Height - TeamAInformationLabel.Height) / 2F - 5);
			TeamAListView.Height = height;
			TeamBListView.Height = height;
			TeamAInformationLabel.Top = TeamAListView.Bottom + 5;
			TeamBListView.Top = TeamAInformationLabel.Bottom + 5;
			TeamBInformationLabel.Top = TeamBListView.Bottom + 5;
			StateLabel.Top = TeamBInformationLabel.Top;
		}

		void AutoResizeResultListViewColumns () {
			API.AutoResizeListViewColumns (CombatListView, true);
			TeamAListView.Left = CombatListView.Right + 5;
			TeamAListView.Width = QueryButton.Right - TeamAListView.Left;
			TeamAInformationLabel.Left = TeamAListView.Left;
			TeamAInformationLabel.Width = TeamAListView.Width;
			TeamBListView.Left = TeamAListView.Left;
			TeamBListView.Width = TeamAListView.Width;
			TeamBInformationLabel.Left = TeamAListView.Left;
			TeamBInformationLabel.Width = TeamAListView.Width;
		}

		void SetState (string text) {
			Invoke (new Action (() => StateLabel.Text = text));
		}

		void Query (string name, bool isSameDay = true) {
			NameTextBox.Enabled = false;
			StartDateTimePicker.Enabled = false;
			EndDateTimePicker.Enabled = false;
			QueryButton.Enabled = false;
			CombatListView.Items.Clear ();
			TeamAListView.Items.Clear ();
			TeamBListView.Items.Clear ();
			TeamAInformationLabel.Text = string.Empty;
			TeamBInformationLabel.Text = string.Empty;
			Config.Instance.BoxCombatAnalysisPlayerName = NameTextBox.Text;
			ConfigDao.Instance.Save (Config.Instance);
			AutoResetEvent autoResetEvent = new AutoResetEvent (false);
			int count = 0;
			object summaryLock = new object ();
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
							SetState ($"{API.Divide ((DateTime.Now.Date - dateTime).TotalDays, dayDifference):P0} 页：{page} 时间：{dateTime:yyyy年MM月dd日}");
						}
					);
					count = combatRecords.Count;
					SetState ($"进度：0% 0/{combatRecords.Count}");
					for (int i = 0; i < combatRecords.Count; i++) {
						ThreadPool.QueueUserWorkItem (state => {
							BoxCombatRecord innerCombatRecord = (BoxCombatRecord)state;
							BoxService.Instance.FillCombatRecord (innerCombatRecord);
							innerCombatRecord.Tag = innerCombatRecord;
							lock (summaryLock) {
								count--;
								SetState ($"进度：{API.Divide (combatRecords.Count - count, combatRecords.Count):P0} {combatRecords.Count - count}/{combatRecords.Count}");
								if (count <= 0) {
									autoResetEvent.Set ();
								}
							}
						}, combatRecords[i]);
					}
					if (count <= 0) {
						autoResetEvent.Set ();
					}
					autoResetEvent.WaitOne ();
					SetState ("显示结果中");
					Invoke (new Action (() => {
						if (combatRecords.Count == 0) {
							MessageBox.Show (this, "没有战斗数据");
							return;
						}
						CombatListView.BeginUpdate ();
						foreach (BoxCombatRecord combatRecord in combatRecords) {
							ListViewItem listViewItem = new ListViewItem (API.CombatResultToString (combatRecord.Result)) {
								Tag = combatRecord
							};
							listViewItem.SubItems.Insert (ModeColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = combatRecord.Mode });
							listViewItem.SubItems.Insert (DateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{combatRecord.DateTime:yyyy年MM月dd日 HH时mm分}" });
							listViewItem.SubItems.Insert (CombatListCombatColumnHeader.Index, new ListViewItem.ListViewSubItem () {
								Text = $"{combatRecord.TeamPlayer.Combat:F2}",
								Tag = API.GetCombatColor (combatRecord.TeamPlayer.Combat)
							});
							CombatListView.Items.Add (listViewItem);
						}
						AutoResizeResultListViewColumns ();
						CombatListView.EndUpdate ();
						if (CombatListView.Items.Count > 0) {
							CombatListView.Items[0].Selected = true;
						}
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
						StateLabel.Text = string.Empty;
					}));
				}
			}) {
				IsBackground = true
			}.Start ();
		}

		void OnSelectedCombatRecord (BoxCombatRecord combatRecord) {
			if (combatRecord == CurrentCombatRecord) {
				return;
			}
			CurrentCombatRecord = combatRecord;
			TeamAInformationLabel.Text = string.Empty;
			TeamBInformationLabel.Text = string.Empty;
			if (combatRecord == null) {
				TeamAListView.Items.Clear ();
				TeamBListView.Items.Clear ();
				return;
			}
			CombatListView.Enabled = false;
			ThreadPool.QueueUserWorkItem (state => {
				if (combatRecord.Tag == null) {
					BoxService.Instance.FillCombatRecord (combatRecord);
					combatRecord.Tag = combatRecord;
				}
				AutoResetEvent autoResetEvent = new AutoResetEvent (false);
				int count = 2;
				object summaryLock = new object ();
				Execute (TeamAListView, TeamAInformationLabel, combatRecord.TeamAPlayers);
				Execute (TeamBListView, TeamBInformationLabel, combatRecord.TeamBPlayers);
				void Execute (ListView listView, Label label, JsonArray players) {
					ThreadPool.QueueUserWorkItem (innerState => {
						try {
							RefreshTeamListView (listView, label, players);
						} finally {
							OnDone ();
						}
					});
				}
				void OnDone () {
					lock (summaryLock) {
						count--;
						if (count <= 0) {
							autoResetEvent.Set ();
						}
					}
				}
				autoResetEvent.WaitOne ();
				Invoke (new Action (() => {
					CombatListView.Enabled = true;
				}));
			});
		}

		void RefreshTeamListView (ListView listView, Label label, JsonArray players) {
			Invoke (new Action (() => {
				listView.Items.Clear ();
				listView.BeginUpdate ();
			}));
			int count = 0;
			object summaryLock = new object ();
			AutoResetEvent autoResetEvent = new AutoResetEvent (false);
			int effectiveCount = 0;
			float totalBoxCombat = 0;
			float totalBoxWinRate = 0;
			float totalBoxDamage = 0;
			foreach (JsonValue playerJsonValue in players) {
				if (CachedPlayers.TryGetValue (playerJsonValue, out CachedPlayer cachedPlayer) && cachedPlayer.BoxPersonalCombatRecord.Exception == null) {
					Append ();
				} else {
					lock (summaryLock) {
						count++;
					}
					ThreadPool.QueueUserWorkItem (state => {
						try {
							cachedPlayer = new CachedPlayer {
								TeamPlayer = new CombatRecordTeamPlayer ()
							};
							CachedPlayers[playerJsonValue] = cachedPlayer;
							BoxService.Instance.FillCombatRecordTeamPlayer (cachedPlayer.TeamPlayer, playerJsonValue);
							try {
								cachedPlayer.BoxPersonalCombatRecord = BoxService.Instance.GetPersonalCombatRecord (cachedPlayer.TeamPlayer.Name);
								Append ();
								cachedPlayer.BoxPersonalCombatRecord.Exception = null;
							} catch (Exception exception) {
								cachedPlayer.BoxPersonalCombatRecord = new BoxPersonalCombatRecord () {
									Name = cachedPlayer.TeamPlayer.Name,
									CombatText = exception.Message
								};
								cachedPlayer.BoxPersonalCombatRecord.Exception = exception;
							}
						} finally {
							lock (summaryLock) {
								count--;
								if (count <= 0) {
									autoResetEvent.Set ();
								}
							}
						}
					});
				}
				void Append () {
					if (cachedPlayer.BoxPersonalCombatRecord.Combat > 0) {
						effectiveCount++;
						totalBoxCombat += cachedPlayer.BoxPersonalCombatRecord.Combat;
						totalBoxWinRate += cachedPlayer.BoxPersonalCombatRecord.WinRate;
						totalBoxDamage += cachedPlayer.BoxPersonalCombatRecord.Damage;
					}
				}
			}
			if (count <= 0) {
				autoResetEvent.Set ();
			}
			autoResetEvent.WaitOne ();
			Invoke (new Action (() => {
				foreach (JsonValue playerJsonValue in players) {
					CachedPlayer cachedPlayer = CachedPlayers[playerJsonValue];
					float hitRate = API.Divide (cachedPlayer.TeamPlayer.HitCount, cachedPlayer.TeamPlayer.ShootCount);
					float penetrationRate = API.Divide (cachedPlayer.TeamPlayer.PenetrationCount, cachedPlayer.TeamPlayer.HitCount);
					float penetrationRateIncludeNoHit = API.Divide (cachedPlayer.TeamPlayer.PenetrationCount, cachedPlayer.TeamPlayer.ShootCount);
					ListViewItem listViewItem = new ListViewItem (cachedPlayer.TeamPlayer.Name);
					listViewItem.SubItems.Insert (ClanColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = cachedPlayer.TeamPlayer.ClanAbbrev });
					listViewItem.SubItems.Insert (BoxCombatColumnHeader.Index, new ListViewItem.ListViewSubItem () {
						Text = $"{cachedPlayer.BoxPersonalCombatRecord.CombatText}",
						Tag = API.GetCombatColor (cachedPlayer.BoxPersonalCombatRecord.Combat)
					});
					listViewItem.SubItems.Insert (BoxWinRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.BoxPersonalCombatRecord.WinRate:P0}" });
					listViewItem.SubItems.Insert (BoxHitRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.BoxPersonalCombatRecord.HitRate:P0}" });
					listViewItem.SubItems.Insert (BoxCombatLevelColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.BoxPersonalCombatRecord.CombatLevel}" });
					listViewItem.SubItems.Insert (BoxDamageColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.BoxPersonalCombatRecord.Damage}" });
					listViewItem.SubItems.Insert (TankColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = cachedPlayer.TeamPlayer.TankName });
					listViewItem.SubItems.Insert (CombatColumnHeader.Index, new ListViewItem.ListViewSubItem () {
						Text = $"{cachedPlayer.TeamPlayer.Combat:F2}",
						Tag = API.GetCombatColor (cachedPlayer.TeamPlayer.Combat)
					});
					listViewItem.SubItems.Insert (DamageColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.TeamPlayer.Damage}" });
					listViewItem.SubItems.Insert (AssistColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.TeamPlayer.Assist}" });
					listViewItem.SubItems.Insert (ArmorResistanceColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.TeamPlayer.ArmorResistence}" });
					listViewItem.SubItems.Insert (HitRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{hitRate:P2}" });
					listViewItem.SubItems.Insert (PenetrationRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{penetrationRate:P2}" });
					listViewItem.SubItems.Insert (PenetrationRateIncludeNoHitColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{penetrationRateIncludeNoHit:P2}" });
					listViewItem.SubItems.Insert (ShootCountColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.TeamPlayer.ShootCount}" });
					listViewItem.SubItems.Insert (HitCountColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.TeamPlayer.HitCount}" });
					listViewItem.SubItems.Insert (PenetrationCountColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.TeamPlayer.PenetrationCount}" });
					listView.Items.Add (listViewItem);
				}
				API.AutoResizeListViewColumns (listView);
				((ListViewComparer)listView.ListViewItemSorter).SortColumn (CombatColumnHeader.Index, SortOrder.Descending);
				listView.EndUpdate ();
				label.Text = string.Format (
					$"平均千场效率：{API.Divide (totalBoxCombat, effectiveCount):F2} " +
					$"平均千场胜率：{API.Divide (totalBoxWinRate, effectiveCount):P2} " +
					$"平均千场均伤：{API.Divide (totalBoxDamage, effectiveCount):F2}"
				);
			}));
		}

		class CachedPlayer {

			public BoxPersonalCombatRecord BoxPersonalCombatRecord;
			public CombatRecordTeamPlayer TeamPlayer;

		}

	}

}