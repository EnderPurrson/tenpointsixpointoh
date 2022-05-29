using Facebook.MiniJSON;
using Facebook.Unity;
using Facebook.Unity.Editor;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Facebook.Unity.Editor.Dialogs
{
	internal class MockShareDialog : EditorFacebookMockDialog
	{
		protected override string DialogTitle
		{
			get
			{
				return string.Concat("Mock ", this.SubTitle, " Dialog");
			}
		}

		public string SubTitle
		{
			private get;
			set;
		}

		public MockShareDialog()
		{
		}

		protected override void DoGui()
		{
		}

		private string GenerateFakePostID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(AccessToken.CurrentAccessToken.UserId);
			stringBuilder.Append('\u005F');
			for (int i = 0; i < 17; i++)
			{
				stringBuilder.Append(UnityEngine.Random.Range(0, 10));
			}
			return stringBuilder.ToString();
		}

		protected override void SendCancelResult()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			strs["cancelled"] = "true";
			if (!string.IsNullOrEmpty(base.CallbackID))
			{
				strs["callback_id"] = base.CallbackID;
			}
			base.Callback(Json.Serialize(strs));
		}

		protected override void SendSuccessResult()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			if (!FB.IsLoggedIn)
			{
				strs["did_complete"] = true;
			}
			else
			{
				strs["postId"] = this.GenerateFakePostID();
			}
			if (!string.IsNullOrEmpty(base.CallbackID))
			{
				strs["callback_id"] = base.CallbackID;
			}
			if (base.Callback != null)
			{
				base.Callback(Json.Serialize(strs));
			}
		}
	}
}