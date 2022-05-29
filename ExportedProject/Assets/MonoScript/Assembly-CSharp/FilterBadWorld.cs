using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FilterBadWorld
{
	private const string CensoredText = "***";

	private const string PatternTemplate = "\\b({0})(s?)\\b";

	private const RegexOptions Options = RegexOptions.IgnoreCase;

	private static string[] badWordsConst;

	private static string[] badSymbolsConst;

	private static string[] badWords;

	private static string[] badSymbols;

	private static bool badArraysInit;

	static FilterBadWorld()
	{
		FilterBadWorld.badWordsConst = new string[] { "drugs", "drugz", "alcohol", "penis", "vagina", "sexx", "sexxy", "boobs", "cumshot", "facial", "masturbate", "nipples", "orgasm", "slut", "porn", "porno", "pornography", "ass", "arse", "assbag", "assbandit", "assbanger", "assbite", "asscock", "assfuck", "asshead", "asshole", "asshopper", "asslicker", "assshole", "asswipe", "bampot", "bastard", "beaner", "bitch", "blow job", "blowjob", "boner", "brotherfucker", "bullshit", "butt plug", "butt-pirate", "buttfucker", "camel toe", "carpetmuncher", "chink", "choad", "chode", "clit", "cock", "cockbite", "cockface", "cockfucker", "cockmaster", "cockmongruel", "cockmuncher", "cocksmoker", "cocksucker", "coon", "cooter", "cracker", "cum", "cumtart", "cunnilingus", "cunt", "cunthole", "damn", "deggo", "dick", "dickbag", "dickhead", "dickhole", "dicks", "dickweed", "dickwod", "dildo", "dipshit", "dookie", "douche", "douchebag", "douchewaffle", "dumass", "dumb ass", "dumbass", "dumbfuck", "dumbshit", "dyke", "fag", "fagbag", "fagfucker", "faggit", "faggot", "fagtard", "fatass", "fellatio", "fuck", "fuckass", "fucked", "fucker", "fuckface", "fuckhead", "fuckhole", "fuckin", "fucking", "fucknut", "fucks", "fuckstick", "fucktard", "fuckup", "fuckwad", "fuckwit", "fudgepacker", "gay", "gaydo", "gaytard", "gaywad", "goddamn", "goddamnit", "gooch", "gook", "gringo", "guido", "hard on", "heeb", "hell", "ho", "homo", "homodumbshit", "honkey", "humping", "jackass", "jap", "jerk off", "jigaboo", "jizz", "jungle bunny", "kike", "kooch", "kootch", "kyke", "lesbian", "lesbo", "lezzie", "mcfagget", "mick", "minge", "mothafucka", "motherfucker", "motherfucking", "muff", "negro", "nigga", "nigger", "niglet", "nut sack", "nutsack", "paki", "panooch", "pecker", "peckerhead", "penis", "piss", "pissed", "pissed off", "pollock", "poon", "poonani", "poonany", "porch monkey", "porchmonkey", "prick", "punta", "pussy", "pussylicking", "puto", "queef", "queer", "queerbait", "renob", "rimjob", "sand nigger", "sandnigger", "schlong", "scrote", "shit", "shitcunt", "shitdick", "shitface", "shitfaced", "shithead", "shitter", "shittiest", "shitting", "shitty", "skank", "skeet", "slut", "slutbag", "snatch", "spic", "spick", "splooge", "tard", "testicle", "thundercunt", "tit", "titfuck", "tits", "twat", "twatlips", "twats", "twatwaffle", "va-j-j", "vag", "vjayjay", "wank", "wetback", "whore", "whorebag", "wop", "sex", "sexy" };
		FilterBadWorld.badSymbolsConst = new string[] { "卐", "卍" };
		FilterBadWorld.badArraysInit = false;
	}

	public FilterBadWorld()
	{
	}

	public static string FilterString(string inputStr)
	{
		if (!FilterBadWorld.badArraysInit)
		{
			FilterBadWorld.InitBadLists();
		}
		inputStr = NGUIText.StripSymbols(inputStr);
		string[] strArrays = new string[] { ".", ",", "%", "!", "@", "#", "$", "*", "&", ";", ":", "?", "/", "<", ">", "|", "-", "_", "\"" };
		string lower = inputStr;
		string empty = string.Empty;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			lower = lower.Replace(strArrays[i], " ");
		}
		lower = lower.ToLower();
		int num = 0;
		int num1 = lower.IndexOf(" ", num);
		while (num1 != -1)
		{
			empty = (!FilterBadWorld.scanMatInWold(lower.Substring(num, num1 - num)) ? string.Concat(empty, inputStr.Substring(num, num1 - num + 1)) : string.Concat(empty, "***", inputStr.Substring(num1, 1)));
			num = num1 + 1;
			num1 = (num > lower.Length - 1 ? -1 : lower.IndexOf(" ", num));
		}
		if (num < lower.Length)
		{
			empty = (!FilterBadWorld.scanMatInWold(lower.Substring(num, lower.Length - num)) ? string.Concat(empty, inputStr.Substring(num, lower.Length - num)) : string.Concat(empty, "***"));
		}
		for (int j = 0; j < (int)FilterBadWorld.badSymbols.Length; j++)
		{
			empty = empty.Replace(FilterBadWorld.badSymbols[j], "*");
		}
		return empty;
	}

	public static void InitBadLists()
	{
		List<object> objs = new List<object>();
		List<object> objs1 = new List<object>();
		if (PlayerPrefs.HasKey("PixelFilterWordsKey"))
		{
			objs = Json.Deserialize(PlayerPrefs.GetString("PixelFilterWordsKey")) as List<object>;
		}
		if (PlayerPrefs.HasKey("PixelFilterSymbolsKey"))
		{
			objs1 = Json.Deserialize(PlayerPrefs.GetString("PixelFilterSymbolsKey")) as List<object>;
		}
		if (objs == null)
		{
			FilterBadWorld.badWords = new string[(int)FilterBadWorld.badWordsConst.Length];
		}
		else
		{
			FilterBadWorld.badWords = new string[(int)FilterBadWorld.badWordsConst.Length + objs.Count];
			for (int i = 0; i < objs.Count; i++)
			{
				FilterBadWorld.badWords[(int)FilterBadWorld.badWordsConst.Length + i] = (string)objs[i];
			}
		}
		FilterBadWorld.badWordsConst.CopyTo(FilterBadWorld.badWords, 0);
		if (objs1 == null)
		{
			FilterBadWorld.badSymbols = new string[(int)FilterBadWorld.badSymbolsConst.Length];
		}
		else
		{
			FilterBadWorld.badSymbols = new string[(int)FilterBadWorld.badSymbolsConst.Length + objs1.Count];
			for (int j = 0; j < objs1.Count; j++)
			{
				FilterBadWorld.badSymbols[(int)FilterBadWorld.badSymbolsConst.Length + j] = (string)objs1[j];
			}
		}
		FilterBadWorld.badSymbolsConst.CopyTo(FilterBadWorld.badSymbols, 0);
		FilterBadWorld.badArraysInit = true;
	}

	private static bool scanMatInWold(string str)
	{
		if (str.Length < 3)
		{
			return false;
		}
		string[] strArrays = FilterBadWorld.badWords;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			if (strArrays[i].Equals(str))
			{
				return true;
			}
		}
		return false;
	}
}