using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class GemsUpdater : MonoBehaviour
{
	public readonly static string trainCoinsStub;

	private UILabel coinsLabel;

	private string _trainingMsg = "0";

	private bool _disposed;

	static GemsUpdater()
	{
		GemsUpdater.trainCoinsStub = "999";
	}

	public GemsUpdater()
	{
	}

	private void _ReplaceMsgForTraining(bool isGems, int count)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this._trainingMsg = GemsUpdater.trainCoinsStub;
		}
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		GemsUpdater.u003cMyWaitForSecondsu003ec__Iterator7B variable = null;
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
		string str = Storager.getInt("GemsCurrency", false).ToString();
		if (this.coinsLabel != null)
		{
			this.coinsLabel.text = str;
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdateCoinsLabel()
	{
		GemsUpdater.u003cUpdateCoinsLabelu003ec__Iterator7A variable = null;
		return variable;
	}

	private void UpdateMoney()
	{
		if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None)
		{
			string str = Storager.getInt("GemsCurrency", false).ToString();
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