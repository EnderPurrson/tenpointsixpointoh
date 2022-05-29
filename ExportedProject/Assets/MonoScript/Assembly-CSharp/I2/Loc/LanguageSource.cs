using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/Source")]
	public class LanguageSource : MonoBehaviour
	{
		public string Google_WebServiceURL;

		public string Google_SpreadsheetKey;

		public string Google_SpreadsheetName;

		public string Google_LastUpdatedVersion;

		private CoroutineManager mCoroutineManager;

		public static string EmptyCategory;

		public static char[] CategorySeparators;

		public List<TermData> mTerms = new List<TermData>();

		public List<LanguageData> mLanguages = new List<LanguageData>();

		[NonSerialized]
		public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>();

		public UnityEngine.Object[] Assets;

		public bool NeverDestroy = true;

		public bool UserAgreesToHaveItOnTheScene;

		static LanguageSource()
		{
			LanguageSource.EmptyCategory = "Default";
			LanguageSource.CategorySeparators = "/\\".ToCharArray();
		}

		public LanguageSource()
		{
		}

		public void AddLanguage(string LanguageName, string LanguageCode)
		{
			if (this.GetLanguageIndex(LanguageName, true) >= 0)
			{
				return;
			}
			LanguageData languageDatum = new LanguageData()
			{
				Name = LanguageName,
				Code = LanguageCode
			};
			this.mLanguages.Add(languageDatum);
			int count = this.mLanguages.Count;
			int num = 0;
			int count1 = this.mTerms.Count;
			while (num < count1)
			{
				Array.Resize<string>(ref this.mTerms[num].Languages, count);
				num++;
			}
		}

		public TermData AddTerm(string term)
		{
			return this.AddTerm(term, eTermType.Text);
		}

		public TermData AddTerm(string NewTerm, eTermType termType)
		{
			LanguageSource.ValidateFullTerm(ref NewTerm);
			NewTerm = NewTerm.Trim();
			TermData termData = this.GetTermData(NewTerm);
			if (termData == null)
			{
				termData = new TermData()
				{
					Term = NewTerm,
					TermType = termType,
					Languages = new string[this.mLanguages.Count]
				};
				this.mTerms.Add(termData);
				this.mDictionary.Add(NewTerm, termData);
			}
			return termData;
		}

		private static void AppendString(StringBuilder Builder, string Text)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny(",\n\"".ToCharArray()) < 0)
			{
				Builder.Append(Text);
			}
			else
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}\"", Text);
			}
		}

		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = LanguageSource.GetLanguageWithoutRegion(Language1);
			Language2 = LanguageSource.GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		private void Awake()
		{
			if (this.NeverDestroy)
			{
				if (this.ManagerHasASimilarSource())
				{
					UnityEngine.Object.Destroy(this);
					return;
				}
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			LocalizationManager.AddSource(this);
			this.UpdateDictionary();
		}

		public void ClearAllData()
		{
			this.mTerms.Clear();
			this.mLanguages.Clear();
			this.mDictionary.Clear();
		}

		public bool ContainsTerm(string term)
		{
			return this.GetTermData(term) != null;
		}

		internal static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators));
			if (num >= 0)
			{
				Category = FullTerm.Substring(0, num);
				Key = FullTerm.Substring(num + 1);
			}
			else
			{
				Category = LanguageSource.EmptyCategory;
				Key = FullTerm;
			}
		}

		public string Export_CSV(string Category)
		{
			string term;
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.mLanguages.Count;
			stringBuilder.Append("Key,Type,Desc");
			foreach (LanguageData mLanguage in this.mLanguages)
			{
				stringBuilder.Append(",");
				LanguageSource.AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(mLanguage.Name, mLanguage.Code));
			}
			stringBuilder.Append("\n");
			foreach (TermData mTerm in this.mTerms)
			{
				if (string.IsNullOrEmpty(Category) || Category == LanguageSource.EmptyCategory && mTerm.Term.IndexOfAny(LanguageSource.CategorySeparators) < 0)
				{
					term = mTerm.Term;
				}
				else if (!mTerm.Term.StartsWith(Category) || !(Category != mTerm.Term))
				{
					continue;
				}
				else
				{
					term = mTerm.Term.Substring(Category.Length + 1);
				}
				LanguageSource.AppendString(stringBuilder, term);
				stringBuilder.AppendFormat(",{0}", mTerm.TermType.ToString());
				stringBuilder.Append(",");
				LanguageSource.AppendString(stringBuilder, mTerm.Description);
				for (int i = 0; i < Mathf.Min(count, (int)mTerm.Languages.Length); i++)
				{
					stringBuilder.Append(",");
					LanguageSource.AppendString(stringBuilder, mTerm.Languages[i]);
				}
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}

		private string Export_Google_CreateData()
		{
			List<string> categories = this.GetCategories(true);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string category in categories)
			{
				if (!flag)
				{
					stringBuilder.Append("<I2Loc>");
				}
				else
				{
					flag = false;
				}
				string str = this.Export_CSV(category);
				stringBuilder.Append(category);
				stringBuilder.Append("<I2Loc>");
				stringBuilder.Append(str);
			}
			return stringBuilder.ToString();
		}

		public WWW Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = 1)
		{
			string str = this.Export_Google_CreateData();
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("key", this.Google_SpreadsheetKey);
			wWWForm.AddField("action", "SetLanguageSource");
			wWWForm.AddField("data", str);
			wWWForm.AddField("updateMode", UpdateMode.ToString());
			return new WWW(this.Google_WebServiceURL, wWWForm);
		}

		public UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int num = 0;
				int length = (int)this.Assets.Length;
				while (num < length)
				{
					if (this.Assets[num] != null && this.Assets[num].name == Name)
					{
						return this.Assets[num];
					}
					num++;
				}
			}
			return null;
		}

		public List<string> GetCategories(bool OnlyMainCategory = false)
		{
			List<string> strs = new List<string>();
			foreach (TermData mTerm in this.mTerms)
			{
				string categoryFromFullTerm = LanguageSource.GetCategoryFromFullTerm(mTerm.Term, OnlyMainCategory);
				if (strs.Contains(categoryFromFullTerm))
				{
					continue;
				}
				strs.Add(categoryFromFullTerm);
			}
			strs.Sort();
			return strs;
		}

		internal static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators));
			return (num >= 0 ? FullTerm.Substring(0, num) : LanguageSource.EmptyCategory);
		}

		internal static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators));
			return (num >= 0 ? FullTerm.Substring(num + 1) : FullTerm);
		}

		public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true)
		{
			int num = 0;
			int count = this.mLanguages.Count;
			while (num < count)
			{
				if (string.Compare(this.mLanguages[num].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return num;
				}
				num++;
			}
			if (AllowDiscartingRegion)
			{
				int num1 = 0;
				int count1 = this.mLanguages.Count;
				while (num1 < count1)
				{
					if (LanguageSource.AreTheSameLanguage(this.mLanguages[num1].Name, language))
					{
						return num1;
					}
					num1++;
				}
			}
			return -1;
		}

		public List<string> GetLanguages()
		{
			List<string> strs = new List<string>();
			int num = 0;
			int count = this.mLanguages.Count;
			while (num < count)
			{
				strs.Add(this.mLanguages[num].Name);
				num++;
			}
			return strs;
		}

		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				return Language;
			}
			return Language.Substring(0, num).Trim();
		}

		public string GetSourceName()
		{
			string str = base.gameObject.name;
			for (Transform i = base.transform.parent; i; i = i.parent)
			{
				str = string.Concat(i.name, "_", str);
			}
			return str;
		}

		public TermData GetTermData(string term)
		{
			TermData termDatum;
			if (this.mDictionary.Count == 0)
			{
				this.UpdateDictionary();
			}
			this.mDictionary.TryGetValue(term, out termDatum);
			return termDatum;
		}

		public List<string> GetTermsList()
		{
			return new List<string>(this.mDictionary.Keys);
		}

		public string GetTermTranslation(string term)
		{
			int languageIndex = this.GetLanguageIndex(LocalizationManager.CurrentLanguage, true);
			if (languageIndex < 0)
			{
				return string.Empty;
			}
			TermData termData = this.GetTermData(term);
			if (termData == null)
			{
				return string.Empty;
			}
			return termData.Languages[languageIndex];
		}

		public static eTermType GetTermType(string type)
		{
			int num = 0;
			int num1 = 8;
			while (num <= num1)
			{
				if (string.Equals((eTermType)num.ToString(), type, StringComparison.OrdinalIgnoreCase))
				{
					return (eTermType)num;
				}
				num++;
			}
			return eTermType.Text;
		}

		public bool HasAsset(UnityEngine.Object Obj)
		{
			return Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		public bool HasGoogleSpreadsheet()
		{
			return (string.IsNullOrEmpty(this.Google_WebServiceURL) ? false : !string.IsNullOrEmpty(this.Google_SpreadsheetKey));
		}

		public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = 1)
		{
			string str;
			string str1;
			List<string[]> strArrays = LocalizationReader.ReadCSV(CSVstring);
			string[] item = strArrays[0];
			if (UpdateMode == eSpreadsheetUpdateMode.Replace)
			{
				this.ClearAllData();
			}
			int count = Mathf.Max((int)item.Length - 3, 0);
			int[] numArray = new int[count];
			for (int i = 0; i < count; i++)
			{
				GoogleLanguages.UnPackCodeFromLanguageName(item[i + 3], out str, out str1);
				int languageIndex = this.GetLanguageIndex(str, true);
				if (languageIndex < 0)
				{
					LanguageData languageDatum = new LanguageData()
					{
						Name = str,
						Code = str1
					};
					this.mLanguages.Add(languageDatum);
					languageIndex = this.mLanguages.Count - 1;
				}
				numArray[i] = languageIndex;
			}
			count = this.mLanguages.Count;
			int num = 0;
			int count1 = this.mTerms.Count;
			while (num < count1)
			{
				TermData termDatum = this.mTerms[num];
				if ((int)termDatum.Languages.Length < count)
				{
					Array.Resize<string>(ref termDatum.Languages, count);
				}
				num++;
			}
			int num1 = 1;
			int count2 = strArrays.Count;
			while (num1 < count2)
			{
				item = strArrays[num1];
				string str2 = (!string.IsNullOrEmpty(Category) ? string.Concat(Category, "/", item[0]) : item[0]);
				LanguageSource.ValidateFullTerm(ref str2);
				TermData termData = this.GetTermData(str2);
				if (termData != null)
				{
					if (UpdateMode != eSpreadsheetUpdateMode.AddNewTerms)
					{
						goto Label1;
					}
					goto Label0;
				}
				else
				{
					termData = new TermData()
					{
						Term = str2,
						Languages = new string[this.mLanguages.Count]
					};
					for (int j = 0; j < this.mLanguages.Count; j++)
					{
						termData.Languages[j] = string.Empty;
					}
					this.mTerms.Add(termData);
					this.mDictionary.Add(str2, termData);
				}
			Label1:
				termData.TermType = LanguageSource.GetTermType(item[1]);
				termData.Description = item[2];
				for (int k = 0; k < (int)numArray.Length && k < (int)item.Length - 3; k++)
				{
					if (!string.IsNullOrEmpty(item[k + 3]))
					{
						termData.Languages[numArray[k]] = item[k + 3];
					}
				}
			Label0:
				num1++;
			}
			return string.Empty;
		}

		public void Import_Google()
		{
		}

		[DebuggerHidden]
		private IEnumerator Import_Google_Coroutine()
		{
			LanguageSource.u003cImport_Google_Coroutineu003ec__Iterator81 variable = null;
			return variable;
		}

		public WWW Import_Google_CreateWWWcall(bool ForceUpdate = false)
		{
			if (!this.HasGoogleSpreadsheet())
			{
				return null;
			}
			return new WWW(string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", this.Google_WebServiceURL, this.Google_SpreadsheetKey, (!ForceUpdate ? this.Google_LastUpdatedVersion : "0")));
		}

		public void Import_Google_Result(string JsonString, eSpreadsheetUpdateMode UpdateMode)
		{
			if (JsonString == "\"\"")
			{
				UnityEngine.Debug.Log("Language Source was up to date");
				return;
			}
			if (UpdateMode == eSpreadsheetUpdateMode.Replace)
			{
				this.ClearAllData();
			}
			this.Import_CSV(string.Empty, JsonString, UpdateMode);
		}

		public bool IsEqualTo(LanguageSource Source)
		{
			if (Source.mLanguages.Count != this.mLanguages.Count)
			{
				return false;
			}
			int num = 0;
			int count = this.mLanguages.Count;
			while (num < count)
			{
				if (Source.GetLanguageIndex(this.mLanguages[num].Name, true) < 0)
				{
					return false;
				}
				num++;
			}
			return true;
		}

		internal bool ManagerHasASimilarSource()
		{
			int num = 0;
			int count = LocalizationManager.Sources.Count;
			while (num < count)
			{
				LanguageSource item = LocalizationManager.Sources[num];
				if (item != null && item.IsEqualTo(this) && item != this)
				{
					return true;
				}
				num++;
			}
			return false;
		}

		public void RemoveLanguage(string LanguageName)
		{
			int languageIndex = this.GetLanguageIndex(LanguageName, true);
			if (languageIndex < 0)
			{
				return;
			}
			int count = this.mLanguages.Count;
			int languages = 0;
			int num = this.mTerms.Count;
			while (languages < num)
			{
				for (int i = languageIndex + 1; i < count; i++)
				{
					this.mTerms[languages].Languages[i - 1] = this.mTerms[languages].Languages[i];
				}
				Array.Resize<string>(ref this.mTerms[languages].Languages, count - 1);
				languages++;
			}
			this.mLanguages.RemoveAt(languageIndex);
		}

		public void RemoveTerm(string term)
		{
			int num = 0;
			int count = this.mTerms.Count;
			while (num < count)
			{
				if (this.mTerms[num].Term == term)
				{
					this.mTerms.RemoveAt(num);
					this.mDictionary.Remove(term);
					return;
				}
				num++;
			}
		}

		public void UpdateDictionary()
		{
			this.mDictionary.Clear();
			int item = 0;
			int count = this.mTerms.Count;
			while (item < count)
			{
				LanguageSource.ValidateFullTerm(ref this.mTerms[item].Term);
				this.mDictionary[this.mTerms[item].Term] = this.mTerms[item];
				item++;
			}
		}

		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (Term.StartsWith(LanguageSource.EmptyCategory) && Term.Length > LanguageSource.EmptyCategory.Length && Term[LanguageSource.EmptyCategory.Length] == '/')
			{
				Term = Term.Substring(LanguageSource.EmptyCategory.Length + 1);
			}
		}
	}
}