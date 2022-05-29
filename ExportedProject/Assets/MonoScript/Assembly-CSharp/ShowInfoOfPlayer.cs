using Photon;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ShowInfoOfPlayer : Photon.MonoBehaviour
{
	private GameObject textGo;

	private TextMesh tm;

	public float CharacterSize;

	public Font font;

	public bool DisableOnOwnObjects;

	public ShowInfoOfPlayer()
	{
	}

	private void Start()
	{
		if (this.font == null)
		{
			this.font = (Font)Resources.FindObjectsOfTypeAll(typeof(Font))[0];
			Debug.LogWarning(string.Concat("No font defined. Found font: ", this.font));
		}
		if (this.tm == null)
		{
			this.textGo = new GameObject("3d text");
			this.textGo.transform.parent = base.gameObject.transform;
			this.textGo.transform.localPosition = Vector3.zero;
			this.textGo.AddComponent<MeshRenderer>().material = this.font.material;
			this.tm = this.textGo.AddComponent<TextMesh>();
			this.tm.font = this.font;
			this.tm.anchor = TextAnchor.MiddleCenter;
			if (this.CharacterSize > 0f)
			{
				this.tm.characterSize = this.CharacterSize;
			}
		}
	}

	private void Update()
	{
		bool flag = (!this.DisableOnOwnObjects ? true : base.photonView.isMine);
		if (this.textGo != null)
		{
			this.textGo.SetActive(flag);
		}
		if (!flag)
		{
			return;
		}
		PhotonPlayer photonPlayer = base.photonView.owner;
		if (photonPlayer != null)
		{
			this.tm.text = (!string.IsNullOrEmpty(photonPlayer.name) ? photonPlayer.name : string.Concat("player", photonPlayer.ID));
		}
		else if (!base.photonView.isSceneView)
		{
			this.tm.text = "n/a";
		}
		else
		{
			this.tm.text = "scn";
		}
	}
}