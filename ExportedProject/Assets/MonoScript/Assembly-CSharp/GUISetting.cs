using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GUISetting : MonoBehaviour
{
	public GUIStyle back;

	public GUIStyle soundOnOff;

	public GUIStyle restore;

	public GUIStyle sliderStyle;

	public GUIStyle thumbStyle;

	public Texture settingPlashka;

	public Texture settingTitle;

	public Texture fon;

	public Texture slow_fast;

	public Texture polzunok;

	private float mySens;

	public GUISetting()
	{
	}

	private void OnDestroy()
	{
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void OnGUI()
	{
		object obj;
		GUI.depth = 2;
		float single = (float)Screen.height / 768f;
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 683f * single, 0f, 1366f * single, (float)Screen.height), this.fon);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)this.settingPlashka.width * single * 0.5f, (float)Screen.height * 0.52f - (float)this.settingPlashka.height * single * 0.5f, (float)this.settingPlashka.width * single, (float)this.settingPlashka.height * single), this.settingPlashka);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)this.settingTitle.width / 2f * Defs.Coef, (float)Screen.height * 0.08f, (float)this.settingTitle.width * Defs.Coef, (float)this.settingTitle.height * Defs.Coef), this.settingTitle);
		Rect rect = new Rect((float)Screen.width * 0.5f - (float)this.soundOnOff.normal.background.width * 0.5f * single, (float)Screen.height * 0.52f - (float)this.soundOnOff.normal.background.height * 0.5f * single, (float)this.soundOnOff.normal.background.width * single, (float)this.soundOnOff.normal.background.height * single);
		bool flag = PlayerPrefsX.GetBool(PlayerPrefsX.SndSetting, true);
		flag = GUI.Toggle(rect, flag, string.Empty, this.soundOnOff);
		if (!flag)
		{
			obj = null;
		}
		else
		{
			obj = 1;
		}
		AudioListener.volume = (float)obj;
		PlayerPrefsX.SetBool(PlayerPrefsX.SndSetting, flag);
		PlayerPrefs.Save();
		Rect rect1 = new Rect((float)Screen.width * 0.5f - (float)this.soundOnOff.normal.background.width * 0.5f * single, (float)Screen.height * 0.72f - (float)this.soundOnOff.normal.background.height * 0.5f * single, (float)this.soundOnOff.normal.background.width * single, (float)this.soundOnOff.normal.background.height * single);
		bool isChatOn = Defs.IsChatOn;
		isChatOn = GUI.Toggle(rect1, isChatOn, string.Empty, this.soundOnOff);
		Defs.IsChatOn = isChatOn;
		PlayerPrefs.Save();
		if (GUI.Button(new Rect(21f * single, (float)Screen.height - (21f + (float)this.back.normal.background.height) * single, (float)this.back.normal.background.width * single, (float)this.back.normal.background.height * single), string.Empty, this.back))
		{
			FlurryPluginWrapper.LogEvent("Back to Main Menu");
			Singleton<SceneLoader>.Instance.LoadScene(Defs.MainMenuScene, LoadSceneMode.Single);
		}
		GUI.enabled = !StoreKitEventListener.purchaseInProcess;
		Rect rect2 = new Rect((float)Screen.width / 2f - (float)this.restore.normal.background.width * single * 0.5f, (float)Screen.height - (21f + (float)this.restore.normal.background.height) * single, (float)this.restore.normal.background.width * single, (float)this.restore.normal.background.height * single);
		if (GUI.Button(rect2, string.Empty, this.restore))
		{
			StoreKitEventListener.purchaseInProcess = true;
		}
		GUI.enabled = true;
		this.sliderStyle.fixedWidth = (float)this.slow_fast.width * single;
		this.sliderStyle.fixedHeight = (float)this.slow_fast.height * single;
		this.thumbStyle.fixedWidth = (float)this.polzunok.width * single;
		this.thumbStyle.fixedHeight = (float)this.polzunok.height * single;
		Rect rect3 = new Rect((float)Screen.width * 0.5f - (float)this.slow_fast.width * 0.5f * single, (float)Screen.height * 0.81f - (float)this.slow_fast.height * 0.5f * single, (float)this.slow_fast.width * single, (float)this.slow_fast.height * single);
		this.mySens = GUI.HorizontalSlider(rect3, Defs.Sensitivity, 6f, 18f, this.sliderStyle, this.thumbStyle);
		Defs.Sensitivity = this.mySens;
	}

	private void Start()
	{
	}

	private void Update()
	{
		ActivityIndicator.IsActiveIndicator = StoreKitEventListener.purchaseInProcess;
	}
}