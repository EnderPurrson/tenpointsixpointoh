using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkinEditorController : MonoBehaviour
{
	public static Color colorForPaint;

	public static Color[] colorHistory;

	public static SkinEditorController.BrashMode brashMode;

	public static SkinEditorController.BrashMode brashModeOld;

	public GameObject topPart;

	public GameObject downPart;

	public GameObject leftPart;

	public GameObject frontPart;

	public GameObject rigthPart;

	public GameObject backPart;

	public static SkinEditorController.ModeEditor modeEditor;

	public static SkinEditorController sharedController;

	public ButtonHandler saveButton;

	public ButtonHandler backButton;

	public ButtonHandler fillButton;

	public ButtonHandler eraserButton;

	public ButtonHandler brashButton;

	public ButtonHandler pencilButton;

	public ButtonHandler pipetteButton;

	public ButtonHandler colorButton;

	public ButtonHandler okColorInPalitraButton;

	public ButtonHandler setColorButton;

	public ButtonHandler saveChangesButton;

	public ButtonHandler cancelInSaveChangesButton;

	public ButtonHandler okInSaveSkin;

	public ButtonHandler cancelInSaveSkin;

	public ButtonHandler yesInLeaveSave;

	public ButtonHandler noInLeaveSave;

	public ButtonHandler prevHistoryButton;

	public ButtonHandler nextHistoryButton;

	public ButtonHandler yesSaveButtonInEdit;

	public ButtonHandler noSaveButtonInEdit;

	public ButtonHandler presetsButton;

	public ButtonHandler closePresetPanelButton;

	public ButtonHandler selectPresetButton;

	public ButtonHandler centeredPresetButton;

	public GameObject previewPers;

	public GameObject previewPersShadow;

	public GameObject skinPreviewPanel;

	public GameObject partPreviewPanel;

	public GameObject editorPanel;

	public GameObject colorPanel;

	public GameObject saveChangesPanel;

	public GameObject saveSkinPanel;

	public GameObject leavePanel;

	public GameObject savePanelInEditorTexture;

	public GameObject presetsPanel;

	public string selectedPartName;

	public GameObject selectedSide;

	public static Texture2D currentSkin;

	public static string currentSkinName;

	public static Dictionary<string, Dictionary<string, Rect>> rectsPartsInSkin;

	public static Dictionary<string, Dictionary<string, Texture2D>> texturesParts;

	public UILabel pensilLabel;

	public UILabel brashLabel;

	public UILabel eraserLabel;

	public UILabel fillLabel;

	public UILabel pipetteLabel;

	public UITexture editorTexture;

	public UISprite oldColor;

	public UISprite newColor;

	public UISprite[] colorHistorySprites;

	public ButtonHandler[] colorHistoryButtons;

	public UIInput skinNameInput;

	public static bool isEditingPartSkin;

	public static bool isEditingSkin;

	public bool isSaveAndExit;

	private List<GameObject> currentPreviewsSkin = new List<GameObject>();

	private string newNameSkin;

	public UISprite[] colorOnBrashSprites;

	public FrameResizer frameResizer;

	public UIWrapContent skinPresentsWrap;

	private List<string> presentSkins = SkinsController.GetSkinsIdList();

	private IDisposable _backSubscription;

	static SkinEditorController()
	{
		SkinEditorController.colorForPaint = new Color(0f, 1f, 0f, 1f);
		SkinEditorController.colorHistory = new Color[] { Color.clear, Color.clear, Color.clear, Color.clear, Color.clear };
		SkinEditorController.brashMode = SkinEditorController.BrashMode.Pencil;
		SkinEditorController.brashModeOld = SkinEditorController.BrashMode.Pencil;
		SkinEditorController.modeEditor = SkinEditorController.ModeEditor.SkinPers;
		SkinEditorController.sharedController = null;
		SkinEditorController.currentSkin = null;
		SkinEditorController.currentSkinName = null;
		SkinEditorController.rectsPartsInSkin = new Dictionary<string, Dictionary<string, Rect>>();
		SkinEditorController.texturesParts = new Dictionary<string, Dictionary<string, Texture2D>>();
		SkinEditorController.isEditingPartSkin = false;
		SkinEditorController.isEditingSkin = false;
		SkinEditorController.ExitFromSkinEditor = null;
	}

	public SkinEditorController()
	{
	}

	private void AddColor(Color color)
	{
		this.SetCurrentColor(color);
		for (int i = 0; i < (int)SkinEditorController.colorHistory.Length; i++)
		{
			if (SkinEditorController.colorHistory[i] == color)
			{
				return;
			}
		}
		for (int j = 1; j < (int)SkinEditorController.colorHistory.Length; j++)
		{
			SkinEditorController.colorHistory[(int)SkinEditorController.colorHistory.Length - j] = SkinEditorController.colorHistory[(int)SkinEditorController.colorHistory.Length - j - 1];
		}
		SkinEditorController.colorHistory[0] = color;
		this.UpdateHistoryColors();
	}

	public static Texture2D BuildSkin(Dictionary<string, Dictionary<string, Texture2D>> _texturesParts)
	{
		int num = SkinEditorController.currentSkin.width;
		int num1 = SkinEditorController.currentSkin.height;
		Texture2D texture2D = new Texture2D(num, num1, TextureFormat.RGBA32, false);
		Color color = Color.clear;
		for (int i = 0; i < num1; i++)
		{
			for (int j = 0; j < num; j++)
			{
				texture2D.SetPixel(j, i, color);
			}
		}
		foreach (KeyValuePair<string, Dictionary<string, Texture2D>> _texturesPart in _texturesParts)
		{
			foreach (KeyValuePair<string, Texture2D> item in _texturesParts[_texturesPart.Key])
			{
				Rect rect = SkinEditorController.rectsPartsInSkin[_texturesPart.Key][item.Key];
				int num2 = Mathf.RoundToInt(rect.x);
				Rect item1 = SkinEditorController.rectsPartsInSkin[_texturesPart.Key][item.Key];
				int num3 = Mathf.RoundToInt(item1.y);
				Rect rect1 = SkinEditorController.rectsPartsInSkin[_texturesPart.Key][item.Key];
				int num4 = Mathf.RoundToInt(rect1.width);
				Rect item2 = SkinEditorController.rectsPartsInSkin[_texturesPart.Key][item.Key];
				texture2D.SetPixels(num2, num3, num4, Mathf.RoundToInt(item2.height), _texturesParts[_texturesPart.Key][item.Key].GetPixels());
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}

	private void ExitFromScene(EventArgs e = null)
	{
		if (SkinEditorController.ExitFromSkinEditor != null)
		{
			SkinEditorController.ExitFromSkinEditor((SkinEditorController.modeEditor != SkinEditorController.ModeEditor.LogoClan || e == null || !(e is EditorClosingEventArgs) || !(e as EditorClosingEventArgs).ClanLogoSaved ? this.newNameSkin : "SAVED"));
			SkinEditorController.currentSkinName = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		if (SkinEditorController.modeEditor != SkinEditorController.ModeEditor.LogoClan && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
	}

	private void GetPresetSkins()
	{
		this.presentSkins = SkinsController.GetSkinsIdList();
		this.skinPresentsWrap.maxIndex = this.presentSkins.Count - 1;
	}

	private void HandleBackButtonClicked(object sender, EventArgs e)
	{
		if (!this.partPreviewPanel.activeSelf)
		{
			if (!this.editorPanel.activeSelf)
			{
				if (this.colorPanel.activeSelf)
				{
					this.editorPanel.SetActive(true);
					this.colorPanel.SetActive(false);
					return;
				}
				if (!this.skinPreviewPanel.activeSelf)
				{
					return;
				}
				if (!SkinEditorController.isEditingSkin)
				{
					this.ExitFromScene(e);
				}
				else
				{
					this.leavePanel.SetActive(true);
					this.HidePreviewSkin();
				}
				return;
			}
			if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.SkinPers)
			{
				this.editorPanel.SetActive(false);
				this.partPreviewPanel.SetActive(true);
				this.selectedSide.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture((Texture2D)this.editorTexture.mainTexture);
			}
			else if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.Cape || SkinEditorController.modeEditor == SkinEditorController.ModeEditor.LogoClan)
			{
				if (!SkinEditorController.isEditingPartSkin)
				{
					this.ExitFromScene(e);
				}
				else
				{
					this.savePanelInEditorTexture.SetActive(true);
				}
			}
			return;
		}
		if (!SkinEditorController.isEditingPartSkin)
		{
			this.partPreviewPanel.SetActive(false);
			this.skinPreviewPanel.SetActive(true);
			this.backButton.gameObject.GetComponent<UIButton>().isEnabled = true;
			this.topPart.GetComponent<UIButton>().isEnabled = true;
			this.downPart.GetComponent<UIButton>().isEnabled = true;
			this.leftPart.GetComponent<UIButton>().isEnabled = true;
			this.frontPart.GetComponent<UIButton>().isEnabled = true;
			this.rigthPart.GetComponent<UIButton>().isEnabled = true;
			this.backPart.GetComponent<UIButton>().isEnabled = true;
		}
		else
		{
			this.saveChangesPanel.SetActive(true);
			this.backButton.gameObject.GetComponent<UIButton>().isEnabled = false;
			this.topPart.GetComponent<UIButton>().isEnabled = false;
			this.downPart.GetComponent<UIButton>().isEnabled = false;
			this.leftPart.GetComponent<UIButton>().isEnabled = false;
			this.frontPart.GetComponent<UIButton>().isEnabled = false;
			this.rigthPart.GetComponent<UIButton>().isEnabled = false;
			this.backPart.GetComponent<UIButton>().isEnabled = false;
		}
	}

	private void HandleCancelInSaveChangesButtonClicked(object sender, EventArgs e)
	{
		SkinEditorController.isEditingPartSkin = false;
		this.saveChangesPanel.SetActive(false);
		this.HandleBackButtonClicked(null, null);
	}

	private void HandleCancelInSaveSkinClicked(object sender, EventArgs e)
	{
		if (!this.isSaveAndExit)
		{
			this.ShowPreviewSkin();
			this.saveSkinPanel.SetActive(false);
		}
		else
		{
			this.saveSkinPanel.SetActive(false);
			this.leavePanel.SetActive(true);
		}
	}

	private void HandleClosePresetClicked(object sender, EventArgs e)
	{
		this.ShowPreviewSkin();
		this.presetsPanel.SetActive(false);
		this.OnEnable();
	}

	private void HandleNoInLeaveSaveClicked(object sender, EventArgs e)
	{
		SkinEditorController.isEditingSkin = false;
		this.ShowPreviewSkin();
		this.leavePanel.SetActive(false);
		this.HandleBackButtonClicked(null, null);
	}

	private void HandleNoSaveButtonInEditClicked(object sender, EventArgs e)
	{
		SkinEditorController.isEditingPartSkin = false;
		this.HandleBackButtonClicked(null, null);
	}

	private void HandleOkInSaveSkinClicked(object sender, EventArgs e)
	{
		this.ShowPreviewSkin();
		this.newNameSkin = SkinsController.AddUserSkin(this.skinNameInput.@value, SkinEditorController.currentSkin, SkinEditorController.currentSkinName);
		SkinsController.SetCurrentSkin(this.newNameSkin);
		SkinEditorController.isEditingSkin = false;
		this.saveSkinPanel.SetActive(false);
		this.HandleBackButtonClicked(null, null);
	}

	private void HandlePresetsButtonClicked(object sender, EventArgs e)
	{
		this.OpenPresetsWindow();
	}

	private void HandleSaveButtonClicked(object sender, EventArgs e)
	{
		this.isSaveAndExit = false;
		this.saveSkinPanel.SetActive(true);
		this.HidePreviewSkin();
	}

	private void HandleSaveChangesButtonClicked(object sender, EventArgs e)
	{
		SkinEditorController.isEditingPartSkin = false;
		SkinEditorController.isEditingSkin = true;
		this.saveChangesPanel.SetActive(false);
		this.SavePartInTexturesParts(this.selectedPartName);
		SkinEditorController.currentSkin = SkinEditorController.BuildSkin(SkinEditorController.texturesParts);
		SkinsController.SetTextureRecursivelyFrom(this.previewPers, SkinEditorController.currentSkin, null);
		this.UpdateTexturesPartsInDictionary();
		this.HandleBackButtonClicked(null, null);
	}

	private void HandleSelectBrashClicked(object sender, EventArgs e)
	{
		string str = (sender as MonoBehaviour).gameObject.name;
		Debug.Log(str);
		if (str.Equals("Fill"))
		{
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Fill;
		}
		if (str.Equals("Brash"))
		{
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Brash;
		}
		if (str.Equals("Pencil"))
		{
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Pencil;
		}
		if (str.Equals("Eraser"))
		{
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Eraser;
		}
		if (str.Equals("Pipette"))
		{
			if (SkinEditorController.brashMode != SkinEditorController.BrashMode.Pipette)
			{
				SkinEditorController.brashModeOld = SkinEditorController.brashMode;
			}
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Pipette;
		}
	}

	private void HandleSelectColorClicked(object sender, EventArgs e)
	{
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pipette)
		{
			SkinEditorController.brashMode = SkinEditorController.brashModeOld;
			this.pencilButton.gameObject.GetComponent<UIToggle>().@value = true;
		}
		this.editorPanel.SetActive(false);
		this.colorPanel.SetActive(true);
		this.oldColor.color = SkinEditorController.colorForPaint;
		this.newColor.color = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
	}

	private void HandleSelectPresetClicked(object sender, EventArgs e)
	{
		GameObject component = this.skinPresentsWrap.GetComponent<MyCenterOnChild>().centeredObject;
		if (component == null)
		{
			return;
		}
		this.SetPresetSkin(component);
		this.HandleClosePresetClicked(null, null);
	}

	public void HandleSetColorClicked(object sender, EventArgs e)
	{
		this.editorPanel.SetActive(true);
		this.colorPanel.SetActive(false);
		this.AddColor(this.newColor.color);
	}

	public void HandleSetHistoryColorClicked(object sender, EventArgs e)
	{
		for (int i = 0; i < (int)this.colorHistoryButtons.Length; i++)
		{
			if (this.colorHistoryButtons[i].Equals(sender))
			{
				this.SetCurrentColor(SkinEditorController.colorHistory[i]);
			}
		}
	}

	private void HandleSideClicked(object sender, EventArgs e)
	{
		this.selectedSide = (sender as MonoBehaviour).gameObject;
		this.editorPanel.SetActive(true);
		this.partPreviewPanel.SetActive(false);
		this.editorTexture.gameObject.GetComponent<EditorTextures>().SetStartCanvas((Texture2D)this.selectedSide.transform.GetChild(0).GetComponent<UITexture>().mainTexture);
	}

	private void HandleYesInLeaveSaveClicked(object sender, EventArgs e)
	{
		this.leavePanel.SetActive(false);
		this.saveSkinPanel.SetActive(true);
		this.isSaveAndExit = true;
	}

	private void HandleYesSaveButtonInEditClicked(object sender, EventArgs e)
	{
		EventArgs editorClosingEventArg;
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.LogoClan)
		{
			Debug.Log("modeEditor==ModeEditor.LogoClan");
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture((Texture2D)this.editorTexture.mainTexture);
		}
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.Cape)
		{
			SkinsController.capeUserTexture = EditorTextures.CreateCopyTexture((Texture2D)this.editorTexture.mainTexture);
			string str = SkinsController.StringFromTexture(SkinsController.capeUserTexture);
			CapeMemento capeMemento = new CapeMemento(DateTime.UtcNow.Ticks, str);
			PlayerPrefs.SetString("NewUserCape", JsonUtility.ToJson(capeMemento));
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer)
			{
				SkinsSynchronizer.Instance.Push();
			}
			else
			{
				SkinsSynchronizer.Instance.Sync();
			}
		}
		SkinEditorController.isEditingPartSkin = false;
		if (SkinEditorController.modeEditor != SkinEditorController.ModeEditor.LogoClan)
		{
			editorClosingEventArg = null;
		}
		else
		{
			editorClosingEventArg = new EditorClosingEventArgs()
			{
				ClanLogoSaved = true
			};
		}
		this.HandleBackButtonClicked(null, editorClosingEventArg);
	}

	private void HidePreviewSkin()
	{
		foreach (GameObject gameObject in this.currentPreviewsSkin)
		{
			gameObject.SetActive(false);
		}
		this.backButton.gameObject.GetComponent<UIButton>().isEnabled = false;
		this.saveButton.gameObject.GetComponent<UIButton>().isEnabled = false;
		this.presetsButton.gameObject.GetComponent<UIButton>().isEnabled = false;
	}

	private void OnDestroy()
	{
		SkinEditorController.sharedController = null;
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(() => this.HandleBackButtonClicked(this, EventArgs.Empty), "Skin Editor");
	}

	private void OpenPresetsWindow()
	{
		this.HidePreviewSkin();
		this.presetsPanel.SetActive(true);
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(() => this.HandleClosePresetClicked(this, EventArgs.Empty), "Skin Editor Presets Window");
	}

	private void SavePartInTexturesParts(string _partName)
	{
		Dictionary<string, Texture2D> strs = new Dictionary<string, Texture2D>();
		foreach (KeyValuePair<string, Texture2D> item in SkinEditorController.texturesParts[_partName])
		{
			if (item.Key.Equals("Top"))
			{
				strs.Add(item.Key, EditorTextures.CreateCopyTexture((Texture2D)this.topPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (item.Key.Equals("Down"))
			{
				strs.Add(item.Key, EditorTextures.CreateCopyTexture((Texture2D)this.downPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (item.Key.Equals("Left"))
			{
				strs.Add(item.Key, EditorTextures.CreateCopyTexture((Texture2D)this.leftPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (item.Key.Equals("Front"))
			{
				strs.Add(item.Key, EditorTextures.CreateCopyTexture((Texture2D)this.frontPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (item.Key.Equals("Right"))
			{
				strs.Add(item.Key, EditorTextures.CreateCopyTexture((Texture2D)this.rigthPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (!item.Key.Equals("Back"))
			{
				continue;
			}
			strs.Add(item.Key, EditorTextures.CreateCopyTexture((Texture2D)this.backPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
		}
		if (_partName.Equals("Arm_right") || _partName.Equals("Arm_left"))
		{
			SkinEditorController.texturesParts.Remove("Arm_right");
			SkinEditorController.texturesParts.Add("Arm_right", strs);
			SkinEditorController.texturesParts.Remove("Arm_left");
			SkinEditorController.texturesParts.Add("Arm_left", strs);
		}
		if (_partName.Equals("Foot_right") || _partName.Equals("Foot_left"))
		{
			SkinEditorController.texturesParts.Remove("Foot_right");
			SkinEditorController.texturesParts.Add("Foot_right", strs);
			SkinEditorController.texturesParts.Remove("Foot_left");
			SkinEditorController.texturesParts.Add("Foot_left", strs);
		}
		SkinEditorController.texturesParts.Remove(_partName);
		SkinEditorController.texturesParts.Add(_partName, strs);
	}

	public void SelectPart(string _partName)
	{
		if (!SkinEditorController.texturesParts.ContainsKey(_partName))
		{
			Debug.Log("texturesParts not contain key");
			return;
		}
		SkinEditorController.isEditingPartSkin = false;
		this.selectedPartName = _partName;
		this.topPart.SetActive(false);
		this.downPart.SetActive(false);
		this.leftPart.SetActive(false);
		this.frontPart.SetActive(false);
		this.rigthPart.SetActive(false);
		this.backPart.SetActive(false);
		int num = 22;
		foreach (KeyValuePair<string, Texture2D> item in SkinEditorController.texturesParts[_partName])
		{
			if (item.Key.Equals("Top"))
			{
				this.topPart.SetActive(true);
				this.topPart.GetComponent<BoxCollider>().size = new Vector3((float)(item.Value.width * num), (float)(item.Value.height * num), 0f);
				this.topPart.transform.GetChild(0).GetComponent<UITexture>().width = item.Value.width * num;
				this.topPart.transform.GetChild(0).GetComponent<UITexture>().height = item.Value.height * num;
				this.topPart.transform.localPosition = new Vector3((float)(-item.Value.width) * 0.5f * (float)num, (float)(SkinEditorController.texturesParts[_partName]["Front"].height + item.Value.height) * 0.5f * (float)num, 0f);
				this.topPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(item.Value);
			}
			if (item.Key.Equals("Down"))
			{
				this.downPart.SetActive(true);
				this.downPart.GetComponent<BoxCollider>().size = new Vector3((float)(item.Value.width * num), (float)(item.Value.height * num), 0f);
				this.downPart.transform.GetChild(0).GetComponent<UITexture>().width = item.Value.width * num;
				this.downPart.transform.GetChild(0).GetComponent<UITexture>().height = item.Value.height * num;
				this.downPart.transform.localPosition = new Vector3((float)(-item.Value.width) * 0.5f * (float)num, (float)(-(SkinEditorController.texturesParts[_partName]["Front"].height + item.Value.height)) * 0.5f * (float)num, 0f);
				this.downPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(item.Value);
			}
			if (item.Key.Equals("Left"))
			{
				this.leftPart.SetActive(true);
				this.leftPart.GetComponent<BoxCollider>().size = new Vector3((float)(item.Value.width * num), (float)(item.Value.height * num), 0f);
				this.leftPart.transform.GetChild(0).GetComponent<UITexture>().width = item.Value.width * num;
				this.leftPart.transform.GetChild(0).GetComponent<UITexture>().height = item.Value.height * num;
				this.leftPart.transform.localPosition = new Vector3(-((float)item.Value.width * 0.5f + (float)SkinEditorController.texturesParts[_partName]["Front"].width) * (float)num, 0f, 0f);
				this.leftPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(item.Value);
			}
			if (item.Key.Equals("Front"))
			{
				this.frontPart.SetActive(true);
				this.frontPart.GetComponent<BoxCollider>().size = new Vector3((float)(item.Value.width * num), (float)(item.Value.height * num), 0f);
				this.frontPart.transform.GetChild(0).GetComponent<UITexture>().width = item.Value.width * num;
				this.frontPart.transform.GetChild(0).GetComponent<UITexture>().height = item.Value.height * num;
				this.frontPart.transform.localPosition = new Vector3(-((float)item.Value.width * 0.5f) * (float)num, 0f, 0f);
				this.frontPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(item.Value);
			}
			if (item.Key.Equals("Right"))
			{
				this.rigthPart.SetActive(true);
				this.rigthPart.GetComponent<BoxCollider>().size = new Vector3((float)(item.Value.width * num), (float)(item.Value.height * num), 0f);
				this.rigthPart.transform.GetChild(0).GetComponent<UITexture>().width = item.Value.width * num;
				this.rigthPart.transform.GetChild(0).GetComponent<UITexture>().height = item.Value.height * num;
				this.rigthPart.transform.localPosition = new Vector3((float)item.Value.width * 0.5f * (float)num, 0f, 0f);
				this.rigthPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(item.Value);
			}
			if (!item.Key.Equals("Back"))
			{
				continue;
			}
			this.backPart.SetActive(true);
			this.backPart.GetComponent<BoxCollider>().size = new Vector3((float)(item.Value.width * num), (float)(item.Value.height * num), 0f);
			this.backPart.transform.GetChild(0).GetComponent<UITexture>().width = item.Value.width * num;
			this.backPart.transform.GetChild(0).GetComponent<UITexture>().height = item.Value.height * num;
			this.backPart.transform.localPosition = new Vector3(((float)item.Value.width * 0.5f + (float)SkinEditorController.texturesParts[_partName]["Right"].width) * (float)num, 0f, 0f);
			this.backPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(item.Value);
		}
		this.partPreviewPanel.SetActive(true);
		this.skinPreviewPanel.SetActive(false);
	}

	public void SetColorClickedUp()
	{
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pipette)
		{
			SkinEditorController.brashMode = SkinEditorController.brashModeOld;
			switch (SkinEditorController.brashMode)
			{
				case SkinEditorController.BrashMode.Pencil:
				{
					this.pencilButton.gameObject.GetComponent<UIToggle>().@value = true;
					break;
				}
				case SkinEditorController.BrashMode.Brash:
				{
					this.brashButton.gameObject.GetComponent<UIToggle>().@value = true;
					break;
				}
				case SkinEditorController.BrashMode.Eraser:
				{
					SkinEditorController.brashMode = SkinEditorController.BrashMode.Pencil;
					this.pencilButton.gameObject.GetComponent<UIToggle>().@value = true;
					break;
				}
				case SkinEditorController.BrashMode.Fill:
				{
					this.fillButton.gameObject.GetComponent<UIToggle>().@value = true;
					break;
				}
			}
		}
	}

	private void SetCurrentColor(Color color)
	{
		SkinEditorController.colorForPaint = color;
		this.colorButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.colorButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.colorButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
		PlayerPrefs.SetFloat("ColorForPaintR", SkinEditorController.colorForPaint.r);
		PlayerPrefs.SetFloat("ColorForPaintG", SkinEditorController.colorForPaint.g);
		PlayerPrefs.SetFloat("ColorForPaintB", SkinEditorController.colorForPaint.b);
		for (int i = 0; i < (int)this.colorOnBrashSprites.Length; i++)
		{
			this.colorOnBrashSprites[i].color = SkinEditorController.colorForPaint;
		}
	}

	private void SetPartsRect()
	{
		Dictionary<string, Rect> strs = new Dictionary<string, Rect>();
		SkinEditorController.rectsPartsInSkin.Clear();
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.SkinPers)
		{
			Dictionary<string, Rect> strs1 = new Dictionary<string, Rect>()
			{
				{ "Top", new Rect(8f, 24f, 8f, 8f) },
				{ "Down", new Rect(16f, 24f, 8f, 8f) },
				{ "Left", new Rect(0f, 16f, 8f, 8f) },
				{ "Front", new Rect(8f, 16f, 8f, 8f) },
				{ "Right", new Rect(16f, 16f, 8f, 8f) },
				{ "Back", new Rect(24f, 16f, 8f, 8f) }
			};
			SkinEditorController.rectsPartsInSkin.Add("Head", strs1);
			Dictionary<string, Rect> strs2 = new Dictionary<string, Rect>()
			{
				{ "Top", new Rect(4f, 12f, 4f, 4f) },
				{ "Down", new Rect(8f, 12f, 4f, 4f) },
				{ "Left", new Rect(0f, 0f, 4f, 12f) },
				{ "Front", new Rect(4f, 0f, 4f, 12f) },
				{ "Right", new Rect(8f, 0f, 4f, 12f) },
				{ "Back", new Rect(12f, 0f, 4f, 12f) }
			};
			SkinEditorController.rectsPartsInSkin.Add("Foot_left", strs2);
			Dictionary<string, Rect> strs3 = new Dictionary<string, Rect>()
			{
				{ "Top", new Rect(4f, 12f, 4f, 4f) },
				{ "Down", new Rect(8f, 12f, 4f, 4f) },
				{ "Left", new Rect(0f, 0f, 4f, 12f) },
				{ "Front", new Rect(4f, 0f, 4f, 12f) },
				{ "Right", new Rect(8f, 0f, 4f, 12f) },
				{ "Back", new Rect(12f, 0f, 4f, 12f) }
			};
			SkinEditorController.rectsPartsInSkin.Add("Foot_right", strs3);
			Dictionary<string, Rect> strs4 = new Dictionary<string, Rect>()
			{
				{ "Top", new Rect(20f, 12f, 8f, 4f) },
				{ "Down", new Rect(28f, 12f, 8f, 4f) },
				{ "Left", new Rect(16f, 0f, 4f, 12f) },
				{ "Front", new Rect(20f, 0f, 8f, 12f) },
				{ "Right", new Rect(28f, 0f, 4f, 12f) },
				{ "Back", new Rect(32f, 0f, 8f, 12f) }
			};
			SkinEditorController.rectsPartsInSkin.Add("Body", strs4);
			Dictionary<string, Rect> strs5 = new Dictionary<string, Rect>()
			{
				{ "Top", new Rect(44f, 12f, 4f, 4f) },
				{ "Down", new Rect(48f, 12f, 4f, 4f) },
				{ "Left", new Rect(40f, 0f, 4f, 12f) },
				{ "Front", new Rect(44f, 0f, 4f, 12f) },
				{ "Right", new Rect(48f, 0f, 4f, 12f) },
				{ "Back", new Rect(52f, 0f, 4f, 12f) }
			};
			SkinEditorController.rectsPartsInSkin.Add("Arm_right", strs5);
			Dictionary<string, Rect> strs6 = new Dictionary<string, Rect>()
			{
				{ "Top", new Rect(44f, 12f, 4f, 4f) },
				{ "Down", new Rect(48f, 12f, 4f, 4f) },
				{ "Left", new Rect(40f, 0f, 4f, 12f) },
				{ "Front", new Rect(44f, 0f, 4f, 12f) },
				{ "Right", new Rect(48f, 0f, 4f, 12f) },
				{ "Back", new Rect(52f, 0f, 4f, 12f) }
			};
			SkinEditorController.rectsPartsInSkin.Add("Arm_left", strs6);
		}
	}

	private void SetPresetSkin(GameObject obj)
	{
		MeshRenderer component = obj.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		SkinEditorController.currentSkin = component.material.mainTexture as Texture2D;
		SkinsController.SetTextureRecursivelyFrom(this.previewPers, SkinEditorController.currentSkin, null);
		this.SetPartsRect();
		this.UpdateTexturesPartsInDictionary();
	}

	private void SetPresetSkin(int index)
	{
		SkinEditorController.currentSkin = SkinsController.skinsForPers[this.presentSkins[index]];
		SkinsController.SetTextureRecursivelyFrom(this.previewPers, SkinEditorController.currentSkin, null);
		this.SetPartsRect();
		this.UpdateTexturesPartsInDictionary();
	}

	private void ShowPreviewSkin()
	{
		foreach (GameObject gameObject in this.currentPreviewsSkin)
		{
			gameObject.SetActive(true);
		}
		this.backButton.gameObject.GetComponent<UIButton>().isEnabled = true;
		this.saveButton.gameObject.GetComponent<UIButton>().isEnabled = true;
		this.presetsButton.gameObject.GetComponent<UIButton>().isEnabled = true;
	}

	private void Start()
	{
		SkinEditorController.brashMode = SkinEditorController.BrashMode.Pencil;
		SkinEditorController.isEditingSkin = false;
		SkinEditorController.isEditingPartSkin = false;
		SkinEditorController.sharedController = this;
		MenuBackgroundMusic.sharedMusic.PlayCustomMusicFrom(base.gameObject);
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
		SkinEditorController.colorForPaint = new Color(PlayerPrefs.GetFloat("ColorForPaintR", 0f), PlayerPrefs.GetFloat("ColorForPaintG", 1f), PlayerPrefs.GetFloat("ColorForPaintB", 0f), 1f);
		this.colorButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.colorButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.colorButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
		for (int i = 0; i < (int)this.colorOnBrashSprites.Length; i++)
		{
			this.colorOnBrashSprites[i].color = SkinEditorController.colorForPaint;
		}
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.SkinPers)
		{
			if (SkinEditorController.currentSkinName != null)
			{
				SkinEditorController.currentSkin = SkinsController.skinsForPers[SkinEditorController.currentSkinName];
				if (SkinsController.skinsNamesForPers.ContainsKey(SkinEditorController.currentSkinName))
				{
					this.skinNameInput.@value = SkinsController.skinsNamesForPers[SkinEditorController.currentSkinName];
				}
			}
			else
			{
				SkinEditorController.currentSkin = Resources.Load("Clear_Skin") as Texture2D;
				SkinEditorController.currentSkin.filterMode = FilterMode.Point;
				SkinEditorController.currentSkin.Apply();
				this.skinNameInput.@value = string.Empty;
			}
			Debug.Log("modeEditor== ModeEditor.SkinPers");
			this.partPreviewPanel.SetActive(false);
			this.skinPreviewPanel.SetActive(true);
			this.editorPanel.SetActive(false);
			this.currentPreviewsSkin.Add(this.previewPers);
			this.currentPreviewsSkin.Add(this.previewPersShadow);
			this.ShowPreviewSkin();
		}
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.Cape || SkinEditorController.modeEditor == SkinEditorController.ModeEditor.LogoClan)
		{
			Texture2D texture2D = null;
			if (SkinEditorController.modeEditor != SkinEditorController.ModeEditor.Cape)
			{
				texture2D = SkinsController.logoClanUserTexture;
				if (texture2D == null)
				{
					texture2D = Resources.Load<Texture2D>("Clan_Previews/icon_clan_001");
				}
			}
			else
			{
				texture2D = SkinsController.capeUserTexture;
				if (texture2D == null)
				{
					texture2D = Resources.Load<Texture2D>("cape_CustomTexture");
				}
			}
			SkinEditorController.currentSkin = EditorTextures.CreateCopyTexture(texture2D);
			this.partPreviewPanel.SetActive(false);
			this.skinPreviewPanel.SetActive(false);
			this.editorPanel.SetActive(true);
			this.editorTexture.gameObject.GetComponent<EditorTextures>().SetStartCanvas(SkinEditorController.currentSkin);
		}
		this.savePanelInEditorTexture.SetActive(false);
		SkinsController.SetTextureRecursivelyFrom(this.previewPers, SkinEditorController.currentSkin, null);
		this.SetPartsRect();
		this.UpdateTexturesPartsInDictionary();
		this.colorPanel.SetActive(false);
		this.saveChangesPanel.SetActive(false);
		this.saveSkinPanel.SetActive(false);
		this.leavePanel.SetActive(false);
		if (this.topPart != null)
		{
			ButtonHandler component = this.topPart.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += new EventHandler(this.HandleSideClicked);
			}
		}
		if (this.downPart != null)
		{
			ButtonHandler buttonHandler = this.downPart.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.HandleSideClicked);
			}
		}
		if (this.leftPart != null)
		{
			ButtonHandler component1 = this.leftPart.GetComponent<ButtonHandler>();
			if (component1 != null)
			{
				component1.Clicked += new EventHandler(this.HandleSideClicked);
			}
		}
		if (this.frontPart != null)
		{
			ButtonHandler buttonHandler1 = this.frontPart.GetComponent<ButtonHandler>();
			if (buttonHandler1 != null)
			{
				buttonHandler1.Clicked += new EventHandler(this.HandleSideClicked);
			}
		}
		if (this.rigthPart != null)
		{
			ButtonHandler component2 = this.rigthPart.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += new EventHandler(this.HandleSideClicked);
			}
		}
		if (this.backPart != null)
		{
			ButtonHandler buttonHandler2 = this.backPart.GetComponent<ButtonHandler>();
			if (buttonHandler2 != null)
			{
				buttonHandler2.Clicked += new EventHandler(this.HandleSideClicked);
			}
		}
		if (this.saveButton != null)
		{
			this.saveButton.Clicked += new EventHandler(this.HandleSaveButtonClicked);
		}
		if (this.backButton != null)
		{
			this.backButton.Clicked += new EventHandler(this.HandleBackButtonClicked);
		}
		if (this.fillButton != null)
		{
			this.fillButton.Clicked += new EventHandler(this.HandleSelectBrashClicked);
		}
		if (this.brashButton != null)
		{
			this.brashButton.Clicked += new EventHandler(this.HandleSelectBrashClicked);
		}
		if (this.pencilButton != null)
		{
			this.pencilButton.Clicked += new EventHandler(this.HandleSelectBrashClicked);
		}
		if (this.pipetteButton != null)
		{
			this.pipetteButton.Clicked += new EventHandler(this.HandleSelectBrashClicked);
		}
		if (this.eraserButton != null)
		{
			this.eraserButton.Clicked += new EventHandler(this.HandleSelectBrashClicked);
		}
		if (this.colorButton != null)
		{
			this.colorButton.Clicked += new EventHandler(this.HandleSelectColorClicked);
		}
		if (this.setColorButton != null)
		{
			this.setColorButton.Clicked += new EventHandler(this.HandleSetColorClicked);
		}
		if (this.saveChangesButton != null)
		{
			(this.saveChangesButton.GetComponent<DialogEscape>() ?? this.saveChangesButton.gameObject.AddComponent<DialogEscape>()).Context = "Save Skin Changes Dialog";
			this.saveChangesButton.Clicked += new EventHandler(this.HandleSaveChangesButtonClicked);
		}
		if (this.cancelInSaveChangesButton != null)
		{
			this.cancelInSaveChangesButton.Clicked += new EventHandler(this.HandleCancelInSaveChangesButtonClicked);
		}
		if (this.okInSaveSkin != null)
		{
			(this.okInSaveSkin.GetComponent<DialogEscape>() ?? this.okInSaveSkin.gameObject.AddComponent<DialogEscape>()).Context = "Save Skin as... Dialog";
			this.okInSaveSkin.Clicked += new EventHandler(this.HandleOkInSaveSkinClicked);
		}
		if (this.cancelInSaveSkin != null)
		{
			this.cancelInSaveSkin.Clicked += new EventHandler(this.HandleCancelInSaveSkinClicked);
		}
		if (this.yesInLeaveSave != null)
		{
			(this.yesInLeaveSave.GetComponent<DialogEscape>() ?? this.yesInLeaveSave.gameObject.AddComponent<DialogEscape>()).Context = "Save Skin Dialog";
			this.yesInLeaveSave.Clicked += new EventHandler(this.HandleYesInLeaveSaveClicked);
		}
		if (this.noInLeaveSave != null)
		{
			this.noInLeaveSave.Clicked += new EventHandler(this.HandleNoInLeaveSaveClicked);
		}
		if (this.yesSaveButtonInEdit != null)
		{
			(this.yesSaveButtonInEdit.GetComponent<DialogEscape>() ?? this.yesSaveButtonInEdit.gameObject.AddComponent<DialogEscape>()).Context = "Save Cape Dialog";
			this.yesSaveButtonInEdit.Clicked += new EventHandler(this.HandleYesSaveButtonInEditClicked);
		}
		if (this.noSaveButtonInEdit != null)
		{
			this.noSaveButtonInEdit.Clicked += new EventHandler(this.HandleNoSaveButtonInEditClicked);
		}
		for (int j = 0; j < (int)this.colorHistoryButtons.Length; j++)
		{
			this.colorHistoryButtons[j].Clicked += new EventHandler(this.HandleSetHistoryColorClicked);
		}
		if (this.presetsButton != null)
		{
			this.presetsButton.Clicked += new EventHandler(this.HandlePresetsButtonClicked);
		}
		if (this.closePresetPanelButton != null)
		{
			this.closePresetPanelButton.Clicked += new EventHandler(this.HandleClosePresetClicked);
		}
		if (this.selectPresetButton != null)
		{
			this.selectPresetButton.Clicked += new EventHandler(this.HandleSelectPresetClicked);
		}
		if (this.centeredPresetButton != null)
		{
			this.centeredPresetButton.Clicked += new EventHandler(this.HandleSelectPresetClicked);
		}
		this.AddColor(SkinEditorController.colorForPaint);
		this.UpdateHistoryColors();
		this.GetPresetSkins();
		this.skinPresentsWrap.onInitializeItem += new UIWrapContent.OnInitializeItem(this.WrapSkinPreset);
	}

	private Texture2D TextureFromRect(Texture2D texForCut, Rect rectForCut)
	{
		Color[] pixels = texForCut.GetPixels((int)rectForCut.x, (int)rectForCut.y, (int)rectForCut.width, (int)rectForCut.height);
		Texture2D texture2D = new Texture2D((int)rectForCut.width, (int)rectForCut.height)
		{
			filterMode = FilterMode.Point
		};
		texture2D.SetPixels(pixels);
		texture2D.Apply();
		return texture2D;
	}

	private void UpdateHistoryColors()
	{
		for (int i = 0; i < (int)SkinEditorController.colorHistory.Length; i++)
		{
			this.colorHistoryButtons[i].gameObject.SetActive(SkinEditorController.colorHistory[i] != Color.clear);
			this.colorHistorySprites[i].color = SkinEditorController.colorHistory[i];
			this.colorHistoryButtons[i].GetComponent<UIButton>().defaultColor = SkinEditorController.colorHistory[i];
			this.colorHistoryButtons[i].GetComponent<UIButton>().pressed = SkinEditorController.colorHistory[i];
			this.colorHistoryButtons[i].GetComponent<UIButton>().hover = SkinEditorController.colorHistory[i];
		}
		this.frameResizer.ResizeFrame();
	}

	public void UpdateTexturesPartsInDictionary()
	{
		SkinEditorController.texturesParts.Clear();
		foreach (KeyValuePair<string, Dictionary<string, Rect>> keyValuePair in SkinEditorController.rectsPartsInSkin)
		{
			Dictionary<string, Texture2D> strs = new Dictionary<string, Texture2D>();
			foreach (KeyValuePair<string, Rect> item in SkinEditorController.rectsPartsInSkin[keyValuePair.Key])
			{
				strs.Add(item.Key, this.TextureFromRect(SkinEditorController.currentSkin, SkinEditorController.rectsPartsInSkin[keyValuePair.Key][item.Key]));
			}
			SkinEditorController.texturesParts.Add(keyValuePair.Key, strs);
		}
	}

	private void WrapSkinPreset(GameObject obj, int wrapIndex, int realIndex)
	{
		MeshRenderer component = obj.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		component.material.mainTexture = SkinsController.skinsForPers[this.presentSkins[realIndex]];
	}

	public static event Action<string> ExitFromSkinEditor;

	public enum BrashMode
	{
		Pencil,
		Brash,
		Eraser,
		Fill,
		Pipette
	}

	public enum ModeEditor
	{
		SkinPers,
		Cape,
		LogoClan
	}
}