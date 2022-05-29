using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class PurchasesSynchronizerListener : MonoBehaviour
	{
		private IDisposable _escapeSubscription;

		public PurchasesSynchronizerListener()
		{
		}

		private void HandleEscape()
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Ignoring [Escape] while syncing.");
			}
		}

		private void HandlePurchasesSavingStarted(object sender, PurchasesSavingEventArgs e)
		{
			string str = string.Format(CultureInfo.InvariantCulture, "{0}.HandlePurchasesSavingStarted()", new object[] { base.GetType().Name });
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				try
				{
					this._escapeSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "PurchasesSynchronizerListener");
					ActivityIndicator.SetActiveWithCaption(LocalizationStore.Get("Key_1974"));
					InfoWindowController.BlockAllClick();
					base.StartCoroutine(this.WaitCompletionCoroutine(e.Future));
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void OnDestroy()
		{
			PurchasesSynchronizer.Instance.PurchasesSavingStarted -= new EventHandler<PurchasesSavingEventArgs>(this.HandlePurchasesSavingStarted);
			if (this._escapeSubscription != null)
			{
				this._escapeSubscription.Dispose();
			}
		}

		private void Start()
		{
			PurchasesSynchronizer.Instance.PurchasesSavingStarted += new EventHandler<PurchasesSavingEventArgs>(this.HandlePurchasesSavingStarted);
		}

		[DebuggerHidden]
		private IEnumerator WaitCompletionCoroutine(Task<bool> future)
		{
			PurchasesSynchronizerListener.u003cWaitCompletionCoroutineu003ec__Iterator180 variable = null;
			return variable;
		}
	}
}