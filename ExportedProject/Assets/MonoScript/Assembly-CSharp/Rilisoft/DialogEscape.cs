using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class DialogEscape : MonoBehaviour
	{
		private readonly Lazy<ButtonHandler> _buttonHandler;

		private IDisposable _escapeSubscription;

		public string Context
		{
			get;
			set;
		}

		public DialogEscape()
		{
			this._buttonHandler = new Lazy<ButtonHandler>(new Func<ButtonHandler>(this.GetComponent<ButtonHandler>));
		}

		private void HandleEscape()
		{
			if (this._buttonHandler.Value != null)
			{
				this._buttonHandler.Value.DoClick();
			}
		}

		private void OnDisable()
		{
			if (this._escapeSubscription != null)
			{
				this._escapeSubscription.Dispose();
				this._escapeSubscription = null;
			}
		}

		private void OnEnable()
		{
			if (this._escapeSubscription != null)
			{
				this._escapeSubscription.Dispose();
			}
			this._escapeSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), this.Context ?? "Dialog");
		}
	}
}