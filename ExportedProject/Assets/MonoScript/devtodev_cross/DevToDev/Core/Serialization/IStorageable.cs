using System;
using System.IO;
using System.Text;
using DevToDev.Core.Utils;
using DevToDev.Logic;
using UnityEngine;

namespace DevToDev.Core.Serialization
{
	public abstract class IStorageable<T> : ISaveable where T : ISaveable
	{
		public abstract string StorageName();

		public abstract ISaveable GetBlankObject();

		public abstract ISaveable GetObject(byte[] data);

		public abstract byte[] SaveObject(ISaveable obj);

		public void Save(ISaveable storage)
		{
			string text = FullPath();
			string text2 = FileName();
			Stream val = null;
			if (!UnityPlayerPlatform.isUnityWebPlatform())
			{
				try
				{
					if (!Directory.Exists(text))
					{
						try
						{
							Directory.CreateDirectory(text);
						}
						catch
						{
						}
					}
					string text3 = text + text2;
					if (File.Exists(text3))
					{
						try
						{
							File.Delete(text3);
						}
						catch
						{
						}
					}
					val = (Stream)(object)File.Open(text3, (FileMode)1);
					byte[] array = SaveObject(storage);
					val.Write(array, 0, array.Length);
					DataSaverPlatform.CloseStream(val);
					return;
				}
				catch (global::System.Exception ex)
				{
					Log.E("Error write to file: " + ex.get_Message() + "\n" + ex.get_StackTrace());
					return;
				}
				finally
				{
					if (val != null)
					{
						DataSaverPlatform.CloseStream(val);
					}
				}
			}
			try
			{
				byte[] array2 = SaveObject(storage);
				PlayerPrefs.SetString(text + text2, Encoding.get_UTF8().GetString(array2, 0, array2.Length));
				PlayerPrefs.Save();
			}
			catch (global::System.Exception ex2)
			{
				Log.E(ex2.get_Message() + ":" + ex2.get_StackTrace());
			}
		}

		public ISaveable Load()
		{
			ISaveable saveable = null;
			string text = FullPath();
			string text2 = FileName();
			Stream val = null;
			try
			{
				saveable = GetBlankObject();
				((object)saveable).GetType();
				if (!UnityPlayerPlatform.isUnityWebPlatform())
				{
					val = (Stream)(object)File.Open(text + text2, (FileMode)3);
					byte[] array = new byte[val.get_Length()];
					val.Read(array, 0, array.Length);
					saveable = GetObject(array);
					if (val != null)
					{
						DataSaverPlatform.CloseStream(val);
						return saveable;
					}
					return saveable;
				}
				try
				{
					string @string = PlayerPrefs.GetString(text + text2);
					if (!string.IsNullOrEmpty(@string))
					{
						saveable = GetObject(Encoding.get_UTF8().GetBytes(@string));
						return saveable;
					}
					return saveable;
				}
				catch (global::System.Exception ex)
				{
					Log.E(ex.get_Message() + ":" + ex.get_StackTrace());
					return saveable;
				}
			}
			catch (global::System.Exception ex2)
			{
				Log.D(string.Concat(new string[6]
				{
					"File not found: ",
					text,
					text2,
					"\r\n",
					ex2.get_Message(),
					ex2.get_StackTrace()
				}));
				if (val != null)
				{
					DataSaverPlatform.CloseStream(val);
				}
				if (saveable != null)
				{
					Save(saveable);
					return saveable;
				}
				return saveable;
			}
		}

		private string FullPath()
		{
			if (!UnityPlayerPlatform.isUnityWebPlatform())
			{
				return DataSaverPlatform.GetFullPath(SDKClient.Instance.AppKey);
			}
			return "devtodev_" + SDKClient.Instance.AppKey + "_";
		}

		private string FileName()
		{
			return StorageName();
		}
	}
}
