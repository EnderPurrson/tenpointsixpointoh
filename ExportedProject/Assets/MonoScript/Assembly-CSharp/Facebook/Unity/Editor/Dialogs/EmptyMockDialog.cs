using Facebook.MiniJSON;
using Facebook.Unity.Editor;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facebook.Unity.Editor.Dialogs
{
	internal class EmptyMockDialog : EditorFacebookMockDialog
	{
		protected override string DialogTitle
		{
			get
			{
				return this.EmptyDialogTitle;
			}
		}

		public string EmptyDialogTitle
		{
			get;
			set;
		}

		public EmptyMockDialog()
		{
		}

		protected override void DoGui()
		{
		}

		protected override void SendSuccessResult()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			strs["did_complete"] = true;
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