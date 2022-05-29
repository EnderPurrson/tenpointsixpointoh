using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonStream
{
	private bool write;

	private Queue<object> writeData;

	private object[] readData;

	internal byte currentItem;

	public int Count
	{
		get
		{
			return (!this.isWriting ? (int)this.readData.Length : this.writeData.Count);
		}
	}

	public bool isReading
	{
		get
		{
			return !this.write;
		}
	}

	public bool isWriting
	{
		get
		{
			return this.write;
		}
	}

	public PhotonStream(bool write, object[] incomingData)
	{
		this.write = write;
		if (incomingData != null)
		{
			this.readData = incomingData;
		}
		else
		{
			this.writeData = new Queue<object>(10);
		}
	}

	public object PeekNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		return this.readData[this.currentItem];
	}

	public object ReceiveNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		object obj = this.readData[this.currentItem];
		PhotonStream photonStream = this;
		photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		return obj;
	}

	internal void ResetWriteStream()
	{
		this.writeData.Clear();
	}

	public void SendNext(object obj)
	{
		if (!this.write)
		{
			Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
			return;
		}
		this.writeData.Enqueue(obj);
	}

	public void Serialize(ref bool myBool)
	{
		if (this.write)
		{
			this.writeData.Enqueue(myBool);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			myBool = (bool)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref int myInt)
	{
		if (this.write)
		{
			this.writeData.Enqueue(myInt);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			myInt = (int)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref string value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			value = (string)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref char value)
	{
		if (this.write)
		{
			this.writeData.Enqueue((char)value);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			value = (char)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref short value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			value = (short)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref float obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			obj = (float)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref PhotonPlayer obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			obj = (PhotonPlayer)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref Vector3 obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			obj = (Vector3)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref Vector2 obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			obj = (Vector2)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public void Serialize(ref Quaternion obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if ((int)this.readData.Length > this.currentItem)
		{
			obj = (Quaternion)this.readData[this.currentItem];
			PhotonStream photonStream = this;
			photonStream.currentItem = (byte)(photonStream.currentItem + 1);
		}
	}

	public object[] ToArray()
	{
		return (!this.isWriting ? this.readData : this.writeData.ToArray());
	}
}