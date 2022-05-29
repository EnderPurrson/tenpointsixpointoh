using I2.Loc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[ExecuteInEditMode]
	public class TextGroup : MonoBehaviour
	{
		[ReadOnly]
		[SerializeField]
		private List<UILabel> _labels = new List<UILabel>();

		[SerializeField]
		private string _text;

		[SerializeField]
		private string _localizationKey;

		public string LocalizationKey
		{
			get
			{
				return this._localizationKey;
			}
			set
			{
				this._localizationKey = value;
				if (this._localizationKey.IsNullOrEmpty())
				{
					this.Text = this._text;
				}
				else if (!this.UseLocalizationComponents)
				{
					this.Text = LocalizationStore.Get(value);
				}
				else
				{
					this.SetLocalizeComponents();
				}
			}
		}

		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				if (this._labels != null)
				{
					this._labels.ForEach((UILabel l) => l.text = this._text);
				}
			}
		}

		public bool UseLocalizationComponents
		{
			get
			{
				return base.GetComponent<Localize>() != null;
			}
		}

		public TextGroup()
		{
		}

		private void HandleLocalizationChanged()
		{
			if (this.LocalizationKey.IsNullOrEmpty())
			{
				this.Text = this._text;
			}
			else if (!this.UseLocalizationComponents)
			{
				this.Text = LocalizationStore.Get(this.LocalizationKey);
			}
			else
			{
				this.SetLocalizeComponents();
			}
		}

		private void OnDisable()
		{
			LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		}

		private void OnEnable()
		{
			LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
			this.SetLabels();
			if (this.UseLocalizationComponents)
			{
				this.SetLocalizeComponents();
			}
			else if (this.LocalizationKey.IsNullOrEmpty())
			{
				this.Text = this._text;
			}
			else
			{
				this.Text = LocalizationStore.Get(this.LocalizationKey);
			}
		}

		private void SetLabels()
		{
			this._labels.Clear();
			UILabel component = base.GetComponent<UILabel>();
			if (component != null)
			{
				this._labels.Add(component);
			}
			this._labels.AddRange(base.GetComponentsInChildren<UILabel>(true));
		}

		private void SetLocalizeComponents()
		{
			foreach (UILabel _label in this._labels)
			{
				Localize component = _label.gameObject.GetComponent<Localize>();
				if (component == null)
				{
					component = _label.gameObject.AddComponent<Localize>();
				}
				component.Term = this.LocalizationKey;
			}
			this.Text = LocalizationStore.Get(this.LocalizationKey);
		}
	}
}