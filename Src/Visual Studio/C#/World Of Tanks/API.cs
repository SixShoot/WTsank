using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WorldOfTanks {

	class API {

		public static void SetCompareColor (Control a, Control b, int compareResult) {
			if (compareResult == 0) {
				a.ForeColor = Color.Black;
				a.BackColor = Color.White;
				b.ForeColor = Color.Black;
				b.BackColor = Color.White;
				return;
			}
			if (compareResult > 0) {
				a.ForeColor = Color.Black;
				a.BackColor = Color.LightGreen;
				b.ForeColor = Color.White;
				b.BackColor = Color.LightCoral;
				return;
			}
			a.ForeColor = Color.White;
			a.BackColor = Color.LightCoral;
			b.ForeColor = Color.Black;
			b.BackColor = Color.LightGreen;
		}

		public static float GetMedian (IList<float> list) {
			int half = list.Count / 2;
			if (list.Count % 2 == 0) {
				return (list[half - 1] + list[half]) / 2;
			} else {
				return list[half];
			}
		}

		public static void AutoResizeListViewColumns (ListView listView) {
			listView.BeginUpdate ();
			int[] widths = new int[listView.Columns.Count];
			listView.AutoResizeColumns (ColumnHeaderAutoResizeStyle.HeaderSize);
			for (int i = 0; i < listView.Columns.Count; i++) {
				widths[i] = listView.Columns[i].Width;
			}
			listView.AutoResizeColumns (ColumnHeaderAutoResizeStyle.ColumnContent);
			for (int i = 0; i < listView.Columns.Count; i++) {
				if (listView.Columns[i].Width < widths[i]) {
					listView.Columns[i].Width = widths[i];
				}
			}
			listView.EndUpdate ();
		}

	}

}