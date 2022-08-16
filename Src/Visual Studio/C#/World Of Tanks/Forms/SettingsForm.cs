using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorldOfTanks {

	public partial class SettingsForm : Form, IFormPage {

		public SettingsForm () {
			InitializeComponent ();
		}

		public void OnShow () {
			ConfigService.Load ();
			NetworkRequestIntervalTextBox.Text = ConfigService.Instance.NetworkRequestInterval.ToString ();
			RefreshQueryCacheSize ();
			MaxQueryHistoryCountTextBox.Text = ConfigService.Instance.MaxQueryHistoryCount.ToString ();
		}

		private void ClearQueryCacheButton_Click (object sender, EventArgs e) {
			if (MessageBox.Show ("缓存越大清理越慢，期间程序会无响应，请耐心等待，是否继续清理？", string.Empty, MessageBoxButtons.OKCancel) == DialogResult.OK) {
				BoxService.Instance.ClearCache ();
				RefreshQueryCacheSize ();
			}
		}

		private void NetworkRequestIntervalTextBox_TextChanged (object sender, EventArgs e) {
			try {
				int networkRequestInterval;
				if (string.IsNullOrWhiteSpace (NetworkRequestIntervalTextBox.Text)) {
					networkRequestInterval = 0;
				} else {
					networkRequestInterval = int.Parse (NetworkRequestIntervalTextBox.Text);
				}
				ConfigService.Instance.NetworkRequestInterval = networkRequestInterval;
				ConfigService.Save ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		private void MaxQueryHistoryCountTextBox_TextChanged (object sender, EventArgs e) {
			try {
				int maxQueryHistoryCount;
				if (string.IsNullOrWhiteSpace (MaxQueryHistoryCountTextBox.Text)) {
					maxQueryHistoryCount = 0;
				} else {
					maxQueryHistoryCount = int.Parse (MaxQueryHistoryCountTextBox.Text);
				}
				ConfigService.Instance.MaxQueryHistoryCount = maxQueryHistoryCount;
				ConfigService.Save ();
			} catch (Exception exception) {
				MessageBox.Show (exception.ToString ());
			}
		}

		void RefreshQueryCacheSize () {
			QueryCacheSizeLabel.Text = string.Format ("查询缓存大小：{0}", Api.FormatFileSize (BoxService.Instance.GetCacheSize ()));
		}

	}

}