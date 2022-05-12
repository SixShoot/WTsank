using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class SetColumnForm : Form {

		List<ColumnHeader> Columns;

		public SetColumnForm (List<ColumnHeader> columns) {
			InitializeComponent ();
			Columns = columns;
			for (int i = 0; i < columns.Count; i++) {
				ListViewItem listViewItem = new ListViewItem ();
				listViewItem.SubItems.Add (columns[i].Text);
				ListView.Items.Add (listViewItem);
				listViewItem.Checked = columns[i].Tag == null;
			}
			API.AutoResizeListViewColumns (ListView, true);
			Width = ListView.Width + 20;
		}

		private void ConfirmButton_Click (object sender, EventArgs e) {
			for (int i = 0; i < ListView.Items.Count; i++) {
				string name = ListView.Items[i].SubItems[1].Text;
				ColumnHeader column = Columns.Find (item => item.Text == name);
				if (ListView.Items[i].Checked) {
					column.Tag = null;
					continue;
				}
				column.Tag = 0;
			}
			DialogResult = DialogResult.OK;
			Close ();
		}

	}

}