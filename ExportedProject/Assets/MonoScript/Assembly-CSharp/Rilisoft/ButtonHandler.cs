using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public sealed class ButtonHandler : MonoBehaviour
	{
		public bool noSound;

		[NonSerialized]
		public bool isEnable = true;

		private EventHandler Clicked;

		public bool HasClickedHandlers
		{
			get
			{
				return this.Clicked != null;
			}
		}

		public ButtonHandler()
		{
		}

		public void DoClick()
		{
			if (!this.isEnable)
			{
				return;
			}
			this.OnClick();
		}

		private void OnClick()
		{
			if (!this.isEnable)
			{
				return;
			}
			if (ButtonClickSound.Instance != null && !this.noSound)
			{
				ButtonClickSound.Instance.PlayClick();
			}
			EventHandler clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, EventArgs.Empty);
			}
		}

		public event EventHandler Clicked
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Clicked += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Clicked -= value;
			}
		}
	}
}