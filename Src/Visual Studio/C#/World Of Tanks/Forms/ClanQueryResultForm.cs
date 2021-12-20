using System;
using System.Collections;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class ClanQueryResultForm : Form {

		public ClanQueryResultForm () {
			InitializeComponent ();
		}

		private void ClanQueryResultForm_Load (object sender, EventArgs e) {
			ResultListView.ListViewItemSorter = new ResultListViewComparer ();
		}

		public void ClearResultListViewItems () {
			ResultListView.Items.Clear ();
		}

		public void AddResultListViewItem (Action<ListView> action) {
			if (InvokeRequired) {
				Invoke (new Action<Action<ListView>> (AddResultListViewItem), action);
				return;
			}
			ResultListView.BeginUpdate ();
			action (ResultListView);
			API.AutoResizeListViewColumns (ResultListView);
			ResultListView.EndUpdate ();
		}

		class ResultListViewComparer : IComparer {

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

	}

}