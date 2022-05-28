using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Facebook.MiniJSON;
using UnityEngine;

namespace Facebook.Unity.Editor.Dialogs
{
	internal class MockLoginDialog : EditorFacebookMockDialog
	{
		[CompilerGenerated]
		private sealed class _003CSendSuccessResult_003Ec__AnonStorey1F0
		{
			internal string facebookID;

			internal MockLoginDialog _003C_003Ef__this;

			internal void _003C_003Em__49(IGraphResult permResult)
			{
				if (!string.IsNullOrEmpty(permResult.Error))
				{
					_003C_003Ef__this.SendErrorResult("Graph API error: " + permResult.Error);
					return;
				}
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				List<object> list3 = permResult.ResultDictionary["data"] as List<object>;
				foreach (Dictionary<string, object> item in list3)
				{
					if (item["status"] as string== "granted")
					{
						list.Add(item["permission"] as string);
					}
					else
					{
						list2.Add(item["permission"] as string);
					}
				}
				AccessToken accessToken = new AccessToken(_003C_003Ef__this.accessToken, facebookID, DateTime.Now.AddDays(60.0), list, DateTime.Now);
				IDictionary<string, object> dictionary2 = (IDictionary<string, object>)Json.Deserialize(accessToken.ToJson());
				dictionary2.Add("granted_permissions", list);
				dictionary2.Add("declined_permissions", list2);
				if (!string.IsNullOrEmpty(_003C_003Ef__this.CallbackID))
				{
					dictionary2["callback_id"] = _003C_003Ef__this.CallbackID;
				}
				if (_003C_003Ef__this.Callback != null)
				{
					_003C_003Ef__this.Callback(Json.Serialize(dictionary2));
				}
			}
		}

		private string accessToken = string.Empty;

		protected override string DialogTitle
		{
			get
			{
				return "Mock Login Dialog";
			}
		}

		protected override void DoGui()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("User Access Token:");
			accessToken = GUILayout.TextField(accessToken, GUI.skin.textArea, GUILayout.MinWidth(400f));
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			if (GUILayout.Button("Find Access Token"))
			{
				Application.OpenURL(string.Format("https://developers.facebook.com/tools/accesstoken/?app_id={0}", FB.AppId));
			}
			GUILayout.Space(20f);
		}

		protected override void SendSuccessResult()
		{
			if (string.IsNullOrEmpty(accessToken))
			{
				SendErrorResult("Empty Access token string");
			}
			else
			{
				FB.API("/me?fields=id&access_token=" + accessToken, HttpMethod.GET, _003CSendSuccessResult_003Em__48);
			}
		}

		[CompilerGenerated]
		private void _003CSendSuccessResult_003Em__48(IGraphResult graphResult)
		{
			_003CSendSuccessResult_003Ec__AnonStorey1F0 _003CSendSuccessResult_003Ec__AnonStorey1F = new _003CSendSuccessResult_003Ec__AnonStorey1F0();
			_003CSendSuccessResult_003Ec__AnonStorey1F._003C_003Ef__this = this;
			if (!string.IsNullOrEmpty(graphResult.Error))
			{
				SendErrorResult("Graph API error: " + graphResult.Error);
				return;
			}
			_003CSendSuccessResult_003Ec__AnonStorey1F.facebookID = graphResult.ResultDictionary["id"] as string;
			FB.API("/me/permissions?access_token=" + accessToken, HttpMethod.GET, _003CSendSuccessResult_003Ec__AnonStorey1F._003C_003Em__49);
		}
	}
}
