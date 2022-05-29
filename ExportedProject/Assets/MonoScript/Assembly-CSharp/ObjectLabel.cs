using System;
using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class ObjectLabel : MonoBehaviour
{
	public static Camera currentCamera;

	public Transform target;

	public Vector3 offset = Vector3.up;

	public bool clampToScreen;

	public float clampBorderSize = 0.05f;

	public bool useMainCamera = true;

	public Camera cameraToUse;

	public Camera cam;

	public float timeShow;

	public Vector3 posLabel;

	public bool isShow;

	public bool isMenu;

	public bool isShadow;

	private Transform thisTransform;

	private Transform camTransform;

	private ExperienceController expController;

	private bool isSetColor;

	public WeaponManager _weaponManager;

	private int rank = 1;

	private float koofScreen = (float)Screen.height / 768f;

	private HungerGameController hungerGameController;

	private bool isHunger;

	private bool isCompany;

	public GUITexture clanTexture;

	public GUIText clanName;

	static ObjectLabel()
	{
	}

	public ObjectLabel()
	{
	}

	public void ResetTimeShow()
	{
		this.timeShow = 1f;
	}

	private void Start()
	{
		this._weaponManager = WeaponManager.sharedManager;
		this.isHunger = Defs.isHunger;
		if (this.isHunger && GameObject.FindGameObjectWithTag("HungerGameController") != null)
		{
			this.hungerGameController = GameObject.FindGameObjectWithTag("HungerGameController").GetComponent<HungerGameController>();
		}
		this.expController = GameObject.FindGameObjectWithTag("ExperienceController").GetComponent<ExperienceController>();
		float coef = 36f * Defs.Coef;
		this.thisTransform = base.transform;
		this.cam = ObjectLabel.currentCamera;
		this.camTransform = this.cam.transform;
		base.transform.GetComponent<GUITexture>().pixelInset = new Rect(-75f * this.koofScreen, -3f * this.koofScreen, 30f * this.koofScreen, 30f * this.koofScreen);
		base.transform.GetComponent<GUIText>().pixelOffset = new Vector2(-45f * this.koofScreen, 0f);
		base.transform.GetComponent<GUIText>().fontSize = Mathf.RoundToInt(20f * this.koofScreen);
		this.isCompany = Defs.isCompany;
		this.clanTexture.pixelInset = new Rect(-64f * this.koofScreen, -18f * this.koofScreen, 15f * this.koofScreen, 15f * this.koofScreen);
		this.clanName.pixelOffset = new Vector2(-41f * this.koofScreen, -4f);
		this.clanName.fontSize = Mathf.RoundToInt(16f * this.koofScreen);
	}

	private void Update()
	{
		if (this.timeShow > 0f)
		{
			this.timeShow -= Time.deltaTime;
		}
		if (this.target == null || this.cam == null)
		{
			Debug.Log("target == null || cam == null");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this.isHunger && this.hungerGameController != null && !this.hungerGameController.isGo || this._weaponManager.myPlayer == null)
		{
			this.ResetTimeShow();
		}
		if ((ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints) && this._weaponManager.myPlayer != null && this._weaponManager.myPlayerMoveC != null && this.target.GetComponent<Player_move_c>() != null && this._weaponManager.myPlayerMoveC.myCommand == this.target.GetComponent<Player_move_c>().myCommand)
		{
			this.ResetTimeShow();
		}
		try
		{
			this.cam = ObjectLabel.currentCamera;
			this.camTransform = this.cam.transform;
			GUITexture component = base.transform.GetComponent<GUITexture>();
			if (component != null)
			{
				if (this.isMenu)
				{
					component.pixelInset = new Rect(-130f * this.koofScreen, -6f * this.koofScreen, 36f * this.koofScreen, 36f * this.koofScreen);
					base.transform.GetComponent<GUIText>().pixelOffset = new Vector2(-85f * this.koofScreen, 0f);
					base.transform.GetComponent<GUIText>().fontSize = Mathf.RoundToInt(20f * Defs.Coef);
					this.offset = new Vector3(0f, 2.25f, 0f);
					component.texture = this.expController.marks[this.expController.currentLevel];
					this.clanTexture.pixelInset = new Rect(-110f * this.koofScreen, -18f * this.koofScreen, 15f * this.koofScreen, 15f * this.koofScreen);
					this.clanName.pixelOffset = new Vector2(-85f * this.koofScreen, -2f);
					this.clanName.fontSize = Mathf.RoundToInt(16f * this.koofScreen);
					if (this.clanTexture.texture == null)
					{
						if (string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
						{
							this.clanTexture.texture = null;
						}
						else
						{
							byte[] numArray = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
							Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
							texture2D.LoadImage(numArray);
							texture2D.filterMode = FilterMode.Point;
							texture2D.Apply();
							this.clanTexture.texture = texture2D;
						}
						this.clanName.text = FriendsController.sharedController.clanName;
					}
				}
				else
				{
					Player_move_c playerMoveC = this.target.GetComponent<Player_move_c>();
					if (!this.isSetColor)
					{
						if (playerMoveC.myCommand == 1)
						{
							base.gameObject.GetComponent<GUIText>().color = Color.blue;
							this.isSetColor = true;
						}
						if (playerMoveC.myCommand == 2)
						{
							base.gameObject.GetComponent<GUIText>().color = Color.red;
							this.isSetColor = true;
						}
					}
					int num = playerMoveC.myTable.GetComponent<NetworkStartTable>().myRanks;
					if (num < 0 || num >= (int)this.expController.marks.Length)
					{
						string str = string.Format("Rank is equal to {0}, but the range [0, {1}) expected.", num, (int)this.expController.marks.Length);
						Debug.LogError(str);
					}
					else
					{
						component.texture = this.expController.marks[num];
					}
					this.clanTexture.texture = playerMoveC.myTable.GetComponent<NetworkStartTable>().myClanTexture;
					this.clanName.text = playerMoveC.myTable.GetComponent<NetworkStartTable>().myClanName;
				}
				if (this.timeShow > 0f)
				{
					this.posLabel = this.cam.WorldToViewportPoint(this.target.position + this.offset);
				}
				if (this.timeShow <= 0f || this.posLabel.z < 0f)
				{
					this.thisTransform.position = new Vector3(-1000f, -1000f, -1000f);
				}
				else
				{
					this.thisTransform.position = this.posLabel;
				}
				if (!this.isMenu && this.target.transform.parent.transform.position.y < -1000f)
				{
					this.thisTransform.position = new Vector3(-1000f, -1000f, -1000f);
				}
			}
			else
			{
				Debug.LogError("guiTexture == null");
			}
		}
		catch (Exception exception)
		{
			Debug.Log(string.Concat("Exception in ObjectLabel: ", exception));
		}
	}
}