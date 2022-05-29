using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace I2.Loc
{
	public static class LocalizationManager
	{
		private static string mCurrentLanguage;

		private static string mLanguageCode;

		public static bool IsRight2Left;

		public static List<LanguageSource> Sources;

		private static string[] LanguagesRTL;

		public static string CurrentLanguage
		{
			get
			{
				if (string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage))
				{
					LocalizationManager.RegisterSceneSources();
					LocalizationManager.RegisterSourceInResources();
					LocalizationManager.SelectStartupLanguage();
				}
				return LocalizationManager.mCurrentLanguage;
			}
			set
			{
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(value);
				if (LocalizationManager.mCurrentLanguage != value && !string.IsNullOrEmpty(supportedLanguage))
				{
					LocalizationManager.mCurrentLanguage = supportedLanguage;
					LocalizationManager.CurrentLanguageCode = LocalizationManager.GetLanguageCode(supportedLanguage);
					LocalizationManager.LocalizeAll();
				}
			}
		}

		public static string CurrentLanguageCode
		{
			get
			{
				return LocalizationManager.mLanguageCode;
			}
			set
			{
				LocalizationManager.mLanguageCode = value;
				LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
			}
		}

		static LocalizationManager()
		{
			LocalizationManager.IsRight2Left = false;
			LocalizationManager.Sources = new List<LanguageSource>();
			LocalizationManager.LanguagesRTL = new string[] { "ar-DZ", "ar", "ar-BH", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA", "ar-OM", "ar-QA", "ar-SA", "ar-SY", "ar-TN", "ar-AE", "ar-YE", "he", "ur", "ji" };
		}

		internal static void AddSource(LanguageSource Source)
		{
			if (LocalizationManager.Sources.Contains(Source))
			{
				return;
			}
			LocalizationManager.Sources.Add(Source);
			Source.Import_Google();
		}

		public static UnityEngine.Object FindAsset(string value)
		{
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				UnityEngine.Object obj = LocalizationManager.Sources[num].FindAsset(value);
				if (obj)
				{
					return obj;
				}
				num++;
			}
			return null;
		}

		public static List<string> GetAllLanguages()
		{
			List<string> strs = new List<string>();
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				int num1 = 0;
				int count1 = LocalizationManager.Sources[num].mLanguages.Count;
				while (num1 < count1)
				{
					if (!strs.Contains(LocalizationManager.Sources[num].mLanguages[num1].Name))
					{
						strs.Add(LocalizationManager.Sources[num].mLanguages[num1].Name);
					}
					num1++;
				}
				num++;
			}
			return strs;
		}

		public static string GetLanguageCode(string Language)
		{
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				int languageIndex = LocalizationManager.Sources[num].GetLanguageIndex(Language, true);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[num].mLanguages[languageIndex].Code;
				}
				num++;
			}
			return string.Empty;
		}

		public static string GetSupportedLanguage(string Language)
		{
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				int languageIndex = LocalizationManager.Sources[num].GetLanguageIndex(Language, false);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[num].mLanguages[languageIndex].Name;
				}
				num++;
			}
			int num1 = 0;
			int count1 = LocalizationManager.Sources.Count;
			while (num1 < count1)
			{
				int languageIndex1 = LocalizationManager.Sources[num1].GetLanguageIndex(Language, true);
				if (languageIndex1 >= 0)
				{
					return LocalizationManager.Sources[num1].mLanguages[languageIndex1].Name;
				}
				num1++;
			}
			return string.Empty;
		}

		public static string GetTermTranslation(string Term)
		{
			return LocalizationManager.GetTranslation(Term);
		}

		public static string GetTermTranslationByDefault(string term)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.RegisterSourceInResources();
			}
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				TermData termData = LocalizationManager.Sources[num].GetTermData(term);
				if (termData == null)
				{
					return string.Empty;
				}
				if ((int)termData.Languages.Length != 0)
				{
					return termData.Languages[0];
				}
				num++;
			}
			return string.Empty;
		}

		public static string GetTranslation(string term)
		{
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				TermData termData = LocalizationManager.Sources[num].GetTermData(term);
				if (termData != null)
				{
					int languageIndex = LocalizationManager.Sources[num].GetLanguageIndex(LocalizationManager.CurrentLanguage, true);
					if (languageIndex != -1)
					{
						string languages = termData.Languages[languageIndex];
						if (!string.IsNullOrEmpty(languages))
						{
							return languages;
						}
					}
					if ((int)termData.Languages.Length != 0)
					{
						return termData.Languages[0];
					}
				}
				num++;
			}
			return term;
		}

		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true)
		{
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				if (LocalizationManager.Sources[num].GetLanguageIndex(Language, false) >= 0)
				{
					return true;
				}
				num++;
			}
			if (AllowDiscartingRegion)
			{
				int num1 = 0;
				int count1 = LocalizationManager.Sources.Count;
				while (num1 < count1)
				{
					if (LocalizationManager.Sources[num1].GetLanguageIndex(Language, true) >= 0)
					{
						return true;
					}
					num1++;
				}
			}
			return false;
		}

		private static bool IsRTL(string Code)
		{
			return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
		}

		internal static void LocalizeAll()
		{
			Localize[] localizeArray = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int num = 0;
			int length = (int)localizeArray.Length;
			while (num < length)
			{
				localizeArray[num].OnLocalize();
				num++;
			}
			if (LocalizationManager.OnLocalizeEvent != null)
			{
				LocalizationManager.OnLocalizeEvent();
			}
			ResourceManager.pInstance.CleanResourceCache();
		}

		private static void RegisterSceneSources()
		{
			LanguageSource[] languageSourceArray = (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource));
			int num = 0;
			int length = (int)languageSourceArray.Length;
			while (num < length)
			{
				if (!LocalizationManager.Sources.Contains(languageSourceArray[num]))
				{
					LocalizationManager.AddSource(languageSourceArray[num]);
				}
				num++;
			}
		}

		private static void RegisterSourceInResources()
		{
			LanguageSource component;
			GameObject asset = ResourceManager.pInstance.GetAsset<GameObject>("I2Languages");
			if (!asset)
			{
				component = null;
			}
			else
			{
				component = asset.GetComponent<LanguageSource>();
			}
			LanguageSource languageSource = component;
			if (languageSource && !LocalizationManager.Sources.Contains(languageSource))
			{
				LocalizationManager.AddSource(languageSource);
			}
		}

		internal static void RemoveSource(LanguageSource Source)
		{
			LocalizationManager.Sources.Remove(Source);
		}

		private static void SelectStartupLanguage()
		{
			string str = PlayerPrefs.GetString(Defs.CurrentLanguage, string.Empty);
			string str1 = Application.systemLanguage.ToString();
			if (LocalizationManager.HasLanguage(str, true))
			{
				LocalizationManager.CurrentLanguage = str;
				return;
			}
			string supportedLanguage = LocalizationManager.GetSupportedLanguage(str1);
			if (!string.IsNullOrEmpty(supportedLanguage))
			{
				LocalizationManager.CurrentLanguage = supportedLanguage;
				return;
			}
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				if (LocalizationManager.Sources[num].mLanguages.Count > 0)
				{
					LocalizationManager.CurrentLanguage = LocalizationManager.Sources[num].mLanguages[0].Name;
					return;
				}
				num++;
			}
		}

		public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

		public delegate void OnLocalizeCallback();
	}
}