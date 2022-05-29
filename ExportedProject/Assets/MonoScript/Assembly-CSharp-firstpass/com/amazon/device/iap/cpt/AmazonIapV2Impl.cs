using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace com.amazon.device.iap.cpt
{
	public abstract class AmazonIapV2Impl : MonoBehaviour, IAmazonIapV2
	{
		private static AmazonLogger logger;

		private readonly static Dictionary<string, IDelegator> callbackDictionary;

		private readonly static object callbackLock;

		private readonly static Dictionary<string, List<IDelegator>> eventListeners;

		private readonly static object eventLock;

		public static IAmazonIapV2 Instance
		{
			get
			{
				return AmazonIapV2Impl.Builder.instance;
			}
		}

		static AmazonIapV2Impl()
		{
			AmazonIapV2Impl.callbackDictionary = new Dictionary<string, IDelegator>();
			AmazonIapV2Impl.callbackLock = new object();
			AmazonIapV2Impl.eventListeners = new Dictionary<string, List<IDelegator>>();
			AmazonIapV2Impl.eventLock = new object();
		}

		private AmazonIapV2Impl()
		{
		}

		public abstract void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		public abstract void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		public abstract void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		public abstract void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		public static void callback(string jsonMessage)
		{
			Dictionary<string, object> strs = null;
			try
			{
				AmazonIapV2Impl.logger.Debug("Executing callback");
				strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				string item = strs["callerId"] as string;
				AmazonIapV2Impl.callbackCaller(strs["response"] as Dictionary<string, object>, item);
			}
			catch (KeyNotFoundException keyNotFoundException1)
			{
				KeyNotFoundException keyNotFoundException = keyNotFoundException1;
				AmazonIapV2Impl.logger.Debug("callerId not found in callback");
				throw new AmazonException("Internal Error: Unknown callback id", keyNotFoundException);
			}
			catch (AmazonException amazonException1)
			{
				AmazonException amazonException = amazonException1;
				AmazonIapV2Impl.logger.Debug(string.Concat("Async call threw exception: ", amazonException.ToString()));
			}
		}

		private static void callbackCaller(Dictionary<string, object> response, string callerId)
		{
			IDelegator item = null;
			try
			{
				Jsonable.CheckForErrors(response);
				object obj = AmazonIapV2Impl.callbackLock;
				Monitor.Enter(obj);
				try
				{
					item = AmazonIapV2Impl.callbackDictionary[callerId];
					AmazonIapV2Impl.callbackDictionary.Remove(callerId);
					item.ExecuteSuccess(response);
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}
			catch (AmazonException amazonException1)
			{
				AmazonException amazonException = amazonException1;
				object obj1 = AmazonIapV2Impl.callbackLock;
				Monitor.Enter(obj1);
				try
				{
					if (item == null)
					{
						item = AmazonIapV2Impl.callbackDictionary[callerId];
					}
					AmazonIapV2Impl.callbackDictionary.Remove(callerId);
					item.ExecuteError(amazonException);
				}
				finally
				{
					Monitor.Exit(obj1);
				}
			}
		}

		public static void FireEvent(string jsonMessage)
		{
			// 
			// Current member / type: System.Void com.amazon.device.iap.cpt.AmazonIapV2Impl::FireEvent(System.String)
			// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp-firstpass.dll
			// 
			// Product version: 0.9.2.0
			// Exception in: System.Void FireEvent(System.String)
			// 
			// Object reference not set to an instance of an object.
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.get_Lock() in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 93
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.VisitBlockStatement(BlockStatement node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 24
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.Visit(ICodeNode node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 69
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.VisitTryStatement(TryStatement node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 507
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.Visit(ICodeNode node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 120
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.Visit(IEnumerable collection) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 383
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

		public abstract RequestOutput GetProductData(SkusInput skusInput);

		public abstract RequestOutput GetPurchaseUpdates(ResetInput resetInput);

		public abstract RequestOutput GetUserData();

		public abstract void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput);

		public abstract RequestOutput Purchase(SkuInput skuInput);

		public abstract void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		public abstract void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		public abstract void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		public abstract void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		public abstract void UnityFireEvent(string jsonMessage);

		private abstract class AmazonIapV2Base : AmazonIapV2Impl
		{
			private readonly static object startLock;

			private static volatile bool startCalled;

			static AmazonIapV2Base()
			{
				AmazonIapV2Impl.AmazonIapV2Base.startLock = new object();
				AmazonIapV2Impl.AmazonIapV2Base.startCalled = false;
			}

			public AmazonIapV2Base()
			{
				AmazonIapV2Impl.logger = new AmazonLogger(base.GetType().Name);
			}

			public override void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate)
			{
				this.Start();
				string str = "getProductDataResponse";
				object obj = AmazonIapV2Impl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonIapV2Impl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new GetProductDataResponseDelegator(responseDelegate)
						};
						AmazonIapV2Impl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonIapV2Impl.eventListeners[str].Add(new GetProductDataResponseDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate)
			{
				this.Start();
				string str = "getPurchaseUpdatesResponse";
				object obj = AmazonIapV2Impl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonIapV2Impl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new GetPurchaseUpdatesResponseDelegator(responseDelegate)
						};
						AmazonIapV2Impl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonIapV2Impl.eventListeners[str].Add(new GetPurchaseUpdatesResponseDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate)
			{
				this.Start();
				string str = "getUserDataResponse";
				object obj = AmazonIapV2Impl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonIapV2Impl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new GetUserDataResponseDelegator(responseDelegate)
						};
						AmazonIapV2Impl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonIapV2Impl.eventListeners[str].Add(new GetUserDataResponseDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate)
			{
				this.Start();
				string str = "purchaseResponse";
				object obj = AmazonIapV2Impl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonIapV2Impl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new PurchaseResponseDelegator(responseDelegate)
						};
						AmazonIapV2Impl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonIapV2Impl.eventListeners[str].Add(new PurchaseResponseDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override RequestOutput GetProductData(SkusInput skusInput)
			{
				this.Start();
				return RequestOutput.CreateFromJson(this.GetProductDataJson(skusInput.ToJson()));
			}

			private string GetProductDataJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeGetProductDataJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override RequestOutput GetPurchaseUpdates(ResetInput resetInput)
			{
				this.Start();
				return RequestOutput.CreateFromJson(this.GetPurchaseUpdatesJson(resetInput.ToJson()));
			}

			private string GetPurchaseUpdatesJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeGetPurchaseUpdatesJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override RequestOutput GetUserData()
			{
				this.Start();
				return RequestOutput.CreateFromJson(this.GetUserDataJson("{}"));
			}

			private string GetUserDataJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeGetUserDataJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			protected abstract void Init();

			protected abstract string NativeGetProductDataJson(string jsonMessage);

			protected abstract string NativeGetPurchaseUpdatesJson(string jsonMessage);

			protected abstract string NativeGetUserDataJson(string jsonMessage);

			protected abstract string NativeNotifyFulfillmentJson(string jsonMessage);

			protected abstract string NativePurchaseJson(string jsonMessage);

			public override void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput)
			{
				this.Start();
				Jsonable.CheckForErrors(Json.Deserialize(this.NotifyFulfillmentJson(notifyFulfillmentInput.ToJson())) as Dictionary<string, object>);
			}

			private string NotifyFulfillmentJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeNotifyFulfillmentJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override RequestOutput Purchase(SkuInput skuInput)
			{
				this.Start();
				return RequestOutput.CreateFromJson(this.PurchaseJson(skuInput.ToJson()));
			}

			private string PurchaseJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativePurchaseJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			protected abstract void RegisterCallback();

			protected abstract void RegisterCrossPlatformTool();

			protected abstract void RegisterEventListener();

			public override void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate)
			{
				this.Start();
				string str = "getProductDataResponse";
				object obj = AmazonIapV2Impl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(str))
					{
						foreach (GetProductDataResponseDelegator item in AmazonIapV2Impl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonIapV2Impl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate)
			{
				this.Start();
				string str = "getPurchaseUpdatesResponse";
				object obj = AmazonIapV2Impl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(str))
					{
						foreach (GetPurchaseUpdatesResponseDelegator item in AmazonIapV2Impl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonIapV2Impl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate)
			{
				this.Start();
				string str = "getUserDataResponse";
				object obj = AmazonIapV2Impl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(str))
					{
						foreach (GetUserDataResponseDelegator item in AmazonIapV2Impl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonIapV2Impl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate)
			{
				this.Start();
				string str = "purchaseResponse";
				object obj = AmazonIapV2Impl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(str))
					{
						foreach (PurchaseResponseDelegator item in AmazonIapV2Impl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonIapV2Impl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			protected void Start()
			{
				// 
				// Current member / type: System.Void com.amazon.device.iap.cpt.AmazonIapV2Impl/AmazonIapV2Base::Start()
				// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp-firstpass.dll
				// 
				// Product version: 0.9.2.0
				// Exception in: System.Void Start()
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

			public override void UnityFireEvent(string jsonMessage)
			{
				AmazonIapV2Impl.FireEvent(jsonMessage);
			}
		}

		private class AmazonIapV2Default : AmazonIapV2Impl.AmazonIapV2Base
		{
			public AmazonIapV2Default()
			{
			}

			protected override void Init()
			{
			}

			protected override string NativeGetProductDataJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeGetPurchaseUpdatesJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeGetUserDataJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeNotifyFulfillmentJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativePurchaseJson(string jsonMessage)
			{
				return "{}";
			}

			protected override void RegisterCallback()
			{
			}

			protected override void RegisterCrossPlatformTool()
			{
			}

			protected override void RegisterEventListener()
			{
			}
		}

		private abstract class AmazonIapV2DelegatesBase : AmazonIapV2Impl.AmazonIapV2Base
		{
			private const string CrossPlatformTool = "XAMARIN";

			protected AmazonIapV2Impl.CallbackDelegate callbackDelegate;

			protected AmazonIapV2Impl.CallbackDelegate eventDelegate;

			protected AmazonIapV2DelegatesBase()
			{
			}

			protected override void Init()
			{
				this.NativeInit();
			}

			protected abstract void NativeInit();

			protected abstract void NativeRegisterCallback(AmazonIapV2Impl.CallbackDelegate callback);

			protected abstract void NativeRegisterCrossPlatformTool(string crossPlatformTool);

			protected abstract void NativeRegisterEventListener(AmazonIapV2Impl.CallbackDelegate callback);

			protected override void RegisterCallback()
			{
				this.callbackDelegate = new AmazonIapV2Impl.CallbackDelegate(AmazonIapV2Impl.callback);
				this.NativeRegisterCallback(this.callbackDelegate);
			}

			protected override void RegisterCrossPlatformTool()
			{
				this.NativeRegisterCrossPlatformTool("XAMARIN");
			}

			protected override void RegisterEventListener()
			{
				this.eventDelegate = new AmazonIapV2Impl.CallbackDelegate(AmazonIapV2Impl.FireEvent);
				this.NativeRegisterEventListener(this.eventDelegate);
			}

			public override void UnityFireEvent(string jsonMessage)
			{
				throw new NotSupportedException("UnityFireEvent is not supported");
			}
		}

		private class AmazonIapV2UnityAndroid : AmazonIapV2Impl.AmazonIapV2UnityBase
		{
			public new static AmazonIapV2Impl.AmazonIapV2UnityAndroid Instance
			{
				get
				{
					return AmazonIapV2Impl.AmazonIapV2UnityBase.getInstance<AmazonIapV2Impl.AmazonIapV2UnityAndroid>();
				}
			}

			public AmazonIapV2UnityAndroid()
			{
			}

			[DllImport("AmazonIapV2Bridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeGetProductDataJson(string jsonMessage);

			protected override string NativeGetProductDataJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeGetProductDataJson(jsonMessage);
			}

			[DllImport("AmazonIapV2Bridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeGetPurchaseUpdatesJson(string jsonMessage);

			protected override string NativeGetPurchaseUpdatesJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeGetPurchaseUpdatesJson(jsonMessage);
			}

			[DllImport("AmazonIapV2Bridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeGetUserDataJson(string jsonMessage);

			protected override string NativeGetUserDataJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeGetUserDataJson(jsonMessage);
			}

			[DllImport("AmazonIapV2Bridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeInit();

			protected override void NativeInit()
			{
				AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeInit();
			}

			[DllImport("AmazonIapV2Bridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeNotifyFulfillmentJson(string jsonMessage);

			protected override string NativeNotifyFulfillmentJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeNotifyFulfillmentJson(jsonMessage);
			}

			[DllImport("AmazonIapV2Bridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativePurchaseJson(string jsonMessage);

			protected override string NativePurchaseJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativePurchaseJson(jsonMessage);
			}

			[DllImport("AmazonIapV2Bridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeRegisterCallbackGameObject(string name);

			protected override void NativeRegisterCrossPlatformTool(string crossPlatformTool)
			{
			}

			protected override void RegisterCallback()
			{
				AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeRegisterCallbackGameObject(base.gameObject.name);
			}

			protected override void RegisterEventListener()
			{
				AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeRegisterCallbackGameObject(base.gameObject.name);
			}
		}

		private abstract class AmazonIapV2UnityBase : AmazonIapV2Impl.AmazonIapV2Base
		{
			private const string CrossPlatformTool = "UNITY";

			private static AmazonIapV2Impl.AmazonIapV2UnityBase instance;

			private static Type instanceType;

			private static volatile bool quit;

			private static object initLock;

			static AmazonIapV2UnityBase()
			{
				AmazonIapV2Impl.AmazonIapV2UnityBase.quit = false;
				AmazonIapV2Impl.AmazonIapV2UnityBase.initLock = new object();
			}

			protected AmazonIapV2UnityBase()
			{
			}

			private static void assertTrue(bool statement, string errorMessage)
			{
				if (!statement)
				{
					throw new AmazonException("FATAL: An internal error occurred", new InvalidOperationException(errorMessage));
				}
			}

			public static T getInstance<T>()
			where T : AmazonIapV2Impl.AmazonIapV2UnityBase
			{
				// 
				// Current member / type: T com.amazon.device.iap.cpt.AmazonIapV2Impl/AmazonIapV2UnityBase::getInstance()
				// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp-firstpass.dll
				// 
				// Product version: 0.9.2.0
				// Exception in: T getInstance()
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

			protected override void Init()
			{
				this.NativeInit();
			}

			protected abstract void NativeInit();

			protected abstract void NativeRegisterCrossPlatformTool(string crossPlatformTool);

			public void OnDestroy()
			{
				AmazonIapV2Impl.AmazonIapV2UnityBase.quit = true;
			}

			protected override void RegisterCrossPlatformTool()
			{
				this.NativeRegisterCrossPlatformTool("UNITY");
			}
		}

		private class Builder
		{
			internal readonly static IAmazonIapV2 instance;

			static Builder()
			{
				AmazonIapV2Impl.Builder.instance = AmazonIapV2Impl.AmazonIapV2UnityAndroid.Instance;
			}

			public Builder()
			{
			}
		}

		protected delegate void CallbackDelegate(string jsonMessage);
	}
}