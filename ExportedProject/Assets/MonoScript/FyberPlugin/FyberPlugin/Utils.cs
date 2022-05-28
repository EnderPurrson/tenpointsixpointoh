using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace FyberPlugin
{
	public class Utils
	{
		public static void printWarningMessage()
		{
			Debug.Log("WARNING: Fyber plugin is not available on this platform.");
			Debug.Log("WARNING: the \"" + GetMethodName() + "\" method does not do anything");
		}

		private static string GetMethodName()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			StackTrace val = new StackTrace();
			StackFrame frame = val.GetFrame(2);
			return ((MemberInfo)frame.GetMethod()).get_Name();
		}
	}
}
