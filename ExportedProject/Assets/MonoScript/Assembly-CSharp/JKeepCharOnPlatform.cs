using System;
using System.Collections;
using UnityEngine;

public class JKeepCharOnPlatform : MonoBehaviour
{
	public float verticalOffset = 0.5f;

	private Hashtable onPlatform = new Hashtable();

	private Vector3 lastPos;

	public JKeepCharOnPlatform()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		CharacterController component = other.GetComponent(typeof(CharacterController)) as CharacterController;
		if (component == null)
		{
			return;
		}
		Transform transforms = other.transform;
		float single = component.height / 2f;
		Vector3 vector3 = component.center;
		float single1 = single - vector3.y + this.verticalOffset;
		JKeepCharOnPlatform.Data datum = new JKeepCharOnPlatform.Data(component, transforms, single1);
		this.onPlatform.Add(other.transform, datum);
	}

	private void OnTriggerExit(Collider other)
	{
		this.onPlatform.Remove(other.transform);
	}

	private void Start()
	{
		this.lastPos = base.transform.position;
	}

	private void Update()
	{
		Vector3 vector3 = base.transform.position;
		float single = vector3.y;
		Vector3 vector31 = vector3 - this.lastPos;
		float single1 = vector31.y;
		vector31.y = 0f;
		this.lastPos = vector3;
		IDictionaryEnumerator enumerator = this.onPlatform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JKeepCharOnPlatform.Data value = (JKeepCharOnPlatform.Data)((DictionaryEntry)enumerator.Current).Value;
				float single2 = value.ctrl.velocity.y;
				if (single2 > 0f && single2 > single1)
				{
					continue;
				}
				Vector3 vector32 = value.t.position;
				vector32.y = single + value.yOffset;
				vector32 += vector31;
				value.t.position = vector32;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public struct Data
	{
		public CharacterController ctrl;

		public Transform t;

		public float yOffset;

		public Data(CharacterController ctrl, Transform t, float yOffset)
		{
			this.ctrl = ctrl;
			this.t = t;
			this.yOffset = yOffset;
		}
	}
}