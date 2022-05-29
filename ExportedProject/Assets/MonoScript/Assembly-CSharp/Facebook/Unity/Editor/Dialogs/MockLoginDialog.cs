using Facebook.MiniJSON;
using Facebook.Unity;
using Facebook.Unity.Editor;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facebook.Unity.Editor.Dialogs
{
	internal class MockLoginDialog : EditorFacebookMockDialog
	{
		private string accessToken = string.Empty;

		protected override string DialogTitle
		{
			get
			{
				return "Mock Login Dialog";
			}
		}

		public MockLoginDialog()
		{
		}

		protected override void DoGui()
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("User Access Token:", new GUILayoutOption[0]);
			this.accessToken = GUILayout.TextField(this.accessToken, GUI.skin.textArea, new GUILayoutOption[] { GUILayout.MinWidth(400f) });
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			if (GUILayout.Button("Find Access Token", new GUILayoutOption[0]))
			{
				Application.OpenURL(string.Format("https://developers.facebook.com/tools/accesstoken/?app_id={0}", FB.AppId));
			}
			GUILayout.Space(20f);
		}

		protected override void SendSuccessResult()
		{
			if (string.IsNullOrEmpty(this.accessToken))
			{
				this.SendErrorResult("Empty Access token string");
				return;
			}
			FB.API(string.Concat("/me?fields=id&access_token=", this.accessToken), HttpMethod.GET, (IGraphResult graphResult) => {
				if (!string.IsNullOrEmpty(graphResult.Error))
				{
					this.SendErrorResult(string.Concat("Graph API error: ", graphResult.Error));
					return;
				}
				string str = graphResult.ResultDictionary["id"] as string;
				FB.API(string.Concat("/me/permissions?access_token=", this.accessToken), HttpMethod.GET, (IGraphResult permResult) => {
					if (!string.IsNullOrEmpty(permResult.Error))
					{
						this.SendErrorResult(string.Concat("Graph API error: ", permResult.Error));
						return;
					}
					List<string> strs = new List<string>();
					List<string> strs1 = new List<string>();
					foreach (Dictionary<string, object> item in permResult.ResultDictionary["data"] as List<object>)
					{
						if (item["status"] as string != "granted")
						{
							strs1.Add(item["permission"] as string);
						}
						else
						{
							strs.Add(item["permission"] as string);
						}
					}
					IDictionary<string, object> callbackID = (IDictionary<string, object>)Json.Deserialize((new AccessToken(this.accessToken, str, DateTime.Now.AddDays(60), strs, new DateTime?(DateTime.Now))).ToJson());
					callbackID.Add("granted_permissions", strs);
					callbackID.Add("declined_permissions", strs1);
					if (!string.IsNullOrEmpty(this.CallbackID))
					{
						callbackID["callback_id"] = this.CallbackID;
					}
					if (this.Callback != null)
					{
						this.Callback(Json.Serialize(callbackID));
					}
				}, (IDictionary<string, string>)null);
			}, (IDictionary<string, string>)null);
		}
	}
}