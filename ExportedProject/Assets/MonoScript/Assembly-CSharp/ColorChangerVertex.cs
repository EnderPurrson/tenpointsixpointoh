using System;
using UnityEngine;

public class ColorChangerVertex : MonoBehaviour
{
	private Mesh mesh;

	private Color[] meshColors;

	public ColorChangerVertex()
	{
	}

	private void Start()
	{
		this.mesh = base.GetComponent<MeshFilter>().mesh;
		this.meshColors = new Color[(int)this.mesh.vertices.Length];
	}

	private void Update()
	{
		for (int i = 0; i < (int)this.meshColors.Length; i++)
		{
			float single = this.mesh.vertices[i].magnitude;
			float single1 = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad + single));
			float single2 = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 0.45f + single));
			float single3 = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 1.2f + single));
			Color color = new Color(single1, single2, single3);
			this.meshColors[i] = color;
		}
		this.mesh.colors = this.meshColors;
	}
}