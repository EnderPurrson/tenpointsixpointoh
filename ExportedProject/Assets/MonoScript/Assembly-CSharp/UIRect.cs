using System;
using UnityEngine;

public abstract class UIRect : MonoBehaviour
{
	public UIRect.AnchorPoint leftAnchor = new UIRect.AnchorPoint();

	public UIRect.AnchorPoint rightAnchor = new UIRect.AnchorPoint(1f);

	public UIRect.AnchorPoint bottomAnchor = new UIRect.AnchorPoint();

	public UIRect.AnchorPoint topAnchor = new UIRect.AnchorPoint(1f);

	public UIRect.AnchorUpdate updateAnchors = UIRect.AnchorUpdate.OnUpdate;

	[NonSerialized]
	protected GameObject mGo;

	[NonSerialized]
	protected Transform mTrans;

	[NonSerialized]
	protected BetterList<UIRect> mChildren = new BetterList<UIRect>();

	[NonSerialized]
	protected bool mChanged = true;

	[NonSerialized]
	protected bool mParentFound;

	[NonSerialized]
	private bool mUpdateAnchors = true;

	[NonSerialized]
	private int mUpdateFrame = -1;

	[NonSerialized]
	private bool mAnchorsCached;

	[NonSerialized]
	private UIRoot mRoot;

	[NonSerialized]
	private UIRect mParent;

	[NonSerialized]
	private bool mRootSet;

	[NonSerialized]
	protected Camera mCam;

	protected bool mStarted;

	[NonSerialized]
	public float finalAlpha = 1f;

	protected static Vector3[] mSides;

	public abstract float alpha
	{
		get;
		set;
	}

	public Camera anchorCamera
	{
		get
		{
			if (!this.mAnchorsCached)
			{
				this.ResetAnchors();
			}
			return this.mCam;
		}
	}

	public GameObject cachedGameObject
	{
		get
		{
			if (this.mGo == null)
			{
				this.mGo = base.gameObject;
			}
			return this.mGo;
		}
	}

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	protected float cameraRayDistance
	{
		get
		{
			float single;
			if (this.anchorCamera == null)
			{
				return 0f;
			}
			if (!this.mCam.orthographic)
			{
				Transform transforms = this.cachedTransform;
				Transform transforms1 = this.mCam.transform;
				Plane plane = new Plane(transforms.rotation * Vector3.back, transforms.position);
				Ray ray = new Ray(transforms1.position, transforms1.rotation * Vector3.forward);
				if (plane.Raycast(ray, out single))
				{
					return single;
				}
			}
			return Mathf.Lerp(this.mCam.nearClipPlane, this.mCam.farClipPlane, 0.5f);
		}
	}

	public virtual bool canBeAnchored
	{
		get
		{
			return true;
		}
	}

	public bool isAnchored
	{
		get
		{
			return (this.leftAnchor.target || this.rightAnchor.target || this.topAnchor.target || this.bottomAnchor.target ? this.canBeAnchored : false);
		}
	}

	public virtual bool isAnchoredHorizontally
	{
		get
		{
			return (this.leftAnchor.target ? true : this.rightAnchor.target);
		}
	}

	public virtual bool isAnchoredVertically
	{
		get
		{
			return (this.bottomAnchor.target ? true : this.topAnchor.target);
		}
	}

	public bool isFullyAnchored
	{
		get
		{
			return (!this.leftAnchor.target || !this.rightAnchor.target || !this.topAnchor.target ? false : this.bottomAnchor.target);
		}
	}

	public abstract Vector3[] localCorners
	{
		get;
	}

	public UIRect parent
	{
		get
		{
			if (!this.mParentFound)
			{
				this.mParentFound = true;
				this.mParent = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
			}
			return this.mParent;
		}
	}

