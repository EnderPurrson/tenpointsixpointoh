using System;
using UnityEngine;

public class UIGeometry
{
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	public BetterList<Color32> cols = new BetterList<Color32>();

	private BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();

	private Vector3 mRtpNormal;

	private Vector4 mRtpTan;

	public bool hasTransformed
	{
		get
		{
			return (this.mRtpVerts == null || this.mRtpVerts.size <= 0 ? false : this.mRtpVerts.size == this.verts.size);
		}
	}

	public bool hasVertices
	{
		get
		{
			return this.verts.size > 0;
		}
	}

	public UIGeometry()
	{
	}

	public void ApplyTransform(Matrix4x4 widgetToPanel, bool generateNormals = true)
	{
		if (this.verts.size <= 0)
		{
			this.mRtpVerts.Clear();
		}
		else
		{
			this.mRtpVerts.Clear();
			int num = 0;
			int num1 = this.verts.size;
			while (num < num1)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[num]));
				num++;
			}
			if (generateNormals)
			{
				this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
				Vector3 vector3 = widgetToPanel.MultiplyVector(Vector3.right).normalized;
				this.mRtpTan = new Vector4(vector3.x, vector3.y, vector3.z, -1f);
			}
		}
	}

	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
		{
			if (n != null)
			{
				for (int i = 0; i < this.mRtpVerts.size; i++)
				{
					v.Add(this.mRtpVerts.buffer[i]);
					u.Add(this.uvs.buffer[i]);
					c.Add(this.cols.buffer[i]);
					n.Add(this.mRtpNormal);
					t.Add(this.mRtpTan);
				}
			}
			else
			{
				for (int j = 0; j < this.mRtpVerts.size; j++)
				{
					v.Add(this.mRtpVerts.buffer[j]);
					u.Add(this.uvs.buffer[j]);
					c.Add(this.cols.buffer[j]);
				}
			}
		}
	}
}