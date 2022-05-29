using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class UploadShopItemDataToServer : MonoBehaviour
{
	public UIToggle defaultToggle;

	public UIToggle defaultFilterToggle;

	public List<EditorShopItemData> itemsData;

	public UILabel buttonApplyLabel;

	public UISprite generateButton;

	public UIWidget filtersContainer;

	public UIWidget checkAllContainer;

	private UploadShopItemDataToServer.PlatformType _typePlatform;

	private UploadShopItemDataToServer.TypeWindow _typeWindow;

	public UploadShopItemDataToServer()
	{
	}

	public void ApplyButtonClick()
	{
		UploadShopItemDataToServer.TypeWindow typeWindow = this._typeWindow;
		if (typeWindow == UploadShopItemDataToServer.TypeWindow.UploadFileToServer)
		{
			this.UploadFileToServer();
		}
		else if (typeWindow == UploadShopItemDataToServer.TypeWindow.ChangePlatform)
		{
			this.generateButton.gameObject.SetActive(true);
			this.filtersContainer.gameObject.SetActive(true);
			this.checkAllContainer.gameObject.SetActive(true);
			this.defaultFilterToggle.@value = true;
		}
		this.Hide();
	}

	public void ChangeCurrentPlatform(UIToggle toggle)
	{
		int num;
		if (toggle == null || !toggle.@value)
		{
			return;
		}
		string str = toggle.name;
		if (str != null)
		{
			if (UploadShopItemDataToServer.u003cu003ef__switchu0024mapA == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(5)
				{
					{ "IOSCheckbox", 0 },
					{ "TestCheckbox", 1 },
					{ "AndroidCheckbox", 2 },
					{ "AmazonCheckbox", 3 },
					{ "WindowsPhoneCheckbox", 4 }
				};
				UploadShopItemDataToServer.u003cu003ef__switchu0024mapA = strs;
			}
			if (UploadShopItemDataToServer.u003cu003ef__switchu0024mapA.TryGetValue(str, out num))
			{
				switch (num)
				{
					case 0:
					{
						this._typePlatform = UploadShopItemDataToServer.PlatformType.IOS;
						break;
					}
					case 1:
					{
						this._typePlatform = UploadShopItemDataToServer.PlatformType.Test;
						break;
					}
					case 2:
					{
						this._typePlatform = UploadShopItemDataToServer.PlatformType.Android;
						break;
					}
					case 3:
					{
						this._typePlatform = UploadShopItemDataToServer.PlatformType.Amazon;
						break;
					}
					case 4:
					{
						this._typePlatform = UploadShopItemDataToServer.PlatformType.WindowsPhone;
						break;
					}
				}
			}
		}
	}

	private string CreatePhpFileByString(string text)
	{
		string fileNameForPlatform = this.GetFileNameForPlatform(this._typePlatform);
		try
		{
			if (File.Exists(fileNameForPlatform))
			{
				File.Delete(fileNameForPlatform);
			}
			using (FileStream fileStream = File.Create(fileNameForPlatform))
			{
				byte[] bytes = (new UTF8Encoding(true)).GetBytes(text);
				fileStream.Write(bytes, 0, (int)bytes.Length);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError(exception.ToString());
		}
		return fileNameForPlatform;
	}

	private string GenerateJsonStringWithData()
	{
		Dictionary<string, object> strs = new Dictionary<string, object>();
		List<List<object>> lists = new List<List<object>>();
		List<string> strs1 = new List<string>()
		{
			WeaponTags.DragonGun_Tag,
			WeaponTags.FreezeGun_0_Tag,
			WeaponTags.FreezeGunTag,
			WeaponTags.AK74Tag
		};
		List<string> strs2 = strs1;
		strs1 = new List<string>()
		{
			WeaponTags.RailgunTag,
			WeaponTags.MinigunTag,
			WeaponTags.GlockTag
		};
		List<string> strs3 = strs1;
		List<string> strs4 = new List<string>();
		List<string> strs5 = new List<string>();
		List<List<object>> lists1 = new List<List<object>>();
		for (int i = 0; i < this.itemsData.Count; i++)
		{
			if (this.itemsData[i].isNew)
			{
				strs4.Add(this.itemsData[i].tag);
			}
			if (this.itemsData[i].isTop)
			{
				strs5.Add(this.itemsData[i].tag);
			}
			if (this.itemsData[i].discount > 0)
			{
				List<object> objs = new List<object>()
				{
					this.itemsData[i].tag,
					this.itemsData[i].discount
				};
				lists1.Add(objs);
			}
		}
		strs.Add("discounts", lists);
		strs.Add("news", strs2);
		strs.Add("news_up", strs4);
		strs.Add("topSellers", strs3);
		strs.Add("topSellers_up", strs5);
		strs.Add("discounts_up", lists1);
		return Json.Serialize(strs);
	}

	public string GenerateTextForUploadFile()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?php");
		string str = this.GenerateJsonStringWithData().Replace("\"", "\\\"");
		str = string.Concat(str, "\\r\\n");
		stringBuilder.AppendFormat("$val = \"{0}\";\n", str);
		stringBuilder.AppendLine("echo $val;");
		stringBuilder.AppendLine("?>");
		return stringBuilder.ToString();
	}

	private string GetFileNameForPlatform(UploadShopItemDataToServer.PlatformType type)
	{
		switch (this._typePlatform)
		{
			case UploadShopItemDataToServer.PlatformType.IOS:
			{
				return "promo_actions.php";
			}
			case UploadShopItemDataToServer.PlatformType.Test:
			{
				return "promo_actions_test1.php";
			}
			case UploadShopItemDataToServer.PlatformType.Android:
			{
				return "promo_actions_android.php";
			}
			case UploadShopItemDataToServer.PlatformType.Amazon:
			{
				return "promo_actions_amazon.php";
			}
			case UploadShopItemDataToServer.PlatformType.WindowsPhone:
			{
				return "promo_actions_wp8.php";
			}
		}
		return "promo_actions_test1.php";
	}

	public string GetPromoActionUrl()
	{
		switch (this._typePlatform)
		{
			case UploadShopItemDataToServer.PlatformType.IOS:
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions.json";
			}
			case UploadShopItemDataToServer.PlatformType.Test:
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_test.json";
			}
			case UploadShopItemDataToServer.PlatformType.Android:
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_android.json";
			}
			case UploadShopItemDataToServer.PlatformType.Amazon:
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_amazon.json";
			}
			case UploadShopItemDataToServer.PlatformType.WindowsPhone:
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_wp8.json";
			}
		}
		return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions.json";
	}

	public void Hide()
	{
		base.gameObject.GetComponent<UIPanel>().alpha = 0f;
	}

	public void Show(UploadShopItemDataToServer.TypeWindow type)
	{
		base.gameObject.GetComponent<UIPanel>().alpha = 1f;
		this._typePlatform = UploadShopItemDataToServer.PlatformType.Test;
		this.defaultToggle.@value = true;
		this._typeWindow = type;
		if (this._typeWindow == UploadShopItemDataToServer.TypeWindow.UploadFileToServer)
		{
			this.buttonApplyLabel.text = "Upload to server";
		}
		else if (this._typeWindow == UploadShopItemDataToServer.TypeWindow.ChangePlatform)
		{
			this.buttonApplyLabel.text = "Download from server";
		}
	}

	private void UploadFileToServer()
	{
		string str = this.CreatePhpFileByString(this.GenerateTextForUploadFile());
		this.UploadPhpFileToServer(str);
		Debug.Log(str);
	}

	private void UploadPhpFileToServer(string fileName)
	{
		try
		{
			FtpWebRequest networkCredential = (FtpWebRequest)WebRequest.Create("ftp://secure.pixelgunserver.com//test.htm");
			networkCredential.Method = "STOR";
			networkCredential.UsePassive = false;
			networkCredential.Credentials = new NetworkCredential("rilisoft", "11QQwwee");
			FtpWebResponse response = (FtpWebResponse)networkCredential.GetResponse();
			Debug.Log(string.Format("Upload File Complete, status {0}", response.StatusDescription));
			response.Close();
		}
		catch (WebException webException)
		{
			Debug.Log(((FtpWebResponse)webException.Response).StatusDescription);
		}
	}

	private enum PlatformType
	{
		IOS,
		Test,
		Android,
		Amazon,
		WindowsPhone
	}

	public enum TypeWindow
	{
		UploadFileToServer,
		ChangePlatform
	}
}