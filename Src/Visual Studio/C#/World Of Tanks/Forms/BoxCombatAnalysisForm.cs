using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Eruru.Json;

namespace WorldOfTanks {

	public partial class BoxCombatAnalysisForm : Form, IFormPage {

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
			TeamBListView.Columns.Clear ();
			foreach (ColumnHeader column in TeamAListView.Columns) {
				TeamBListView.Columns.Add (column.Text, column.Width);
			}
			AutoResizeResultListViewColumns ();
			Api.AutoResizeListViewColumns (TeamAListView);
			Api.AutoResizeListViewColumns (TeamBListView);
		}

		public void OnShow () {
			LoadQueryNames ();
		}

		private void QueryButton_Click (object sender, EventArgs e) {
			if (Api.CheckDateTime (StartDateTimePicker.Value, EndDateTimePicker.Value)) {
				Query (NameComboBox.Text, StartDateTimePicker.Value.Date == EndDateTimePicker.Value.Date);
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

		}

		private void EndDateTimePicker_ValueChanged (object sender, EventArgs e) {

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
			StateLabel.Width = CombatListView.Width;
		}

		void AutoResizeResultListViewColumns () {
			Api.AutoResizeListViewColumns (CombatListView, true);
			StateLabel.Width = CombatListView.Width;
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
			Api.Invoke (this, () => {
				StateLabel.Text = text;
			});
		}
		void SetState (string format, params object[] args) {
			SetState (string.Format (format, args));
		}

		void SetEnabled (bool enabled) {
			Api.Invoke (this, () => {
				NameComboBox.Enabled = enabled;
				StartDateTimePicker.Enabled = enabled;
				EndDateTimePicker.Enabled = enabled;
				QueryButton.Enabled = enabled;
				CombatListView.Enabled = enabled;
				TeamAListView.Enabled = enabled;
				TeamBListView.Enabled = enabled;
			});
		}

		void Query (string name, bool isSameDay = true) {
			SetEnabled (false);
			CombatListView.Items.Clear ();
			TeamAListView.Items.Clear ();
			TeamBListView.Items.Clear ();
			TeamAInformationLabel.Text = string.Empty;
			TeamBInformationLabel.Text = string.Empty;
			ConfigService.Instance.AddBoxCombatQueryPlayerName (name);
			ConfigDao.Instance.Save (ConfigService.Instance);
			LoadQueryNames ();
			DateTime queryDateTime = DateTime.Now;
			ThreadPool.QueueUserWorkItem (state => {
				try {
					BoxPersonalCombatRecord player = BoxService.Instance.GetPersonalCombatRecord (name);
					double dayDifference = (DateTime.Now - StartDateTimePicker.Value).TotalDays;
					List<BoxCombatRecord> combatRecords = BoxService.Instance.GetCombatRecords (
						player,
						StartDateTimePicker.Value,
						EndDateTimePicker.Value,
						isSameDay,
						(page, dateTime) => {
							double dateTimeProgress = Api.Divide ((DateTime.Now.Date - dateTime).TotalDays, dayDifference);
							double pageProgress = Api.Divide (page, 215);
							SetState ($"进度：{Math.Max (dateTimeProgress, pageProgress):P0} 页：{page} 时间：{dateTime.ToString (Api.DateFormatText)}");
						}
					);
					if (combatRecords.Count > 0) {
						AutoResetEvent autoResetEvent = new AutoResetEvent (false);
						int count = 0;
						SetState ($"进度：0% 0/{combatRecords.Count}");
						for (int i = 0; i < combatRecords.Count; i++) {
							ThreadPool.QueueUserWorkItem (innerState => {
								try {
									BoxCombatRecord innerCombatRecord = (BoxCombatRecord)innerState;
									BoxService.Instance.FillCombatRecord (innerCombatRecord);
									innerCombatRecord.Tag = innerCombatRecord;
								} finally {
									lock (autoResetEvent) {
										count++;
										if (count >= combatRecords.Count) {
											autoResetEvent.Set ();
										}
										SetState ($"进度：{Api.Divide (count, combatRecords.Count):P0} {count}/{combatRecords.Count}");
									}
								}
							}, combatRecords[i]);
						}
						autoResetEvent.WaitOne ();
					}
					Api.Invoke (this, () => {
						SetState ("显示结果中");
						if (combatRecords.Count == 0) {
							MessageBox.Show (this, "没有战斗数据");
							return;
						}
						CombatListView.BeginUpdate ();
						foreach (BoxCombatRecord combatRecord in combatRecords) {
							ListViewItem listViewItem = new ListViewItem (Api.CombatResultToString (combatRecord.Result)) {
								Tag = combatRecord
							};
							listViewItem.SubItems.Insert (ModeColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = combatRecord.Mode });
							listViewItem.SubItems.Insert (DateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = combatRecord.DateTime.ToString (Api.DateTimeExcludeSecondFormatText) });
							listViewItem.SubItems.Insert (CombatListCombatColumnHeader.Index, new ListViewItem.ListViewSubItem () {
								Text = $"{combatRecord.TeamPlayer.Combat:F2}",
								Tag = Api.GetCombatColor (combatRecord.TeamPlayer.Combat)
							});
							CombatListView.Items.Add (listViewItem);
						}
						AutoResizeResultListViewColumns ();
						CombatListView.EndUpdate ();
						if (CombatListView.Items.Count > 0) {
							CombatListView.Items[0].Selected = true;
						}
						SetState ("查询完毕");
					});
				} catch (Exception exception) {
					Api.Invoke (this, () => {
						SetState (exception.Message);
						MessageBox.Show (exception.ToString ());
					});
				} finally {
					SetEnabled (true);
				}
			});
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
				int count = 0;
				Execute (TeamAListView, TeamAInformationLabel, combatRecord.TeamAPlayers);
				Execute (TeamBListView, TeamBInformationLabel, combatRecord.TeamBPlayers);
				void Execute (ListView listView, Label label, JsonArray players) {
					ThreadPool.QueueUserWorkItem (innerState => {
						try {
							RefreshTeamListView (listView, label, players);
						} finally {
							lock (autoResetEvent) {
								count++;
								if (count >= 2) {
									autoResetEvent.Set ();
								}
							}
						}
					});
				}
				autoResetEvent.WaitOne ();
				Api.Invoke (this, () => {
					CombatListView.Enabled = true;
				});
			});
		}

		void RefreshTeamListView (ListView listView, Label label, JsonArray players) {
			Api.Invoke (this, () => {
				listView.Items.Clear ();
				listView.BeginUpdate ();
			});
			int effectiveCount = 0;
			float totalBoxCombat = 0;
			float totalBoxWinRate = 0;
			float totalBoxDamage = 0;
			if (players.Count > 0) {
				AutoResetEvent autoResetEvent = new AutoResetEvent (false);
				int count = 0;
				foreach (JsonValue playerJsonValue in players) {
					if (CachedPlayers.TryGetValue (playerJsonValue, out CachedPlayer cachedPlayer) && cachedPlayer.BoxPersonalCombatRecord.Exception == null) {
						count++;
						Append ();
					} else {
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
								lock (autoResetEvent) {
									count++;
									if (count >= players.Count) {
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
				autoResetEvent.WaitOne ();
			}
			Api.Invoke (this, () => {
				try {
					foreach (JsonValue playerJsonValue in players) {
						CachedPlayer cachedPlayer = CachedPlayers[playerJsonValue];
						float hitRate = Api.Divide (cachedPlayer.TeamPlayer.HitCount, cachedPlayer.TeamPlayer.ShootCount);
						float penetrationRate = Api.Divide (cachedPlayer.TeamPlayer.PenetrationCount, cachedPlayer.TeamPlayer.HitCount);
						float penetrationRateIncludeNoHit = Api.Divide (cachedPlayer.TeamPlayer.PenetrationCount, cachedPlayer.TeamPlayer.ShootCount);
						ListViewItem listViewItem = new ListViewItem (cachedPlayer.TeamPlayer.Name);
						listViewItem.SubItems.Insert (ClanColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = cachedPlayer.TeamPlayer.ClanAbbrev });
						listViewItem.SubItems.Insert (BoxCombatColumnHeader.Index, new ListViewItem.ListViewSubItem () {
							Text = $"{cachedPlayer.BoxPersonalCombatRecord.CombatText}",
							Tag = Api.GetCombatColor (cachedPlayer.BoxPersonalCombatRecord.Combat)
						});
						listViewItem.SubItems.Insert (BoxWinRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.BoxPersonalCombatRecord.WinRate:P0}" });
						listViewItem.SubItems.Insert (BoxHitRateColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.BoxPersonalCombatRecord.HitRate:P0}" });
						listViewItem.SubItems.Insert (BoxCombatLevelColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.BoxPersonalCombatRecord.CombatLevel}" });
						listViewItem.SubItems.Insert (BoxDamageColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = $"{cachedPlayer.BoxPersonalCombatRecord.Damage}" });
						listViewItem.SubItems.Insert (TankColumnHeader.Index, new ListViewItem.ListViewSubItem () { Text = cachedPlayer.TeamPlayer.TankName });
						listViewItem.SubItems.Insert (CombatColumnHeader.Index, new ListViewItem.ListViewSubItem () {
							Text = $"{cachedPlayer.TeamPlayer.Combat:F2}",
							Tag = Api.GetCombatColor (cachedPlayer.TeamPlayer.Combat)
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
					Api.AutoResizeListViewColumns (listView);
					((ListViewComparer)listView.ListViewItemSorter).SortColumn (CombatColumnHeader.Index, SortOrder.Descending);
					listView.EndUpdate ();
					label.Text = string.Format (
						$"平均千场效率：{Api.Divide (totalBoxCombat, effectiveCount):F2} " +
						$"平均千场胜率：{Api.Divide (totalBoxWinRate, effectiveCount):P2} " +
						$"平均千场均伤：{Api.Divide (totalBoxDamage, effectiveCount):F2}"
					);
				} catch (Exception e) {
					MessageBox.Show ($"出现错误，请尝试重新加载{Environment.NewLine}{e}");
				}
			});
		}

		void LoadQueryNames () {
			NameComboBox.Items.Clear ();
			for (int i = 0; i < ConfigService.Instance.BoxCombatQueryHistoryPlayerNames.Count; i++) {
				NameComboBox.Items.Add (ConfigService.Instance.BoxCombatQueryHistoryPlayerNames[i]);
			}
			if (NameComboBox.Items.Count > 0) {
				NameComboBox.SelectedIndex = 0;
			}
		}

		class CachedPlayer {

			public BoxPersonalCombatRecord BoxPersonalCombatRecord;
			public CombatRecordTeamPlayer TeamPlayer;

		}

	}

}