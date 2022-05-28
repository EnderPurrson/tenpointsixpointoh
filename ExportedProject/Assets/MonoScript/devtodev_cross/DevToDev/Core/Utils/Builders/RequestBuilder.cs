using System;
using System.Collections.Generic;
using System.Text;
using DevToDev.Core.Network;

namespace DevToDev.Core.Utils.Builders
{
	internal class RequestBuilder
	{
		public static readonly string DEFAULT_SIGNATURE_LETTER = "s";

		private string url;

		private string secret;

		private SortedDictionary<string, string> parameters;

		private byte[] postData;

		private bool isNeedSigned;

		public RequestBuilder()
		{
			parameters = new SortedDictionary<string, string>();
		}

		public RequestBuilder Url(string url)
		{
			this.url = url;
			return this;
		}

		public RequestBuilder AddParameter(string key, object value)
		{
			parameters.Add(key, value.ToString());
			return this;
		}

		public RequestBuilder NeedSigned(bool isNeedSigned)
		{
			this.isNeedSigned = isNeedSigned;
			return this;
		}

		public RequestBuilder Secret(string secret)
		{
			this.secret = secret;
			return this;
		}

		public RequestBuilder PostData(byte[] postData)
		{
			this.postData = postData;
			return this;
		}

		public Request Build()
		{
			return new Request(CalculateUrl(), (postData != null) ? new GZipHelper().Pack(postData) : null);
		}

		private string CalculateUrl()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder val = new StringBuilder();
			Enumerator<string, string> enumerator = parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					val.AppendFormat("{0}={1}&", (object)current.get_Key(), (object)Uri.EscapeDataString(current.get_Value()));
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			val.Remove(val.get_Length() - 1, 1);
			if (isNeedSigned)
			{
				val.AppendFormat("&{0}={1}", (object)DEFAULT_SIGNATURE_LETTER, (object)Sign());
			}
			return ((object)new StringBuilder(url).Append((object)val)).ToString();
		}

		private string Sign()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (secret == null && secret.Equals(string.Empty))
			{
				Log.D("Sign required but secret key doesn't set");
			}
			List<byte> val = new List<byte>();
			Enumerator<string, string> enumerator = parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					val.AddRange((global::System.Collections.Generic.IEnumerable<byte>)Encoding.get_UTF8().GetBytes(string.Format("{0}={1}", (object)current.get_Key(), (object)current.get_Value())));
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			if (postData != null)
			{
				val.AddRange((global::System.Collections.Generic.IEnumerable<byte>)postData);
			}
			val.AddRange((global::System.Collections.Generic.IEnumerable<byte>)Encoding.get_UTF8().GetBytes(secret));
			return MD5Helper.GetMd5(val.ToArray());
		}
	}
}
