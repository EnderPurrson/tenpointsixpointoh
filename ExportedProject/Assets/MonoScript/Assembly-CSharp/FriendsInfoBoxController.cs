using I2.Loc;
using System;
using UnityEngine;

public class FriendsInfoBoxController : MonoBehaviour
{
	public UIWidget background;

	[Header("Processing data box")]
	public UIWidget processindDataBoxContainer;

	public UILabel processingDataBoxLabel;

	[Header("Info box")]
	public UIWidget infoBoxContainer;

	public UILabel infoBoxLabel;

	private FriendsInfoBoxController.BoxType _currentTypeBox = FriendsInfoBoxController.BoxType.None;

	public FriendsInfoBoxController()
	{
	}

	private void HandleLocalizationChanged()
	{
		this.processingDataBoxLabel.text = LocalizationStore.Key_0348;
	}

	public void Hide()
	{
		this._currentTypeBox = FriendsInfoBoxController.BoxType.None;
		base.gameObject.SetActive(false);
	}

	public void OnClickExitButton()
	{
		if (this._currentTypeBox == FriendsInfoBoxController.BoxType.processDataWindow || this._currentTypeBox == FriendsInfoBoxController.BoxType.blockClick)
		{
			return;
		}
		this.Hide();
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	public void SetBlockClickState()
	{
		this._currentTypeBox = FriendsInfoBoxController.BoxType.blockClick;
		base.gameObject.SetActive(true);
		this.processindDataBoxContainer.gameObject.SetActive(false);
		this.infoBoxContainer.gameObject.SetActive(false);
		this.background.gameObject.SetActive(false);
	}

	public void ShowInfoBox(string text)
	{
		this._currentTypeBox = FriendsInfoBoxController.BoxType.infoWindow;
		base.gameObject.SetActive(true);
		this.processindDataBoxContainer.gameObject.SetActive(false);
		this.infoBoxLabel.text = text;
		this.infoBoxContainer.gameObject.SetActive(true);
		this.background.gameObject.SetActive(true);
	}

	public void ShowProcessingDataBox()
	{
		this._currentTypeBox = FriendsInfoBoxController.BoxType.processDataWindow;
		base.gameObject.SetActive(true);
		this.processindDataBoxContainer.gameObject.SetActive(true);
		this.infoBoxContainer.gameObject.SetActive(false);
		this.background.gameObject.SetActive(false);
	}

	private void Start()
	{
		this.processingDataBoxLabel.text = LocalizationStore.Key_0348;
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	private enum BoxType
	{
		infoWindow,
		processDataWindow,
		blockClick,
		None
	}
}