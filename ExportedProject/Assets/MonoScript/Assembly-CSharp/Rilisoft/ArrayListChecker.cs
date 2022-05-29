using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class ArrayListChecker : IDisposable
	{
		private const int CapacityThreshold = 1000;

		private const int CountThreshold = 50;

		private ArrayList _arrayList;

		private bool _disposed;

		private string _label;

		public ArrayListChecker(ArrayList arrayList, string label)
		{
			this._arrayList = arrayList;
			this._label = label ?? string.Empty;
			this.CheckOverflowIfDebug();
		}

		private void CheckOverflowIfDebug()
		{
			if (Debug.isDebugBuild)
			{
				if (this._arrayList == null)
				{
					Debug.LogWarning(string.Concat(this._label, ": ArrayList is null."));
				}
				else if (this._arrayList.Count > 50 || this._arrayList.Capacity > 1000)
				{
					this.HandleOverflow();
				}
			}
		}

		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this.CheckOverflowIfDebug();
			this._disposed = true;
		}

		private void HandleOverflow()
		{
			string str = string.Format("{0}: Count: {1}, Capacity: {2}", this._label, this._arrayList.Count, this._arrayList.Capacity);
			string str1 = string.Concat(str, Environment.NewLine, Environment.NewLine, Environment.StackTrace);
			Debug.LogWarning(str1);
		}
	}
}