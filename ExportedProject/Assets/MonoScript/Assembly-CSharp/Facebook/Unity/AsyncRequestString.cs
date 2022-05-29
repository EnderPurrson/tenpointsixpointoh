using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facebook.Unity
{
	internal class AsyncRequestString : MonoBehaviour
	{
		private Uri url;

		private HttpMethod method;

		private IDictionary<string, string> formData;

		private WWWForm query;

		private FacebookDelegate<IGraphResult> callback;

		public AsyncRequestString()
		{
		}

		internal static void Get(Uri url, Dictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			AsyncRequestString.Request(url, HttpMethod.GET, formData, callback);
		}

		internal static void Post(Uri url, Dictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			AsyncRequestString.Request(url, HttpMethod.POST, formData, callback);
		}

		internal static void Request(Uri url, HttpMethod method, WWWForm query = null, FacebookDelegate<IGraphResult> callback = null)
		{
			ComponentFactory.AddComponent<AsyncRequestString>().SetUrl(url).SetMethod(method).SetQuery(query).SetCallback(callback);
		}

		internal static void Request(Uri url, HttpMethod method, IDictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			ComponentFactory.AddComponent<AsyncRequestString>().SetUrl(url).SetMethod(method).SetFormData(formData).SetCallback(callback);
		}

		internal AsyncRequestString SetCallback(FacebookDelegate<IGraphResult> callback)
		{
			this.callback = callback;
			return this;
		}

		internal AsyncRequestString SetFormData(IDictionary<string, string> formData)
		{
			this.formData = formData;
			return this;
		}

		internal AsyncRequestString SetMethod(HttpMethod method)
		{
			this.method = method;
			return this;
		}

		internal AsyncRequestString SetQuery(WWWForm query)
		{
			this.query = query;
			return this;
		}

		internal AsyncRequestString SetUrl(Uri url)
		{
			this.url = url;
			return this;
		}

		[DebuggerHidden]
		internal IEnumerator Start()
		{
			AsyncRequestString.u003cStartu003ec__Iterator27 variable = null;
			return variable;
		}
	}
}