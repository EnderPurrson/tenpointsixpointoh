using System;
using UnityEngine;

public class Blinking : MonoBehaviour
{
	public float halfCycle = 1f;

	private UISprite mySprite;

	private float _time;

	public Blinking()
	{
	}

	private void Start()
	{
		this.mySprite = base.GetComponent<UISprite>();
	}

	private void Update()
	{
		this._time += Time.deltaTime;
		if (this.mySprite != null)
		{
			Color color = this.mySprite.color;
			float single = 2f * (this._time - Mathf.Floor(this._time / this.halfCycle) * this.halfCycle) / this.halfCycle;
			if (single > 1f)
			{
				single = 2f - single;
			}
			this.mySprite.color = new Color(color.r, color.g, color.b, single);
		}
	}
}