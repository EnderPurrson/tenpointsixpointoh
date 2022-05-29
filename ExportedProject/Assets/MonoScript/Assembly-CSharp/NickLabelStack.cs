using System;
using UnityEngine;

public class NickLabelStack : MonoBehaviour
{
	public static NickLabelStack sharedStack;

	public int lengthStack = 30;

	public NickLabelController[] lables;

	private int currentIndexLabel;

	public NickLabelStack()
	{
	}

	private void Awake()
	{
		NickLabelStack.sharedStack = this;
	}

	public NickLabelController GetCurrentLabel()
	{
		return this.lables[this.currentIndexLabel];
	}

	public NickLabelController GetNextCurrentLabel()
	{
		base.transform.localPosition = Vector3.zero;
		bool flag = true;
		do
		{
			this.currentIndexLabel++;
			if (this.currentIndexLabel < (int)this.lables.Length)
			{
				continue;
			}
			if (!flag)
			{
				return null;
			}
			this.currentIndexLabel = 0;
			flag = false;
		}
		while (this.lables[this.currentIndexLabel].target != null);
		this.lables[this.currentIndexLabel].currentType = NickLabelController.TypeNickLabel.None;
		return this.lables[this.currentIndexLabel];
	}

	private void OnDestroy()
	{
		NickLabelStack.sharedStack = null;
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.transform.localPosition = Vector3.zero;
		Transform child = base.transform.GetChild(0).transform;
		base.transform.position = Vector3.zero;
		this.lables = new NickLabelController[this.lengthStack];
		this.lables[0] = child.GetChild(0).GetComponent<NickLabelController>();
		while (child.childCount < this.lengthStack)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(child.GetChild(0).gameObject);
			Transform vector3 = gameObject.transform;
			vector3.parent = child;
			vector3.localPosition = Vector3.zero;
			vector3.localScale = new Vector3(1f, 1f, 1f);
			vector3.rotation = Quaternion.identity;
			this.lables[child.childCount - 1] = gameObject.GetComponent<NickLabelController>();
		}
	}
}