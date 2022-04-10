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

		public BoxCombatAnalysisForm () {
			InitializeComponent ();
			TeamAListView.ListViewItemSorter = new ListViewComparer (TeamAListView);
			TeamBListView.ListViewItemSorter = new ListViewComparer (TeamBListView);
			NameTextBox.Text = Config.Instance.BoxCombatAnalysisPlayerName;
			CombatListView.HideSelection = false;
			TeamAListView.HideSelection = true;
			TeamBListView.Columns.Clear ();
			foreach (ColumnHeader column in TeamAListView.Columns) {
				TeamBListView.Columns.Add (column.Text, column.Width);
			}
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
				OnSelectedCombatRecord (null);
				return;
			}
			OnSelectedCombatRecord ((CombatRecord)CombatListView.SelectedItems[0].Tag);
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
			if (listViewComparer.SelectedColumnIndex == e.Column) {
				listViewComparer.ToggleSortOrder ();
			} else {
				listViewComparer.ListView.Sorting = SortOrder.Descending;
			}
			SortListViewColumn (listViewComparer, e.Column, listViewComparer.ListView.Sorting);
		}

		private void BoxCombatAnalysisForm_Resize (object sender, EventArgs e) {
			int height = (int)(CombatListView.Height / 2F - 8 - TeamAInformationLabel.Height);
			TeamAListView.Height = height;
			TeamBListView.Height = height;
			TeamAInformationLabel.Top = TeamAListView.Bottom + 5;
			TeamBListView.Top = TeamAInformationLabel.Bottom + 5;
			TeamBInformationLabel.Top = TeamBListView.Bottom + 5;
		}

		void SortListViewColumn (ListViewComparer listViewComparer, int column, SortOrder sortOrder) {
			listViewComparer.SelectedColumnIndex = column;
			listViewComparer.ListView.Sorting = sortOrder;
			if (column == NameColumnHeader.Index || column == TankColumnHeader.Index) {
				listViewComparer.SelectedColumnType = typeof (string);
			} else {
				listViewComparer.SelectedColumnType = typeof (float);
			}
			listViewComparer.ListView.Sort ();
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
			new Thread (() => {
				try {
					CombatRecordPlayer player = BoxService.Instance.CreatePlayer (name);
					List<CombatRecord> combatRecords = BoxService.Instance.GetCombatRecords (player, StartDateTimePicker.Value, EndDateTimePicker.Value, isSameDay);
					Invoke (new Action (() => {
						if (combatRecords.Count == 0) {
							MessageBox.Show (this, "没有战斗数据");
							return;
						}
						CombatListView.BeginUpdate ();
						foreach (CombatRecord combatRecord in combatRecords) {
							ListViewItem listViewItem = new ListViewItem (API.CombatResultToString (combatRecord.Result)) {
								Tag = combatRecord
							};
							listViewItem.SubItems.Insert (ModeColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = combatRecord.Mode });
							listViewItem.SubItems.Insert (DateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{combatRecord.DateTime:yyyy年MM月dd日}" });
							CombatListView.Items.Add (listViewItem);
						}
						API.AutoResizeListViewColumns (CombatListView);
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
					}));
				}
			}) {
				IsBackground = true
			}.Start ();
		}

		void OnSelectedCombatRecord (CombatRecord combatRecord) {
			TeamAInformationLabel.Text = string.Empty;
			TeamBInformationLabel.Text = string.Empty;
			if (combatRecord == null) {
				TeamAListView.Items.Clear ();
				TeamBListView.Items.Clear ();
				return;
			}
			if (combatRecord.Tag == null) {
				CombatListView.Enabled = false;
				new Thread (() => {
					BoxService.Instance.FillCombatRecord (combatRecord);
					combatRecord.Tag = combatRecord;
					AutoResetEvent autoResetEvent = new AutoResetEvent (false);
					int count = 2;
					object summaryLock = new object ();
					new Thread (() => {
						try {
							RefreshTeamListView (TeamAListView, TeamAInformationLabel, combatRecord.TeamAPlayers);
						} finally {
							Done ();
						}
					}) {
						IsBackground = true
					}.Start ();
					new Thread (() => {
						try {
							RefreshTeamListView (TeamBListView, TeamBInformationLabel, combatRecord.TeamBPlayers);
						} finally {
							Done ();
						}
					}) {
						IsBackground = true
					}.Start ();
					void Done () {
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
				}) {
					IsBackground = true
				}.Start ();
				return;
			}
			RefreshTeamListView (TeamAListView, TeamAInformationLabel, combatRecord.TeamAPlayers);
			RefreshTeamListView (TeamBListView, TeamBInformationLabel, combatRecord.TeamBPlayers);
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
				if (CachedPlayers.TryGetValue (playerJsonValue, out CachedPlayer cachedPlayer)) {
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
							BoxService.Instance.FillCombatRecordTeamPlayer (cachedPlayer.TeamPlayer, playerJsonValue);
							try {
								cachedPlayer.Player = BoxService.Instance.CreatePlayer (cachedPlayer.TeamPlayer.Name);
								cachedPlayer.Player.CombatText = cachedPlayer.Player.Combat.ToString ();
								Append ();
							} catch (Exception exception) {
								cachedPlayer.Player = new CombatRecordPlayer () {
									Name = cachedPlayer.TeamPlayer.Name,
									CombatText = exception.Message
								};
								cachedPlayer.Player.Exception = exception;
							}
							CachedPlayers[playerJsonValue] = cachedPlayer;
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
					if (cachedPlayer.Player.Combat > 0) {
						effectiveCount++;
						totalBoxCombat += cachedPlayer.Player.Combat;
						totalBoxWinRate += cachedPlayer.Player.WinRate;
						totalBoxDamage += cachedPlayer.Player.AverageDamage;
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
					listViewItem.SubItems.Insert (BoxCombatColumnHeader.Index, new ListViewItem.ListViewSubItem () {
						Text = $"{cachedPlayer.Player.CombatText}",
						Tag = API.GetCombatColor (cachedPlayer.Player.Combat)
					});
					listViewItem.SubItems.Insert (BoxWinRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.Player.WinRate:P0}" });
					listViewItem.SubItems.Insert (BoxDamageColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.Player.AverageDamage}" });
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
				SortListViewColumn ((ListViewComparer)listView.ListViewItemSorter, CombatColumnHeader.Index, SortOrder.Descending);
				listView.EndUpdate ();
				label.Text = string.Format (
					$"平均千场效率：{API.Divide (totalBoxCombat, effectiveCount):F2} " +
					$"平均千场胜率：{API.Divide (totalBoxWinRate, effectiveCount):P2} " +
					$"平均千场均伤：{API.Divide (totalBoxDamage, effectiveCount):F2}"
				);
			}));
		}

		class CachedPlayer {

			public CombatRecordPlayer Player;
			public CombatRecordTeamPlayer TeamPlayer;

		}

	}

}