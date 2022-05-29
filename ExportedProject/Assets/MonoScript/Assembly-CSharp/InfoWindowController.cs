using I2.Loc;
using Rilisoft;
using System;
using UnityEngine;

public class InfoWindowController : MonoBehaviour
{
	public UIWidget background;

	[Header("Processing data box")]
	public UIWidget processindDataBoxContainer;

	public UILabel processingDataBoxLabel;

	[Header("Info box")]
	public UIWidget infoBoxContainer;

	public UILabel infoBoxLabel;

	[Header("Dialog box Warning")]
	public UIWidget dialogBoxContainer;

	public UILabel dialogBoxText;

	[Header("Restore Window")]
	public GameObject restoreWindowPanel;

	[Header("Achievement box")]
	public AchieveBox achievementBox;

	public UILabel achievementHeader;

	public UILabel achievementText;

	public AudioClip questCompleteSound;

	public Transform InfoWindowsRoot;

	private GameObject developerConsole;

	private Action DialogBoxOkClick;

	private Action DialogBoxCancelClick;

	private InfoWindowController.WindowType _typeCurrentWindow = InfoWindowController.WindowType.None;

	private static InfoWindowController _instance;

	private IDisposable _backSubscription;

	private Action _unsubscribe;

	public static InfoWindowController Instance
	{
		get
		{
			if (InfoWindowController._instance != null)
			{
				return InfoWindowController._instance;
			}
			UnityEngine.Object obj = Resources.Load("InfoWindows");
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(obj, Vector3.down * 567f, Quaternion.identity);
			InfoWindowController._instance = gameObject.GetComponent<InfoWindowController>();
			return InfoWindowController._instance;
		}
	}

	public static bool IsActive
	{
		get
		{
			return (!(InfoWindowController._instance != null) || !(InfoWindowController._instance.infoBoxContainer != null) || !(InfoWindowController._instance.infoBoxContainer.gameObject != null) ? false : InfoWindowController._instance.infoBoxContainer.gameObject.activeInHierarchy);
		}
	}

	public InfoWindowController()
	{
	}

