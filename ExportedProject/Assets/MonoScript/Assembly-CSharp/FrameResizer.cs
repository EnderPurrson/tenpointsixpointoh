using System;
using UnityEngine;

public class FrameResizer : MonoBehaviour
{
	public GameObject[] objects;

	public UISprite frame;

	public UITable table;

	public Vector2[] frameSize;

	private int activeObjectsCounter;

	public FrameResizer()
	{
	}

	public void ResizeFrame()
	{
		this.activeObjectsCounter = 0;
		for (int i = 0; i < (int)this.objects.Length; i++)
		{
			if (this.objects[i].activeSelf)
			{
				this.activeObjectsCounter++;
			}
		}
		if (this.activeObjectsCounter > 0)
		{
			this.frame.width = Mathf.RoundToInt(this.frameSize[this.activeObjectsCounter - 1].x);
			this.frame.height = Mathf.RoundToInt(this.frameSize[this.activeObjectsCounter - 1].y);
		}
		if (this.table != null)
		{
			this.table.sorting = UITable.Sorting.Alphabetic;
			this.table.Reposition();
			this.table.repositionNow = true;
		}
	}
}