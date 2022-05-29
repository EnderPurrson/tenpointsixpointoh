using System;
using System.IO;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class CrashReporter : MonoBehaviour
	{
		private string _reportText = string.Empty;

		private string _reportTime = string.Empty;

		private bool _showReport;

		public CrashReporter()
		{
		}

		private static void HandleException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception exceptionObject = e.ExceptionObject as Exception;
			if (exceptionObject != null)
			{
				string str = string.Format("Report_{0:s}.txt", DateTime.Now).Replace(':', '-');
				string str1 = Path.Combine(Application.persistentDataPath, str);
				File.WriteAllText(str1, exceptionObject.ToString());
			}
		}

		internal void OnGUI()
		{
			float single = (Screen.dpi != 0f ? Screen.dpi : 160f);
			if (GUILayout.Button("Simulate exception", new GUILayoutOption[] { GUILayout.Width(1f * single) }))
			{
				throw new InvalidOperationException(DateTime.Now.ToString("s"));
			}
			GUILayout.Label(string.Concat("Report path: ", Application.persistentDataPath), new GUILayoutOption[0]);
			if (!string.IsNullOrEmpty(this._reportText))
			{
				this._showReport = GUILayout.Toggle(this._showReport, string.Concat("Show: ", this._reportTime), new GUILayoutOption[0]);
				if (this._showReport)
				{
					GUILayout.Label(this._reportText, new GUILayoutOption[0]);
				}
			}
		}

		internal void Start()
		{
			if (!Debug.isDebugBuild)
			{
				base.enabled = false;
			}
			else
			{
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CrashReporter.HandleException);
				string[] files = Directory.GetFiles(Application.persistentDataPath, "Report_*.txt", SearchOption.TopDirectoryOnly);
				if ((int)files.Length > 0)
				{
					string str = files[(int)files.Length - 1];
					this._reportTime = Path.GetFileNameWithoutExtension(str);
					this._reportText = File.ReadAllText(str);
				}
			}
		}
	}
}