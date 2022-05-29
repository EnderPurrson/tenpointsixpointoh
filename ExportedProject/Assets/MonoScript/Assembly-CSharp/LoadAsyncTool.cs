using System;
using System.Collections.Generic;
using UnityEngine;

public class LoadAsyncTool
{
	private static Dictionary<string, LoadAsyncTool.ObjectRequest> bufferDict;

	private static string[] keyBuffer;

	private static int currentIndex;

	static LoadAsyncTool()
	{
		LoadAsyncTool.bufferDict = new Dictionary<string, LoadAsyncTool.ObjectRequest>();
		LoadAsyncTool.keyBuffer = new string[70];
	}

	public LoadAsyncTool()
	{
	}

	private static void AddToBuffer(string key, LoadAsyncTool.ObjectRequest value)
	{
		if (LoadAsyncTool.bufferDict.ContainsKey(key))
		{
			return;
		}
		if (LoadAsyncTool.keyBuffer[LoadAsyncTool.currentIndex] != null)
		{
			LoadAsyncTool.bufferDict[LoadAsyncTool.keyBuffer[LoadAsyncTool.currentIndex]].asset = null;
			LoadAsyncTool.bufferDict.Remove(LoadAsyncTool.keyBuffer[LoadAsyncTool.currentIndex]);
		}
		LoadAsyncTool.keyBuffer[LoadAsyncTool.currentIndex] = key;
		LoadAsyncTool.bufferDict.Add(key, value);
		LoadAsyncTool.currentIndex++;
		if (LoadAsyncTool.currentIndex >= (int)LoadAsyncTool.keyBuffer.Length)
		{
			if (Device.isPixelGunLow)
			{
				Resources.UnloadUnusedAssets();
				Debug.Log("<color=#FF5555>Resources.UnloadUnusedAssets</color>");
			}
			LoadAsyncTool.currentIndex = 0;
		}
	}

	public static LoadAsyncTool.ObjectRequest Get(string path, bool loadImmediately = false)
	{
		LoadAsyncTool.ObjectRequest fromBuffer;
		if (!Device.isPixelGunLow)
		{
			fromBuffer = LoadAsyncTool.GetFromBuffer(path);
		}
		else
		{
			fromBuffer = null;
		}
		LoadAsyncTool.ObjectRequest objectRequest = fromBuffer;
		if (objectRequest == null)
		{
			objectRequest = new LoadAsyncTool.ObjectRequest(path, loadImmediately);
			if (!Device.isPixelGunLow)
			{
				LoadAsyncTool.AddToBuffer(path, objectRequest);
			}
		}
		return objectRequest;
	}

	private static LoadAsyncTool.ObjectRequest GetFromBuffer(string key)
	{
		if (!LoadAsyncTool.bufferDict.ContainsKey(key))
		{
			return null;
		}
		return LoadAsyncTool.bufferDict[key];
	}

	public class ObjectRequest
	{
		private ResourceRequest request;

		private bool done;

		private string assetPath;

		private UnityEngine.Object _asset;

		public UnityEngine.Object asset
		{
			get
			{
				if (this._asset == null && !this.isDone)
				{
					this.LoadImmediately();
				}
				return this._asset;
			}
			set
			{
				this._asset = value;
			}
		}

		public bool isDone
		{
			get
			{
				if (this.done)
				{
					return true;
				}
				if (!this.request.isDone)
				{
					return false;
				}
				Debug.Log(string.Concat("<color=#5555FF>Request done: ", this.assetPath, "</color>"));
				this.asset = this.request.asset;
				this.request = null;
				this.done = true;
				return true;
			}
		}

		public ObjectRequest(string path, bool loadImmediately)
		{
			this.assetPath = path;
			if (loadImmediately)
			{
				this.LoadImmediately();
			}
			else
			{
				Debug.Log(string.Concat("<color=#5555FF>Load: ", this.assetPath, "</color>"));
				this.request = Resources.LoadAsync(this.assetPath);
			}
		}

		public void LoadImmediately()
		{
			if (this.request == null)
			{
				Debug.Log(string.Concat("<color=#5555FF>Load immediately: ", this.assetPath, "</color>"));
			}
			else
			{
				this.request = null;
				Debug.Log(string.Concat("<color=#5555FF>Load immediately (async request not done): ", this.assetPath, "</color>"));
			}
			this.asset = Resources.Load(this.assetPath);
			this.done = true;
		}
	}
}