using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FyberPlugin.LitJson
{
	public class JsonMapper
	{
		[CompilerGenerated]
		private sealed class _003CRegisterExporter_003Ec__AnonStorey0<T>
		{
			internal ExporterFunc<T> exporter;

			internal void _003C_003Em__19(object obj, JsonWriter writer)
			{
				exporter((T)obj, writer);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRegisterImporter_003Ec__AnonStorey1<TJson, TValue>
		{
			internal ImporterFunc<TJson, TValue> importer;

			internal object _003C_003Em__1A(object input)
			{
				return importer((TJson)input);
			}
		}

		private static int max_nesting_depth;

		private static IFormatProvider datetime_format;

		private static IDictionary<global::System.Type, ExporterFunc> base_exporters_table;

		private static IDictionary<global::System.Type, ExporterFunc> custom_exporters_table;

		private static IDictionary<global::System.Type, IDictionary<global::System.Type, ImporterFunc>> base_importers_table;

		private static IDictionary<global::System.Type, IDictionary<global::System.Type, ImporterFunc>> custom_importers_table;

		private static IDictionary<global::System.Type, ArrayMetadata> array_metadata;

		private static readonly object array_metadata_lock;

		private static IDictionary<global::System.Type, IDictionary<global::System.Type, MethodInfo>> conv_ops;

		private static readonly object conv_ops_lock;

		private static IDictionary<global::System.Type, ObjectMetadata> object_metadata;

		private static readonly object object_metadata_lock;

		private static IDictionary<global::System.Type, global::System.Collections.Generic.IList<PropertyMetadata>> type_properties;

		private static readonly object type_properties_lock;

		private static JsonWriter static_writer;

		private static readonly object static_writer_lock;

		[CompilerGenerated]
		private static WrapperFactory _003C_003Ef__am_0024cache10;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache11;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache12;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache13;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache14;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache15;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache16;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache17;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache18;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache19;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1A;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1B;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1C;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1D;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1E;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1F;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache20;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache21;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache22;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache23;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache24;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache25;

		[CompilerGenerated]
		private static WrapperFactory _003C_003Ef__am_0024cache26;

		[CompilerGenerated]
		private static WrapperFactory _003C_003Ef__am_0024cache27;

		[CompilerGenerated]
		private static WrapperFactory _003C_003Ef__am_0024cache28;

		static JsonMapper()
		{
			array_metadata_lock = new object();
			conv_ops_lock = new object();
			object_metadata_lock = new object();
			type_properties_lock = new object();
			static_writer_lock = new object();
			max_nesting_depth = 100;
			array_metadata = (IDictionary<global::System.Type, ArrayMetadata>)(object)new Dictionary<global::System.Type, ArrayMetadata>();
			conv_ops = (IDictionary<global::System.Type, IDictionary<global::System.Type, MethodInfo>>)(object)new Dictionary<global::System.Type, IDictionary<global::System.Type, MethodInfo>>();
			object_metadata = (IDictionary<global::System.Type, ObjectMetadata>)(object)new Dictionary<global::System.Type, ObjectMetadata>();
			type_properties = (IDictionary<global::System.Type, global::System.Collections.Generic.IList<PropertyMetadata>>)(object)new Dictionary<global::System.Type, global::System.Collections.Generic.IList<PropertyMetadata>>();
			static_writer = new JsonWriter();
			datetime_format = (IFormatProvider)(object)DateTimeFormatInfo.get_InvariantInfo();
			base_exporters_table = (IDictionary<global::System.Type, ExporterFunc>)(object)new Dictionary<global::System.Type, ExporterFunc>();
			custom_exporters_table = (IDictionary<global::System.Type, ExporterFunc>)(object)new Dictionary<global::System.Type, ExporterFunc>();
			base_importers_table = (IDictionary<global::System.Type, IDictionary<global::System.Type, ImporterFunc>>)(object)new Dictionary<global::System.Type, IDictionary<global::System.Type, ImporterFunc>>();
			custom_importers_table = (IDictionary<global::System.Type, IDictionary<global::System.Type, ImporterFunc>>)(object)new Dictionary<global::System.Type, IDictionary<global::System.Type, ImporterFunc>>();
			RegisterBaseExporters();
			RegisterBaseImporters();
		}

		private static void AddArrayMetadata(global::System.Type type)
		{
			//Discarded unreachable code: IL_00d8
			if (array_metadata.ContainsKey(type))
			{
				return;
			}
			ArrayMetadata arrayMetadata = default(ArrayMetadata);
			arrayMetadata.IsArray = type.get_IsArray();
			if (type.GetInterface("System.Collections.IList") != null)
			{
				arrayMetadata.IsList = true;
			}
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo val in properties)
			{
				if (!(((MemberInfo)val).get_Name() != "Item"))
				{
					ParameterInfo[] indexParameters = val.GetIndexParameters();
					if (indexParameters.Length == 1 && indexParameters[0].get_ParameterType() == typeof(int))
					{
						arrayMetadata.ElementType = val.get_PropertyType();
					}
				}
			}
			lock (array_metadata_lock)
			{
				try
				{
					array_metadata.Add(type, arrayMetadata);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		private static void AddObjectMetadata(global::System.Type type)
		{
			//Discarded unreachable code: IL_016c
			if (object_metadata.ContainsKey(type))
			{
				return;
			}
			ObjectMetadata objectMetadata = default(ObjectMetadata);
			if (type.GetInterface("System.Collections.IDictionary") != null)
			{
				objectMetadata.IsDictionary = true;
			}
			objectMetadata.Properties = (IDictionary<string, PropertyMetadata>)(object)new Dictionary<string, PropertyMetadata>();
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo val in properties)
			{
				if (((MemberInfo)val).get_Name() == "Item")
				{
					ParameterInfo[] indexParameters = val.GetIndexParameters();
					if (indexParameters.Length == 1 && indexParameters[0].get_ParameterType() == typeof(string))
					{
						objectMetadata.ElementType = val.get_PropertyType();
					}
				}
				else
				{
					PropertyMetadata propertyMetadata = default(PropertyMetadata);
					propertyMetadata.Info = (MemberInfo)(object)val;
					propertyMetadata.Type = val.get_PropertyType();
					objectMetadata.Properties.Add(((MemberInfo)val).get_Name(), propertyMetadata);
				}
			}
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo val2 in fields)
			{
				PropertyMetadata propertyMetadata2 = default(PropertyMetadata);
				propertyMetadata2.Info = (MemberInfo)(object)val2;
				propertyMetadata2.IsField = true;
				propertyMetadata2.Type = val2.get_FieldType();
				objectMetadata.Properties.Add(((MemberInfo)val2).get_Name(), propertyMetadata2);
			}
			lock (object_metadata_lock)
			{
				try
				{
					object_metadata.Add(type, objectMetadata);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		private static void AddTypeProperties(global::System.Type type)
		{
			//Discarded unreachable code: IL_00de
			if (type_properties.ContainsKey(type))
			{
				return;
			}
			global::System.Collections.Generic.IList<PropertyMetadata> list = (global::System.Collections.Generic.IList<PropertyMetadata>)new List<PropertyMetadata>();
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo val in properties)
			{
				if (!(((MemberInfo)val).get_Name() == "Item"))
				{
					PropertyMetadata propertyMetadata = default(PropertyMetadata);
					propertyMetadata.Info = (MemberInfo)(object)val;
					propertyMetadata.IsField = false;
					((global::System.Collections.Generic.ICollection<PropertyMetadata>)list).Add(propertyMetadata);
				}
			}
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo info in fields)
			{
				PropertyMetadata propertyMetadata2 = default(PropertyMetadata);
				propertyMetadata2.Info = (MemberInfo)(object)info;
				propertyMetadata2.IsField = true;
				((global::System.Collections.Generic.ICollection<PropertyMetadata>)list).Add(propertyMetadata2);
			}
			lock (type_properties_lock)
			{
				try
				{
					type_properties.Add(type, list);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		private static MethodInfo GetConvOp(global::System.Type t1, global::System.Type t2)
		{
			//Discarded unreachable code: IL_00b1
			lock (conv_ops_lock)
			{
				if (!conv_ops.ContainsKey(t1))
				{
					conv_ops.Add(t1, (IDictionary<global::System.Type, MethodInfo>)(object)new Dictionary<global::System.Type, MethodInfo>());
				}
			}
			if (conv_ops.get_Item(t1).ContainsKey(t2))
			{
				return conv_ops.get_Item(t1).get_Item(t2);
			}
			MethodInfo method = t1.GetMethod("op_Implicit", new global::System.Type[1] { t2 });
			lock (conv_ops_lock)
			{
				try
				{
					conv_ops.get_Item(t1).Add(t2, method);
					return method;
				}
				catch (ArgumentException)
				{
					return conv_ops.get_Item(t1).get_Item(t2);
				}
			}
		}

		private static object ReadValue(global::System.Type inst_type, JsonReader reader)
		{
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Expected O, but got Unknown
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Expected O, but got Unknown
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd)
			{
				return null;
			}
			global::System.Type underlyingType = Nullable.GetUnderlyingType(inst_type);
			global::System.Type type = underlyingType ?? inst_type;
			if (reader.Token == JsonToken.Null)
			{
				if (inst_type.get_IsClass() || underlyingType != null)
				{
					return null;
				}
				throw new JsonException(string.Format("Can't assign null to an instance of type {0}", (object)inst_type));
			}
			if (reader.Token == JsonToken.Double || reader.Token == JsonToken.Int || reader.Token == JsonToken.Long || reader.Token == JsonToken.String || reader.Token == JsonToken.Boolean)
			{
				global::System.Type type2 = reader.Value.GetType();
				if (type.IsAssignableFrom(type2))
				{
					return reader.Value;
				}
				if (custom_importers_table.ContainsKey(type2) && custom_importers_table.get_Item(type2).ContainsKey(type))
				{
					ImporterFunc importerFunc = custom_importers_table.get_Item(type2).get_Item(type);
					return importerFunc(reader.Value);
				}
				if (base_importers_table.ContainsKey(type2) && base_importers_table.get_Item(type2).ContainsKey(type))
				{
					ImporterFunc importerFunc2 = base_importers_table.get_Item(type2).get_Item(type);
					return importerFunc2(reader.Value);
				}
				if (type.get_IsEnum())
				{
					return global::System.Enum.ToObject(type, reader.Value);
				}
				MethodInfo convOp = GetConvOp(type, type2);
				if (convOp != null)
				{
					return ((MethodBase)convOp).Invoke((object)default(object), new object[1] { reader.Value });
				}
				throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, (object)type2, (object)inst_type));
			}
			object obj = null;
			if (reader.Token == JsonToken.ArrayStart)
			{
				AddArrayMetadata(inst_type);
				ArrayMetadata arrayMetadata = array_metadata.get_Item(inst_type);
				if (!arrayMetadata.IsArray && !arrayMetadata.IsList)
				{
					throw new JsonException(string.Format("Type {0} can't act as an array", (object)inst_type));
				}
				global::System.Collections.IList list;
				global::System.Type elementType;
				if (!arrayMetadata.IsArray)
				{
					list = (global::System.Collections.IList)Activator.CreateInstance(inst_type);
					elementType = arrayMetadata.ElementType;
				}
				else
				{
					list = (global::System.Collections.IList)new ArrayList();
					elementType = inst_type.GetElementType();
				}
				while (true)
				{
					object obj2 = ReadValue(elementType, reader);
					if (obj2 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					list.Add(obj2);
				}
				if (arrayMetadata.IsArray)
				{
					int count = ((global::System.Collections.ICollection)list).get_Count();
					obj = global::System.Array.CreateInstance(elementType, count);
					for (int i = 0; i < count; i++)
					{
						((global::System.Array)obj).SetValue(list.get_Item(i), i);
					}
				}
				else
				{
					obj = list;
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				AddObjectMetadata(type);
				ObjectMetadata objectMetadata = object_metadata.get_Item(type);
				obj = Activator.CreateInstance(type);
				while (true)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string text = (string)reader.Value;
					if (objectMetadata.Properties.ContainsKey(text))
					{
						PropertyMetadata propertyMetadata = objectMetadata.Properties.get_Item(text);
						if (propertyMetadata.IsField)
						{
							((FieldInfo)propertyMetadata.Info).SetValue(obj, ReadValue(propertyMetadata.Type, reader));
							continue;
						}
						PropertyInfo val = (PropertyInfo)propertyMetadata.Info;
						if (val.get_CanWrite())
						{
							val.SetValue(obj, ReadValue(propertyMetadata.Type, reader), (object[])default(object[]));
						}
						else
						{
							ReadValue(propertyMetadata.Type, reader);
						}
					}
					else if (!objectMetadata.IsDictionary)
					{
						if (!reader.SkipNonMembers)
						{
							throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", (object)inst_type, (object)text));
						}
						ReadSkip(reader);
					}
					else
					{
						((IDictionary)obj).Add((object)text, ReadValue(objectMetadata.ElementType, reader));
					}
				}
			}
			return obj;
		}

		private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null)
			{
				return null;
			}
			IJsonWrapper jsonWrapper = factory();
			if (reader.Token == JsonToken.String)
			{
				jsonWrapper.SetString((string)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Double)
			{
				jsonWrapper.SetDouble((double)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Int)
			{
				jsonWrapper.SetInt((int)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Long)
			{
				jsonWrapper.SetLong((long)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Boolean)
			{
				jsonWrapper.SetBoolean((bool)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.ArrayStart)
			{
				jsonWrapper.SetJsonType(JsonType.Array);
				while (true)
				{
					IJsonWrapper jsonWrapper2 = ReadValue(factory, reader);
					if (jsonWrapper2 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					((global::System.Collections.IList)jsonWrapper).Add((object)jsonWrapper2);
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				jsonWrapper.SetJsonType(JsonType.Object);
				while (true)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string text = (string)reader.Value;
					((IDictionary)jsonWrapper).set_Item((object)text, (object)ReadValue(factory, reader));
				}
			}
			return jsonWrapper;
		}

		private static void ReadSkip(JsonReader reader)
		{
			if (_003C_003Ef__am_0024cache10 == null)
			{
				_003C_003Ef__am_0024cache10 = _003CReadSkip_003Em__0;
			}
			ToWrapper(_003C_003Ef__am_0024cache10, reader);
		}

		private static void RegisterBaseExporters()
		{
			IDictionary<global::System.Type, ExporterFunc> obj = base_exporters_table;
			global::System.Type typeFromHandle = typeof(byte);
			if (_003C_003Ef__am_0024cache11 == null)
			{
				_003C_003Ef__am_0024cache11 = _003CRegisterBaseExporters_003Em__1;
			}
			obj.set_Item(typeFromHandle, _003C_003Ef__am_0024cache11);
			IDictionary<global::System.Type, ExporterFunc> obj2 = base_exporters_table;
			global::System.Type typeFromHandle2 = typeof(char);
			if (_003C_003Ef__am_0024cache12 == null)
			{
				_003C_003Ef__am_0024cache12 = _003CRegisterBaseExporters_003Em__2;
			}
			obj2.set_Item(typeFromHandle2, _003C_003Ef__am_0024cache12);
			IDictionary<global::System.Type, ExporterFunc> obj3 = base_exporters_table;
			global::System.Type typeFromHandle3 = typeof(global::System.DateTime);
			if (_003C_003Ef__am_0024cache13 == null)
			{
				_003C_003Ef__am_0024cache13 = _003CRegisterBaseExporters_003Em__3;
			}
			obj3.set_Item(typeFromHandle3, _003C_003Ef__am_0024cache13);
			IDictionary<global::System.Type, ExporterFunc> obj4 = base_exporters_table;
			global::System.Type typeFromHandle4 = typeof(decimal);
			if (_003C_003Ef__am_0024cache14 == null)
			{
				_003C_003Ef__am_0024cache14 = _003CRegisterBaseExporters_003Em__4;
			}
			obj4.set_Item(typeFromHandle4, _003C_003Ef__am_0024cache14);
			IDictionary<global::System.Type, ExporterFunc> obj5 = base_exporters_table;
			global::System.Type typeFromHandle5 = typeof(sbyte);
			if (_003C_003Ef__am_0024cache15 == null)
			{
				_003C_003Ef__am_0024cache15 = _003CRegisterBaseExporters_003Em__5;
			}
			obj5.set_Item(typeFromHandle5, _003C_003Ef__am_0024cache15);
			IDictionary<global::System.Type, ExporterFunc> obj6 = base_exporters_table;
			global::System.Type typeFromHandle6 = typeof(short);
			if (_003C_003Ef__am_0024cache16 == null)
			{
				_003C_003Ef__am_0024cache16 = _003CRegisterBaseExporters_003Em__6;
			}
			obj6.set_Item(typeFromHandle6, _003C_003Ef__am_0024cache16);
			IDictionary<global::System.Type, ExporterFunc> obj7 = base_exporters_table;
			global::System.Type typeFromHandle7 = typeof(ushort);
			if (_003C_003Ef__am_0024cache17 == null)
			{
				_003C_003Ef__am_0024cache17 = _003CRegisterBaseExporters_003Em__7;
			}
			obj7.set_Item(typeFromHandle7, _003C_003Ef__am_0024cache17);
			IDictionary<global::System.Type, ExporterFunc> obj8 = base_exporters_table;
			global::System.Type typeFromHandle8 = typeof(uint);
			if (_003C_003Ef__am_0024cache18 == null)
			{
				_003C_003Ef__am_0024cache18 = _003CRegisterBaseExporters_003Em__8;
			}
			obj8.set_Item(typeFromHandle8, _003C_003Ef__am_0024cache18);
			IDictionary<global::System.Type, ExporterFunc> obj9 = base_exporters_table;
			global::System.Type typeFromHandle9 = typeof(ulong);
			if (_003C_003Ef__am_0024cache19 == null)
			{
				_003C_003Ef__am_0024cache19 = _003CRegisterBaseExporters_003Em__9;
			}
			obj9.set_Item(typeFromHandle9, _003C_003Ef__am_0024cache19);
		}

		private static void RegisterBaseImporters()
		{
			if (_003C_003Ef__am_0024cache1A == null)
			{
				_003C_003Ef__am_0024cache1A = _003CRegisterBaseImporters_003Em__A;
			}
			ImporterFunc importer = _003C_003Ef__am_0024cache1A;
			RegisterImporter(base_importers_table, typeof(int), typeof(byte), importer);
			if (_003C_003Ef__am_0024cache1B == null)
			{
				_003C_003Ef__am_0024cache1B = _003CRegisterBaseImporters_003Em__B;
			}
			importer = _003C_003Ef__am_0024cache1B;
			RegisterImporter(base_importers_table, typeof(int), typeof(ulong), importer);
			if (_003C_003Ef__am_0024cache1C == null)
			{
				_003C_003Ef__am_0024cache1C = _003CRegisterBaseImporters_003Em__C;
			}
			importer = _003C_003Ef__am_0024cache1C;
			RegisterImporter(base_importers_table, typeof(int), typeof(sbyte), importer);
			if (_003C_003Ef__am_0024cache1D == null)
			{
				_003C_003Ef__am_0024cache1D = _003CRegisterBaseImporters_003Em__D;
			}
			importer = _003C_003Ef__am_0024cache1D;
			RegisterImporter(base_importers_table, typeof(int), typeof(short), importer);
			if (_003C_003Ef__am_0024cache1E == null)
			{
				_003C_003Ef__am_0024cache1E = _003CRegisterBaseImporters_003Em__E;
			}
			importer = _003C_003Ef__am_0024cache1E;
			RegisterImporter(base_importers_table, typeof(int), typeof(ushort), importer);
			if (_003C_003Ef__am_0024cache1F == null)
			{
				_003C_003Ef__am_0024cache1F = _003CRegisterBaseImporters_003Em__F;
			}
			importer = _003C_003Ef__am_0024cache1F;
			RegisterImporter(base_importers_table, typeof(int), typeof(uint), importer);
			if (_003C_003Ef__am_0024cache20 == null)
			{
				_003C_003Ef__am_0024cache20 = _003CRegisterBaseImporters_003Em__10;
			}
			importer = _003C_003Ef__am_0024cache20;
			RegisterImporter(base_importers_table, typeof(int), typeof(float), importer);
			if (_003C_003Ef__am_0024cache21 == null)
			{
				_003C_003Ef__am_0024cache21 = _003CRegisterBaseImporters_003Em__11;
			}
			importer = _003C_003Ef__am_0024cache21;
			RegisterImporter(base_importers_table, typeof(int), typeof(double), importer);
			if (_003C_003Ef__am_0024cache22 == null)
			{
				_003C_003Ef__am_0024cache22 = _003CRegisterBaseImporters_003Em__12;
			}
			importer = _003C_003Ef__am_0024cache22;
			RegisterImporter(base_importers_table, typeof(double), typeof(decimal), importer);
			if (_003C_003Ef__am_0024cache23 == null)
			{
				_003C_003Ef__am_0024cache23 = _003CRegisterBaseImporters_003Em__13;
			}
			importer = _003C_003Ef__am_0024cache23;
			RegisterImporter(base_importers_table, typeof(long), typeof(uint), importer);
			if (_003C_003Ef__am_0024cache24 == null)
			{
				_003C_003Ef__am_0024cache24 = _003CRegisterBaseImporters_003Em__14;
			}
			importer = _003C_003Ef__am_0024cache24;
			RegisterImporter(base_importers_table, typeof(string), typeof(char), importer);
			if (_003C_003Ef__am_0024cache25 == null)
			{
				_003C_003Ef__am_0024cache25 = _003CRegisterBaseImporters_003Em__15;
			}
			importer = _003C_003Ef__am_0024cache25;
			RegisterImporter(base_importers_table, typeof(string), typeof(global::System.DateTime), importer);
		}

		private static void RegisterImporter(IDictionary<global::System.Type, IDictionary<global::System.Type, ImporterFunc>> table, global::System.Type json_type, global::System.Type value_type, ImporterFunc importer)
		{
			if (!table.ContainsKey(json_type))
			{
				table.Add(json_type, (IDictionary<global::System.Type, ImporterFunc>)(object)new Dictionary<global::System.Type, ImporterFunc>());
			}
			table.get_Item(json_type).set_Item(value_type, importer);
		}

		private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
		{
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Expected O, but got Unknown
			if (depth > max_nesting_depth)
			{
				throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", (object)obj.GetType()));
			}
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj is IJsonWrapper)
			{
				if (writer_is_private)
				{
					writer.TextWriter.Write(((IJsonWrapper)obj).ToJson());
				}
				else
				{
					((IJsonWrapper)obj).ToJson(writer);
				}
				return;
			}
			if (obj is string)
			{
				writer.Write((string)obj);
				return;
			}
			if (obj is double)
			{
				writer.Write((double)obj);
				return;
			}
			if (obj is int)
			{
				writer.Write((int)obj);
				return;
			}
			if (obj is bool)
			{
				writer.Write((bool)obj);
				return;
			}
			if (obj is long)
			{
				writer.Write((long)obj);
				return;
			}
			if (obj is global::System.Array)
			{
				writer.WriteArrayStart();
				global::System.Collections.IEnumerator enumerator = ((global::System.Array)obj).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.get_Current();
						WriteValue(current, writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					global::System.IDisposable disposable;
					if ((disposable = enumerator as global::System.IDisposable) != null)
					{
						disposable.Dispose();
					}
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj is global::System.Collections.IList)
			{
				writer.WriteArrayStart();
				global::System.Collections.IEnumerator enumerator2 = ((global::System.Collections.IEnumerable)(global::System.Collections.IList)obj).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object current2 = enumerator2.get_Current();
						WriteValue(current2, writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					global::System.IDisposable disposable2;
					if ((disposable2 = enumerator2 as global::System.IDisposable) != null)
					{
						disposable2.Dispose();
					}
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj is IDictionary)
			{
				writer.WriteObjectStart();
				IDictionaryEnumerator enumerator3 = ((IDictionary)obj).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator3).MoveNext())
					{
						DictionaryEntry val = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator3).get_Current();
						writer.WritePropertyName((string)((DictionaryEntry)(ref val)).get_Key());
						WriteValue(((DictionaryEntry)(ref val)).get_Value(), writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					global::System.IDisposable disposable3;
					if ((disposable3 = enumerator3 as global::System.IDisposable) != null)
					{
						disposable3.Dispose();
					}
				}
				writer.WriteObjectEnd();
				return;
			}
			global::System.Type type = obj.GetType();
			if (custom_exporters_table.ContainsKey(type))
			{
				ExporterFunc exporterFunc = custom_exporters_table.get_Item(type);
				exporterFunc(obj, writer);
				return;
			}
			if (base_exporters_table.ContainsKey(type))
			{
				ExporterFunc exporterFunc2 = base_exporters_table.get_Item(type);
				exporterFunc2(obj, writer);
				return;
			}
			if (obj is global::System.Enum)
			{
				global::System.Type underlyingType = global::System.Enum.GetUnderlyingType(type);
				if (underlyingType == typeof(long) || underlyingType == typeof(uint) || underlyingType == typeof(ulong))
				{
					writer.Write((ulong)obj);
				}
				else
				{
					writer.Write((int)obj);
				}
				return;
			}
			AddTypeProperties(type);
			global::System.Collections.Generic.IList<PropertyMetadata> list = type_properties.get_Item(type);
			writer.WriteObjectStart();
			global::System.Collections.Generic.IEnumerator<PropertyMetadata> enumerator4 = ((global::System.Collections.Generic.IEnumerable<PropertyMetadata>)list).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator4).MoveNext())
				{
					PropertyMetadata current3 = enumerator4.get_Current();
					if (current3.IsField)
					{
						writer.WritePropertyName(current3.Info.get_Name());
						WriteValue(((FieldInfo)current3.Info).GetValue(obj), writer, writer_is_private, depth + 1);
						continue;
					}
					PropertyInfo val2 = (PropertyInfo)current3.Info;
					if (val2.get_CanRead())
					{
						writer.WritePropertyName(current3.Info.get_Name());
						WriteValue(val2.GetValue(obj, (object[])default(object[])), writer, writer_is_private, depth + 1);
					}
				}
			}
			finally
			{
				if (enumerator4 != null)
				{
					((global::System.IDisposable)enumerator4).Dispose();
				}
			}
			writer.WriteObjectEnd();
		}

		public static string ToJson(object obj)
		{
			//Discarded unreachable code: IL_0033
			lock (static_writer_lock)
			{
				static_writer.Reset();
				WriteValue(obj, static_writer, true, 0);
				return ((object)static_writer).ToString();
			}
		}

		public static void ToJson(object obj, JsonWriter writer)
		{
			WriteValue(obj, writer, false, 0);
		}

		public static JsonData ToObject(JsonReader reader)
		{
			if (_003C_003Ef__am_0024cache26 == null)
			{
				_003C_003Ef__am_0024cache26 = _003CToObject_003Em__16;
			}
			return (JsonData)ToWrapper(_003C_003Ef__am_0024cache26, reader);
		}

		public static JsonData ToObject(TextReader reader)
		{
			JsonReader reader2 = new JsonReader(reader);
			if (_003C_003Ef__am_0024cache27 == null)
			{
				_003C_003Ef__am_0024cache27 = _003CToObject_003Em__17;
			}
			return (JsonData)ToWrapper(_003C_003Ef__am_0024cache27, reader2);
		}

		public static JsonData ToObject(string json)
		{
			if (_003C_003Ef__am_0024cache28 == null)
			{
				_003C_003Ef__am_0024cache28 = _003CToObject_003Em__18;
			}
			return (JsonData)ToWrapper(_003C_003Ef__am_0024cache28, json);
		}

		public static T ToObject<T>(JsonReader reader)
		{
			return (T)ReadValue(typeof(T), reader);
		}

		public static T ToObject<T>(TextReader reader)
		{
			JsonReader reader2 = new JsonReader(reader);
			return (T)ReadValue(typeof(T), reader2);
		}

		public static T ToObject<T>(string json)
		{
			JsonReader reader = new JsonReader(json);
			return (T)ReadValue(typeof(T), reader);
		}

		public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader)
		{
			return ReadValue(factory, reader);
		}

		public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
		{
			JsonReader reader = new JsonReader(json);
			return ReadValue(factory, reader);
		}

		public static void RegisterExporter<T>(ExporterFunc<T> exporter)
		{
			_003CRegisterExporter_003Ec__AnonStorey0<T> _003CRegisterExporter_003Ec__AnonStorey = new _003CRegisterExporter_003Ec__AnonStorey0<T>();
			_003CRegisterExporter_003Ec__AnonStorey.exporter = exporter;
			ExporterFunc exporterFunc = _003CRegisterExporter_003Ec__AnonStorey._003C_003Em__19;
			custom_exporters_table.set_Item(typeof(T), exporterFunc);
		}

		public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
		{
			_003CRegisterImporter_003Ec__AnonStorey1<TJson, TValue> _003CRegisterImporter_003Ec__AnonStorey = new _003CRegisterImporter_003Ec__AnonStorey1<TJson, TValue>();
			_003CRegisterImporter_003Ec__AnonStorey.importer = importer;
			ImporterFunc importer2 = _003CRegisterImporter_003Ec__AnonStorey._003C_003Em__1A;
			RegisterImporter(custom_importers_table, typeof(TJson), typeof(TValue), importer2);
		}

		public static void UnregisterExporters()
		{
			((global::System.Collections.Generic.ICollection<KeyValuePair<global::System.Type, ExporterFunc>>)custom_exporters_table).Clear();
		}

		public static void UnregisterImporters()
		{
			((global::System.Collections.Generic.ICollection<KeyValuePair<global::System.Type, IDictionary<global::System.Type, ImporterFunc>>>)custom_importers_table).Clear();
		}

		[CompilerGenerated]
		private static IJsonWrapper _003CReadSkip_003Em__0()
		{
			return new JsonMockWrapper();
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__1(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToInt32((byte)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__2(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToString((char)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__3(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToString((global::System.DateTime)obj, datetime_format));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__4(object obj, JsonWriter writer)
		{
			writer.Write((decimal)obj);
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__5(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToInt32((sbyte)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__6(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToInt32((short)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__7(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToInt32((ushort)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__8(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToUInt64((uint)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__9(object obj, JsonWriter writer)
		{
			writer.Write((ulong)obj);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__A(object input)
		{
			return Convert.ToByte((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__B(object input)
		{
			return Convert.ToUInt64((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__C(object input)
		{
			return Convert.ToSByte((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__D(object input)
		{
			return Convert.ToInt16((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__E(object input)
		{
			return Convert.ToUInt16((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__F(object input)
		{
			return Convert.ToUInt32((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__10(object input)
		{
			return Convert.ToSingle((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__11(object input)
		{
			return Convert.ToDouble((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__12(object input)
		{
			return Convert.ToDecimal((double)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__13(object input)
		{
			return Convert.ToUInt32((long)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__14(object input)
		{
			return Convert.ToChar((string)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__15(object input)
		{
			return Convert.ToDateTime((string)input, datetime_format);
		}

		[CompilerGenerated]
		private static IJsonWrapper _003CToObject_003Em__16()
		{
			return new JsonData();
		}

		[CompilerGenerated]
		private static IJsonWrapper _003CToObject_003Em__17()
		{
			return new JsonData();
		}

		[CompilerGenerated]
		private static IJsonWrapper _003CToObject_003Em__18()
		{
			return new JsonData();
		}
	}
}
