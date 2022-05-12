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
			ListViewItem a = (ListViewItem)x;
			ListViewItem b = (ListViewItem)y;
			string textA = a.SubItems[SelectedColumnIndex].Text;
			string textB = b.SubItems[SelectedColumnIndex].Text;
			int value;
			if (SelectedColumnType == typeof (float)) {
				Match matchA = Regex.Match (textA, @"[0-9+-.eE]+");
				Match matchB = Regex.Match (textB, @"[0-9+-.eE]+");
				if (!float.TryParse (matchA.Value, out float floatA)) {
					floatA = -1;
				}
				if (!float.TryParse (matchB.Value, out float floatB)) {
					floatB = -1;
				}
				value = floatA.CompareTo (floatB);
			} else if (SelectedColumnType == typeof (ClanMemberPosition)) {
				ClanMember clanMemberA = (ClanMember)a.Tag;
				ClanMember clanMemberB = (ClanMember)b.Tag;
				value = clanMemberA.PositionRank.CompareTo (clanMemberB.PositionRank);
			} else {
				value = textA.CompareTo (textB);
			}
			if (ListView.Sorting == SortOrder.Descending) {
				value *= -1;
			}
			return value;
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