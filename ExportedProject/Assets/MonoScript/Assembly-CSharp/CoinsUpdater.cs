using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class CoinsUpdater : MonoBehaviour
{
	public readonly static string trainCoinsStub;

	private UILabel coinsLabel;

	private string _trainingMsg = "0";

	private bool _disposed;

	static CoinsUpdater()
	{
		CoinsUpdater.trainCoinsStub = "999";
	}

	public CoinsUpdater()
	{
	}

	private void _ReplaceMsgForTraining(bool isGems, int count)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this._trainingMsg = CoinsUpdater.trainCoinsStub;
		}
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		CoinsUpdater.u003cMyWaitForSecondsu003ec__Iterator19 variable = null;
		return variable;
	}

	private void OnDestroy()
	{
		CoinsMessage.CoinsLabelDisappeared -= new CoinsMessage.CoinsLabelDisappearedDelegate(this._ReplaceMsgForTraining);
		this._disposed = true;
	}

	private void OnDisable()
	{
		BankController.onUpdateMoney -= new Action(this.UpdateMoney);
	}

	private void OnEnable()
	{
		BankController.onUpdateMoney += new Action(this.UpdateMoney);
		base.StartCoroutine(this.UpdateCoinsLabel());
	}

	private void Start()
	{
		this.coinsLabel = base.GetComponent<UILabel>();
		CoinsMessage.CoinsLabelDisappeared += new CoinsMessage.CoinsLabelDisappearedDelegate(this._ReplaceMsgForTraining);
		string str = Storager.getInt("Coins", false).ToString();
		if (this.coinsLabel != null)
		{
			this.coinsLabel.text = str;
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdateCoinsLabel()
	{
		CoinsUpdater.u003cUpdateCoinsLabelu003ec__Iterator18 variable = null;
		return variable;
	}

	private void UpdateMoney()
	{
		if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None)
		{
			string str = Storager.getInt("Coins", false).ToString();
			if (this.coinsLabel != null)
			{
				this.coinsLabel.text = str;
			}
		}
		else if (this.coinsLabel != null)
		{
			if (!(ShopNGUIController.sharedShop != null) || !ShopNGUIController.GuiActive)
			{
				this.coinsLabel.text = this._trainingMsg;
			}
			else
			{
				this.coinsLabel.text = "999";
			}
		}
	}
}