using System.Collections.Generic;
using System.Windows.Forms;

namespace WorldOfTanks {

	class PageChanger<T> {

		public Form MDIForm { get; set; }
		public Control Parent { get; set; }

		readonly Dictionary<T, Form> Pages = new Dictionary<T, Form> ();

		T LastKey;

		public void Add (T key, Form page) {
			Pages[key] = page;
			page.MdiParent = MDIForm;
			page.TopLevel = false;
			page.Parent = Parent;
			page.FormBorderStyle = FormBorderStyle.None;
			page.Show ();
			page.Hide ();
		}

		public void Change (T key) {
			if (Pages.TryGetValue (LastKey, out Form lastPage)) {
				lastPage.Hide ();
			}
			if (Pages.TryGetValue (key, out Form page)) {
				ResizePage (page);
				page.Show ();
			}
			LastKey = key;
		}

		public void ResizeCurrentPage () {
			if (Pages.TryGetValue (LastKey, out Form page)) {
				ResizePage (page);
			}
		}

		void ResizePage (Form page) {
			page.Size = Parent.Size;
		}

	}

}