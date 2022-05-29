using I2.Loc;
using System;
using UnityEngine;

public class DaterDayLivedLabel : MonoBehaviour
{
	private UILabel myLabel;

	public DaterDayLivedLabel()
	{
	}

	private void Awake()
	{
		this.myLabel = base.GetComponent<UILabel>();
	}

	private void HandleLocalizationChanged()
	{
		this.SetText();
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	private void OnEnable()
	{
		this.SetText();
	}

	private void SetText()
	{
		this.myLabel.text = string.Concat(LocalizationStore.Get("Key_1615"), ": ", Storager.getInt("DaterDayLived", false));
	}

	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}
}