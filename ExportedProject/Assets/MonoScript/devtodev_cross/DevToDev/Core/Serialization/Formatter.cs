using System;
using System.Text;
using DevToDev.Core.Utils;

namespace DevToDev.Core.Serialization
{
	internal class Formatter<T> where T : ISaveable
	{
		public byte[] Save(T saveable)
		{
			ObjectInfo objectInfo = new ObjectInfo(saveable);
			string text = objectInfo.ToJson().ToJSON(0);
			byte[] bytes = Encoding.get_UTF8().GetBytes(text);
			if (UnityPlayerPlatform.isUnityWebPlatform())
			{
				return bytes;
			}
			return new GZipHelper().Pack(bytes);
		}

		public T Load(byte[] data)
		{
			string text = null;
			text = (UnityPlayerPlatform.isUnityWebPlatform() ? Encoding.get_UTF8().GetString(data, 0, data.Length) : new GZipHelper().UnPack(data));
			JSONNode json = null;
			try
			{
				json = JSON.Parse(text);
			}
			catch (global::System.Exception ex)
			{
				Log.D(ex.get_StackTrace());
			}
			ObjectInfo objectInfo = new ObjectInfo(json);
			return objectInfo.SelfValue(typeof(T)) as T;
		}
	}
}
