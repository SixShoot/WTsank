using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WorldOfTanks {

	class ListViewComparer : IComparer {

		public ListView ListView;
		public int SelectedColumnIndex;
		public Type SelectedColumnType;
		public Func<int, Type> OnGetColumnType;
		public Action OnSorted;

		public ListViewComparer (ListView listView) {
			ListView = listView;
		}

		public int Compare (object x, object y) {
			ListViewItem listViewItemX = (ListViewItem)x;
			ListViewItem listViewItemY = (ListViewItem)y;
			string textX = listViewItemX.SubItems[SelectedColumnIndex].Text;
			string textY = listViewItemY.SubItems[SelectedColumnIndex].Text;
			int result;
			if (SelectedColumnType == typeof (float)) {
				Match matchX = Regex.Match (textX, @"[0-9+-.eE]+");
				Match matchY = Regex.Match (textY, @"[0-9+-.eE]+");
				if (!float.TryParse (matchX.Value, out float floatX)) {
					floatX = -1;
				}
				if (!float.TryParse (matchY.Value, out float floatY)) {
					floatY = -1;
				}
				result = floatX.CompareTo (floatY);
			} else if (SelectedColumnType == typeof (ClanMemberPosition)) {
				ClanMember clanMemberX = (ClanMember)listViewItemX.Tag;
				ClanMember clanMemberY = (ClanMember)listViewItemY.Tag;
				result = clanMemberX.PositionRank.CompareTo (clanMemberY.PositionRank);
			} else {
				result = textX.CompareTo (textY);
			}
			if (ListView.Sorting == SortOrder.Descending) {
				result *= -1;
			}
			return result;
		}

		public void ToggleSortOrder () {
			ListView.Sorting = ListView.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}

		public void OnClickColumn (int column) {
			if (SelectedColumnIndex == column) {
				ToggleSortOrder ();
			} else {
				ListView.Sorting = SortOrder.Descending;
			}
			SortColumn (column, ListView.Sorting);
		}

		public void SortColumn (int column, SortOrder sortOrder) {
			SelectedColumnIndex = column;
			ListView.Sorting = sortOrder;
			SelectedColumnType = OnGetColumnType?.Invoke (column) ?? typeof (string);
			ListView.Sort ();
			OnSorted?.Invoke ();
		}

	}

}