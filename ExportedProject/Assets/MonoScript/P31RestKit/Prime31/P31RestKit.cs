using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Prime31
{
	public class P31RestKit
	{
		protected string _baseUrl;

		public bool debugRequests = false;

		protected bool forceJsonResponse;

		private GameObject _surrogateGameObject;

		private MonoBehaviour _surrogateMonobehaviour;

		protected virtual GameObject surrogateGameObject
		{
			get
			{
				if (_surrogateGameObject == null)
				{
					_surrogateGameObject = GameObject.Find("P31CoroutineSurrogate");
					if (_surrogateGameObject == null)
					{
						_surrogateGameObject = new GameObject("P31CoroutineSurrogate");
						Object.DontDestroyOnLoad(_surrogateGameObject);
					}
				}
				return _surrogateGameObject;
			}
			set
			{
				_surrogateGameObject = value;
			}
		}

		protected MonoBehaviour surrogateMonobehaviour
		{
			get
			{
				if (_surrogateMonobehaviour == null)
				{
					_surrogateMonobehaviour = surrogateGameObject.AddComponent<MonoBehaviour>();
				}
				return _surrogateMonobehaviour;
			}
			set
			{
				_surrogateMonobehaviour = value;
			}
		}

		[DebuggerHidden]
		protected virtual global::System.Collections.IEnumerator send(string path, HTTPVerb httpVerb, Dictionary<string, object> parameters, Action<string, object> onComplete)
		{
			if (path.StartsWith("/"))
			{
				path = path.Substring(1);
			}
			WWW www = processRequest(path, httpVerb, parameters);
			yield return www;
			if (debugRequests)
			{
				Debug.Log("response error: " + www.error);
				Debug.Log("response text: " + www.text);
				StringBuilder val = new StringBuilder();
				val.Append("Response Headers:\n");
				Enumerator<string, string> enumerator = www.responseHeaders.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> current = enumerator.get_Current();
						val.AppendFormat("{0}: {1}\n", (object)current.get_Key(), (object)current.get_Value());
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				Debug.Log(((object)val).ToString());
			}
			if (onComplete != null)
			{
				processResponse(www, onComplete);
			}
			www.Dispose();
		}

		protected virtual WWW processRequest(string path, HTTPVerb httpVerb, Dictionary<string, object> parameters)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder val = new StringBuilder();
			if (!path.StartsWith("http"))
			{
				val.Append(_baseUrl).Append(path);
			}
			else
			{
				val.Append(path);
			}
			bool flag = httpVerb != HTTPVerb.GET;
			WWWForm wWWForm = ((!flag) ? null : new WWWForm());
			if (parameters != null && parameters.get_Count() > 0)
			{
				if (flag)
				{
					Enumerator<string, object> enumerator = parameters.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, object> current = enumerator.get_Current();
							if (current.get_Value() is string)
							{
								wWWForm.AddField(current.get_Key(), current.get_Value() as string);
							}
							else if (current.get_Value() is byte[])
							{
								wWWForm.AddBinaryData(current.get_Key(), current.get_Value() as byte[]);
							}
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator).Dispose();
					}
				}
				else
				{
					bool flag2 = true;
					if (path.Contains("?"))
					{
						flag2 = false;
					}
					Enumerator<string, object> enumerator2 = parameters.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<string, object> current2 = enumerator2.get_Current();
							if (current2.get_Value() is string)
							{
								val.AppendFormat("{0}{1}={2}", (object)((!flag2) ? "&" : "?"), (object)WWW.EscapeURL(current2.get_Key()), (object)WWW.EscapeURL(current2.get_Value() as string));
								flag2 = false;
							}
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2).Dispose();
					}
				}
			}
			if (debugRequests)
			{
				Debug.Log("url: " + ((object)val).ToString());
			}
			Dictionary<string, string> val2 = null;
			if (flag)
			{
				IDictionary headersFromForm = getHeadersFromForm(wWWForm);
				if (headersFromForm != null)
				{
					val2 = new Dictionary<string, string>();
					if (headersFromForm.Contains((object)"Content-Type"))
					{
						val2.Add("Content-Type", headersFromForm.get_Item((object)"Content-Type").ToString());
					}
					if (debugRequests)
					{
						Debug.Log("Found a POST request. Fetching headers from WWWForm and starting with these as a base: ");
						Utils.logObject(val2);
					}
				}
			}
			return (!flag) ? new WWW(((object)val).ToString()) : new WWW(((object)val).ToString(), wWWForm.data, headersForRequest(httpVerb, val2));
		}

		protected virtual Dictionary<string, string> headersForRequest(HTTPVerb httpVerb, Dictionary<string, string> headers = null)
		{
			headers = headers ?? new Dictionary<string, string>();
			switch (httpVerb)
			{
			case HTTPVerb.GET:
				headers.Add("METHOD", "GET");
				break;
			case HTTPVerb.POST:
				headers.Add("METHOD", "POST");
				break;
			case HTTPVerb.PUT:
				headers.Add("METHOD", "PUT");
				headers.Add("X-HTTP-Method-Override", "PUT");
				break;
			case HTTPVerb.DELETE:
				headers.Add("METHOD", "DELETE");
				headers.Add("X-HTTP-Method-Override", "DELETE");
				break;
			}
			return headers;
		}

		protected virtual void processResponse(WWW www, Action<string, object> onComplete)
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				onComplete.Invoke(www.error, (object)default(object));
			}
			else if (isResponseJson(www))
			{
				object obj = Json.decode(www.text);
				if (obj == null)
				{
					obj = www.text;
				}
				onComplete.Invoke((string)default(string), obj);
			}
			else
			{
				onComplete.Invoke((string)default(string), (object)www.text);
			}
		}

		protected bool isResponseJson(WWW www)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			if (forceJsonResponse)
			{
				flag = true;
			}
			if (!flag)
			{
				Enumerator<string, string> enumerator = www.responseHeaders.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> current = enumerator.get_Current();
						if (current.get_Key().ToLower() == "content-type" && (current.get_Value().ToLower().Contains("/json") || current.get_Value().ToLower().Contains("/javascript")))
						{
							flag = true;
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
			if (flag && !www.text.StartsWith("[") && !www.text.StartsWith("{"))
			{
				return false;
			}
			return flag;
		}

		protected virtual IDictionary getHeadersFromForm(WWWForm form)
		{
			try
			{
				PropertyInfo property = ((object)form).GetType().GetProperty("headers");
				if (property != null)
				{
					object value = property.GetValue((object)form, (object[])default(object[]));
					return (IDictionary)((value is IDictionary) ? value : null);
				}
				Debug.Log(string.Concat((object)"couldnt find the 'headers' property on the WWWForm object: ", (object)form));
			}
			catch (global::System.Exception ex)
			{
				Debug.Log(string.Concat((object)"ran into a problem transferring headers from WWWForm to the WWW request: ", (object)ex));
			}
			return null;
		}

		public void setBaseUrl(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		public void get(string path, Action<string, object> completionHandler)
		{
			get(path, null, completionHandler);
		}

		public void get(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler)
		{
			surrogateMonobehaviour.StartCoroutine(send(path, HTTPVerb.GET, parameters, completionHandler));
		}

		public void post(string path, Action<string, object> completionHandler)
		{
			post(path, null, completionHandler);
		}

		public void post(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler)
		{
			surrogateMonobehaviour.StartCoroutine(send(path, HTTPVerb.POST, parameters, completionHandler));
		}

		public void put(string path, Action<string, object> completionHandler)
		{
			put(path, null, completionHandler);
		}

		public void put(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler)
		{
			surrogateMonobehaviour.StartCoroutine(send(path, HTTPVerb.PUT, parameters, completionHandler));
		}
	}
}
