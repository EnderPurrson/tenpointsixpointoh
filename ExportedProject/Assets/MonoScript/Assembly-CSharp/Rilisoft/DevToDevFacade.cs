using DevToDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class DevToDevFacade
	{
		private readonly string _appKey;

		public static bool LoggingEnabled
		{
			set
			{
				Analytics.SetActiveLog(value);
			}
		}

		public bool UserIsCheater
		{
			set
			{
				if (string.IsNullOrEmpty(this._appKey))
				{
					return;
				}
			}
		}

		public static string Version
		{
			get
			{
				return Analytics.SDKVersion;
			}
		}

		public DevToDevFacade(string appKey, string secretKey)
		{
			if (appKey == null)
			{
				throw new ArgumentNullException("appKey");
			}
			if (secretKey == null)
			{
				throw new ArgumentNullException("secretKey");
			}
			this._appKey = appKey;
			Analytics.Initialize(appKey, secretKey);
		}

		public void CurrencyAccrual(int amount, string currencyName, AccrualType accrualType)
		{
			Analytics.CurrencyAccrual(amount, currencyName, accrualType);
		}

		public void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, int purchasePrice, string purchaseCurrency)
		{
			purchaseId = purchaseId.Substring(0, Math.Min(purchaseId.Length, 32));
			purchaseType = purchaseType.Substring(0, Math.Min(purchaseType.Length, 96));
			Analytics.InAppPurchase(purchaseId, purchaseType, purchaseAmount, purchasePrice, purchaseCurrency);
		}

		public void LevelUp(int level, Dictionary<string, int> resources)
		{
			Analytics.LevelUp(level, resources);
		}

		public void RealPayment(string paymentId, float inAppPrice, string inAppName, string inAppCurrencyISOCode)
		{
			Analytics.RealPayment(paymentId, inAppPrice, inAppName, inAppCurrencyISOCode);
		}

		public void SendBufferedEvents()
		{
			if (string.IsNullOrEmpty(this._appKey))
			{
				return;
			}
			Analytics.SendBufferedEvents();
		}

		public void SendCustomEvent(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentException("Event name must not be empty.", "eventName");
			}
			if (string.IsNullOrEmpty(this._appKey))
			{
				return;
			}
			Analytics.CustomEvent(eventName);
		}

		public void SendCustomEvent(string eventName, IDictionary<string, object> eventParams)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentException("Event name must not be empty.", "eventName");
			}
			if (string.IsNullOrEmpty(this._appKey))
			{
				return;
			}
			CustomEventParams customEventParam = new CustomEventParams();
			IEnumerator<KeyValuePair<string, object>> enumerator = eventParams.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, object> current = enumerator.Current;
					string value = current.Value as string;
					if (value != null)
					{
						customEventParam.AddParam(current.Key, value);
					}
					else if (current.Value is int)
					{
						customEventParam.AddParam(current.Key, (int)current.Value);
					}
					else if (current.Value is long)
					{
						customEventParam.AddParam(current.Key, (long)current.Value);
					}
					else if (current.Value is float || current.Value is double)
					{
						customEventParam.AddParam(current.Key, Convert.ToDouble(current.Value));
					}
					else if (!(current.Value is DateTimeOffset))
					{
						customEventParam.AddParam(current.Key, Convert.ToString(current.Value));
					}
					else
					{
						Debug.LogError(string.Concat("Trying to pass DateTimeOffset parameter to DevToDev: ", current.Key ?? "null"));
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
			Analytics.CustomEvent(eventName, customEventParam);
		}

		public void Tutorial(int step)
		{
			Analytics.Tutorial(step);
		}
	}
}