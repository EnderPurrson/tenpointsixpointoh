using ArabicSupport;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/Localize")]
	public class Localize : MonoBehaviour
	{
		public string mTerm;

		public string mTermSecondary;

		public string FinalTerm;

		public string FinalSecondaryTerm;

		public Localize.TermModification PrimaryTermModifier;

		public Localize.TermModification SecondaryTermModifier;

		public UnityEngine.Object mTarget;

		public Localize.DelegateSetFinalTerms EventSetFinalTerms;

		public Localize.DelegateDoLocalize EventDoLocalize;

		public bool CanUseSecondaryTerm;

		public bool AllowMainTermToBeRTL;

		public bool AllowSecondTermToBeRTL;

		public bool IgnoreRTL;

		public UnityEngine.Object[] TranslatedObjects;

		public EventCallback LocalizeCallBack = new EventCallback();

		public static string MainTranslation;

		public static string SecondaryTranslation;

		private UILabel mTarget_UILabel;

		private UISprite mTarget_UISprite;

		private UITexture mTarget_UITexture;

		private Text mTarget_uGUI_Text;

		private Image mTarget_uGUI_Image;

		private RawImage mTarget_uGUI_RawImage;

		private GUIText mTarget_GUIText;

		private TextMesh mTarget_TextMesh;

		private AudioSource mTarget_AudioSource;

		private GUITexture mTarget_GUITexture;

		private GameObject mTarget_Child;

		private Action EventFindTarget;

		public string SecondaryTerm
		{
			get
			{
				return this.mTermSecondary;
			}
			set
			{
				this.mTermSecondary = value;
			}
		}

		public string Term
		{
			get
			{
				return this.mTerm;
			}
			set
			{
				this.mTerm = value;
			}
		}

		public Localize()
		{
		}

		private void Awake()
		{
			this.RegisterTargets();
			this.EventFindTarget();
		}

		private void DeserializeTranslation(string translation, out string value, out string secondary)
		{
			if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
			{
				int num = translation.IndexOf(']');
				if (num > 0)
				{
					secondary = translation.Substring(1, num - 1);
					value = translation.Substring(num + 1);
					return;
				}
			}
			value = translation;
			secondary = string.Empty;
		}

		private void DoLocalize_AudioSource(string MainTranslation, string SecondaryTranslation)
		{
			bool mTargetAudioSource = this.mTarget_AudioSource.isPlaying;
			AudioClip audioClip = this.mTarget_AudioSource.clip;
			AudioClip audioClip1 = this.FindTranslatedObject<AudioClip>(MainTranslation);
			if (audioClip == audioClip1)
			{
				return;
			}
			this.mTarget_AudioSource.clip = audioClip1;
			if (mTargetAudioSource && this.mTarget_AudioSource.clip)
			{
				this.mTarget_AudioSource.Play();
			}
		}

		private void DoLocalize_Child(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_Child && this.mTarget_Child.name == MainTranslation)
			{
				return;
			}
			GameObject mTargetChild = this.mTarget_Child;
			GameObject gameObject = this.FindTranslatedObject<GameObject>(MainTranslation);
			if (gameObject)
			{
				this.mTarget_Child = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				Transform transforms = this.mTarget_Child.transform;
				Transform transforms1 = (!mTargetChild ? gameObject.transform : mTargetChild.transform);
				transforms.parent = base.transform;
				transforms.localScale = transforms1.localScale;
				transforms.localRotation = transforms1.localRotation;
				transforms.localPosition = transforms1.localPosition;
			}
			if (mTargetChild)
			{
				UnityEngine.Object.Destroy(mTargetChild);
			}
		}

		private void DoLocalize_GUIText(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_GUIText.text == MainTranslation)
			{
				return;
			}
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_GUIText.font = secondaryTranslatedObj;
			}
			this.mTarget_GUIText.text = MainTranslation;
		}

		private void DoLocalize_GUITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture mTargetGUITexture = this.mTarget_GUITexture.texture;
			if (mTargetGUITexture && mTargetGUITexture.name == MainTranslation)
			{
				return;
			}
			this.mTarget_GUITexture.texture = this.FindTranslatedObject<Texture>(MainTranslation);
		}

		private void DoLocalize_TextMesh(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_TextMesh.text == MainTranslation)
			{
				return;
			}
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_TextMesh.font = secondaryTranslatedObj;
				base.GetComponent<Renderer>().sharedMaterial = secondaryTranslatedObj.material;
			}
			this.mTarget_TextMesh.text = MainTranslation;
		}

		public void DoLocalize_uGUI_Image(string MainTranslation, string SecondaryTranslation)
		{
			Sprite mTargetUGUIImage = this.mTarget_uGUI_Image.sprite;
			if (mTargetUGUIImage && mTargetUGUIImage.name == MainTranslation)
			{
				return;
			}
			this.mTarget_uGUI_Image.sprite = this.FindTranslatedObject<Sprite>(MainTranslation);
		}

		public void DoLocalize_uGUI_RawImage(string MainTranslation, string SecondaryTranslation)
		{
			Texture mTargetUGUIRawImage = this.mTarget_uGUI_RawImage.texture;
			if (mTargetUGUIRawImage && mTargetUGUIRawImage.name == MainTranslation)
			{
				return;
			}
			this.mTarget_uGUI_RawImage.texture = this.FindTranslatedObject<Texture>(MainTranslation);
		}

		public void DoLocalize_uGUI_Text(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_uGUI_Text.text == MainTranslation)
			{
				return;
			}
			this.mTarget_uGUI_Text.text = MainTranslation;
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_uGUI_Text.font = secondaryTranslatedObj;
			}
		}

		public void DoLocalize_UILabel(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_UILabel.ambigiousFont = secondaryTranslatedObj;
				return;
			}
			UIFont uIFont = this.GetSecondaryTranslatedObj<UIFont>(ref MainTranslation, ref SecondaryTranslation);
			if (uIFont != null)
			{
				this.mTarget_UILabel.ambigiousFont = uIFont;
				return;
			}
			UIInput mainTranslation = NGUITools.FindInParents<UIInput>(this.mTarget_UILabel.gameObject);
			if (!(mainTranslation != null) || !(mainTranslation.label == this.mTarget_UILabel))
			{
				if (this.mTarget_UILabel.text == MainTranslation)
				{
					return;
				}
				this.mTarget_UILabel.text = MainTranslation;
			}
			else
			{
				if (mainTranslation.defaultText == MainTranslation)
				{
					return;
				}
				mainTranslation.defaultText = MainTranslation;
			}
		}

		public void DoLocalize_UISprite(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_UISprite.spriteName == MainTranslation)
			{
				return;
			}
			UIAtlas secondaryTranslatedObj = this.GetSecondaryTranslatedObj<UIAtlas>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_UISprite.atlas = secondaryTranslatedObj;
			}
			this.mTarget_UISprite.spriteName = MainTranslation;
			this.mTarget_UISprite.MakePixelPerfect();
		}

		public void DoLocalize_UITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture mTargetUITexture = this.mTarget_UITexture.mainTexture;
			if (mTargetUITexture && mTargetUITexture.name == MainTranslation)
			{
				return;
			}
			this.mTarget_UITexture.mainTexture = this.FindTranslatedObject<Texture>(MainTranslation);
		}

		public void FindAndCacheTarget<T>(ref T targetCache, Localize.DelegateSetFinalTerms setFinalTerms, Localize.DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL)
		where T : Component
		{
			if (this.mTarget == null)
			{
				T component = base.GetComponent<T>();
				T t = component;
				targetCache = component;
				this.mTarget = t;
			}
			else
			{
				targetCache = (T)(this.mTarget as T);
			}
			if (targetCache != null)
			{
				this.EventSetFinalTerms = setFinalTerms;
				this.EventDoLocalize = doLocalize;
				this.CanUseSecondaryTerm = UseSecondaryTerm;
				this.AllowMainTermToBeRTL = MainRTL;
				this.AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		private void FindAndCacheTarget(ref GameObject targetCache, Localize.DelegateSetFinalTerms setFinalTerms, Localize.DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL)
		{
			object child;
			if (this.mTarget != targetCache && targetCache)
			{
				UnityEngine.Object.Destroy(targetCache);
			}
			if (this.mTarget == null)
			{
				Transform transforms = base.transform;
				if (transforms.childCount >= 1)
				{
					child = transforms.GetChild(0).gameObject;
				}
				else
				{
					child = null;
				}
				GameObject gameObject = (GameObject)child;
				targetCache = (GameObject)child;
				this.mTarget = gameObject;
			}
			else
			{
				targetCache = this.mTarget as GameObject;
			}
			if (targetCache != null)
			{
				this.EventSetFinalTerms = setFinalTerms;
				this.EventDoLocalize = doLocalize;
				this.CanUseSecondaryTerm = UseSecondaryTerm;
				this.AllowMainTermToBeRTL = MainRTL;
				this.AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		public static T FindInParents<T>(Transform tr)
		where T : Component
		{
			if (!tr)
			{
				return (T)null;
			}
			T component = tr.GetComponent<T>();
			while (!component && tr)
			{
				component = tr.GetComponent<T>();
				tr = tr.parent;
			}
			return component;
		}

		public bool FindTarget()
		{
			if (this.EventFindTarget == null)
			{
				this.RegisterTargets();
			}
			this.EventFindTarget();
			return this.HasTargetCache();
		}

		private void FindTarget_AudioSource()
		{
			this.FindAndCacheTarget<AudioSource>(ref this.mTarget_AudioSource, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_AudioSource), new Localize.DelegateDoLocalize(this.DoLocalize_AudioSource), false, false, false);
		}

		private void FindTarget_Child()
		{
			this.FindAndCacheTarget(ref this.mTarget_Child, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_Child), new Localize.DelegateDoLocalize(this.DoLocalize_Child), false, false, false);
		}

		private void FindTarget_GUIText()
		{
			this.FindAndCacheTarget<GUIText>(ref this.mTarget_GUIText, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_GUIText), new Localize.DelegateDoLocalize(this.DoLocalize_GUIText), true, true, false);
		}

		private void FindTarget_GUITexture()
		{
			this.FindAndCacheTarget<GUITexture>(ref this.mTarget_GUITexture, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_GUITexture), new Localize.DelegateDoLocalize(this.DoLocalize_GUITexture), false, false, false);
		}

		private void FindTarget_TextMesh()
		{
			this.FindAndCacheTarget<TextMesh>(ref this.mTarget_TextMesh, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_TextMesh), new Localize.DelegateDoLocalize(this.DoLocalize_TextMesh), true, true, false);
		}

		private void FindTarget_uGUI_Image()
		{
			this.FindAndCacheTarget<Image>(ref this.mTarget_uGUI_Image, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_Image), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_Image), false, false, false);
		}

		private void FindTarget_uGUI_RawImage()
		{
			this.FindAndCacheTarget<RawImage>(ref this.mTarget_uGUI_RawImage, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_RawImage), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_RawImage), false, false, false);
		}

		private void FindTarget_uGUI_Text()
		{
			this.FindAndCacheTarget<Text>(ref this.mTarget_uGUI_Text, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_Text), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_Text), true, true, false);
		}

		private void FindTarget_UILabel()
		{
			this.FindAndCacheTarget<UILabel>(ref this.mTarget_UILabel, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UIlabel), new Localize.DelegateDoLocalize(this.DoLocalize_UILabel), true, true, false);
		}

		private void FindTarget_UISprite()
		{
			this.FindAndCacheTarget<UISprite>(ref this.mTarget_UISprite, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UISprite), new Localize.DelegateDoLocalize(this.DoLocalize_UISprite), true, false, false);
		}

		private void FindTarget_UITexture()
		{
			this.FindAndCacheTarget<UITexture>(ref this.mTarget_UITexture, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UITexture), new Localize.DelegateDoLocalize(this.DoLocalize_UITexture), false, false, false);
		}

		public T FindTranslatedObject<T>(string value)
		where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(value))
			{
				return (T)null;
			}
			if (this.TranslatedObjects != null)
			{
				int num = 0;
				int length = (int)this.TranslatedObjects.Length;
				while (num < length)
				{
					if ((T)(this.TranslatedObjects[num] as T) != null && value == this.TranslatedObjects[num].name)
					{
						return (T)(this.TranslatedObjects[num] as T);
					}
					num++;
				}
			}
			T asset = (T)(LocalizationManager.FindAsset(value) as T);
			if (asset)
			{
				return asset;
			}
			asset = ResourceManager.pInstance.GetAsset<T>(value);
			return asset;
		}

		public bool GetFinalTerms(out string PrimaryTerm, out string SecondaryTerm)
		{
			if (!this.mTarget && !this.HasTargetCache())
			{
				this.FindTarget();
			}
			if (!string.IsNullOrEmpty(this.mTerm))
			{
				return this.SetFinalTerms(this.mTerm, this.mTermSecondary, out PrimaryTerm, out SecondaryTerm);
			}
			if (!string.IsNullOrEmpty(this.FinalTerm))
			{
				return this.SetFinalTerms(this.FinalTerm, this.FinalSecondaryTerm, out PrimaryTerm, out SecondaryTerm);
			}
			if (this.EventSetFinalTerms == null)
			{
				return this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm);
			}
			return this.EventSetFinalTerms(this.mTerm, this.mTermSecondary, out PrimaryTerm, out SecondaryTerm);
		}

		private T GetSecondaryTranslatedObj<T>(ref string MainTranslation, ref string SecondaryTranslation)
		where T : UnityEngine.Object
		{
			string secondaryTranslation;
			this.DeserializeTranslation(MainTranslation, out MainTranslation, out secondaryTranslation);
			if (string.IsNullOrEmpty(secondaryTranslation))
			{
				secondaryTranslation = SecondaryTranslation;
			}
			if (string.IsNullOrEmpty(secondaryTranslation))
			{
				return (T)null;
			}
			T translatedObject = this.GetTranslatedObject<T>(secondaryTranslation);
			if (translatedObject == null)
			{
				int num = secondaryTranslation.LastIndexOfAny("/\\".ToCharArray());
				if (num >= 0)
				{
					secondaryTranslation = secondaryTranslation.Substring(num + 1);
					translatedObject = this.GetTranslatedObject<T>(secondaryTranslation);
				}
			}
			return translatedObject;
		}

		private T GetTranslatedObject<T>(string Translation)
		where T : UnityEngine.Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		private bool HasTargetCache()
		{
			return this.EventDoLocalize != null;
		}

		private bool HasTranslatedObject(UnityEngine.Object Obj)
		{
			if (Array.IndexOf<UnityEngine.Object>(this.TranslatedObjects, Obj) >= 0)
			{
				return true;
			}
			return ResourceManager.pInstance.HasAsset(Obj);
		}

		private void OnEnable()
		{
			this.OnLocalize();
		}

		public void OnLocalize()
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			if (!this.HasTargetCache())
			{
				this.FindTarget();
			}
			if (!this.HasTargetCache())
			{
				return;
			}
			this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
			if (string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				return;
			}
			Localize.MainTranslation = LocalizationManager.GetTermTranslation(this.FinalTerm);
			Localize.SecondaryTranslation = LocalizationManager.GetTermTranslation(this.FinalSecondaryTerm);
			if (string.IsNullOrEmpty(Localize.MainTranslation) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
			{
				return;
			}
			this.LocalizeCallBack.Execute(this);
			if (LocalizationManager.IsRight2Left && !this.IgnoreRTL)
			{
				if (this.AllowMainTermToBeRTL && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					Localize.MainTranslation = ArabicFixer.Fix(Localize.MainTranslation);
				}
				if (this.AllowSecondTermToBeRTL && !string.IsNullOrEmpty(Localize.SecondaryTranslation))
				{
					Localize.SecondaryTranslation = ArabicFixer.Fix(Localize.SecondaryTranslation);
				}
			}
			Localize.TermModification primaryTermModifier = this.PrimaryTermModifier;
			if (primaryTermModifier == Localize.TermModification.ToUpper)
			{
				Localize.MainTranslation = Localize.MainTranslation.ToUpper();
			}
			else if (primaryTermModifier == Localize.TermModification.ToLower)
			{
				Localize.MainTranslation = Localize.MainTranslation.ToLower();
			}
			primaryTermModifier = this.SecondaryTermModifier;
			if (primaryTermModifier == Localize.TermModification.ToUpper)
			{
				Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
			}
			else if (primaryTermModifier == Localize.TermModification.ToLower)
			{
				Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
			}
			this.EventDoLocalize(Localize.MainTranslation, Localize.SecondaryTranslation);
		}

		public static void RegisterEvents_2DToolKit()
		{
		}

		public static void RegisterEvents_DFGUI()
		{
		}

		public void RegisterEvents_NGUI()
		{
			this.EventFindTarget += new Action(this.FindTarget_UILabel);
			this.EventFindTarget += new Action(this.FindTarget_UISprite);
			this.EventFindTarget += new Action(this.FindTarget_UITexture);
		}

		public static void RegisterEvents_TextMeshPro()
		{
		}

		public void RegisterEvents_UGUI()
		{
			this.EventFindTarget += new Action(this.FindTarget_uGUI_Text);
			this.EventFindTarget += new Action(this.FindTarget_uGUI_Image);
			this.EventFindTarget += new Action(this.FindTarget_uGUI_RawImage);
		}

		public void RegisterEvents_UnityStandard()
		{
			this.EventFindTarget += new Action(this.FindTarget_GUIText);
			this.EventFindTarget += new Action(this.FindTarget_TextMesh);
			this.EventFindTarget += new Action(this.FindTarget_AudioSource);
			this.EventFindTarget += new Action(this.FindTarget_GUITexture);
			this.EventFindTarget += new Action(this.FindTarget_Child);
		}

		private void RegisterTargets()
		{
			if (this.EventFindTarget != null)
			{
				return;
			}
			this.RegisterEvents_NGUI();
			Localize.RegisterEvents_DFGUI();
			this.RegisterEvents_UGUI();
			Localize.RegisterEvents_2DToolKit();
			Localize.RegisterEvents_TextMeshPro();
			this.RegisterEvents_UnityStandard();
		}

		private bool SetFinalTerms(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			PrimaryTerm = Main;
			SecondaryTerm = Secondary;
			return true;
		}

		public bool SetFinalTerms_AudioSource(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (!this.mTarget_AudioSource || !this.mTarget_AudioSource.clip)
			{
				this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm);
				return false;
			}
			return this.SetFinalTerms(this.mTarget_AudioSource.clip.name, string.Empty, out PrimaryTerm, out SecondaryTerm);
		}

		public bool SetFinalTerms_Child(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			return this.SetFinalTerms(this.mTarget_Child.name, string.Empty, out PrimaryTerm, out SecondaryTerm);
		}

		public bool SetFinalTerms_GUIText(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			string str = (this.mTarget_GUIText.font == null ? string.Empty : this.mTarget_GUIText.font.name);
			return this.SetFinalTerms(this.mTarget_GUIText.text, str, out PrimaryTerm, out SecondaryTerm);
		}

		public bool SetFinalTerms_GUITexture(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (!this.mTarget_GUITexture || !this.mTarget_GUITexture.texture)
			{
				this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm);
				return false;
			}
			return this.SetFinalTerms(this.mTarget_GUITexture.texture.name, string.Empty, out PrimaryTerm, out SecondaryTerm);
		}

		public bool SetFinalTerms_TextMesh(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			string str = (this.mTarget_TextMesh.font == null ? string.Empty : this.mTarget_TextMesh.font.name);
			return this.SetFinalTerms(this.mTarget_TextMesh.text, str, out PrimaryTerm, out SecondaryTerm);
		}

		public bool SetFinalTerms_uGUI_Image(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			return this.SetFinalTerms(this.mTarget_uGUI_Image.mainTexture.name, null, out primaryTerm, out secondaryTerm);
		}

		public bool SetFinalTerms_uGUI_RawImage(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			return this.SetFinalTerms(this.mTarget_uGUI_RawImage.texture.name, null, out primaryTerm, out secondaryTerm);
		}

		private bool SetFinalTerms_uGUI_Text(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string str = (this.mTarget_uGUI_Text.font == null ? string.Empty : this.mTarget_uGUI_Text.font.name);
			return this.SetFinalTerms(this.mTarget_uGUI_Text.text, str, out primaryTerm, out secondaryTerm);
		}

		private bool SetFinalTerms_UIlabel(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string str = (this.mTarget_UILabel.ambigiousFont == null ? string.Empty : this.mTarget_UILabel.ambigiousFont.name);
			return this.SetFinalTerms(this.mTarget_UILabel.text, str, out primaryTerm, out secondaryTerm);
		}

		public bool SetFinalTerms_UISprite(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string str = (this.mTarget_UISprite.atlas == null ? string.Empty : this.mTarget_UISprite.atlas.name);
			return this.SetFinalTerms(this.mTarget_UISprite.spriteName, str, out primaryTerm, out secondaryTerm);
		}

		public bool SetFinalTerms_UITexture(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			return this.SetFinalTerms(this.mTarget_UITexture.mainTexture.name, null, out primaryTerm, out secondaryTerm);
		}

		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		public void SetTerm(string primary, string secondary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.Term = primary;
			}
			if (!string.IsNullOrEmpty(secondary))
			{
				this.SecondaryTerm = secondary;
			}
			this.OnLocalize();
		}

		public event Action EventFindTarget
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.EventFindTarget += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.EventFindTarget -= value;
			}
		}

		public delegate void DelegateDoLocalize(string primaryTerm, string secondaryTerm);

		public delegate bool DelegateSetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		public enum TermModification
		{
			DontModify,
			ToUpper,
			ToLower
		}
	}
}