using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class AmmoButtonInGamePanel : MonoBehaviour
{
	public GameObject fullLabel;

	public UIButton myButton;

	public UILabel priceLabel;

	public InGameGUI inGameGui;

	public AmmoButtonInGamePanel()
	{
	}

	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			Weapon maxAmmoWithEffectApplied = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			maxAmmoWithEffectApplied.currentAmmoInBackpack = maxAmmoWithEffectApplied.weaponPrefab.GetComponent<WeaponSounds>().MaxAmmoWithEffectApplied;
			return;
		}
		ShopNGUIController.TryToBuy(this.inGameGui.gameObject, new ItemPrice(Defs.ammoInGamePanelPrice, "Coins"), () => {
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Ammo);
			}
			Weapon item = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			item.currentAmmoInBackpack = item.weaponPrefab.GetComponent<WeaponSounds>().MaxAmmoWithEffectApplied;
			FlurryPluginWrapper.LogPurchaseByModes(ShopNGUIController.CategoryNames.GearCategory, "Ammo", 1, true);
			FlurryPluginWrapper.LogGearPurchases("Ammo", 1, true);
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", new Dictionary<string, string>()
			{
				{ "Succeeded", "Ammo" }
			}, true);
			FlurryPluginWrapper.LogFastPurchase("Ammo");
		}, () => {
			JoystickController.leftJoystick.Reset();
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", new Dictionary<string, string>()
			{
				{ "Failed", "Ammo" }
			}, true);
		}, null, null, null, null);
	}

	private void OnEnable()
	{
		this.UpdateState(false);
	}

	private void Start()
	{
		this.priceLabel.text = Defs.ammoInGamePanelPrice.ToString();
	}

	private void Update()
	{
		this.UpdateState(true);
	}

	private void UpdateState(bool isDelta = true)
	{
		Weapon item = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
		int num = item.currentAmmoInBackpack;
		bool maxAmmoWithEffectApplied = num == item.weaponPrefab.GetComponent<WeaponSounds>().MaxAmmoWithEffectApplied;
		if (maxAmmoWithEffectApplied == this.myButton.isEnabled || !isDelta)
		{
			this.fullLabel.SetActive(maxAmmoWithEffectApplied);
			this.myButton.isEnabled = !maxAmmoWithEffectApplied;
			this.priceLabel.gameObject.SetActive(!maxAmmoWithEffectApplied);
		}
	}
}