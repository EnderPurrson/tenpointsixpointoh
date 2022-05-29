using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Rilisoft
{
	internal static class SslUtils
	{
		[DebuggerHidden]
		internal static IEnumerable<string> ReadStreamRoutine(Stream stream)
		{
			SslUtils.u003cReadStreamRoutineu003ec__IteratorFD variable = null;
			return variable;
		}

		internal static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors != SslPolicyErrors.None)
			{
				UnityEngine.Debug.LogWarning(string.Concat("SslPolicyError:    ", sslPolicyErrors));
			}
			return true;
		}
	}
}