	private void ActivateAchievementBox(string header, string text)
	{
		if (this.achievementBox.isOpened)
		{
			if (Application.isEditor)
			{
				Debug.LogWarningFormat("Skipping activating achievement box: {0}", new object[] { text });
			}
			return;
		}
		this.achievementText.text = text;
		this.achievementBox.ShowBox();
		base.Invoke("DeactivateAchievementBox", 3f);
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.questCompleteSound);
		}
	}

	private void ActivateDevConsole()
	{
	}

	private void ActivateDialogBox(string text, Action onOkClick, Action onCancelClick)
	{
		this.dialogBoxText.text = text;
		this.dialogBoxContainer.gameObject.SetActive(true);
		this.SetActiveBackground(true);
		this.DialogBoxOkClick = onOkClick;
		this.DialogBoxCancelClick = onCancelClick;
		if (InfoWindowController.Instance._backSubscription != null)
		{
			InfoWindowController.Instance._backSubscription.Dispose();
		}
		InfoWindowController.Instance._backSubscription = BackSystem.Instance.Register(new Action(InfoWindowController.Instance.HandleEscape), "Dialog Box");
	}

	private void ActivateInfoBox(string text)
	{
		if (InfoWindowController.Instance._backSubscription != null)
		{
			InfoWindowController.Instance._backSubscription.Dispose();
		}
		InfoWindowController.Instance._backSubscription = BackSystem.Instance.Register(new Action(InfoWindowController.Instance.HandleEscape), "Info Window");
		this.infoBoxLabel.text = text;
		this.infoBoxContainer.gameObject.SetActive(true);
		this.background.gameObject.SetActive(true);
	}

	public void ActivateRestorePanel(Action okCallback)
	{
		if (this.restoreWindowPanel == null)
		{
			return;
		}
		this.restoreWindowPanel.SetActive(true);
		this.SetActiveBackground(false);
		this.DialogBoxOkClick = okCallback;
		if (InfoWindowController.Instance._backSubscription != null)
		{
			InfoWindowController.Instance._backSubscription.Dispose();
		}
		InfoWindowController.Instance._backSubscription = BackSystem.Instance.Register(new Action(InfoWindowController.Instance.BackButtonFromRestoreClick), "Restore Panel");
	}

	private void BackButtonFromRestoreClick()
	{
	}

	public static void BlockAllClick()
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.blockClick);
		InfoWindowController.Instance.SetActiveBackground(true);
	}

	public static void CheckShowRequestServerInfoBox(bool isComplete, bool isRequestExist)
	{
		if (!isComplete)
		{
			InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1528"));
		}
		else if (isRequestExist)
		{
			InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1563"));
		}
	}

	private void DeactivateAchievementBox()
	{
		this.achievementBox.HideBox();
	}

	private void DeactivateDialogBox()
	{
		this.DialogBoxOkClick = null;
		this.DialogBoxCancelClick = null;
		this.dialogBoxContainer.gameObject.SetActive(false);
	}

	private void DeactivateInfoBox()
	{
		this.background.gameObject.SetActive(false);
		this.infoBoxContainer.gameObject.SetActive(false);
	}

	private void DeactivateRestorePanel()
	{
		this.DialogBoxOkClick = null;
		this.DialogBoxCancelClick = null;
		if (this.restoreWindowPanel != null)
		{
			this.restoreWindowPanel.SetActive(false);
		}
	}

	private void HandleEscape()
	{
		this.OnClickCancelDialog();
	}

	private void HandleLocalizationChanged()
	{
		this.processingDataBoxLabel.text = LocalizationStore.Key_0348;
	}

	private void Hide()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		if (this._unsubscribe != null)
		{
			this._unsubscribe();
		}
		if (this._typeCurrentWindow == InfoWindowController.WindowType.None)
		{
			return;
		}
		if (this._typeCurrentWindow == InfoWindowController.WindowType.infoBox)
		{
			this.DeactivateInfoBox();
		}
		else if (this._typeCurrentWindow == InfoWindowController.WindowType.processDataBox)
		{
			this.SetActiveProcessDataBox(false);
		}
		else if (this._typeCurrentWindow == InfoWindowController.WindowType.DialogBox)
		{
			this.DeactivateDialogBox();
		}
		else if (this._typeCurrentWindow == InfoWindowController.WindowType.RestoreInventory)
		{
			this.DeactivateRestorePanel();
		}
		this.SetActiveBackground(true);
		this._typeCurrentWindow = InfoWindowController.WindowType.None;
		base.gameObject.SetActive(false);
	}

	public static void HideCurrentWindow()
	{
		InfoWindowController.Instance.Hide();
	}

	private void HideInfoAndProcessingBox()
	{
		if (this._unsubscribe != null)
		{
			this._unsubscribe();
		}
		if (this._typeCurrentWindow == InfoWindowController.WindowType.None || this._typeCurrentWindow != InfoWindowController.WindowType.infoBox && this._typeCurrentWindow != InfoWindowController.WindowType.processDataBox)
		{
			return;
		}
		if (this._typeCurrentWindow == InfoWindowController.WindowType.infoBox)
		{
			this.DeactivateInfoBox();
		}
		else if (this._typeCurrentWindow == InfoWindowController.WindowType.processDataBox)
		{
			this.SetActiveProcessDataBox(false);
		}
		this.SetActiveBackground(true);
		this._typeCurrentWindow = InfoWindowController.WindowType.None;
		base.gameObject.SetActive(false);
	}

	public static void HideProcessing(float time)
	{
		InfoWindowController.Instance.Invoke("HideInfoAndProcessingBox", time);
	}

	public static void HideProcessing()
	{
		InfoWindowController.Instance.HideInfoAndProcessingBox();
	}

	private void Initialize(InfoWindowController.WindowType typeWindow)
	{
		this._typeCurrentWindow = typeWindow;
		base.gameObject.SetActive(true);
	}

	public void OnClickCancelDialog()
	{
		if (this.DialogBoxCancelClick != null)
		{
			this.DialogBoxCancelClick();
		}
		this.Hide();
	}

	public void OnClickExitButton()
	{
		if (this._typeCurrentWindow == InfoWindowController.WindowType.blockClick)
		{
			return;
		}
		this.Hide();
	}

	public void OnClickOkDialog()
	{
		if (this.DialogBoxOkClick != null)
		{
			this.DialogBoxOkClick();
		}
		this.Hide();
	}

	private void OnDestroy()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		if (this._unsubscribe != null)
		{
			this._unsubscribe();
		}
	}

	private void SetActiveBackground(bool enable)
	{
		this.background.gameObject.SetActive(enable);
	}

	private void SetActiveProcessDataBox(bool enable)
	{
		this.processindDataBoxContainer.gameObject.SetActive(enable);
	}

	public static void ShowAchievementBox(string header, string text)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.AchievementMessage);
		InfoWindowController.Instance.ActivateAchievementBox(header, text);
	}

	public static void ShowDevConsole()
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.DeveloperConsoleMini);
		InfoWindowController.Instance.ActivateDevConsole();
	}

	public static void ShowDialogBox(string text, Action callbackOkButton, Action callbackCancelButton = null)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.DialogBox);
		InfoWindowController.Instance.ActivateDialogBox(text, callbackOkButton, callbackCancelButton);
	}

	public static void ShowInfoBox(string text)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.infoBox);
		InfoWindowController.Instance.ActivateInfoBox(text);
	}

	public static void ShowProcessingDataBox()
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.processDataBox);
		InfoWindowController.Instance.SetActiveProcessDataBox(true);
		InfoWindowController.Instance.SetActiveBackground(false);
	}

	public static void ShowRestorePanel(Action okCallback)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.RestoreInventory);
		InfoWindowController.Instance.ActivateRestorePanel(okCallback);
	}

	private void Start()
	{
		this.processingDataBoxLabel.text = LocalizationStore.Key_0348;
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	private enum WindowType
	{
		infoBox,
		processDataBox,
		blockClick,
		DialogBox,
		AchievementMessage,
		RestoreInventory,
		DeveloperConsoleMini,
		None
	}
}