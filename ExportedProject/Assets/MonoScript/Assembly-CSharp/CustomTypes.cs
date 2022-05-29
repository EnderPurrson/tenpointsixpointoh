using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

internal static class CustomTypes
{
	public readonly static byte[] memVector3;

	public readonly static byte[] memVector2;

	public readonly static byte[] memQuarternion;

	public readonly static byte[] memPlayer;

	static CustomTypes()
	{
		CustomTypes.memVector3 = new byte[12];
		CustomTypes.memVector2 = new byte[8];
		CustomTypes.memQuarternion = new byte[16];
		CustomTypes.memPlayer = new byte[4];
	}

	private static object DeserializePhotonPlayer(StreamBuffer inStream, short length)
	{
		int num;
		byte[] numArray = CustomTypes.memPlayer;
		Monitor.Enter(numArray);
		try
		{
			inStream.Read(CustomTypes.memPlayer, 0, length);
			int num1 = 0;
			Protocol.Deserialize(out num, CustomTypes.memPlayer, ref num1);
		}
		finally
		{
			Monitor.Exit(numArray);
		}
		if (!PhotonNetwork.networkingPeer.mActors.ContainsKey(num))
		{
			return null;
		}
		return PhotonNetwork.networkingPeer.mActors[num];
	}

	private static object DeserializeQuaternion(StreamBuffer inStream, short length)
	{
		Quaternion quaternion = new Quaternion();
		byte[] numArray = CustomTypes.memQuarternion;
		Monitor.Enter(numArray);
		try
		{
			inStream.Read(CustomTypes.memQuarternion, 0, 16);
			int num = 0;
			Protocol.Deserialize(out quaternion.w, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.x, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.y, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.z, CustomTypes.memQuarternion, ref num);
		}
		finally
		{
			Monitor.Exit(numArray);
		}
		return quaternion;
	}

	private static object DeserializeVector2(StreamBuffer inStream, short length)
	{
		Vector2 vector2 = new Vector2();
		byte[] numArray = CustomTypes.memVector2;
		Monitor.Enter(numArray);
		try
		{
			inStream.Read(CustomTypes.memVector2, 0, 8);
			int num = 0;
			Protocol.Deserialize(out vector2.x, CustomTypes.memVector2, ref num);
			Protocol.Deserialize(out vector2.y, CustomTypes.memVector2, ref num);
		}
		finally
		{
			Monitor.Exit(numArray);
		}
		return vector2;
	}

	private static object DeserializeVector3(StreamBuffer inStream, short length)
	{
		Vector3 vector3 = new Vector3();
		byte[] numArray = CustomTypes.memVector3;
		Monitor.Enter(numArray);
		try
		{
			inStream.Read(CustomTypes.memVector3, 0, 12);
			int num = 0;
			Protocol.Deserialize(out vector3.x, CustomTypes.memVector3, ref num);
			Protocol.Deserialize(out vector3.y, CustomTypes.memVector3, ref num);
			Protocol.Deserialize(out vector3.z, CustomTypes.memVector3, ref num);
		}
		finally
		{
			Monitor.Exit(numArray);
		}
		return vector3;
	}

	internal static void Register()
	{
		PhotonPeer.RegisterType(typeof(Vector2), 87, new SerializeStreamMethod(CustomTypes.SerializeVector2), new DeserializeStreamMethod(CustomTypes.DeserializeVector2));
		PhotonPeer.RegisterType(typeof(Vector3), 86, new SerializeStreamMethod(CustomTypes.SerializeVector3), new DeserializeStreamMethod(CustomTypes.DeserializeVector3));
		PhotonPeer.RegisterType(typeof(Quaternion), 81, new SerializeStreamMethod(CustomTypes.SerializeQuaternion), new DeserializeStreamMethod(CustomTypes.DeserializeQuaternion));
		PhotonPeer.RegisterType(typeof(PhotonPlayer), 80, new SerializeStreamMethod(CustomTypes.SerializePhotonPlayer), new DeserializeStreamMethod(CustomTypes.DeserializePhotonPlayer));
	}

	private static short SerializePhotonPlayer(StreamBuffer outStream, object customobject)
	{
		short num;
		int d = ((PhotonPlayer)customobject).ID;
		byte[] numArray = CustomTypes.memPlayer;
		Monitor.Enter(numArray);
		try
		{
			byte[] numArray1 = CustomTypes.memPlayer;
			int num1 = 0;
			Protocol.Serialize(d, numArray1, ref num1);
			outStream.Write(numArray1, 0, 4);
			num = 4;
		}
		finally
		{
			Monitor.Exit(numArray);
		}
		return num;
	}

	private static short SerializeQuaternion(StreamBuffer outStream, object customobject)
	{
		Quaternion quaternion = (Quaternion)customobject;
		byte[] numArray = CustomTypes.memQuarternion;
		Monitor.Enter(numArray);
		try
		{
			byte[] numArray1 = CustomTypes.memQuarternion;
			int num = 0;
			Protocol.Serialize(quaternion.w, numArray1, ref num);
			Protocol.Serialize(quaternion.x, numArray1, ref num);
			Protocol.Serialize(quaternion.y, numArray1, ref num);
			Protocol.Serialize(quaternion.z, numArray1, ref num);
			outStream.Write(numArray1, 0, 16);
		}
		finally
		{
			Monitor.Exit(numArray);
		}
		return 16;
	}

	private static short SerializeVector2(StreamBuffer outStream, object customobject)
	{
		Vector2 vector2 = (Vector2)customobject;
		byte[] numArray = CustomTypes.memVector2;
		Monitor.Enter(numArray);
		try
		{
			byte[] numArray1 = CustomTypes.memVector2;
			int num = 0;
			Protocol.Serialize(vector2.x, numArray1, ref num);
			Protocol.Serialize(vector2.y, numArray1, ref num);
			outStream.Write(numArray1, 0, 8);
		}
		finally
		{
			Monitor.Exit(numArray);
		}
		return 8;
	}

	private static short SerializeVector3(StreamBuffer outStream, object customobject)
	{
		Vector3 vector3 = (Vector3)customobject;
		int num = 0;
		byte[] numArray = CustomTypes.memVector3;
		Monitor.Enter(numArray);
		try
		{
			byte[] numArray1 = CustomTypes.memVector3;
			Protocol.Serialize(vector3.x, numArray1, ref num);
			Protocol.Serialize(vector3.y, numArray1, ref num);
			Protocol.Serialize(vector3.z, numArray1, ref num);
			outStream.Write(numArray1, 0, 12);
		}
		finally
		{
			Monitor.Exit(numArray);
		}
		return 12;
	}
}