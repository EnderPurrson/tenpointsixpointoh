using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public class AskNameManager : MonoBehaviour
{
	public const string keyNameAlreadySet = "keyNameAlreadySet";

	public static AskNameManager instance;

	public GameObject objWindow;

	public GameObject objPanelSetName;

	public GameObject objPanelEnterName;

	public UILabel lbPlayerName;

	public UIInput inputPlayerName;

	public UIButton btnSetName;

	public GameObject objLbWarning;

	private int _NameAlreadySet = -1;

	private string curChooseName = string.Empty;

	private bool isAutoName;

	public static bool isComplete;

	public static bool isShow;

	private IDisposable _backSubcripter;

	private bool CanSetName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.curChooseName.Trim()))
			{
				return true;
			}
			return false;
		}
	}

	private bool CanShowWindow
	{
		get
		{
			if (this.NameAlreadySet)
			{
				return false;
			}
			if (TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.ShopCompleted)
			{
				return false;
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return true;
			}
			if (MainMenuController.sharedController.SyncFuture.IsCompleted)
			{
				return true;
			}
			return false;
		}
	}

	private bool NameAlreadySet
	{
		get
		{
			if (this._NameAlreadySet == -1)
			{
				this._NameAlreadySet = Load.LoadInt("keyNameAlreadySet");
			}
			return this._NameAlreadySet == 1;
		}
		set
		{
			this._NameAlreadySet = (!value ? 0 : 1);
			Save.SaveInt("keyNameAlreadySet", this._NameAlreadySet);
		}
	}

	static AskNameManager()
	{
	}

	public AskNameManager()
	{
	}

	private bool AskIsCompleted()
	{
		bool flag = (this.NameAlreadySet ? true : TrainingController.TrainingCompleted);
		if (flag)
		{
			AskNameManager.isComplete = true;
			if (AskNameManager.onComplete != null)
			{
				AskNameManager.onComplete();
			}
		}
		return flag;
	}

	private void Awake()
	{
		AskNameManager.instance = this;
		AskNameManager.isComplete = false;
		AskNameManager.isShow = false;
		this.objWindow.SetActive(false);
		this.objPanelSetName.SetActive(false);
		this.objPanelEnterName.SetActive(false);
		this.objLbWarning.SetActive(false);
		this.AskIsCompleted();
		MainMenuController.onEnableMenuForAskname += new Action(this.ShowWindow);
	}

	private void CheckActiveBtnSetName()
	{
		BoxCollider component = this.btnSetName.GetComponent<BoxCollider>();
		this.objLbWarning.SetActive(false);
		if (!this.CanSetName)
		{
			this.objLbWarning.SetActive(true);
			component.enabled = false;
			this.btnSetName.SetState(UIButtonColor.State.Disabled, true);
		}
		else
		{
			component.enabled = true;
			this.btnSetName.SetState(UIButtonColor.State.Normal, true);
		}
	}

	private string GetNameForAsk()
	{
		return ProfileController.GetPlayerNameOrDefault();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus && this.objWindow != null && this.objWindow.activeInHierarchy)
		{
			this.curChooseName = "Player";
			this.SaveChooseName();
		}
	}

	public void OnChangeName()
	{
		this.curChooseName = this.inputPlayerName.@value;
		this.CheckActiveBtnSetName();
	}

	private void OnCloseAllWindow()
	{
		if (this._backSubcripter != null)
		{
			this._backSubcripter.Dispose();
		}
		this.objWindow.SetActive(false);
		AskNameManager.isComplete = true;
		if (AskNameManager.onComplete != null)
		{
			AskNameManager.onComplete();
		}
		AskNameManager.isShow = false;
	}

	private void OnDestroy()
	{
		MainMenuController.onEnableMenuForAskname -= new Action(this.ShowWindow);
		AskNameManager.instance = null;
	}

	private void OnDisable()
	{
	}

	private void OnEnable()
	{
	}

	public void OnShowWindowEnterName()
	{
		this.objPanelEnterName.SetActive(true);
		this.objPanelSetName.SetActive(false);
		this.OnStartEnterName();
	}

	private void OnShowWindowSetName()
	{
		if (this._backSubcripter != null)
		{
			this._backSubcripter.Dispose();
		}
		this._backSubcripter = BackSystem.Instance.Register(() => {
		}, null);
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("+ OnShowWindowSetName");
		}
		AskNameManager.isShow = true;
		this.curChooseName = this.GetNameForAsk();
		this.lbPlayerName.text = this.curChooseName;
		this.inputPlayerName.@value = this.curChooseName;
		this.CheckActiveBtnSetName();
		this.objPanelSetName.SetActive(true);
		this.objWindow.SetActive(true);
		this.isAutoName = true;
	}

	public void OnStartEnterName()
	{
		if (this.isAutoName)
		{
			this.inputPlayerName.isSelected = true;
			this.curChooseName = string.Empty;
			this.inputPlayerName.@value = this.curChooseName;
			this.CheckActiveBtnSetName();
			this.isAutoName = false;
		}
	}

	public void SaveChooseName()
	{
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SaveNamePlayer(this.curChooseName);
		}
		if (MainMenuController.sharedController != null && MainMenuController.sharedController.persNickLabel != null)
		{
			MainMenuController.sharedController.persNickLabel.UpdateNickInLobby();
			MainMenuController.sharedController.persNickLabel.UpdateInfo();
		}
		this.NameAlreadySet = true;
		this.OnCloseAllWindow();
	}

	public void ShowWindow()
	{
		base.StopCoroutine("WaitAndShowWindow");
		base.StartCoroutine("WaitAndShowWindow");
	}

	[ContextMenu("Show Window")]
	public void TestShow()
	{
		AskNameManager.isComplete = false;
		this.OnShowWindowSetName();
	}

	[DebuggerHidden]
	private IEnumerator WaitAndShowWindow()
	{
		AskNameManager.u003cWaitAndShowWindowu003ec__Iterator16E variable = null;
		return variable;
	}

	public static event Action onComplete;
}