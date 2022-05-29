using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facebook.Unity.Editor
{
	internal abstract class EditorFacebookMockDialog : MonoBehaviour
	{
		private Rect modalRect;

		private GUIStyle modalStyle;

		public EditorFacebookMockDialog.OnComplete Callback
		{
			protected get;
			set;
		}

		public string CallbackID
		{
			protected get;
			set;
		}

		protected abstract string DialogTitle
		{
			get;
		}

		protected EditorFacebookMockDialog()
		{
		}

		protected abstract void DoGui();

		public void OnGUI()
		{
			GUI.ModalWindow(this.GetHashCode(), this.modalRect, new GUI.WindowFunction(this.OnGUIDialog), this.DialogTitle, this.modalStyle);
		}

		private void OnGUIDialog(int windowId)
		{
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("Warning! Mock dialog responses will NOT match production dialogs", new GUILayoutOption[0]);
			GUILayout.Label("Test your app on one of the supported platforms", new GUILayoutOption[0]);
			this.DoGui();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUIContent gUIContent = new GUIContent("Send Success");
			if (GUI.Button(GUILayoutUtility.GetRect(gUIContent, GUI.skin.button), gUIContent))
			{
				this.SendSuccessResult();
				UnityEngine.Object.Destroy(this);
			}
			GUIContent gUIContent1 = new GUIContent("Send Cancel");
			if (GUI.Button(GUILayoutUtility.GetRect(gUIContent1, GUI.skin.button), gUIContent1, GUI.skin.button))
			{
				this.SendCancelResult();
				UnityEngine.Object.Destroy(this);
			}
			GUIContent gUIContent2 = new GUIContent("Send Error");
			if (GUI.Button(GUILayoutUtility.GetRect(gUIContent1, GUI.skin.button), gUIContent2, GUI.skin.button))
			{
				this.SendErrorResult("Error: Error button pressed");
				UnityEngine.Object.Destroy(this);
			}
			GUILayout.EndHorizontal();
		}

		protected virtual void SendCancelResult()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			strs["cancelled"] = true;
			if (!string.IsNullOrEmpty(this.CallbackID))
			{
				strs["callback_id"] = this.CallbackID;
			}
			this.Callback(Json.Serialize(strs));
		}

		protected virtual void SendErrorResult(string errorMessage)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			strs["error"] = errorMessage;
			if (!string.IsNullOrEmpty(this.CallbackID))
			{
				strs["callback_id"] = this.CallbackID;
			}
			this.Callback(Json.Serialize(strs));
		}

		protected abstract void SendSuccessResult();

		public void Start()
		{
			this.modalRect = new Rect(10f, 10f, (float)(Screen.width - 20), (float)(Screen.height - 20));
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1f));
			texture2D.Apply();
			this.modalStyle = new GUIStyle();
			this.modalStyle.normal.background = texture2D;
		}

		public delegate void OnComplete(string result);
	}
}