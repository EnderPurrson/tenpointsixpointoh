using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LitJson
{
	public class JsonMapper
	{
		private static int max_nesting_depth;

		private static IFormatProvider datetime_format;

		private static IDictionary<Type, ExporterFunc> base_exporters_table;

		private static IDictionary<Type, ExporterFunc> custom_exporters_table;

		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table;

		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table;

		private static IDictionary<Type, ArrayMetadata> array_metadata;

		private readonly static object array_metadata_lock;

		private static IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops;

		private readonly static object conv_ops_lock;

		private static IDictionary<Type, ObjectMetadata> object_metadata;

		private readonly static object object_metadata_lock;

		private static IDictionary<Type, IList<PropertyMetadata>> type_properties;

		private readonly static object type_properties_lock;

		private static JsonWriter static_writer;

		private readonly static object static_writer_lock;

		static JsonMapper()
		{
			JsonMapper.array_metadata_lock = new object();
			JsonMapper.conv_ops_lock = new object();
			JsonMapper.object_metadata_lock = new object();
			JsonMapper.type_properties_lock = new object();
			JsonMapper.static_writer_lock = new object();
			JsonMapper.max_nesting_depth = 100;
			JsonMapper.array_metadata = new Dictionary<Type, ArrayMetadata>();
			JsonMapper.conv_ops = new Dictionary<Type, IDictionary<Type, MethodInfo>>();
			JsonMapper.object_metadata = new Dictionary<Type, ObjectMetadata>();
			JsonMapper.type_properties = new Dictionary<Type, IList<PropertyMetadata>>();
			JsonMapper.static_writer = new JsonWriter();
			JsonMapper.datetime_format = DateTimeFormatInfo.InvariantInfo;
			JsonMapper.base_exporters_table = new Dictionary<Type, ExporterFunc>();
			JsonMapper.custom_exporters_table = new Dictionary<Type, ExporterFunc>();
			JsonMapper.base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			JsonMapper.custom_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			JsonMapper.RegisterBaseExporters();
			JsonMapper.RegisterBaseImporters();
		}

		public JsonMapper()
		{
		}

		private static void AddArrayMetadata(Type type)
		{
			// 
			// Current member / type: System.Void LitJson.JsonMapper::AddArrayMetadata(System.Type)
			// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp.dll
			// 
			// Product version: 0.9.2.0
			// Exception in: System.Void AddArrayMetadata(System.Type)
			// 
			// Object reference not set to an instance of an object.
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.get_Lock() in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 93
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.VisitBlockStatement(BlockStatement node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 24
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.Visit(ICodeNode node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 69
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.Process(DecompilationContext context, BlockStatement body) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 18
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.RunInternal(MethodBody body, BlockStatement block, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 81
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.Run(MethodBody body, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 70
			//    at Telerik.JustDecompiler.Decompiler.Extensions.RunPipeline(DecompilationPipeline pipeline, ILanguage language, MethodBody body, DecompilationContext& context) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 95
			//    at Telerik.JustDecompiler.Decompiler.Extensions.Decompile(MethodBody body, ILanguage language, DecompilationContext& context, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 61
			//    at Telerik.JustDecompiler.Decompiler.WriterContextServices.BaseWriterContextService.DecompileMethod(ILanguage language, MethodDefinition method, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 118
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}

		private static void AddObjectMetadata(Type type)
		{
			// 
			// Current member / type: System.Void LitJson.JsonMapper::AddObjectMetadata(System.Type)
			// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp.dll
			// 
			// Product version: 0.9.2.0
			// Exception in: System.Void AddObjectMetadata(System.Type)
			// 
			// Object reference not set to an instance of an object.
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.get_Lock() in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 93
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.VisitBlockStatement(BlockStatement node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 24
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.Visit(ICodeNode node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 69
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.Process(DecompilationContext context, BlockStatement body) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 18
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.RunInternal(MethodBody body, BlockStatement block, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 81
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.Run(MethodBody body, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 70
			//    at Telerik.JustDecompiler.Decompiler.Extensions.RunPipeline(DecompilationPipeline pipeline, ILanguage language, MethodBody body, DecompilationContext& context) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 95
			//    at Telerik.JustDecompiler.Decompiler.Extensions.Decompile(MethodBody body, ILanguage language, DecompilationContext& context, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 61
			//    at Telerik.JustDecompiler.Decompiler.WriterContextServices.BaseWriterContextService.DecompileMethod(ILanguage language, MethodDefinition method, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 118
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}

		private static void AddTypeProperties(Type type)
		{
			// 
			// Current member / type: System.Void LitJson.JsonMapper::AddTypeProperties(System.Type)
			// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp.dll
			// 
			// Product version: 0.9.2.0
			// Exception in: System.Void AddTypeProperties(System.Type)
			// 
			// Object reference not set to an instance of an object.
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.get_Lock() in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 93
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.VisitBlockStatement(BlockStatement node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 24
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.Visit(ICodeNode node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 69
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.Process(DecompilationContext context, BlockStatement body) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 18
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.RunInternal(MethodBody body, BlockStatement block, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 81
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.Run(MethodBody body, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 70
			//    at Telerik.JustDecompiler.Decompiler.Extensions.RunPipeline(DecompilationPipeline pipeline, ILanguage language, MethodBody body, DecompilationContext& context) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 95
			//    at Telerik.JustDecompiler.Decompiler.Extensions.Decompile(MethodBody body, ILanguage language, DecompilationContext& context, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 61
			//    at Telerik.JustDecompiler.Decompiler.WriterContextServices.BaseWriterContextService.DecompileMethod(ILanguage language, MethodDefinition method, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 118
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}

		private static MethodInfo GetConvOp(Type t1, Type t2)
		{
			MethodInfo item;
			object convOpsLock = JsonMapper.conv_ops_lock;
			Monitor.Enter(convOpsLock);
			try
			{
				if (!JsonMapper.conv_ops.ContainsKey(t1))
				{
					JsonMapper.conv_ops.Add(t1, new Dictionary<Type, MethodInfo>());
				}
			}
			finally
			{
				Monitor.Exit(convOpsLock);
			}
			if (JsonMapper.conv_ops[t1].ContainsKey(t2))
			{
				return JsonMapper.conv_ops[t1][t2];
			}
			MethodInfo method = t1.GetMethod("op_Implicit", new Type[] { t2 });
			object obj = JsonMapper.conv_ops_lock;
			Monitor.Enter(obj);
			try
			{
				try
				{
					JsonMapper.conv_ops[t1].Add(t2, method);
				}
				catch (ArgumentException argumentException)
				{
					item = JsonMapper.conv_ops[t1][t2];
					return item;
				}
				return method;
			}
			finally
			{
				Monitor.Exit(obj);
			}
			return item;
		}

		private static void ReadSkip(JsonReader reader)
		{
			JsonMapper.ToWrapper(() => new JsonMockWrapper(), reader);
		}

		private static object ReadValue(Type inst_type, JsonReader reader)
		{
			IList arrayLists;
			Type elementType;
			string value;
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd)
			{
				return null;
			}
			if (reader.Token == JsonToken.Null)
			{
				if (!inst_type.IsClass)
				{
					throw new JsonException(string.Format("Can't assign null to an instance of type {0}", inst_type));
				}
				return null;
			}
			if (reader.Token == JsonToken.Double || reader.Token == JsonToken.Int || reader.Token == JsonToken.Long || reader.Token == JsonToken.String || reader.Token == JsonToken.Boolean)
			{
				Type type = reader.Value.GetType();
				if (inst_type.IsAssignableFrom(type))
				{
					return reader.Value;
				}
				if (JsonMapper.custom_importers_table.ContainsKey(type) && JsonMapper.custom_importers_table[type].ContainsKey(inst_type))
				{
					ImporterFunc item = JsonMapper.custom_importers_table[type][inst_type];
					return item(reader.Value);
				}
				if (JsonMapper.base_importers_table.ContainsKey(type) && JsonMapper.base_importers_table[type].ContainsKey(inst_type))
				{
					ImporterFunc importerFunc = JsonMapper.base_importers_table[type][inst_type];
					return importerFunc(reader.Value);
				}
				if (inst_type.IsEnum)
				{
					return Enum.ToObject(inst_type, reader.Value);
				}
				MethodInfo convOp = JsonMapper.GetConvOp(inst_type, type);
				if (convOp == null)
				{
					throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, type, inst_type));
				}
				return convOp.Invoke(null, new object[] { reader.Value });
			}
			object obj = null;
			if (reader.Token == JsonToken.ArrayStart)
			{
				JsonMapper.AddArrayMetadata(inst_type);
				ArrayMetadata arrayMetadatum = JsonMapper.array_metadata[inst_type];
				if (!arrayMetadatum.IsArray && !arrayMetadatum.IsList)
				{
					throw new JsonException(string.Format("Type {0} can't act as an array", inst_type));
				}
				if (arrayMetadatum.IsArray)
				{
					arrayLists = new ArrayList();
					elementType = inst_type.GetElementType();
				}
				else
				{
					arrayLists = (IList)Activator.CreateInstance(inst_type);
					elementType = arrayMetadatum.ElementType;
				}
				while (true)
				{
					object obj1 = JsonMapper.ReadValue(elementType, reader);
					if (obj1 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					arrayLists.Add(obj1);
				}
				if (!arrayMetadatum.IsArray)
				{
					obj = arrayLists;
				}
				else
				{
					int count = arrayLists.Count;
					obj = Array.CreateInstance(elementType, count);
					for (int i = 0; i < count; i++)
					{
						((Array)obj).SetValue(arrayLists[i], i);
					}
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				JsonMapper.AddObjectMetadata(inst_type);
				ObjectMetadata objectMetadatum = JsonMapper.object_metadata[inst_type];
				obj = Activator.CreateInstance(inst_type);
				while (true)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						return obj;
					}
					value = (string)reader.Value;
					if (objectMetadatum.Properties.ContainsKey(value))
					{
						PropertyMetadata propertyMetadatum = objectMetadatum.Properties[value];
						if (!propertyMetadatum.IsField)
						{
							PropertyInfo info = (PropertyInfo)propertyMetadatum.Info;
							if (!info.CanWrite)
							{
								JsonMapper.ReadValue(propertyMetadatum.Type, reader);
							}
							else
							{
								info.SetValue(obj, JsonMapper.ReadValue(propertyMetadatum.Type, reader), null);
							}
						}
						else
						{
							((FieldInfo)propertyMetadatum.Info).SetValue(obj, JsonMapper.ReadValue(propertyMetadatum.Type, reader));
						}
					}
					else if (objectMetadatum.IsDictionary)
					{
						((IDictionary)obj).Add(value, JsonMapper.ReadValue(objectMetadatum.ElementType, reader));
					}
					else
					{
						if (!reader.SkipNonMembers)
						{
							break;
						}
						JsonMapper.ReadSkip(reader);
						continue;
					}
				}
				throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", inst_type, value));
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
			IJsonWrapper jsonWrappers = factory();
			if (reader.Token == JsonToken.String)
			{
				jsonWrappers.SetString((string)reader.Value);
				return jsonWrappers;
			}
			if (reader.Token == JsonToken.Double)
			{
				jsonWrappers.SetDouble((double)reader.Value);
				return jsonWrappers;
			}
			if (reader.Token == JsonToken.Int)
			{
				jsonWrappers.SetInt((int)reader.Value);
				return jsonWrappers;
			}
			if (reader.Token == JsonToken.Long)
			{
				jsonWrappers.SetLong((long)reader.Value);
				return jsonWrappers;
			}
			if (reader.Token == JsonToken.Boolean)
			{
				jsonWrappers.SetBoolean((bool)reader.Value);
				return jsonWrappers;
			}
			if (reader.Token == JsonToken.ArrayStart)
			{
				jsonWrappers.SetJsonType(JsonType.Array);
				while (true)
				{
					IJsonWrapper jsonWrappers1 = JsonMapper.ReadValue(factory, reader);
					if (jsonWrappers1 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					jsonWrappers.Add(jsonWrappers1);
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				jsonWrappers.SetJsonType(JsonType.Object);
				while (true)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string value = (string)reader.Value;
					jsonWrappers[value] = JsonMapper.ReadValue(factory, reader);
				}
			}
			return jsonWrappers;
		}

		private static void RegisterBaseExporters()
		{
			JsonMapper.base_exporters_table[typeof(byte)] = (object obj, JsonWriter writer) => writer.Write(Convert.ToInt32((byte)obj));
			JsonMapper.base_exporters_table[typeof(char)] = (object obj, JsonWriter writer) => writer.Write(Convert.ToString((char)obj));
			JsonMapper.base_exporters_table[typeof(DateTime)] = (object obj, JsonWriter writer) => writer.Write(Convert.ToString((DateTime)obj, JsonMapper.datetime_format));
			JsonMapper.base_exporters_table[typeof(decimal)] = (object obj, JsonWriter writer) => writer.Write((decimal)obj);
			JsonMapper.base_exporters_table[typeof(sbyte)] = (object obj, JsonWriter writer) => writer.Write(Convert.ToInt32((sbyte)obj));
			JsonMapper.base_exporters_table[typeof(short)] = (object obj, JsonWriter writer) => writer.Write(Convert.ToInt32((short)obj));
			JsonMapper.base_exporters_table[typeof(ushort)] = (object obj, JsonWriter writer) => writer.Write(Convert.ToInt32((ushort)obj));
			JsonMapper.base_exporters_table[typeof(uint)] = (object obj, JsonWriter writer) => writer.Write(Convert.ToUInt64((uint)obj));
			JsonMapper.base_exporters_table[typeof(ulong)] = (object obj, JsonWriter writer) => writer.Write((ulong)obj);
		}

		private static void RegisterBaseImporters()
		{
			ImporterFunc num = (object input) => Convert.ToByte((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(byte), num);
			num = (object input) => Convert.ToUInt64((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ulong), num);
			num = (object input) => Convert.ToSByte((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(sbyte), num);
			num = (object input) => Convert.ToInt16((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(short), num);
			num = (object input) => Convert.ToUInt16((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ushort), num);
			num = (object input) => Convert.ToUInt32((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(uint), num);
			num = (object input) => Convert.ToSingle((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(float), num);
			num = (object input) => Convert.ToDouble((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(double), num);
			num = (object input) => Convert.ToDecimal((double)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(double), typeof(decimal), num);
			num = (object input) => Convert.ToUInt32((long)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(long), typeof(uint), num);
			num = (object input) => Convert.ToChar((string)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(char), num);
			num = (object input) => Convert.ToDateTime((string)input, JsonMapper.datetime_format);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(DateTime), num);
		}

		public static void RegisterExporter<T>(ExporterFunc<T> exporter)
		{
			ExporterFunc exporterFunc = (object obj, JsonWriter writer) => exporter((T)obj, writer);
			JsonMapper.custom_exporters_table[typeof(T)] = exporterFunc;
		}

		private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
		{
			if (!table.ContainsKey(json_type))
			{
				table.Add(json_type, new Dictionary<Type, ImporterFunc>());
			}
			table[json_type][value_type] = importer;
		}

		public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
		{
			ImporterFunc importerFunc = (object input) => importer((TJson)input);
			JsonMapper.RegisterImporter(JsonMapper.custom_importers_table, typeof(TJson), typeof(TValue), importerFunc);
		}

		public static string ToJson(object obj)
		{
			string str;
			object staticWriterLock = JsonMapper.static_writer_lock;
			Monitor.Enter(staticWriterLock);
			try
			{
				JsonMapper.static_writer.Reset();
				JsonMapper.WriteValue(obj, JsonMapper.static_writer, true, 0);
				str = JsonMapper.static_writer.ToString();
			}
			finally
			{
				Monitor.Exit(staticWriterLock);
			}
			return str;
		}

		public static void ToJson(object obj, JsonWriter writer)
		{
			JsonMapper.WriteValue(obj, writer, false, 0);
		}

		public static JsonData ToObject(JsonReader reader)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), reader);
		}

		public static JsonData ToObject(TextReader reader)
		{
			JsonReader jsonReader = new JsonReader(reader);
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), jsonReader);
		}

		public static JsonData ToObject(string json)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), json);
		}

		public static T ToObject<T>(JsonReader reader)
		{
			return (T)JsonMapper.ReadValue(typeof(T), reader);
		}

		public static T ToObject<T>(TextReader reader)
		{
			JsonReader jsonReader = new JsonReader(reader);
			return (T)JsonMapper.ReadValue(typeof(T), jsonReader);
		}

		public static T ToObject<T>(string json)
		{
			JsonReader jsonReader = new JsonReader(json);
			return (T)JsonMapper.ReadValue(typeof(T), jsonReader);
		}

		public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader)
		{
			return JsonMapper.ReadValue(factory, reader);
		}

		public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
		{
			return JsonMapper.ReadValue(factory, new JsonReader(json));
		}

		public static void UnregisterExporters()
		{
			JsonMapper.custom_exporters_table.Clear();
		}

		public static void UnregisterImporters()
		{
			JsonMapper.custom_importers_table.Clear();
		}

		private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
		{
			if (depth > JsonMapper.max_nesting_depth)
			{
				throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", obj.GetType()));
			}
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj is IJsonWrapper)
			{
				if (!writer_is_private)
				{
					((IJsonWrapper)obj).ToJson(writer);
				}
				else
				{
					writer.TextWriter.Write(((IJsonWrapper)obj).ToJson());
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
			if (obj is Array)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator = ((Array)obj).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						JsonMapper.WriteValue(current, writer, writer_is_private, depth + 1);
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
				writer.WriteArrayEnd();
				return;
			}
			if (obj is IList)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator1 = ((IList)obj).GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						object current1 = enumerator1.Current;
						JsonMapper.WriteValue(current1, writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					IDisposable disposable1 = enumerator1 as IDisposable;
					if (disposable1 == null)
					{
					}
					disposable1.Dispose();
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj is IDictionary)
			{
				writer.WriteObjectStart();
				IDictionaryEnumerator dictionaryEnumerator = ((IDictionary)obj).GetEnumerator();
				try
				{
					while (dictionaryEnumerator.MoveNext())
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)dictionaryEnumerator.Current;
						writer.WritePropertyName((string)dictionaryEntry.Key);
						JsonMapper.WriteValue(dictionaryEntry.Value, writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					IDisposable disposable2 = dictionaryEnumerator as IDisposable;
					if (disposable2 == null)
					{
					}
					disposable2.Dispose();
				}
				writer.WriteObjectEnd();
				return;
			}
			Type type = obj.GetType();
			if (JsonMapper.custom_exporters_table.ContainsKey(type))
			{
				JsonMapper.custom_exporters_table[type](obj, writer);
				return;
			}
			if (JsonMapper.base_exporters_table.ContainsKey(type))
			{
				JsonMapper.base_exporters_table[type](obj, writer);
				return;
			}
			if (obj is Enum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
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
			JsonMapper.AddTypeProperties(type);
			IList<PropertyMetadata> item = JsonMapper.type_properties[type];
			writer.WriteObjectStart();
			IEnumerator<PropertyMetadata> enumerator2 = item.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					PropertyMetadata propertyMetadatum = enumerator2.Current;
					if (!propertyMetadatum.IsField)
					{
						PropertyInfo info = (PropertyInfo)propertyMetadatum.Info;
						if (!info.CanRead)
						{
							continue;
						}
						writer.WritePropertyName(propertyMetadatum.Info.Name);
						JsonMapper.WriteValue(info.GetValue(obj, null), writer, writer_is_private, depth + 1);
					}
					else
					{
						writer.WritePropertyName(propertyMetadatum.Info.Name);
						JsonMapper.WriteValue(((FieldInfo)propertyMetadatum.Info).GetValue(obj), writer, writer_is_private, depth + 1);
					}
				}
			}
			finally
			{
				if (enumerator2 == null)
				{
				}
				enumerator2.Dispose();
			}
			writer.WriteObjectEnd();
		}
	}
}