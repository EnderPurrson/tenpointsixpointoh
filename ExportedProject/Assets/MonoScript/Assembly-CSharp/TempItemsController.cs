using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class TempItemsController : MonoBehaviour
{
	private const long _salt = 1002855644958404316L;

	private const string DurationKey = "Duration";

	private const string StartKey = "Start";

	private const string ExpiredItemsKey = "ExpiredITemptemsControllerKey";

	public static TempItemsController sharedController;

	public List<string> ExpiredItems = new List<string>();

	public static Dictionary<string, List<float>> PriceCoefs;

	public static Dictionary<string, string> GunsMappingFromTempToConst;

	private Dictionary<string, Dictionary<string, SaltedLong>> Items = new Dictionary<string, Dictionary<string, SaltedLong>>();

	private static List<int> rentTms;

	static TempItemsController()
	{
		Dictionary<string, List<float>> strs = new Dictionary<string, List<float>>();
		string assaultMachineGunTag = WeaponTags.Assault_Machine_Gun_Tag;
		List<float> singles = new List<float>()
		{
			1f,
			2f,
			4f
		};
		strs.Add(assaultMachineGunTag, singles);
		string impulseSniperRifleTag = WeaponTags.Impulse_Sniper_Rifle_Tag;
		singles = new List<float>()
		{
			1f,
			2.3333333f,
			3.6666667f
		};
		strs.Add(impulseSniperRifleTag, singles);
		singles = new List<float>()
		{
			1f,
			2.6666667f,
			5.3333335f
		};
		strs.Add("Armor_Adamant_3", singles);
		singles = new List<float>()
		{
			1f,
			2.6666667f,
			5.3333335f
		};
		strs.Add("hat_Adamant_3", singles);
		string railRevolver1Tag = WeaponTags.RailRevolver_1_Tag;
		singles = new List<float>()
		{
			1f,
			2f,
			4f
		};
		strs.Add(railRevolver1Tag, singles);
		string autoaimRocketlauncherTag = WeaponTags.Autoaim_Rocketlauncher_Tag;
		singles = new List<float>()
		{
			1f,
			2f,
			3.125f
		};
		strs.Add(autoaimRocketlauncherTag, singles);
		string twoBoltersRentTag = WeaponTags.TwoBoltersRent_Tag;
		singles = new List<float>()
		{
			1f,
			2f,
			3.125f
		};
		strs.Add(twoBoltersRentTag, singles);
		string redStoneRentTag = WeaponTags.Red_StoneRent_Tag;
		singles = new List<float>()
		{
			1f,
			2f,
			3.125f
		};
		strs.Add(redStoneRentTag, singles);
		string dragonGunRentTag = WeaponTags.DragonGunRent_Tag;
		singles = new List<float>()
		{
			1f,
			2f,
			3.125f
		};
		strs.Add(dragonGunRentTag, singles);
		string pumpkinGunRentTag = WeaponTags.PumpkinGunRent_Tag;
		singles = new List<float>()
		{
			1f,
			2f,
			3.125f
		};
		strs.Add(pumpkinGunRentTag, singles);
		string rayMinigunRentTag = WeaponTags.RayMinigunRent_Tag;
		singles = new List<float>()
		{
			1f,
			2f,
			3.125f
		};
		strs.Add(rayMinigunRentTag, singles);
		TempItemsController.PriceCoefs = strs;
		TempItemsController.GunsMappingFromTempToConst = new Dictionary<string, string>();
		TempItemsController.rentTms = null;
		TempItemsController.GunsMappingFromTempToConst.Add(WeaponTags.Assault_Machine_Gun_Tag, WeaponTags.Assault_Machine_GunBuy_Tag);
		TempItemsController.GunsMappingFromTempToConst.Add(WeaponTags.Impulse_Sniper_Rifle_Tag, WeaponTags.Impulse_Sniper_RifleBuy_Tag);
		TempItemsController.GunsMappingFromTempToConst.Add(WeaponTags.RailRevolver_1_Tag, WeaponTags.RailRevolverBuy_Tag);
		TempItemsController.GunsMappingFromTempToConst.Add(WeaponTags.Autoaim_Rocketlauncher_Tag, WeaponTags.Autoaim_RocketlauncherBuy_Tag);
	}

	public TempItemsController()
	{
	}

	public void AddTemporaryItem(string tg, int tm)
	{
		this.AddTimeForItem(tg, (tm < 0 ? 0 : tm));
	}

	public void AddTimeForItem(string item, int time)
	{
	}

	private void Awake()
	{
		TempItemsController.sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.DeserializeItems();
		this.DeserializeExpiredObjects();
		this.CheckForTimeHack();
		this.RemoveExpiredItems();
	}

	public bool CanShowExpiredBannerForTag(string tg)
	{
		return false;
	}

	private void CheckForTimeHack()
	{
	}

	public bool ContainsItem(string item)
	{
		return false;
	}

	private void DeserializeExpiredObjects()
	{
		if (!Storager.hasKey("ExpiredITemptemsControllerKey"))
		{
			Storager.setString("ExpiredITemptemsControllerKey", "[]", false);
		}
		object obj = Json.Deserialize(Storager.getString("ExpiredITemptemsControllerKey", false));
		if (obj == null)
		{
			UnityEngine.Debug.LogWarning("Error Deserializing expired items JSON");
			return;
		}
		List<object> objs = obj as List<object>;
		if (objs == null)
		{
			UnityEngine.Debug.LogWarning("Error casting expired items obj to list");
			return;
		}
		try
		{
			this.ExpiredItems.Clear();
			foreach (string str in objs)
			{
				this.ExpiredItems.Add(str);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogWarning(string.Concat("Exception when iterating expired items list: ", exception));
		}
	}

	private void DeserializeItems()
	{
		object obj;
		long num;
		object obj1;
		long num1;
		TempItemsController.PrepareKeyForItemsJson();
		object obj2 = Json.Deserialize(Storager.getString(Defs.TempItemsDictionaryKey, false));
		if (obj2 == null)
		{
			UnityEngine.Debug.LogWarning("Error Deserializing temp items JSON");
			return;
		}
		Dictionary<string, object> strs = obj2 as Dictionary<string, object>;
		if (strs == null)
		{
			UnityEngine.Debug.LogWarning("Error casting to dict in deserializing temp items JSON");
			return;
		}
		Dictionary<string, Dictionary<string, long>> strs1 = new Dictionary<string, Dictionary<string, long>>();
		foreach (KeyValuePair<string, object> keyValuePair in strs)
		{
			if (keyValuePair.Value != null)
			{
				Dictionary<string, object> value = keyValuePair.Value as Dictionary<string, object>;
				if (value == null)
				{
					UnityEngine.Debug.LogWarning(string.Concat("Error innerDict == null kvp.Key = ", keyValuePair.Key, " in deserializing temp items JSON"));
				}
				else if (!value.TryGetValue("Duration", out obj) || obj == null)
				{
					UnityEngine.Debug.LogWarning(" ! (innerDict.TryGetValue(DurationKey,out DurationValueObj) && DurationValueObj != null) in deserializing temp items JSON");
				}
				else
				{
					try
					{
						num = (long)obj;
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogWarning(string.Concat("Error unboxing DurationValue in deserializing temp items JSON: ", exception.Message));
						continue;
					}
					if (!value.TryGetValue("Start", out obj1) || obj1 == null)
					{
						UnityEngine.Debug.LogWarning(" ! (innerDict.TryGetValue(StartKey,out StartValueObj) && StartValueObj != null) in deserializing temp items JSON");
					}
					else
					{
						try
						{
							num1 = (long)obj1;
						}
						catch (Exception exception1)
						{
							UnityEngine.Debug.LogWarning(string.Concat("Error unboxing StartValue in deserializing temp items JSON: ", exception1.Message));
							continue;
						}
						string key = keyValuePair.Key;
						Dictionary<string, long> strs2 = new Dictionary<string, long>()
						{
							{ "Start", num1 },
							{ "Duration", num }
						};
						strs1.Add(key, strs2);
					}
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning(string.Concat("Error kvp.Value == null kvp.Key = ", keyValuePair.Key, " in deserializing temp items JSON"));
			}
		}
		this.Items = TempItemsController.ToSaltedDictionary(strs1);
	}

	private static long GetLastSuspendTime()
	{
		return PromoActionsManager.GetUnixTimeFromStorage(Defs.LastTimeTempItemsSuspended);
	}

	public static bool IsCategoryContainsTempItems(ShopNGUIController.CategoryNames cat)
	{
		return (ShopNGUIController.IsWeaponCategory(cat) || cat == ShopNGUIController.CategoryNames.ArmorCategory ? true : cat == ShopNGUIController.CategoryNames.HatsCategory);
	}

	private static bool ItemIsArmorOrHat(string tg)
	{
		bool flag;
		int num = PromoActionsGUIController.CatForTg(tg);
		if (num == -1)
		{
			flag = false;
		}
		else
		{
			flag = (num == 7 ? true : num == 6);
		}
		return flag;
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		TempItemsController.u003cMyWaitForSecondsu003ec__Iterator1C0 variable = null;
		return variable;
	}

	private void OnDestroy()
	{
	}

	private static void PrepareKeyForItemsJson()
	{
		if (!Storager.hasKey(Defs.TempItemsDictionaryKey))
		{
			Storager.setString(Defs.TempItemsDictionaryKey, "{}", false);
		}
	}

	private void RemoveExpiredItems()
	{
	}

	private void RemoveTemporaryItem(string key)
	{
		if (!TempItemsController.ItemIsArmorOrHat(key))
		{
			WeaponManager.sharedManager.RemoveTemporaryItem(key);
		}
		else
		{
			Wear.RemoveTemporaryWear(key);
		}
	}

	public static int RentIndexFromDays(int days)
	{
		int num = 0;
		if (days == 1)
		{
			num = 0;
		}
		else if (days == 2)
		{
			num = 3;
		}
		else if (days == 3)
		{
			num = 1;
		}
		else if (days == 5)
		{
			num = 4;
		}
		else if (days == 7)
		{
			num = 2;
		}
		return num;
	}

	public static int RentTimeForIndex(int timeForRentIndex)
	{
		if (TempItemsController.rentTms == null)
		{
			List<int> nums = new List<int>()
			{
				86400,
				259200,
				604800,
				172800,
				432000
			};
			TempItemsController.rentTms = nums;
		}
		int item = 86400;
		if (timeForRentIndex < TempItemsController.rentTms.Count && timeForRentIndex >= 0)
		{
			item = TempItemsController.rentTms[timeForRentIndex];
		}
		return item;
	}

	private static void SaveSuspendTime()
	{
		Storager.setString(Defs.LastTimeTempItemsSuspended, PromoActionsManager.CurrentUnixTime.ToString(), false);
	}

	private void SerializeExpiredItems()
	{
		Storager.setString("ExpiredITemptemsControllerKey", Json.Serialize(this.ExpiredItems), false);
	}

	private void SerializeItems()
	{
		Dictionary<string, Dictionary<string, long>> normalDictionary = TempItemsController.ToNormalDictionary(this.Items ?? new Dictionary<string, Dictionary<string, SaltedLong>>());
		Storager.setString(Defs.TempItemsDictionaryKey, Json.Serialize(normalDictionary), false);
	}

	private void Start()
	{
		base.StartCoroutine(this.Step());
	}

	[DebuggerHidden]
	private IEnumerator Step()
	{
		return new TempItemsController.u003cStepu003ec__Iterator1BF();
	}

	public void TakeTemporaryItemToPlayer(ShopNGUIController.CategoryNames categoryName, string tag, int indexTimeLife)
	{
		ShopNGUIController.ProvideShopItemOnStarterPackBoguht(categoryName, tag, 1, false, indexTimeLife, null, null, true, true, false);
		this.ExpiredItems.Remove(tag);
	}

	public static string TempItemTimeRemainsStringRepresentation(long seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
		string empty = string.Empty;
		if (seconds < (long)86400)
		{
			empty = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}
		else
		{
			long num = seconds / (long)86400;
			if (seconds % (long)86400 > (long)43200)
			{
				num += (long)1;
			}
			empty = num.ToString();
		}
		return empty;
	}

	public long TimeRemainingForItems(string tg)
	{
		return (long)0;
	}

	public string TimeRemainingForItemString(string tg)
	{
		return TempItemsController.TempItemTimeRemainsStringRepresentation(this.TimeRemainingForItems(tg));
	}

	private static Dictionary<string, Dictionary<string, long>> ToNormalDictionary(Dictionary<string, Dictionary<string, SaltedLong>> saltedDict_)
	{
		if (saltedDict_ == null)
		{
			return null;
		}
		Dictionary<string, Dictionary<string, long>> strs = new Dictionary<string, Dictionary<string, long>>();
		foreach (KeyValuePair<string, Dictionary<string, SaltedLong>> keyValuePair in saltedDict_)
		{
			Dictionary<string, long> strs1 = new Dictionary<string, long>();
			if (keyValuePair.Value != null)
			{
				foreach (KeyValuePair<string, SaltedLong> value in keyValuePair.Value)
				{
					if (value.Key == null)
					{
						continue;
					}
					strs1.Add(value.Key, value.Value.Value);
				}
			}
			strs.Add(keyValuePair.Key, strs1);
		}
		return strs;
	}

	private static Dictionary<string, Dictionary<string, SaltedLong>> ToSaltedDictionary(Dictionary<string, Dictionary<string, long>> normalDict)
	{
		if (normalDict == null)
		{
			return null;
		}
		Dictionary<string, Dictionary<string, SaltedLong>> strs = new Dictionary<string, Dictionary<string, SaltedLong>>();
		foreach (KeyValuePair<string, Dictionary<string, long>> keyValuePair in normalDict)
		{
			Dictionary<string, SaltedLong> strs1 = new Dictionary<string, SaltedLong>();
			if (keyValuePair.Value != null)
			{
				foreach (KeyValuePair<string, long> value in keyValuePair.Value)
				{
					if (value.Key == null)
					{
						continue;
					}
					strs1.Add(value.Key, new SaltedLong(1002855644958404316L, value.Value));
				}
			}
			strs.Add(keyValuePair.Key, strs1);
		}
		return strs;
	}
}