	public UIRoot root
	{
		get
		{
			if (this.parent != null)
			{
				return this.mParent.root;
			}
			if (!this.mRootSet)
			{
				this.mRootSet = true;
				this.mRoot = NGUITools.FindInParents<UIRoot>(this.cachedTransform);
			}
			return this.mRoot;
		}
	}

	public abstract Vector3[] worldCorners
	{
		get;
	}

	static UIRect()
	{
		UIRect.mSides = new Vector3[4];
	}

	protected UIRect()
	{
	}

	protected virtual void Awake()
	{
		this.mStarted = false;
		this.mGo = base.gameObject;
		this.mTrans = base.transform;
	}

	public abstract float CalculateFinalAlpha(int frameID);

	private void FindCameraFor(UIRect.AnchorPoint ap)
	{
		if (ap.target == null || ap.rect != null)
		{
			ap.targetCam = null;
		}
		else
		{
			ap.targetCam = NGUITools.FindCameraForLayer(ap.target.gameObject.layer);
		}
	}

	protected Vector3 GetLocalPos(UIRect.AnchorPoint ac, Transform trans)
	{
		if (this.anchorCamera == null || ac.targetCam == null)
		{
			return this.cachedTransform.localPosition;
		}
		Rect rect = ac.targetCam.rect;
		Vector3 viewportPoint = ac.targetCam.WorldToViewportPoint(ac.target.position);
		Vector3 vector3 = new Vector3(viewportPoint.x * rect.width + rect.x, viewportPoint.y * rect.height + rect.y, viewportPoint.z);
		vector3 = this.mCam.ViewportToWorldPoint(vector3);
		if (trans != null)
		{
			vector3 = trans.InverseTransformPoint(vector3);
		}
		vector3.x = Mathf.Floor(vector3.x + 0.5f);
		vector3.y = Mathf.Floor(vector3.y + 0.5f);
		return vector3;
	}

	public virtual Vector3[] GetSides(Transform relativeTo)
	{
		if (this.anchorCamera != null)
		{
			return this.mCam.GetSides(this.cameraRayDistance, relativeTo);
		}
		Vector3 vector3 = this.cachedTransform.position;
		for (int i = 0; i < 4; i++)
		{
			UIRect.mSides[i] = vector3;
		}
		if (relativeTo != null)
		{
			for (int j = 0; j < 4; j++)
			{
				UIRect.mSides[j] = relativeTo.InverseTransformPoint(UIRect.mSides[j]);
			}
		}
		return UIRect.mSides;
	}

