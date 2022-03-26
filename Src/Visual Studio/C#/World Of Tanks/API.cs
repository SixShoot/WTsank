using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
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

		public static void ExportCSV (ListView leftListView, ListView listView, string filePath) {
			FileInfo fileInfo = new FileInfo (filePath);
			Directory.CreateDirectory (fileInfo.DirectoryName);
			var encoding = new UTF8Encoding (true);
			using (StreamWriter streamWriter = new StreamWriter (File.Create (fileInfo.FullName), encoding)) {
				int columnNumber = leftListView.Columns.Count + listView.Columns.Count + 1;
				int rowNumber = Math.Max (leftListView.Items.Count, listView.Items.Count);
				for (int row = -1; row < rowNumber; row++) {
					if (row > -1) {
						streamWriter.WriteLine ();
					}
					for (int column = 0; column < columnNumber; column++) {
						if (column > 0) {
							streamWriter.Write (',');
						}
						if (row == -1) {
							if (column < leftListView.Columns.Count) {
								streamWriter.Write (leftListView.Columns[column].Text);
								continue;
							}
							if (column == leftListView.Columns.Count) {
								continue;
							}
							streamWriter.Write (listView.Columns[column - leftListView.Columns.Count - 1].Text);
							continue;
						}
						if (column < leftListView.Columns.Count) {
							if (row < leftListView.Items.Count) {
								streamWriter.Write (leftListView.Items[row].SubItems[column].Text);
							}
							continue;
						}
						if (column == leftListView.Columns.Count) {
							continue;
						}
						if (row < listView.Items.Count) {
							streamWriter.Write (listView.Items[row].SubItems[column - leftListView.Columns.Count - 1].Text);
						}
					}
				}
			}
			MessageBox.Show ("完成");
		}

		public static bool CheckDateTime (DateTime startDateTime, DateTime endDateTime) {
			if (endDateTime.Date > DateTime.Now.Date) {
				MessageBox.Show ("结束日期不能大于今天");
				return false;
			}
			if (startDateTime.Date > endDateTime.Date) {
				MessageBox.Show ("开始日期不能大于结束日期");
				return false;
			}
			return true;
		}

	}

}