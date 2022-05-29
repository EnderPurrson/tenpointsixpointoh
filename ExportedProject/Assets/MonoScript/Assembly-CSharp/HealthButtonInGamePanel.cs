using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class HealthButtonInGamePanel : MonoBehaviour
{
	public GameObject fullLabel;

	public UIButton myButton;

	public UILabel priceLabel;

	public InGameGUI inGameGui;

	public HealthButtonInGamePanel()
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
			if (this.inGameGui.playerMoveC != null)
			{
				this.inGameGui.playerMoveC.CurHealth = this.inGameGui.playerMoveC.MaxHealth;
			}
			return;
		}
		if (this.inGameGui.playerMoveC.CurHealth <= 0f)
		{
			return;
		}
		ShopNGUIController.TryToBuy(this.inGameGui.gameObject, new ItemPrice(Defs.healthInGamePanelPrice, "Coins"), () => {
			if (this.inGameGui.playerMoveC != null)
			{
				this.inGameGui.playerMoveC.CurHealth = this.inGameGui.playerMoveC.MaxHealth;
				this.inGameGui.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Health);
				this.inGameGui.playerMoveC.timeBuyHealth = Time.time;
			}
			FlurryPluginWrapper.LogPurchaseByModes(ShopNGUIController.CategoryNames.GearCategory, "Health", 1, true);
			FlurryPluginWrapper.LogGearPurchases("Health", 1, true);
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", new Dictionary<string, string>()
			{
				{ "Succeeded", "Health" }
			}, true);
			FlurryPluginWrapper.LogFastPurchase("Health");
		}, () => {
			JoystickController.leftJoystick.Reset();
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", new Dictionary<string, string>()
			{
				{ "Failed", "Health" }
			}, true);
		}, null, null, null, null);
	}

	private void OnEnable()
	{
		this.UpdateState(false);
	}

	private void Start()
	{
		this.priceLabel.text = Defs.healthInGamePanelPrice.ToString();
	}

	private void Update()
	{
		this.UpdateState(true);
	}

	private void UpdateState(bool isDelta = true)
	{
		if (this.inGameGui.playerMoveC == null)
		{
			return;
		}
		bool curHealth = this.inGameGui.playerMoveC.CurHealth == this.inGameGui.playerMoveC.MaxHealth;
		if (this.fullLabel.activeSelf != curHealth)
		{
			this.fullLabel.SetActive(curHealth);
		}
		this.myButton.isEnabled = !curHealth;
		if (this.priceLabel.gameObject.activeSelf != !curHealth)
		{
			this.priceLabel.gameObject.SetActive(!curHealth);
		}
	}
}