using System;
using System.Collections;
using System.Diagnostics;
using FyberPlugin.LitJson;
using UnityEngine;

namespace FyberPlugin
{
	public class FyberGameObject : MonoBehaviour
	{
		private const string ObjectName = "FyberGameObject";

		static FyberGameObject()
		{
			global::System.Type typeFromHandle = typeof(FyberGameObject);
			try
			{
				MonoBehaviour monoBehaviour = Object.FindObjectOfType(typeFromHandle) as MonoBehaviour;
				if (!(monoBehaviour != null))
				{
					GameObject gameObject = new GameObject("FyberGameObject");
					gameObject.AddComponent(typeFromHandle);
					Object.DontDestroyOnLoad(gameObject);
				}
			}
			catch (UnityException)
			{
				Debug.LogWarning(string.Concat((object)"It looks like you have the ", (object)typeFromHandle, (object)" on a GameObject in your scene. Please remove the script from your scene."));
			}
		}

		public static void Init()
		{
		}

		private void OnNativeMessageReceived(string json)
		{
			NativeMessage nativeMessage = JsonMapper.ToObject<NativeMessage>(json);
			if (string.IsNullOrEmpty(nativeMessage.Id))
			{
				FyberCallback.Instance.OnNativeError("An unknown error occurred while processing the ads. Please request again.");
			}
			else
			{
				FyberCallbacksManager.Instance.Process(nativeMessage);
			}
		}

		private void OnNativeErrorOccurred(string json)
		{
			NativeError nativeError = JsonMapper.ToObject<NativeError>(json);
			FyberCallback.Instance.OnNativeError(nativeError.Error);
			if (!string.IsNullOrEmpty(nativeError.Id))
			{
				FyberCallbacksManager.Instance.ClearCallbacks(nativeError.Id);
			}
		}

		[DebuggerHidden]
		internal global::System.Collections.IEnumerator SkipFrameCoroutineWithBlock(Action<int> block)
		{
			yield return null;
			block.Invoke(0);
		}

		public void SkipFrameWithBlock(Action<int> block)
		{
			StartCoroutine(SkipFrameCoroutineWithBlock(block));
		}
	}
}
