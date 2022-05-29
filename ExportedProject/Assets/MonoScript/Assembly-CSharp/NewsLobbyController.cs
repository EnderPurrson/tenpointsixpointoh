using I2.Loc;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NewsLobbyController : MonoBehaviour
{
	public UIGrid newsGrid;

	public UIScrollView newsScroll;

	public UIScrollView fullNewsScroll;

	public UILabel headerLabel;

	public UILabel descLabel;

	public UILabel desc2Label;

	public UILabel dateLabel;

	public UITexture newsPic;

	public string currentURL;

	public string currentNewsName;

	public int selectedIndex;

	public GameObject newsItemPrefab;

	public GameObject urlButton;

	private List<NewsLobbyItem> newsList = new List<NewsLobbyItem>();

	private List<Dictionary<string, object>> newsListInfo = new List<Dictionary<string, object>>();

	private Texture2D[] newsFullPic;

	public static NewsLobbyController sharedController;

	public NewsLobbyController()
	{
	}

	private void Awake()
	{
		NewsLobbyController.sharedController = this;
	}

	public void Close()
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController._isCancellationRequested = true;
		}
	}

	private void FillData()
	{
		object obj;
		object obj1;
		object obj2;
		for (int i = 0; i < this.newsList.Count; i++)
		{
			Dictionary<string, object> item = this.newsListInfo[i];
			Dictionary<string, object> strs = item["short_header"] as Dictionary<string, object>;
			Dictionary<string, object> item1 = item["short_description"] as Dictionary<string, object>;
			if (strs != null && item1 != null)
			{
				if (!strs.TryGetValue(LocalizationManager.CurrentLanguage, out obj))
				{
					strs.TryGetValue("English", out obj);
				}
				if (!item1.TryGetValue(LocalizationManager.CurrentLanguage, out obj1))
				{
					item1.TryGetValue("English", out obj1);
				}
				this.newsList[i].headerLabel.text = (string)obj;
				if (Convert.ToInt32(item["readed"]) != 0)
				{
					this.newsList[i].GetComponent<UISprite>().color = Color.gray;
					this.newsList[i].indicatorNew.SetActive(false);
				}
				else
				{
					this.newsList[i].GetComponent<UISprite>().color = Color.white;
					this.newsList[i].indicatorNew.SetActive(true);
				}
				this.newsList[i].shortDescLabel.text = (string)obj1;
				int num = Convert.ToInt32(item["date"]);
				TimeSpan offset = DateTimeOffset.Now.Offset;
				DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime((long)(num + offset.Hours * 3600));
				this.newsList[i].dateLabel.text = string.Concat(new object[] { currentTimeByUnixTime.Day.ToString("D2"), ".", currentTimeByUnixTime.Month.ToString("D2"), ".", currentTimeByUnixTime.Year, "\n", currentTimeByUnixTime.Hour, ":", currentTimeByUnixTime.Minute.ToString("D2") });
				if (item.TryGetValue("previewpicture", out obj2))
				{
					this.newsList[i].LoadPreview((string)obj2);
				}
			}
		}
	}

	private bool GetNews()
	{
		string str = PlayerPrefs.GetString("LobbyNewsKey", "[]");
		this.newsListInfo = (Json.Deserialize(str) as List<object>).OfType<Dictionary<string, object>>().ToList<Dictionary<string, object>>();
		if (this.newsListInfo == null || this.newsListInfo.Count == 0)
		{
			return false;
		}
		if (this.newsFullPic == null || this.newsListInfo.Count != (int)this.newsFullPic.Length)
		{
			this.newsFullPic = new Texture2D[this.newsListInfo.Count];
		}
		return true;
	}

	[DebuggerHidden]
	private IEnumerator LoadPictureForFullNews(int index, string picLink)
	{
		NewsLobbyController.u003cLoadPictureForFullNewsu003ec__IteratorCE variable = null;
		return variable;
	}

	private void OnDisable()
	{
	}

	private void OnEnable()
	{
		this.UpdateNewsList();
	}

	public void OnNewsItemClick()
	{
		ButtonClickSound.TryPlayClick();
		int num = 0;
		while (num < this.newsList.Count)
		{
			if (!this.newsList[num].GetComponent<UIToggle>().@value)
			{
				num++;
			}
			else
			{
				this.SetNewsIndex(num);
				break;
			}
		}
	}

	public void OnURLClick()
	{
		if (string.IsNullOrEmpty(this.currentURL))
		{
			return;
		}
		try
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Conversion Total", "Source" },
				{ "Conversion By News", this.currentNewsName }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("News", strs, true);
			Dictionary<string, object> strs1 = new Dictionary<string, object>()
			{
				{ "Conversion Total", "Source" },
				{ "Conversion By News", this.currentNewsName }
			};
			AnalyticsFacade.SendCustomEvent("News", strs1);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in log News: ", exception));
		}
		Application.OpenURL(this.currentURL);
	}

	private void SaveReaded()
	{
		PlayerPrefs.SetString("LobbyNewsKey", Json.Serialize(this.newsListInfo));
		bool flag = false;
		for (int i = 0; i < this.newsListInfo.Count; i++)
		{
			if (Convert.ToInt32(this.newsListInfo[i]["readed"]) == 0)
			{
				flag = true;
			}
		}
		PlayerPrefs.SetInt("LobbyIsAnyNewsKey", (!flag ? 0 : 1));
		MainMenuController.sharedController.newsIndicator.SetActive(flag);
		PlayerPrefs.Save();
	}

	private void SetNewsIndex(int index)
	{
		object obj;
		object obj1;
		object obj2;
		object obj3;
		object obj4;
		this.selectedIndex = index;
		this.fullNewsScroll.ResetPosition();
		Dictionary<string, object> item = this.newsListInfo[index];
		Dictionary<string, object> strs = item["header"] as Dictionary<string, object>;
		Dictionary<string, object> item1 = item["description"] as Dictionary<string, object>;
		Dictionary<string, object> strs1 = item["category"] as Dictionary<string, object>;
		if (strs == null || item1 == null || strs1 == null)
		{
			return;
		}
		if (!strs.TryGetValue(LocalizationManager.CurrentLanguage, out obj))
		{
			strs.TryGetValue("English", out obj);
		}
		if (!item1.TryGetValue(LocalizationManager.CurrentLanguage, out obj1))
		{
			item1.TryGetValue("English", out obj1);
		}
		if (!strs1.TryGetValue(LocalizationManager.CurrentLanguage, out obj4))
		{
			strs1.TryGetValue("English", out obj4);
		}
		if (!item.TryGetValue("URL", out obj2) || obj2.Equals(string.Empty))
		{
			this.currentURL = string.Empty;
			this.urlButton.SetActive(false);
		}
		else
		{
			this.currentURL = (string)obj2;
			this.currentNewsName = (!strs.ContainsKey("English") ? "NO ENGLISH TRANSLATION" : strs["English"].ToString());
			this.urlButton.SetActive(true);
		}
		this.headerLabel.text = (string)obj;
		string str = (string)obj1;
		string[] strArrays = str.Split(new string[] { "[news-pic]" }, StringSplitOptions.None);
		item.TryGetValue("fullpicture", out obj3);
		if ((int)strArrays.Length <= 1 || string.IsNullOrEmpty((string)obj3))
		{
			this.descLabel.text = (string)obj1;
			this.desc2Label.text = string.Empty;
			this.newsPic.aspectRatio = 200f;
			this.newsPic.enabled = false;
		}
		else
		{
			this.descLabel.text = strArrays[0];
			this.desc2Label.text = strArrays[1];
			this.newsPic.enabled = true;
			base.StartCoroutine(this.LoadPictureForFullNews(index, (string)obj3));
		}
		int num = Convert.ToInt32(item["date"]);
		TimeSpan offset = DateTimeOffset.Now.Offset;
		DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime((long)(num + offset.Hours * 3600));
		UILabel uILabel = this.dateLabel;
		object[] year = new object[] { "[bababa]", null, null, null, null, null, null, null };
		year[1] = currentTimeByUnixTime.Day.ToString("D2");
		year[2] = ".";
		year[3] = currentTimeByUnixTime.Month.ToString("D2");
		year[4] = ".";
		year[5] = currentTimeByUnixTime.Year;
		year[6] = " / [-]";
		year[7] = obj4;
		uILabel.text = string.Concat(year);
		try
		{
			if (Convert.ToInt32(item["readed"]) == 0)
			{
				Dictionary<string, string> strs2 = new Dictionary<string, string>()
				{
					{ "CTR", "Open" },
					{ "Conversion Total", "Open" },
					{ "News", (!strs.ContainsKey("English") ? "NO ENGLISH TRANSLATION" : strs["English"].ToString()) }
				};
				FlurryPluginWrapper.LogEventAndDublicateToConsole("News", strs2, true);
				Dictionary<string, object> strs3 = new Dictionary<string, object>()
				{
					{ "CTR", "Open" },
					{ "Conversion Total", "Open" },
					{ "News", (!strs.ContainsKey("English") ? "NO ENGLISH TRANSLATION" : strs["English"].ToString()) }
				};
				AnalyticsFacade.SendCustomEvent("News", strs3);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in log News: ", exception));
		}
		item["readed"] = 1;
		this.FillData();
		this.SaveReaded();
	}

	private void UpdateItemsCount()
	{
		while (this.newsList.Count < this.newsListInfo.Count)
		{
			GameObject gameObject = NGUITools.AddChild(this.newsGrid.gameObject, this.newsItemPrefab);
			gameObject.SetActive(true);
			this.newsList.Add(gameObject.GetComponent<NewsLobbyItem>());
		}
		while (this.newsList.Count > this.newsListInfo.Count)
		{
			UnityEngine.Object.Destroy(this.newsList[this.newsList.Count - 1].gameObject);
			this.newsList.RemoveAt(this.newsList.Count - 1);
		}
		this.newsGrid.Reposition();
		this.newsScroll.ResetPosition();
	}

	public void UpdateNewsList()
	{
		if (!this.GetNews())
		{
			while (this.newsList.Count > 0)
			{
				UnityEngine.Object.Destroy(this.newsList[this.newsList.Count - 1].gameObject);
				this.newsList.RemoveAt(this.newsList.Count - 1);
			}
			this.headerLabel.text = LocalizationStore.Get("Key_1807");
			this.dateLabel.text = string.Empty;
			this.descLabel.text = string.Empty;
			this.desc2Label.text = string.Empty;
			this.newsPic.aspectRatio = 200f;
			this.newsPic.enabled = false;
			this.urlButton.SetActive(false);
		}
		else
		{
			this.UpdateItemsCount();
			this.FillData();
			for (int i = 0; i < this.newsList.Count; i++)
			{
				this.newsList[i].GetComponent<UIToggle>().Set(false);
			}
			this.newsList[0].GetComponent<UIToggle>().Set(true);
			this.SetNewsIndex(0);
			this.newsScroll.enabled = this.newsListInfo.Count > 4;
		}
	}
}