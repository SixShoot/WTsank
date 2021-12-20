using System;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class CombatRecordQueryResultForm : Form {

		public CombatRecordQueryResultForm () {
			InitializeComponent ();
		}

		public void ClearAllListViewItems () {
			ResultListView.Items.Clear ();
			TankResultListView.Items.Clear ();
		}

		public void AddResultListViewItem (Action<ListView> action) {
			AddResultListViewItem (ResultListView, action);
		}

		public void AddTankResultListViewItem (Action<ListView> action) {
			AddResultListViewItem (TankResultListView, action);
		}

		void AddResultListViewItem (ListView listView, Action<ListView> action) {
			if (InvokeRequired) {
				Invoke (new Action<ListView, Action<ListView>> (AddResultListViewItem), listView, action);
				return;
			}
			listView.BeginUpdate ();
			action (listView);
			API.AutoResizeListViewColumns (listView);
			listView.EndUpdate ();
		}

	}

}