	public virtual void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		if (includeChildren)
		{
			for (int i = 0; i < this.mChildren.size; i++)
			{
				this.mChildren.buffer[i].Invalidate(true);
			}
		}
	}

	protected abstract void OnAnchor();

	protected virtual void OnDisable()
	{
		if (this.mParent)
		{
			this.mParent.mChildren.Remove(this);
		}
		this.mParent = null;
		this.mRoot = null;
		this.mRootSet = false;
		this.mParentFound = false;
	}

	protected virtual void OnEnable()
	{
		this.mUpdateFrame = -1;
		if (this.updateAnchors == UIRect.AnchorUpdate.OnEnable)
		{
			this.mAnchorsCached = false;
			this.mUpdateAnchors = true;
		}
		if (this.mStarted)
		{
			this.OnInit();
		}
		this.mUpdateFrame = -1;
	}

	protected virtual void OnInit()
	{
		this.mChanged = true;
		this.mRootSet = false;
		this.mParentFound = false;
		if (this.parent != null)
		{
			this.mParent.mChildren.Add(this);
		}
	}

	protected abstract void OnStart();

	protected virtual void OnUpdate()
	{
	}

	public virtual void ParentHasChanged()
	{
		this.mParentFound = false;
		UIRect uIRect = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
		if (this.mParent != uIRect)
		{
			if (this.mParent)
			{
				this.mParent.mChildren.Remove(this);
			}
			this.mParent = uIRect;
			if (this.mParent)
			{
				this.mParent.mChildren.Add(this);
			}
			this.mRootSet = false;
		}
	}

	public void ResetAnchors()
	{
		UIRect component;
		UIRect uIRect;
		UIRect component1;
		UIRect uIRect1;
		this.mAnchorsCached = true;
		UIRect.AnchorPoint anchorPoint = this.leftAnchor;
		if (!this.leftAnchor.target)
		{
			component = null;
		}
		else
		{
			component = this.leftAnchor.target.GetComponent<UIRect>();
		}
		anchorPoint.rect = component;
		UIRect.AnchorPoint anchorPoint1 = this.bottomAnchor;
		if (!this.bottomAnchor.target)
		{
			uIRect = null;
		}
		else
		{
			uIRect = this.bottomAnchor.target.GetComponent<UIRect>();
		}
		anchorPoint1.rect = uIRect;
		UIRect.AnchorPoint anchorPoint2 = this.rightAnchor;
		if (!this.rightAnchor.target)
		{
			component1 = null;
		}
		else
		{
			component1 = this.rightAnchor.target.GetComponent<UIRect>();
		}
		anchorPoint2.rect = component1;
		UIRect.AnchorPoint anchorPoint3 = this.topAnchor;
		if (!this.topAnchor.target)
		{
			uIRect1 = null;
		}
		else
		{
			uIRect1 = this.topAnchor.target.GetComponent<UIRect>();
		}
		anchorPoint3.rect = uIRect1;
		this.mCam = NGUITools.FindCameraForLayer(this.cachedGameObject.layer);
		this.FindCameraFor(this.leftAnchor);
		this.FindCameraFor(this.bottomAnchor);
		this.FindCameraFor(this.rightAnchor);
		this.FindCameraFor(this.topAnchor);
		this.mUpdateAnchors = true;
	}

	public void ResetAndUpdateAnchors()
	{
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	public void SetAnchor(Transform t)
	{
		this.leftAnchor.target = t;
		this.rightAnchor.target = t;
		this.topAnchor.target = t;
		this.bottomAnchor.target = t;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	public void SetAnchor(GameObject go)
	{
		Transform transforms;
		if (go == null)
		{
			transforms = null;
		}
		else
		{
			transforms = go.transform;
		}
		Transform transforms1 = transforms;
		this.leftAnchor.target = transforms1;
		this.rightAnchor.target = transforms1;
		this.topAnchor.target = transforms1;
		this.bottomAnchor.target = transforms1;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	public void SetAnchor(GameObject go, int left, int bottom, int right, int top)
	{
		Transform transforms;
		if (go == null)
		{
			transforms = null;
		}
		else
		{
			transforms = go.transform;
		}
		Transform transforms1 = transforms;
		this.leftAnchor.target = transforms1;
		this.rightAnchor.target = transforms1;
		this.topAnchor.target = transforms1;
		this.bottomAnchor.target = transforms1;
		this.leftAnchor.relative = 0f;
		this.rightAnchor.relative = 1f;
		this.bottomAnchor.relative = 0f;
		this.topAnchor.relative = 1f;
		this.leftAnchor.absolute = left;
		this.rightAnchor.absolute = right;
		this.bottomAnchor.absolute = bottom;
		this.topAnchor.absolute = top;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	public abstract void SetRect(float x, float y, float width, float height);

	protected void Start()
	{
		this.mStarted = true;
		this.OnInit();
		this.OnStart();
	}

	public void Update()
	{
		if (!this.mAnchorsCached)
		{
			this.ResetAnchors();
		}
		int num = Time.frameCount;
		if (this.mUpdateFrame != num)
		{
			if (this.updateAnchors == UIRect.AnchorUpdate.OnUpdate || this.mUpdateAnchors)
			{
				this.UpdateAnchorsInternal(num);
			}
			this.OnUpdate();
		}
	}

	public void UpdateAnchors()
	{
		if (this.isAnchored)
		{
			this.mUpdateFrame = -1;
			this.mUpdateAnchors = true;
			this.UpdateAnchorsInternal(Time.frameCount);
		}
	}

	protected void UpdateAnchorsInternal(int frame)
	{
		this.mUpdateFrame = frame;
		this.mUpdateAnchors = false;
		bool flag = false;
		if (this.leftAnchor.target)
		{
			flag = true;
			if (this.leftAnchor.rect != null && this.leftAnchor.rect.mUpdateFrame != frame)
			{
				this.leftAnchor.rect.Update();
			}
		}
		if (this.bottomAnchor.target)
		{
			flag = true;
			if (this.bottomAnchor.rect != null && this.bottomAnchor.rect.mUpdateFrame != frame)
			{
				this.bottomAnchor.rect.Update();
			}
		}
		if (this.rightAnchor.target)
		{
			flag = true;
			if (this.rightAnchor.rect != null && this.rightAnchor.rect.mUpdateFrame != frame)
			{
				this.rightAnchor.rect.Update();
			}
		}
		if (this.topAnchor.target)
		{
			flag = true;
			if (this.topAnchor.rect != null && this.topAnchor.rect.mUpdateFrame != frame)
			{
				this.topAnchor.rect.Update();
			}
		}
		if (flag)
		{
			this.OnAnchor();
		}
	}

	[Serializable]
	public class AnchorPoint
	{
		public Transform target;

		public float relative;

		public int absolute;

		[NonSerialized]
		public UIRect rect;

		[NonSerialized]
		public Camera targetCam;

		public AnchorPoint()
		{
		}

		public AnchorPoint(float relative)
		{
			this.relative = relative;
		}

		public Vector3[] GetSides(Transform relativeTo)
		{
			if (this.target != null)
			{
				if (this.rect != null)
				{
					return this.rect.GetSides(relativeTo);
				}
				if (this.target.GetComponent<Camera>() != null)
				{
					return this.target.GetComponent<Camera>().GetSides(relativeTo);
				}
			}
			return null;
		}

		public void Set(float relative, float absolute)
		{
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		public void Set(Transform target, float relative, float absolute)
		{
			this.target = target;
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		public void SetHorizontal(Transform parent, float localPos)
		{
			if (!this.rect)
			{
				Vector3 vector3 = this.target.position;
				if (parent != null)
				{
					vector3 = parent.InverseTransformPoint(vector3);
				}
				this.absolute = Mathf.FloorToInt(localPos - vector3.x + 0.5f);
			}
			else
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float single = Mathf.Lerp(sides[0].x, sides[2].x, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - single + 0.5f);
			}
		}

		public void SetToNearest(float abs0, float abs1, float abs2)
		{
			this.SetToNearest(0f, 0.5f, 1f, abs0, abs1, abs2);
		}

		public void SetToNearest(float rel0, float rel1, float rel2, float abs0, float abs1, float abs2)
		{
			float single = Mathf.Abs(abs0);
			float single1 = Mathf.Abs(abs1);
			float single2 = Mathf.Abs(abs2);
			if (single < single1 && single < single2)
			{
				this.Set(rel0, abs0);
			}
			else if (single1 >= single || single1 >= single2)
			{
				this.Set(rel2, abs2);
			}
			else
			{
				this.Set(rel1, abs1);
			}
		}

		public void SetVertical(Transform parent, float localPos)
		{
			if (!this.rect)
			{
				Vector3 vector3 = this.target.position;
				if (parent != null)
				{
					vector3 = parent.InverseTransformPoint(vector3);
				}
				this.absolute = Mathf.FloorToInt(localPos - vector3.y + 0.5f);
			}
			else
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float single = Mathf.Lerp(sides[3].y, sides[1].y, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - single + 0.5f);
			}
		}
	}

	public enum AnchorUpdate
	{
		OnEnable,
		OnUpdate,
		OnStart
	}
}