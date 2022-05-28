using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Prime31
{
	[DefaultMember("Item")]
	public class OAuthManager
	{
		private static readonly global::System.DateTime _epoch = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0);

		private SortedDictionary<string, string> _params;

		private Random _random;

		private static string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

		public string this[string ix]
		{
			get
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				if (_params.ContainsKey(ix))
				{
					return _params.get_Item(ix);
				}
				throw new ArgumentException(ix);
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				if (!_params.ContainsKey(ix))
				{
					throw new ArgumentException(ix);
				}
				_params.set_Item(ix, value);
			}
		}

		public OAuthManager()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			_random = new Random();
			_params = new SortedDictionary<string, string>();
			_params.set_Item("consumer_key", "");
			_params.set_Item("consumer_secret", "");
			_params.set_Item("timestamp", generateTimeStamp());
			_params.set_Item("nonce", generateNonce());
			_params.set_Item("signature_method", "HMAC-SHA1");
			_params.set_Item("signature", "");
			_params.set_Item("token", "");
			_params.set_Item("token_secret", "");
			_params.set_Item("version", "1.0");
		}

		public OAuthManager(string consumerKey, string consumerSecret, string token, string tokenSecret)
			: this()
		{
			_params.set_Item("consumer_key", consumerKey);
			_params.set_Item("consumer_secret", consumerSecret);
			_params.set_Item("token", token);
			_params.set_Item("token_secret", tokenSecret);
		}

		private string generateTimeStamp()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			TimeSpan val = global::System.DateTime.get_UtcNow() - _epoch;
			return ((object)Convert.ToInt64(((TimeSpan)(ref val)).get_TotalSeconds())).ToString();
		}

		private void prepareNewRequest()
		{
			_params.set_Item("nonce", generateNonce());
			_params.set_Item("timestamp", generateTimeStamp());
		}

		private string generateNonce()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			StringBuilder val = new StringBuilder();
			for (int i = 0; i < 8; i++)
			{
				if (_random.Next(3) == 0)
				{
					val.Append((char)(_random.Next(26) + 97), 1);
				}
				else
				{
					val.Append((char)(_random.Next(10) + 48), 1);
				}
			}
			return ((object)val).ToString();
		}

		private SortedDictionary<string, string> extractQueryParameters(string queryString)
		{
			if (queryString.StartsWith("?"))
			{
				queryString = queryString.Remove(0, 1);
			}
			SortedDictionary<string, string> val = new SortedDictionary<string, string>();
			if (string.IsNullOrEmpty(queryString))
			{
				return val;
			}
			string[] array = queryString.Split(new char[1] { '&' });
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("oauth_"))
				{
					if (text.IndexOf('=') > -1)
					{
						string[] array2 = text.Split(new char[1] { '=' });
						val.Add(array2[0], array2[1]);
					}
					else
					{
						val.Add(text, string.Empty);
					}
				}
			}
			return val;
		}

		public static string urlEncode(string value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			StringBuilder val = new StringBuilder();
			for (int i = 0; i < value.get_Length(); i++)
			{
				char c = value.get_Chars(i);
				if (unreservedChars.IndexOf(c) != -1)
				{
					val.Append(c);
				}
				else
				{
					val.Append(string.Concat((object)'%', (object)string.Format("{0:X2}", (object)(int)c)));
				}
			}
			return ((object)val).ToString();
		}

		private static SortedDictionary<string, string> mergePostParamsWithOauthParams(SortedDictionary<string, string> postParams, SortedDictionary<string, string> oAuthParams)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			SortedDictionary<string, string> val = new SortedDictionary<string, string>();
			Enumerator<string, string> enumerator = postParams.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					val.Add(current.get_Key(), current.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			Enumerator<string, string> enumerator2 = oAuthParams.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, string> current2 = enumerator2.get_Current();
					if (!string.IsNullOrEmpty(current2.get_Value()) && !current2.get_Key().EndsWith("secret"))
					{
						val.Add("oauth_" + current2.get_Key(), current2.get_Value());
					}
				}
				return val;
			}
			finally
			{
				((global::System.IDisposable)enumerator2).Dispose();
			}
		}

		private static string encodeRequestParameters(SortedDictionary<string, string> p)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder val = new StringBuilder();
			Enumerator<string, string> enumerator = p.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					if (!string.IsNullOrEmpty(current.get_Value()) && !current.get_Key().EndsWith("secret"))
					{
						val.AppendFormat("oauth_{0}=\"{1}\", ", (object)current.get_Key(), (object)urlEncode(current.get_Value()));
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return ((object)val).ToString().TrimEnd(new char[1] { ' ' }).TrimEnd(new char[1] { ',' });
		}

		public static byte[] encodePostParameters(SortedDictionary<string, string> p)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder val = new StringBuilder();
			Enumerator<string, string> enumerator = p.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					if (!string.IsNullOrEmpty(current.get_Value()))
					{
						val.AppendFormat("{0}={1}, ", (object)urlEncode(current.get_Key()), (object)urlEncode(current.get_Value()));
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return Encoding.get_UTF8().GetBytes(((object)val).ToString().TrimEnd(new char[1] { ' ' }).TrimEnd(new char[1] { ',' }));
		}

		public OAuthResponse acquireRequestToken(string uri, string method)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			prepareNewRequest();
			string authorizationHeader = getAuthorizationHeader(uri, method);
			HttpWebRequest val = (HttpWebRequest)WebRequest.Create(uri);
			((NameValueCollection)((WebRequest)val).get_Headers()).Add("Authorization", authorizationHeader);
			((WebRequest)val).set_Method(method);
			HttpWebResponse val2 = (HttpWebResponse)((WebRequest)val).GetResponse();
			try
			{
				StreamReader val3 = new StreamReader(((WebResponse)val2).GetResponseStream());
				try
				{
					OAuthResponse oAuthResponse = new OAuthResponse(((TextReader)val3).ReadToEnd());
					this["token"] = oAuthResponse["oauth_token"];
					try
					{
						if (oAuthResponse["oauth_token_secret"] != null)
						{
							this["token_secret"] = oAuthResponse["oauth_token_secret"];
						}
					}
					catch
					{
					}
					return oAuthResponse;
				}
				finally
				{
					if (val3 != null)
					{
						((global::System.IDisposable)val3).Dispose();
					}
				}
			}
			finally
			{
				if (val2 != null)
				{
					((global::System.IDisposable)val2).Dispose();
				}
			}
		}

		public OAuthResponse acquireAccessToken(string uri, string method, string verifier)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			prepareNewRequest();
			_params.set_Item("verifier", verifier);
			string authorizationHeader = getAuthorizationHeader(uri, method);
			HttpWebRequest val = (HttpWebRequest)WebRequest.Create(uri);
			((NameValueCollection)((WebRequest)val).get_Headers()).Add("Authorization", authorizationHeader);
			((WebRequest)val).set_Method(method);
			HttpWebResponse val2 = (HttpWebResponse)((WebRequest)val).GetResponse();
			try
			{
				StreamReader val3 = new StreamReader(((WebResponse)val2).GetResponseStream());
				try
				{
					OAuthResponse oAuthResponse = new OAuthResponse(((TextReader)val3).ReadToEnd());
					this["token"] = oAuthResponse["oauth_token"];
					this["token_secret"] = oAuthResponse["oauth_token_secret"];
					return oAuthResponse;
				}
				finally
				{
					if (val3 != null)
					{
						((global::System.IDisposable)val3).Dispose();
					}
				}
			}
			finally
			{
				if (val2 != null)
				{
					((global::System.IDisposable)val2).Dispose();
				}
			}
		}

		public string generateCredsHeader(string uri, string method, string realm)
		{
			prepareNewRequest();
			return getAuthorizationHeader(uri, method, realm);
		}

		public string generateAuthzHeader(string uri, string method)
		{
			prepareNewRequest();
			return getAuthorizationHeader(uri, method, null);
		}

		private string getAuthorizationHeader(string uri, string method)
		{
			return getAuthorizationHeader(uri, method, null);
		}

		private string getAuthorizationHeader(string uri, string method, string realm)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(_params.get_Item("consumer_key")))
			{
				throw new ArgumentNullException("consumer_key");
			}
			if (string.IsNullOrEmpty(_params.get_Item("signature_method")))
			{
				throw new ArgumentNullException("signature_method");
			}
			sign(uri, method);
			string text = encodeRequestParameters(_params);
			return (!string.IsNullOrEmpty(realm)) ? (string.Format("OAuth realm=\"{0}\", ", (object)realm) + text) : ("OAuth " + text);
		}

		private void sign(string uri, string method)
		{
			string signatureBase = getSignatureBase(uri, method);
			HashAlgorithm hash = getHash();
			byte[] bytes = Encoding.get_ASCII().GetBytes(signatureBase);
			byte[] array = hash.ComputeHash(bytes);
			this["signature"] = Convert.ToBase64String(array);
		}

		private string getSignatureBase(string url, string method)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			Uri val = new Uri(url);
			string text = string.Format("{0}://{1}", (object)val.get_Scheme(), (object)val.get_Host());
			if ((!(val.get_Scheme() == "http") || val.get_Port() != 80) && (!(val.get_Scheme() == "https") || val.get_Port() != 443))
			{
				text = string.Concat((object)text, (object)":", (object)val.get_Port());
			}
			text += val.get_AbsolutePath();
			StringBuilder val2 = new StringBuilder();
			val2.Append(method).Append('&').Append(urlEncode(text))
				.Append('&');
			SortedDictionary<string, string> val3 = extractQueryParameters(val.get_Query());
			Enumerator<string, string> enumerator = _params.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					if (!string.IsNullOrEmpty(_params.get_Item(current.get_Key())) && !current.get_Key().EndsWith("_secret") && !current.get_Key().EndsWith("signature"))
					{
						val3.Add("oauth_" + current.get_Key(), current.get_Value());
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			StringBuilder val4 = new StringBuilder();
			Enumerator<string, string> enumerator2 = val3.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, string> current2 = enumerator2.get_Current();
					val4.AppendFormat("{0}={1}&", (object)current2.get_Key(), (object)current2.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2).Dispose();
			}
			val2.Append(urlEncode(((object)val4).ToString().TrimEnd(new char[1] { '&' })));
			return ((object)val2).ToString();
		}

		private HashAlgorithm getHash()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			if (this["signature_method"] != "HMAC-SHA1")
			{
				throw new NotImplementedException();
			}
			string text = string.Format("{0}&{1}", (object)urlEncode(this["consumer_secret"]), (object)urlEncode(this["token_secret"]));
			HMACSHA1 val = new HMACSHA1();
			((KeyedHashAlgorithm)val).set_Key(Encoding.get_ASCII().GetBytes(text));
			return (HashAlgorithm)(object)val;
		}
	}
}
