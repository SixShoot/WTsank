using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WorldOfTanks {

	class ListViewComparer : IComparer {

		public ListView ListView;
		public int SelectedColumnIndex;
		public Type SelectedColumnType;

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
				Match matchA = Regex.Match (textA, @"[0-9]+");
				Match matchB = Regex.Match (textB, @"[0-9]+");
				string valueA = matchA.Success ? matchA.Value : "0";
				string valueB = matchB.Success ? matchB.Value : "0";
				value = float.Parse (valueA).CompareTo (float.Parse (valueB));
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

	}

}