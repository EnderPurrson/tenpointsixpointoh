using System;
using System.Reflection;

internal class RSA
{
	public RSA()
	{
	}

	public static void SimpleParseASN1(string publicKey, ref byte[] modulus, ref byte[] exponent)
	{
		byte[] numArray = Convert.FromBase64String(publicKey);
		Type type = Type.GetType("Mono.Security.ASN1");
		ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(byte[]) });
		PropertyInfo property = type.GetProperty("Value");
		PropertyInfo propertyInfo = type.GetProperty("Item");
		object obj = constructor.Invoke(new object[] { numArray });
		object value = propertyInfo.GetValue(obj, new object[] { 1 });
		byte[] value1 = (byte[])property.GetValue(value, null);
		byte[] numArray1 = new byte[(int)value1.Length - 1];
		Array.Copy(value1, 1, numArray1, 0, (int)value1.Length - 1);
		obj = constructor.Invoke(new object[] { numArray1 });
		object obj1 = propertyInfo.GetValue(obj, new object[] { 0 });
		object value2 = propertyInfo.GetValue(obj, new object[] { 1 });
		modulus = (byte[])property.GetValue(obj1, null);
		exponent = (byte[])property.GetValue(value2, null);
	}
}