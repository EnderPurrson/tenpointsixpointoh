using System.Collections.Generic;
using System.Runtime.CompilerServices;
using I2.Loc;
using UnityEngine;

namespace Rilisoft
{
	[ExecuteInEditMode]
	public class TextGroup : MonoBehaviour
	{
		[SerializeField]
		[ReadOnly]
		private List<UILabel> _labels = new List<UILabel>();

		[SerializeField]
		private string _text;

		[SerializeField]
		private string _localizationKey;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				if (_labels != null)
				{
					_labels.ForEach(_003Cset_Text_003Em__463);
				}
			}
		}

		public string LocalizationKey
		{
			get
			{
				return _localizationKey;
			}
			set
			{
				_localizationKey = value;
				if (!_localizationKey.IsNullOrEmpty())
				{
					if (UseLocalizationComponents)
					{
						SetLocalizeComponents();
					}
					else
					{
						Text = LocalizationStore.Get(value);
					}
				}
				else
				{
					Text = _text;
				}
			}
		}

		public bool UseLocalizationComponents
		{
			get
			{
				return GetComponent<Localize>() != null;
			}
		}

		private void OnEnable()
		{
			LocalizationStore.AddEventCallAfterLocalize(HandleLocalizationChanged);
			SetLabels();
			if (UseLocalizationComponents)
			{
				SetLocalizeComponents();
			}
			else if (!LocalizationKey.IsNullOrEmpty())
			{
				Text = LocalizationStore.Get(LocalizationKey);
			}
			else
			{
				Text = _text;
			}
		}

		private void OnDisable()
		{
			LocalizationStore.DelEventCallAfterLocalize(HandleLocalizationChanged);
		}

		private void SetLabels()
		{
			_labels.Clear();
			UILabel component = GetComponent<UILabel>();
			if (component != null)
			{
				_labels.Add(component);
			}
			_labels.AddRange(GetComponentsInChildren<UILabel>(true));
		}

		private void HandleLocalizationChanged()
		{
			if (!LocalizationKey.IsNullOrEmpty())
			{
				if (UseLocalizationComponents)
				{
					SetLocalizeComponents();
				}
				else
				{
					Text = LocalizationStore.Get(LocalizationKey);
				}
			}
			else
			{
				Text = _text;
			}
		}

		private void SetLocalizeComponents()
		{
			foreach (UILabel label in _labels)
			{
				Localize localize = label.gameObject.GetComponent<Localize>();
				if (localize == null)
				{
					localize = label.gameObject.AddComponent<Localize>();
				}
				localize.Term = LocalizationKey;
			}
			Text = LocalizationStore.Get(LocalizationKey);
		}

		[CompilerGenerated]
		private void _003Cset_Text_003Em__463(UILabel l)
		{
			l.text = _text;
		}
	}
}
