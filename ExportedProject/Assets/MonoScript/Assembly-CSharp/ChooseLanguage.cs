using System;
using UnityEngine;

public class ChooseLanguage : MonoBehaviour
{
	public UIButton[] languageButtons;

	private UIButton _currentLanguage;

	public ChooseLanguage()
	{
	}

	private void SelectLanguage(string languageName)
	{
		ButtonClickSound.TryPlayClick();
		LocalizationStore.CurrentLanguage = languageName;
		this.SetSelectCurrentLanguage();
	}

	public void SetBrazilLanguage()
	{
		this.SelectLanguage("Portuguese (Brazil)");
	}

	public void SetChinseLanguage()
	{
		this.SelectLanguage("Chinese (Chinese)");
	}

	public void SetDeutschLanguage()
	{
		this.SelectLanguage("German");
	}

	public void SetEnglishLanguage()
	{
		this.SelectLanguage("English");
	}

	public void SetEspanolaLanguage()
	{
		this.SelectLanguage("Spanish");
	}

	public void SetFrancianLanguage()
	{
		this.SelectLanguage("French");
	}

	public void SetJapanLanguage()
	{
		this.SelectLanguage("Japanese");
	}

	public void SetKoreanLanguage()
	{
		this.SelectLanguage("Korean");
	}

	public void SetRussianLanguage()
	{
		this.SelectLanguage("Russian");
	}

	private void SetSelectCurrentLanguage()
	{
		int currentLanguageIndex = LocalizationStore.GetCurrentLanguageIndex();
		if (currentLanguageIndex == -1 || currentLanguageIndex >= (int)this.languageButtons.Length)
		{
			return;
		}
		if (this._currentLanguage != null)
		{
			this._currentLanguage.ResetDefaultColor();
		}
		this.languageButtons[currentLanguageIndex].defaultColor = Color.grey;
		this._currentLanguage = this.languageButtons[currentLanguageIndex];
	}

	private void Start()
	{
		this.SetSelectCurrentLanguage();
	}
}