using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class AmmoButtonInGamePanel : MonoBehaviour
{
	public GameObject fullLabel;

	public UIButton myButton;

	public UILabel priceLabel;

	public InGameGUI inGameGui;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache4;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache5;

	private void Start()
	{
		priceLabel.text = Defs.ammoInGamePanelPrice.ToString();
	}

	private void Update()
	{
		UpdateState();
	}

	private void UpdateState(bool isDelta = true)
	{
		Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
		int currentAmmoInBackpack = weapon.currentAmmoInBackpack;
		WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
		int maxAmmoWithEffectApplied = component.MaxAmmoWithEffectApplied;
		bool flag = currentAmmoInBackpack == maxAmmoWithEffectApplied;
		if (flag == myButton.isEnabled || !isDelta)
		{
			fullLabel.SetActive(flag);
			myButton.isEnabled = !flag;
			priceLabel.gameObject.SetActive(!flag);
		}
	}

	private void OnEnable()
	{
		UpdateState(false);
	}

	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
			weapon.currentAmmoInBackpack = component.MaxAmmoWithEffectApplied;
			return;
		}
		GameObject mainPanel = inGameGui.gameObject;
		ItemPrice price = new ItemPrice(Defs.ammoInGamePanelPrice, "Coins");
		if (_003C_003Ef__am_0024cache4 == null)
		{
			_003C_003Ef__am_0024cache4 = _003COnClick_003Em__0;
		}
		Action onSuccess = _003C_003Ef__am_0024cache4;
		if (_003C_003Ef__am_0024cache5 == null)
		{
			_003C_003Ef__am_0024cache5 = _003COnClick_003Em__1;
		}
		ShopNGUIController.TryToBuy(mainPanel, price, onSuccess, _003C_003Ef__am_0024cache5);
	}

	[CompilerGenerated]
	private static void _003COnClick_003Em__0()
	{
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
		{
			InGameGUI.sharedInGameGUI.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Ammo);
		}
		Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
		WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
		weapon.currentAmmoInBackpack = component.MaxAmmoWithEffectApplied;
		FlurryPluginWrapper.LogPurchaseByModes(ShopNGUIController.CategoryNames.GearCategory, "Ammo", 1, true);
		FlurryPluginWrapper.LogGearPurchases("Ammo", 1, true);
		Dictionary<string, string> parameters = new Dictionary<string, string> { { "Succeeded", "Ammo" } };
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", parameters);
		FlurryPluginWrapper.LogFastPurchase("Ammo");
	}

	[CompilerGenerated]
	private static void _003COnClick_003Em__1()
	{
		JoystickController.leftJoystick.Reset();
		Dictionary<string, string> parameters = new Dictionary<string, string> { { "Failed", "Ammo" } };
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", parameters);
	}
}
