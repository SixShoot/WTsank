using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Eruru.Http;

namespace WorldOfTanks {

	static class Program {

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main () {
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Application.ThreadException += Application_ThreadException;
			Application.SetUnhandledExceptionMode (UnhandledExceptionMode.CatchException);
			HttpAPI.SetSecurityProtocol ();
			ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
			Api.Initialize ();
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
			Application.Run (new Form1 ());
		}

		private static void Application_ThreadException (object sender, ThreadExceptionEventArgs e) {
			MessageBox.Show (e.Exception.ToString ());
		}

		private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e) {
			MessageBox.Show (e.ExceptionObject.ToString ());
		}

	}

}