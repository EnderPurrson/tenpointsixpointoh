using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class TrainingBlinking : MonoBehaviour
	{
		private readonly List<KeyValuePair<UISprite, Color>> _sprites = new List<KeyValuePair<UISprite, Color>>(5);

		public TrainingBlinking()
		{
		}

		private static float GetInterpolationCoefficient(float time)
		{
			float single = time - Mathf.Floor(time);
			return 1f - single * single;
		}

		private void OnDestroy()
		{
			this.RestoreColorTints();
			this._sprites.Clear();
		}

		private void OnDisable()
		{
			this.RestoreColorTints();
			this._sprites.Clear();
			UnityEngine.Object.Destroy(this);
		}

		private void RestoreColorTints()
		{
			foreach (KeyValuePair<UISprite, Color> _sprite in this._sprites)
			{
				_sprite.Key.color = _sprite.Value;
			}
		}

		public void SetSprites(IList<UISprite> sprites)
		{
			this.RestoreColorTints();
			this._sprites.Clear();
			if (sprites == null)
			{
				return;
			}
			IEnumerator<UISprite> enumerator = sprites.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					UISprite current = enumerator.Current;
					if (current != null)
					{
						this._sprites.Add(new KeyValuePair<UISprite, Color>(current, current.color));
					}
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}

		private void Update()
		{
			float interpolationCoefficient = TrainingBlinking.GetInterpolationCoefficient(Time.time);
			foreach (KeyValuePair<UISprite, Color> _sprite in this._sprites)
			{
				_sprite.Key.color = Color.Lerp(_sprite.Value, Color.green, interpolationCoefficient);
			}
		}
	}
}