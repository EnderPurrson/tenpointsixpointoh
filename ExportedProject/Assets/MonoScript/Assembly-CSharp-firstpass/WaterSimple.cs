using System;
using UnityEngine;

[ExecuteInEditMode]
public class WaterSimple : MonoBehaviour
{
	public WaterSimple()
	{
	}

	private void Update()
	{
		if (!base.GetComponent<Renderer>())
		{
			return;
		}
		Material component = base.GetComponent<Renderer>().sharedMaterial;
		if (!component)
		{
			return;
		}
		Vector4 vector = component.GetVector("WaveSpeed");
		float num = component.GetFloat("_WaveScale");
		float single = Time.time / 20f;
		Vector4 vector4 = vector * (single * num);
		Vector4 vector41 = new Vector4(Mathf.Repeat(vector4.x, 1f), Mathf.Repeat(vector4.y, 1f), Mathf.Repeat(vector4.z, 1f), Mathf.Repeat(vector4.w, 1f));
		component.SetVector("_WaveOffset", vector41);
		Vector3 vector3 = new Vector3(1f / num, 1f / num, 1f);
		Matrix4x4 matrix4x4 = Matrix4x4.TRS(new Vector3(vector41.x, vector41.y, 0f), Quaternion.identity, vector3);
		component.SetMatrix("_WaveMatrix", matrix4x4);
		matrix4x4 = Matrix4x4.TRS(new Vector3(vector41.z, vector41.w, 0f), Quaternion.identity, vector3 * 0.45f);
		component.SetMatrix("_WaveMatrix2", matrix4x4);
	}
}