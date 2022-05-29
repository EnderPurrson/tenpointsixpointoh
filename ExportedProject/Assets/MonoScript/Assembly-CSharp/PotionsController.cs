using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class PotionsController : MonoBehaviour
{
	public const string InvisibilityPotion = "InvisibilityPotion";

	public static string HastePotion;

	public static string RegenerationPotion;

	public static string MightPotion;

	public static int MaxNumOFPotions;

	public static PotionsController sharedController;

	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> potionMethods;

	public static Dictionary<string, float> potionDurations;

	public static string[] potions;

	public Dictionary<string, float> activePotions = new Dictionary<string, float>();

	public List<string> activePotionsList = new List<string>();

	public static float AntiGravityMult
	{
		get
		{
			return 0.75f;
		}
	}

	static PotionsController()
	{
		PotionsController.HastePotion = "HastePotion";
		PotionsController.RegenerationPotion = "RegenerationPotion";
		PotionsController.MightPotion = "MightPotion";
		PotionsController.MaxNumOFPotions = 1000000;
		PotionsController.sharedController = null;
		PotionsController.potionMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();
		PotionsController.potionDurations = new Dictionary<string, float>();
		PotionsController.potions = new string[] { PotionsController.HastePotion, PotionsController.MightPotion, PotionsController.RegenerationPotion, "InvisibilityPotion" };
		PotionsController.potionMethods.Add(PotionsController.HastePotion, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.HastePotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.HastePotionDeactivation)));
		PotionsController.potionMethods.Add(PotionsController.MightPotion, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MightPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MightPotionDeactivation)));
		PotionsController.potionMethods.Add("InvisibilityPotion", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.InvisibilityPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.InvisibilityPotionDeactivation)));
		PotionsController.potionMethods.Add(PotionsController.RegenerationPotion, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.RegenerationPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.RegenerationPotionDeactivation)));
		PotionsController.potionMethods.Add(GearManager.Jetpack, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MightPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MightPotionDeactivation)));
		PotionsController.potionMethods.Add(GearManager.Turret, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.RegenerationPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.TurretPotionDeactivation)));
		PotionsController.potionMethods.Add(GearManager.Mech, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MechActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MechDeactivation)));
		PotionsController.potionDurations.Add(PotionsController.HastePotion, 180f);
		PotionsController.potionDurations.Add(PotionsController.MightPotion, 60f);
		PotionsController.potionDurations.Add(PotionsController.RegenerationPotion, 300f);
		PotionsController.potionDurations.Add("InvisibilityPotion0", 30f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Turret, "0"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Jetpack, "0"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Mech, "0"), 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion1", 30f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Turret, "1"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Jetpack, "1"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Mech, "1"), 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion2", 30f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Turret, "2"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Jetpack, "2"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Mech, "2"), 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion3", 30f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Turret, "3"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Jetpack, "3"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Mech, "3"), 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion4", 30f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Turret, "4"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Jetpack, "4"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Mech, "4"), 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion5", 30f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Turret, "5"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Jetpack, "5"), 60f);
		PotionsController.potionDurations.Add(string.Concat(GearManager.Mech, "5"), 30f);
	}

	public PotionsController()
	{
	}

	public void ActivatePotion(string potion, Player_move_c move_c, Dictionary<string, object> pars, bool isAddTimeOnActive = false)
	{
		float item;
		float single;
		if (!this.activePotions.ContainsKey(potion))
		{
			Dictionary<string, float> strs = this.activePotions;
			string str = potion;
			if (!Defs.isDaterRegim)
			{
				Dictionary<string, float> strs1 = PotionsController.potionDurations;
				int num = GearManager.CurrentNumberOfUphradesForGear(potion);
				single = strs1[string.Concat(potion, num.ToString())];
			}
			else
			{
				single = 180f;
			}
			strs.Add(str, single);
			this.activePotionsList.Add(potion);
		}
		else if (isAddTimeOnActive)
		{
			this.activePotions.Remove(potion);
			Dictionary<string, float> strs2 = this.activePotions;
			string str1 = potion;
			if (!Defs.isDaterRegim)
			{
				Dictionary<string, float> strs3 = PotionsController.potionDurations;
				int num1 = GearManager.CurrentNumberOfUphradesForGear(potion);
				item = strs3[string.Concat(potion, num1.ToString())];
			}
			else
			{
				item = 180f;
			}
			strs2.Add(str1, item);
			this.activePotionsList.Remove(potion);
			this.activePotionsList.Add(potion);
			if (TableGearController.sharedController != null)
			{
				TableGearController.sharedController.ReactivatePotion(potion);
			}
		}
		if (PotionsController.potionMethods.ContainsKey(potion))
		{
			PotionsController.potionMethods[potion].Key(move_c, pars);
		}
		if (PotionsController.PotionActivated != null)
		{
			PotionsController.PotionActivated(potion);
		}
	}

	public static void AntiGravityPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
	}

	public static void AntiGravityPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
	}

	public void DeActivePotion(string _potion, Player_move_c p, bool isDeleteObject = true)
	{
		if (PotionsController.PotionDisactivated != null)
		{
			PotionsController.PotionDisactivated(_potion);
		}
		if (!this.activePotions.ContainsKey(_potion))
		{
			return;
		}
		this.activePotions.Remove(_potion);
		this.activePotionsList.Remove(_potion);
		if (isDeleteObject)
		{
			PotionsController.potionMethods[_potion].Value(p, new Dictionary<string, object>());
		}
	}

	public static void HastePotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		if (move_c._player && move_c._player != null)
		{
			FirstPersonControlSharp component = move_c._player.GetComponent<FirstPersonControlSharp>();
			if (component && component != null)
			{
				component.gravityMultiplier *= PotionsController.AntiGravityMult;
			}
		}
	}

	public static void HastePotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		if (move_c._player && move_c._player != null)
		{
			FirstPersonControlSharp component = move_c._player.GetComponent<FirstPersonControlSharp>();
			if (component && component != null)
			{
				component.gravityMultiplier /= PotionsController.AntiGravityMult;
			}
		}
	}

	public static void InvisibilityPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		move_c.SetInvisible(true);
	}

	public static void InvisibilityPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		move_c.SetInvisible(false);
	}

	private static void MechActivation(Player_move_c arg1, Dictionary<string, object> arg2)
	{
		Debug.Log("Mech ON");
		arg1.ActivateMech(0);
	}

	private static void MechDeactivation(Player_move_c arg1, Dictionary<string, object> arg2)
	{
		Debug.Log("Mech OFF");
		arg1.DeactivateMech();
	}

	public static void MightPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		GameObject gameObject = null;
		gameObject = (!Defs.isMulti ? GameObject.FindGameObjectWithTag("Player") : WeaponManager.sharedManager.myPlayer);
		if (gameObject != null)
		{
			gameObject.GetComponent<SkinName>().playerMoveC.SetJetpackEnabled(true);
		}
	}

	public static void MightPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		GameObject gameObject = null;
		gameObject = (!Defs.isMulti ? GameObject.FindGameObjectWithTag("Player") : WeaponManager.sharedManager.myPlayer);
		if (gameObject != null)
		{
			gameObject.GetComponent<SkinName>().playerMoveC.SetJetpackEnabled(false);
		}
	}

	public static void NightVisionPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		if (move_c.inGameGUI && move_c.inGameGUI.nightVisionEffect != null)
		{
			move_c.inGameGUI.nightVisionEffect.SetActive(true);
		}
	}

	public static void NightVisionPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		if (move_c.inGameGUI && move_c.inGameGUI.nightVisionEffect != null)
		{
			move_c.inGameGUI.nightVisionEffect.SetActive(false);
		}
	}

	private void OnLevelWasLoaded(int lev)
	{
		this.activePotions.Clear();
		this.activePotionsList.Clear();
	}

	public bool PotionIsActive(string nm)
	{
		return (nm == null || this.activePotions == null ? false : this.activePotions.ContainsKey(nm));
	}

	public void ReactivatePotions(Player_move_c move_c, Dictionary<string, object> pars)
	{
		foreach (string key in this.activePotions.Keys)
		{
			this.ActivatePotion(key, move_c, pars, false);
		}
	}

	public static void RegenerationPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
	}

	public static void RegenerationPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
	}

	public float RemainDuratioForPotion(string potion)
	{
		if (potion == null || !this.activePotions.ContainsKey(potion))
		{
			return 0f;
		}
		return this.activePotions[potion] + EffectsController.AddingForPotionDuration(potion);
	}

	private void Start()
	{
		PotionsController.sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Step(float tm, Player_move_c p)
	{
		List<string> strs = new List<string>();
		List<string> strs1 = new List<string>();
		foreach (string key in this.activePotions.Keys)
		{
			strs1.Add(key);
		}
		foreach (string str in strs1)
		{
			Dictionary<string, float> item = this.activePotions;
			Dictionary<string, float> strs2 = item;
			string str1 = str;
			item[str1] = strs2[str1] - tm;
			if (this.RemainDuratioForPotion(str) > 0f)
			{
				continue;
			}
			strs.Add(str);
		}
		foreach (string str2 in strs)
		{
			this.DeActivePotion(str2, p, true);
		}
	}

	public static void TurretPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		Player_move_c component = null;
		if (!Defs.isMulti)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				component = gameObject.GetComponent<SkinName>().playerMoveC;
			}
		}
		else
		{
			component = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (component == null || component.currentTurret == null)
		{
			return;
		}
		if (!Defs.isMulti)
		{
			UnityEngine.Object.Destroy(component.currentTurret);
		}
		else if (!Defs.isInet)
		{
			Network.Destroy(component.currentTurret);
		}
		else
		{
			PhotonNetwork.Destroy(component.currentTurret);
		}
	}

	public static event Action<string> PotionActivated;

	public static event Action<string> PotionDisactivated;
}