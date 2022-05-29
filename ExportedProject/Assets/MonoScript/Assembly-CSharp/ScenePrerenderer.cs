using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenePrerenderer : MonoBehaviour
{
	public Camera activeCamera;

	private RenderTexture _rt;

	public bool FinishPrerendering;

	public GameObject[] objsToRender;

	private GameObject _enemiesToRender;

	public ScenePrerenderer()
	{
	}

	private void Awake()
	{
		this._rt = new RenderTexture(32, 32, 24);
		this._rt.Create();
		this.activeCamera.targetTexture = this._rt;
		this.activeCamera.useOcclusionCulling = false;
	}

	private void Render_()
	{
		List<GameObject> component = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>().zombiePrefabs;
		GameObject[] gameObjectArray = new GameObject[component.Count];
		int num = 0;
		foreach (GameObject gameObject in component)
		{
			GameObject child = gameObject.transform.GetChild(0).gameObject;
			float single = base.transform.position.x;
			Vector3 vector3 = base.transform.position;
			Vector3 vector31 = base.transform.position;
			GameObject gameObject1 = (GameObject)UnityEngine.Object.Instantiate(child, new Vector3(single, vector3.y - 20f, vector31.z), gameObject.transform.GetChild(0).gameObject.transform.rotation);
			string str = "(Clone)";
			int num1 = gameObject1.name.IndexOf(str);
			gameObject1.name = (num1 >= 0 ? gameObject1.name.Remove(num1, str.Length) : gameObject1.name);
			gameObject1.transform.parent = base.transform.parent;
			SkinsController.GetSkinForObj(gameObject1);
			gameObjectArray[num] = gameObject1;
			num++;
		}
		this.activeCamera.Render();
		RenderTexture.active = this._rt;
		this.activeCamera.targetTexture = null;
		RenderTexture.active = null;
		GameObject[] gameObjectArray1 = this.objsToRender;
		for (int i = 0; i < (int)gameObjectArray1.Length; i++)
		{
			UnityEngine.Object.Destroy(gameObjectArray1[i]);
		}
		UnityEngine.Object.Destroy(base.transform.parent.parent.gameObject);
		UnityEngine.Object.Destroy(this.activeCamera);
	}

	private void Start()
	{
		this.Render_();
	}
}