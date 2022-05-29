using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Rilisoft
{
	public sealed class Tools
	{
		public static int AllAvailabelBotRaycastMask
		{
			get
			{
				return -5 & ~(1 << (LayerMask.NameToLayer("DamageCollider") & 31)) & ~(1 << (LayerMask.NameToLayer("NotDetectMobRaycast") & 31));
			}
		}

		public static int AllWithoutDamageCollidersMask
		{
			get
			{
				return -5 & ~(1 << (LayerMask.NameToLayer("DamageCollider") & 31));
			}
		}

		public static int AllWithoutDamageCollidersMaskAndWithoutRocket
		{
			get
			{
				return -5 & ~(1 << (LayerMask.NameToLayer("DamageCollider") & 31)) & ~(1 << (LayerMask.NameToLayer("Rocket") & 31));
			}
		}

		public static int AllWithoutMyPlayerMask
		{
			get
			{
				return -5 & ~(1 << (LayerMask.NameToLayer("MyPlayer") & 31));
			}
		}

		public static long CurrentUnixTime
		{
			get
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
				return (long)(DateTime.UtcNow - dateTime).TotalSeconds;
			}
		}

		public static UnityEngine.RuntimePlatform RuntimePlatform
		{
			get
			{
				return Application.platform;
			}
		}

		public Tools()
		{
		}

		public static void AddSessionNumber()
		{
			DateTimeOffset dateTimeOffset;
			DateTimeOffset dateTimeOffset1;
			int num = PlayerPrefs.GetInt(Defs.SessionNumberKey, 1);
			PlayerPrefs.SetInt(Defs.SessionNumberKey, num + 1);
			string str = PlayerPrefs.GetString(Defs.LastTimeSessionDayKey, string.Empty);
			DateTimeOffset utcNow = DateTimeOffset.UtcNow;
			DateTimeOffset.TryParse(utcNow.ToString("s"), out dateTimeOffset1);
			if (string.IsNullOrEmpty(str) || DateTimeOffset.TryParse(str, out dateTimeOffset) && (!Defs.IsDeveloperBuild && (dateTimeOffset1 - dateTimeOffset).TotalHours > 23 || Defs.IsDeveloperBuild && (dateTimeOffset1 - dateTimeOffset).TotalMinutes > 3))
			{
				int num1 = PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 0);
				PlayerPrefs.SetInt(Defs.SessionDayNumberKey, num1 + 1);
				PlayerPrefs.SetString(Defs.LastTimeSessionDayKey, DateTimeOffset.UtcNow.ToString("s"));
				GlobalGameController.CountDaySessionInCurrentVersion = GlobalGameController.CountDaySessionInCurrentVersion;
			}
		}

		private static bool ConnectedToPhoton()
		{
			return PhotonNetwork.room != null;
		}

		internal static WWW CreateWww(string url, WWWForm form, string comment = "")
		{
			return Tools.CreateWwwIf(true, url, form, comment, null);
		}

		internal static WWW CreateWwwIf(bool condition, string url, WWWForm form, string comment = "", Dictionary<string, string> headers = null)
		{
			WWW wWW;
			if (!condition)
			{
				wWW = null;
			}
			else
			{
				wWW = (headers == null ? new WWW(url, form) : new WWW(url, form.data, headers));
			}
			WWW wWW1 = wWW;
			if (Application.isEditor && FriendsController.isDebugLogWWW)
			{
				byte[] numArray = form.data ?? new byte[0];
				string str = Encoding.UTF8.GetString(numArray, 0, (int)numArray.Length);
				string str1 = str.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault<string>((string p) => p.StartsWith("action=")) ?? url;
				string str2 = (!string.IsNullOrEmpty(comment) ? string.Format("{0}; {1}", str1, comment) : str1);
				if (!condition)
				{
					Debug.LogFormat("<b><color=orange>Skipping {0}</color></b>", new object[] { str2 });
				}
				else
				{
					Debug.LogFormat("<b><color=yellow>{0}</color></b>", new object[] { str2 });
				}
			}
			return wWW1;
		}

		internal static WWW CreateWwwIfNotConnected(string url, WWWForm form, string comment = "", Dictionary<string, string> headers = null)
		{
			return Tools.CreateWwwIf(!Tools.ConnectedToPhoton(), url, form, comment, headers);
		}

		internal static WWW CreateWwwIfNotConnected(string url)
		{
			WWW wWW;
			if (!Tools.ConnectedToPhoton())
			{
				wWW = new WWW(url);
			}
			else
			{
				wWW = null;
			}
			WWW wWW1 = wWW;
			if (Application.isEditor && FriendsController.isDebugLogWWW)
			{
				string str = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault<string>() ?? url;
				if (wWW1 == null)
				{
					Debug.LogFormat("<color=orange>Skipping {0}</color>", new object[] { str });
				}
				else
				{
					Debug.LogFormat("<color=yellow>{0}</color>", new object[] { str });
				}
			}
			return wWW1;
		}

		internal static T DeserializeJson<T>(string json)
		{
			T t;
			if (string.IsNullOrEmpty(json))
			{
				return default(T);
			}
			try
			{
				t = JsonUtility.FromJson<T>(json);
			}
			catch (Exception exception)
			{
				Debug.LogWarning(exception);
				t = default(T);
			}
			return t;
		}

		public static bool EscapePressed()
		{
			if (BackSystem.Active)
			{
				return false;
			}
			return Input.GetKeyUp(KeyCode.Escape);
		}

		public static Color[] FlipColorsHorizontally(Color[] colors, int width, int height)
		{
			Color[] colorArray = new Color[(int)colors.Length];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					colorArray[i + width * j] = colors[width - i - 1 + width * j];
				}
			}
			return colorArray;
		}

		public static DateTime GetCurrentTimeByUnixTime(long unixTime)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return dateTime.AddSeconds((double)unixTime);
		}

		public static Texture2D GetPreviewFromSkin(string skinStr, Tools.PreviewType type)
		{
			Texture2D texture2D = null;
			if (string.IsNullOrEmpty(skinStr) || skinStr.Equals("empty"))
			{
				texture2D = Resources.Load<Texture2D>(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1"));
			}
			else
			{
				texture2D = new Texture2D(64, 32);
				texture2D.LoadImage(Convert.FromBase64String(skinStr));
			}
			Texture2D texture2D1 = null;
			Tools.PreviewType previewType = type;
			if (previewType == Tools.PreviewType.Head)
			{
				texture2D1 = new Texture2D(8, 8, TextureFormat.ARGB32, false);
				for (int i = 0; i < texture2D1.width; i++)
				{
					for (int j = 0; j < texture2D1.height; j++)
					{
						texture2D1.SetPixel(i, j, Color.clear);
					}
				}
				texture2D1.SetPixels(0, 0, 8, 8, texture2D.GetPixels(8, 16, 8, 8));
			}
			else if (previewType == Tools.PreviewType.HeadAndBody)
			{
				texture2D1 = new Texture2D(16, 14, TextureFormat.ARGB32, false);
				for (int k = 0; k < texture2D1.width; k++)
				{
					for (int l = 0; l < texture2D1.height; l++)
					{
						texture2D1.SetPixel(k, l, Color.clear);
					}
				}
				texture2D1.SetPixels(4, 6, 8, 8, texture2D.GetPixels(8, 16, 8, 8));
				texture2D1.SetPixels(4, 0, 8, 6, texture2D.GetPixels(20, 6, 8, 6));
				texture2D1.SetPixels(0, 0, 4, 6, texture2D.GetPixels(44, 6, 4, 6));
				texture2D1.SetPixels(12, 0, 4, 6, Tools.FlipColorsHorizontally(texture2D.GetPixels(44, 6, 4, 6), 4, 6));
			}
			texture2D1.anisoLevel = 1;
			texture2D1.mipMapBias = -0.5f;
			texture2D1.Apply();
			texture2D1.filterMode = FilterMode.Point;
			return texture2D1;
		}

		public static bool ParseDateTimeFromPlayerPrefs(string dateKey, out DateTime parsedDate)
		{
			return DateTime.TryParse(Storager.getString(dateKey, false), out parsedDate);
		}

		public static void SetLayerRecursively(GameObject obj, int newLayer)
		{
			if (null == obj)
			{
				return;
			}
			obj.layer = newLayer;
			int num = obj.transform.childCount;
			Transform transforms = obj.transform;
			for (int i = 0; i < num; i++)
			{
				Transform child = transforms.GetChild(i);
				if (null != child)
				{
					Tools.SetLayerRecursively(child.gameObject, newLayer);
				}
			}
		}

		public static void SetVibibleNguiObjectByAlpha(GameObject nguiObject, bool isVisible)
		{
			UIWidget component = nguiObject.GetComponent<UIWidget>();
			if (component == null)
			{
				return;
			}
			component.alpha = (!isVisible ? 0.001f : 1f);
		}

		public static Rect SuccessMessageRect()
		{
			return new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.5f, (float)Screen.height * 0.5f - (float)Screen.height * 0.0525f, (float)Screen.height, (float)Screen.height * 0.105f);
		}

		public enum PreviewType
		{
			Head,
			HeadAndBody
		}
	}
}