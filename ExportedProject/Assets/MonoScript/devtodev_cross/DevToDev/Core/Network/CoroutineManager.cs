using System;
using System.Collections;
using System.Collections.Generic;

namespace DevToDev.Core.Network
{
	internal class CoroutineManager
	{
		private List<global::System.Collections.Generic.IEnumerator<Status>> Coroutines = new List<global::System.Collections.Generic.IEnumerator<Status>>();

		public void StartCoroutine(global::System.Collections.Generic.IEnumerator<Status> func)
		{
			Coroutines.Add(func);
		}

		public void Update()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<global::System.Collections.Generic.IEnumerator<Status>> enumerator = Coroutines.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::System.Collections.Generic.IEnumerator<Status> current = enumerator.get_Current();
					((global::System.Collections.IEnumerator)current).MoveNext();
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}
	}
}
