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

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache4;

	private void Start()
	{
		priceLabel.text = Defs.healthInGamePanelPrice.ToString();
	}

	private void Update()
	{
		UpdateState();
	}

	private void UpdateState(bool isDelta = true)
	{
		if (!(inGameGui.playerMoveC == null))
		{
			bool flag = inGameGui.playerMoveC.CurHealth == inGameGui.playerMoveC.MaxHealth;
			if (fullLabel.activeSelf != flag)
			{
				fullLabel.SetActive(flag);
			}
			myButton.isEnabled = !flag;
			if (priceLabel.gameObject.activeSelf != !flag)
			{
				priceLabel.gameObject.SetActive(!flag);
			}
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
			if (inGameGui.playerMoveC != null)
			{
				inGameGui.playerMoveC.CurHealth = inGameGui.playerMoveC.MaxHealth;
			}
		}
		else if (!(inGameGui.playerMoveC.CurHealth <= 0f))
		{
			GameObject mainPanel = inGameGui.gameObject;
			ItemPrice price = new ItemPrice(Defs.healthInGamePanelPrice, "Coins");
			Action onSuccess = _003COnClick_003Em__177;
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003COnClick_003Em__178;
			}
			ShopNGUIController.TryToBuy(mainPanel, price, onSuccess, _003C_003Ef__am_0024cache4);
		}
	}

	[CompilerGenerated]
	private void _003COnClick_003Em__177()
	{
		if (inGameGui.playerMoveC != null)
		{
			inGameGui.playerMoveC.CurHealth = inGameGui.playerMoveC.MaxHealth;
			inGameGui.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Health);
			inGameGui.playerMoveC.timeBuyHealth = Time.time;
		}
		FlurryPluginWrapper.LogPurchaseByModes(ShopNGUIController.CategoryNames.GearCategory, "Health", 1, true);
		FlurryPluginWrapper.LogGearPurchases("Health", 1, true);
		Dictionary<string, string> parameters = new Dictionary<string, string> { { "Succeeded", "Health" } };
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", parameters);
		FlurryPluginWrapper.LogFastPurchase("Health");
	}

	[CompilerGenerated]
	private static void _003COnClick_003Em__178()
	{
		JoystickController.leftJoystick.Reset();
		Dictionary<string, string> parameters = new Dictionary<string, string> { { "Failed", "Health" } };
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", parameters);
	}
}
