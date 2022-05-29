using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace com.amazon.mas.cpt.ads
{
	public abstract class AmazonMobileAdsImpl : MonoBehaviour, IAmazonMobileAds
	{
		private static AmazonLogger logger;

		private readonly static Dictionary<string, IDelegator> callbackDictionary;

		private readonly static object callbackLock;

		private readonly static Dictionary<string, List<IDelegator>> eventListeners;

		private readonly static object eventLock;

		public static IAmazonMobileAds Instance
		{
			get
			{
				return AmazonMobileAdsImpl.Builder.instance;
			}
		}

		static AmazonMobileAdsImpl()
		{
			AmazonMobileAdsImpl.callbackDictionary = new Dictionary<string, IDelegator>();
			AmazonMobileAdsImpl.callbackLock = new object();
			AmazonMobileAdsImpl.eventListeners = new Dictionary<string, List<IDelegator>>();
			AmazonMobileAdsImpl.eventLock = new object();
		}

		private AmazonMobileAdsImpl()
		{
		}

		public abstract void AddAdCollapsedListener(AdCollapsedDelegate responseDelegate);

		public abstract void AddAdDismissedListener(AdDismissedDelegate responseDelegate);

		public abstract void AddAdExpandedListener(AdExpandedDelegate responseDelegate);

		public abstract void AddAdFailedToLoadListener(AdFailedToLoadDelegate responseDelegate);

		public abstract void AddAdLoadedListener(AdLoadedDelegate responseDelegate);

		public abstract void AddAdResizedListener(AdResizedDelegate responseDelegate);

		public abstract IsEqual AreAdsEqual(AdPair adPair);

		public static void callback(string jsonMessage)
		{
			Dictionary<string, object> strs = null;
			try
			{
				AmazonMobileAdsImpl.logger.Debug("Executing callback");
				strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				string item = strs["callerId"] as string;
				AmazonMobileAdsImpl.callbackCaller(strs["response"] as Dictionary<string, object>, item);
			}
			catch (KeyNotFoundException keyNotFoundException1)
			{
				KeyNotFoundException keyNotFoundException = keyNotFoundException1;
				AmazonMobileAdsImpl.logger.Debug("callerId not found in callback");
				throw new AmazonException("Internal Error: Unknown callback id", keyNotFoundException);
			}
			catch (AmazonException amazonException1)
			{
				AmazonException amazonException = amazonException1;
				AmazonMobileAdsImpl.logger.Debug(string.Concat("Async call threw exception: ", amazonException.ToString()));
			}
		}

		private static void callbackCaller(Dictionary<string, object> response, string callerId)
		{
			IDelegator item = null;
			try
			{
				Jsonable.CheckForErrors(response);
				object obj = AmazonMobileAdsImpl.callbackLock;
				Monitor.Enter(obj);
				try
				{
					item = AmazonMobileAdsImpl.callbackDictionary[callerId];
					AmazonMobileAdsImpl.callbackDictionary.Remove(callerId);
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
				object obj1 = AmazonMobileAdsImpl.callbackLock;
				Monitor.Enter(obj1);
				try
				{
					if (item == null)
					{
						item = AmazonMobileAdsImpl.callbackDictionary[callerId];
					}
					AmazonMobileAdsImpl.callbackDictionary.Remove(callerId);
					item.ExecuteError(amazonException);
				}
				finally
				{
					Monitor.Exit(obj1);
				}
			}
		}

		public abstract void CloseFloatingBannerAd(Ad ad);

		public abstract Ad CreateFloatingBannerAd(Placement placement);

		public abstract Ad CreateInterstitialAd();

		public abstract void EnableGeoLocation(ShouldEnable shouldEnable);

		public abstract void EnableLogging(ShouldEnable shouldEnable);

		public abstract void EnableTesting(ShouldEnable shouldEnable);

		public static void FireEvent(string jsonMessage)
		{
			// 
			// Current member / type: System.Void com.amazon.mas.cpt.ads.AmazonMobileAdsImpl::FireEvent(System.String)
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

		public abstract IsReady IsInterstitialAdReady();

		public abstract LoadingStarted LoadAndShowFloatingBannerAd(Ad ad);

		public abstract LoadingStarted LoadInterstitialAd();

		public abstract void RegisterApplication();

		public abstract void RemoveAdCollapsedListener(AdCollapsedDelegate responseDelegate);

		public abstract void RemoveAdDismissedListener(AdDismissedDelegate responseDelegate);

		public abstract void RemoveAdExpandedListener(AdExpandedDelegate responseDelegate);

		public abstract void RemoveAdFailedToLoadListener(AdFailedToLoadDelegate responseDelegate);

		public abstract void RemoveAdLoadedListener(AdLoadedDelegate responseDelegate);

		public abstract void RemoveAdResizedListener(AdResizedDelegate responseDelegate);

		public abstract void SetApplicationKey(ApplicationKey applicationKey);

		public abstract AdShown ShowInterstitialAd();

		public abstract void UnityFireEvent(string jsonMessage);

		private abstract class AmazonMobileAdsBase : AmazonMobileAdsImpl
		{
			private readonly static object startLock;

			private static volatile bool startCalled;

			static AmazonMobileAdsBase()
			{
				AmazonMobileAdsImpl.AmazonMobileAdsBase.startLock = new object();
				AmazonMobileAdsImpl.AmazonMobileAdsBase.startCalled = false;
			}

			public AmazonMobileAdsBase()
			{
				AmazonMobileAdsImpl.logger = new AmazonLogger(base.GetType().Name);
			}

			public override void AddAdCollapsedListener(AdCollapsedDelegate responseDelegate)
			{
				this.Start();
				string str = "adCollapsed";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new AdCollapsedDelegator(responseDelegate)
						};
						AmazonMobileAdsImpl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonMobileAdsImpl.eventListeners[str].Add(new AdCollapsedDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void AddAdDismissedListener(AdDismissedDelegate responseDelegate)
			{
				this.Start();
				string str = "adDismissed";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new AdDismissedDelegator(responseDelegate)
						};
						AmazonMobileAdsImpl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonMobileAdsImpl.eventListeners[str].Add(new AdDismissedDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void AddAdExpandedListener(AdExpandedDelegate responseDelegate)
			{
				this.Start();
				string str = "adExpanded";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new AdExpandedDelegator(responseDelegate)
						};
						AmazonMobileAdsImpl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonMobileAdsImpl.eventListeners[str].Add(new AdExpandedDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void AddAdFailedToLoadListener(AdFailedToLoadDelegate responseDelegate)
			{
				this.Start();
				string str = "adFailedToLoad";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new AdFailedToLoadDelegator(responseDelegate)
						};
						AmazonMobileAdsImpl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonMobileAdsImpl.eventListeners[str].Add(new AdFailedToLoadDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void AddAdLoadedListener(AdLoadedDelegate responseDelegate)
			{
				this.Start();
				string str = "adLoaded";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new AdLoadedDelegator(responseDelegate)
						};
						AmazonMobileAdsImpl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonMobileAdsImpl.eventListeners[str].Add(new AdLoadedDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void AddAdResizedListener(AdResizedDelegate responseDelegate)
			{
				this.Start();
				string str = "adResized";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (!AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						List<IDelegator> delegators = new List<IDelegator>()
						{
							new AdResizedDelegator(responseDelegate)
						};
						AmazonMobileAdsImpl.eventListeners.Add(str, delegators);
					}
					else
					{
						AmazonMobileAdsImpl.eventListeners[str].Add(new AdResizedDelegator(responseDelegate));
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override IsEqual AreAdsEqual(AdPair adPair)
			{
				this.Start();
				return IsEqual.CreateFromJson(this.AreAdsEqualJson(adPair.ToJson()));
			}

			private string AreAdsEqualJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeAreAdsEqualJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override void CloseFloatingBannerAd(Ad ad)
			{
				this.Start();
				Jsonable.CheckForErrors(Json.Deserialize(this.CloseFloatingBannerAdJson(ad.ToJson())) as Dictionary<string, object>);
			}

			private string CloseFloatingBannerAdJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeCloseFloatingBannerAdJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override Ad CreateFloatingBannerAd(Placement placement)
			{
				this.Start();
				return Ad.CreateFromJson(this.CreateFloatingBannerAdJson(placement.ToJson()));
			}

			private string CreateFloatingBannerAdJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeCreateFloatingBannerAdJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override Ad CreateInterstitialAd()
			{
				this.Start();
				return Ad.CreateFromJson(this.CreateInterstitialAdJson("{}"));
			}

			private string CreateInterstitialAdJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeCreateInterstitialAdJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override void EnableGeoLocation(ShouldEnable shouldEnable)
			{
				this.Start();
				Jsonable.CheckForErrors(Json.Deserialize(this.EnableGeoLocationJson(shouldEnable.ToJson())) as Dictionary<string, object>);
			}

			private string EnableGeoLocationJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeEnableGeoLocationJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override void EnableLogging(ShouldEnable shouldEnable)
			{
				this.Start();
				Jsonable.CheckForErrors(Json.Deserialize(this.EnableLoggingJson(shouldEnable.ToJson())) as Dictionary<string, object>);
			}

			private string EnableLoggingJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeEnableLoggingJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override void EnableTesting(ShouldEnable shouldEnable)
			{
				this.Start();
				Jsonable.CheckForErrors(Json.Deserialize(this.EnableTestingJson(shouldEnable.ToJson())) as Dictionary<string, object>);
			}

			private string EnableTestingJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeEnableTestingJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			protected abstract void Init();

			public override IsReady IsInterstitialAdReady()
			{
				this.Start();
				return IsReady.CreateFromJson(this.IsInterstitialAdReadyJson("{}"));
			}

			private string IsInterstitialAdReadyJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeIsInterstitialAdReadyJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override LoadingStarted LoadAndShowFloatingBannerAd(Ad ad)
			{
				this.Start();
				return LoadingStarted.CreateFromJson(this.LoadAndShowFloatingBannerAdJson(ad.ToJson()));
			}

			private string LoadAndShowFloatingBannerAdJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeLoadAndShowFloatingBannerAdJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override LoadingStarted LoadInterstitialAd()
			{
				this.Start();
				return LoadingStarted.CreateFromJson(this.LoadInterstitialAdJson("{}"));
			}

			private string LoadInterstitialAdJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeLoadInterstitialAdJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			protected abstract string NativeAreAdsEqualJson(string jsonMessage);

			protected abstract string NativeCloseFloatingBannerAdJson(string jsonMessage);

			protected abstract string NativeCreateFloatingBannerAdJson(string jsonMessage);

			protected abstract string NativeCreateInterstitialAdJson(string jsonMessage);

			protected abstract string NativeEnableGeoLocationJson(string jsonMessage);

			protected abstract string NativeEnableLoggingJson(string jsonMessage);

			protected abstract string NativeEnableTestingJson(string jsonMessage);

			protected abstract string NativeIsInterstitialAdReadyJson(string jsonMessage);

			protected abstract string NativeLoadAndShowFloatingBannerAdJson(string jsonMessage);

			protected abstract string NativeLoadInterstitialAdJson(string jsonMessage);

			protected abstract string NativeRegisterApplicationJson(string jsonMessage);

			protected abstract string NativeSetApplicationKeyJson(string jsonMessage);

			protected abstract string NativeShowInterstitialAdJson(string jsonMessage);

			public override void RegisterApplication()
			{
				this.Start();
				Jsonable.CheckForErrors(Json.Deserialize(this.RegisterApplicationJson("{}")) as Dictionary<string, object>);
			}

			private string RegisterApplicationJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeRegisterApplicationJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			protected abstract void RegisterCallback();

			protected abstract void RegisterCrossPlatformTool();

			protected abstract void RegisterEventListener();

			public override void RemoveAdCollapsedListener(AdCollapsedDelegate responseDelegate)
			{
				this.Start();
				string str = "adCollapsed";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						foreach (AdCollapsedDelegator item in AmazonMobileAdsImpl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonMobileAdsImpl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void RemoveAdDismissedListener(AdDismissedDelegate responseDelegate)
			{
				this.Start();
				string str = "adDismissed";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						foreach (AdDismissedDelegator item in AmazonMobileAdsImpl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonMobileAdsImpl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void RemoveAdExpandedListener(AdExpandedDelegate responseDelegate)
			{
				this.Start();
				string str = "adExpanded";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						foreach (AdExpandedDelegator item in AmazonMobileAdsImpl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonMobileAdsImpl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void RemoveAdFailedToLoadListener(AdFailedToLoadDelegate responseDelegate)
			{
				this.Start();
				string str = "adFailedToLoad";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						foreach (AdFailedToLoadDelegator item in AmazonMobileAdsImpl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonMobileAdsImpl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void RemoveAdLoadedListener(AdLoadedDelegate responseDelegate)
			{
				this.Start();
				string str = "adLoaded";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						foreach (AdLoadedDelegator item in AmazonMobileAdsImpl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonMobileAdsImpl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void RemoveAdResizedListener(AdResizedDelegate responseDelegate)
			{
				this.Start();
				string str = "adResized";
				object obj = AmazonMobileAdsImpl.eventLock;
				Monitor.Enter(obj);
				try
				{
					if (AmazonMobileAdsImpl.eventListeners.ContainsKey(str))
					{
						foreach (AdResizedDelegator item in AmazonMobileAdsImpl.eventListeners[str])
						{
							if (item.responseDelegate != responseDelegate)
							{
								continue;
							}
							AmazonMobileAdsImpl.eventListeners[str].Remove(item);
							return;
						}
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			public override void SetApplicationKey(ApplicationKey applicationKey)
			{
				this.Start();
				Jsonable.CheckForErrors(Json.Deserialize(this.SetApplicationKeyJson(applicationKey.ToJson())) as Dictionary<string, object>);
			}

			private string SetApplicationKeyJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeSetApplicationKeyJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			public override AdShown ShowInterstitialAd()
			{
				this.Start();
				return AdShown.CreateFromJson(this.ShowInterstitialAdJson("{}"));
			}

			private string ShowInterstitialAdJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string str = this.NativeShowInterstitialAdJson(jsonMessage);
				stopwatch.Stop();
				AmazonMobileAdsImpl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return str;
			}

			protected void Start()
			{
				// 
				// Current member / type: System.Void com.amazon.mas.cpt.ads.AmazonMobileAdsImpl/AmazonMobileAdsBase::Start()
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
				AmazonMobileAdsImpl.FireEvent(jsonMessage);
			}
		}

		private class AmazonMobileAdsDefault : AmazonMobileAdsImpl.AmazonMobileAdsBase
		{
			public AmazonMobileAdsDefault()
			{
			}

			protected override void Init()
			{
			}

			protected override string NativeAreAdsEqualJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeCloseFloatingBannerAdJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeCreateFloatingBannerAdJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeCreateInterstitialAdJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeEnableGeoLocationJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeEnableLoggingJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeEnableTestingJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeIsInterstitialAdReadyJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeLoadAndShowFloatingBannerAdJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeLoadInterstitialAdJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeRegisterApplicationJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeSetApplicationKeyJson(string jsonMessage)
			{
				return "{}";
			}

			protected override string NativeShowInterstitialAdJson(string jsonMessage)
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

		private abstract class AmazonMobileAdsDelegatesBase : AmazonMobileAdsImpl.AmazonMobileAdsBase
		{
			private const string CrossPlatformTool = "XAMARIN";

			protected AmazonMobileAdsImpl.CallbackDelegate callbackDelegate;

			protected AmazonMobileAdsImpl.CallbackDelegate eventDelegate;

			protected AmazonMobileAdsDelegatesBase()
			{
			}

			protected override void Init()
			{
				this.NativeInit();
			}

			protected abstract void NativeInit();

			protected abstract void NativeRegisterCallback(AmazonMobileAdsImpl.CallbackDelegate callback);

			protected abstract void NativeRegisterCrossPlatformTool(string crossPlatformTool);

			protected abstract void NativeRegisterEventListener(AmazonMobileAdsImpl.CallbackDelegate callback);

			protected override void RegisterCallback()
			{
				this.callbackDelegate = new AmazonMobileAdsImpl.CallbackDelegate(AmazonMobileAdsImpl.callback);
				this.NativeRegisterCallback(this.callbackDelegate);
			}

			protected override void RegisterCrossPlatformTool()
			{
				this.NativeRegisterCrossPlatformTool("XAMARIN");
			}

			protected override void RegisterEventListener()
			{
				this.eventDelegate = new AmazonMobileAdsImpl.CallbackDelegate(AmazonMobileAdsImpl.FireEvent);
				this.NativeRegisterEventListener(this.eventDelegate);
			}

			public override void UnityFireEvent(string jsonMessage)
			{
				throw new NotSupportedException("UnityFireEvent is not supported");
			}
		}

		private class AmazonMobileAdsUnityAndroid : AmazonMobileAdsImpl.AmazonMobileAdsUnityBase
		{
			public new static AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid Instance
			{
				get
				{
					return AmazonMobileAdsImpl.AmazonMobileAdsUnityBase.getInstance<AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid>();
				}
			}

			public AmazonMobileAdsUnityAndroid()
			{
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeAreAdsEqualJson(string jsonMessage);

			protected override string NativeAreAdsEqualJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeAreAdsEqualJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeCloseFloatingBannerAdJson(string jsonMessage);

			protected override string NativeCloseFloatingBannerAdJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeCloseFloatingBannerAdJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeCreateFloatingBannerAdJson(string jsonMessage);

			protected override string NativeCreateFloatingBannerAdJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeCreateFloatingBannerAdJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeCreateInterstitialAdJson(string jsonMessage);

			protected override string NativeCreateInterstitialAdJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeCreateInterstitialAdJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeEnableGeoLocationJson(string jsonMessage);

			protected override string NativeEnableGeoLocationJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeEnableGeoLocationJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeEnableLoggingJson(string jsonMessage);

			protected override string NativeEnableLoggingJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeEnableLoggingJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeEnableTestingJson(string jsonMessage);

			protected override string NativeEnableTestingJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeEnableTestingJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeInit();

			protected override void NativeInit()
			{
				AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeInit();
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeIsInterstitialAdReadyJson(string jsonMessage);

			protected override string NativeIsInterstitialAdReadyJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeIsInterstitialAdReadyJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeLoadAndShowFloatingBannerAdJson(string jsonMessage);

			protected override string NativeLoadAndShowFloatingBannerAdJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeLoadAndShowFloatingBannerAdJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeLoadInterstitialAdJson(string jsonMessage);

			protected override string NativeLoadInterstitialAdJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeLoadInterstitialAdJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeRegisterApplicationJson(string jsonMessage);

			protected override string NativeRegisterApplicationJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeRegisterApplicationJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeRegisterCallbackGameObject(string name);

			protected override void NativeRegisterCrossPlatformTool(string crossPlatformTool)
			{
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeSetApplicationKeyJson(string jsonMessage);

			protected override string NativeSetApplicationKeyJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeSetApplicationKeyJson(jsonMessage);
			}

			[DllImport("AmazonMobileAdsBridge", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern string nativeShowInterstitialAdJson(string jsonMessage);

			protected override string NativeShowInterstitialAdJson(string jsonMessage)
			{
				return AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeShowInterstitialAdJson(jsonMessage);
			}

			protected override void RegisterCallback()
			{
				AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeRegisterCallbackGameObject(base.gameObject.name);
			}

			protected override void RegisterEventListener()
			{
				AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.nativeRegisterCallbackGameObject(base.gameObject.name);
			}
		}

		private abstract class AmazonMobileAdsUnityBase : AmazonMobileAdsImpl.AmazonMobileAdsBase
		{
			private const string CrossPlatformTool = "UNITY";

			private static AmazonMobileAdsImpl.AmazonMobileAdsUnityBase instance;

			private static Type instanceType;

			private static volatile bool quit;

			private static object initLock;

			static AmazonMobileAdsUnityBase()
			{
				AmazonMobileAdsImpl.AmazonMobileAdsUnityBase.quit = false;
				AmazonMobileAdsImpl.AmazonMobileAdsUnityBase.initLock = new object();
			}

			protected AmazonMobileAdsUnityBase()
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
			where T : AmazonMobileAdsImpl.AmazonMobileAdsUnityBase
			{
				// 
				// Current member / type: T com.amazon.mas.cpt.ads.AmazonMobileAdsImpl/AmazonMobileAdsUnityBase::getInstance()
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
				AmazonMobileAdsImpl.AmazonMobileAdsUnityBase.quit = true;
			}

			protected override void RegisterCrossPlatformTool()
			{
				this.NativeRegisterCrossPlatformTool("UNITY");
			}
		}

		private class Builder
		{
			internal readonly static IAmazonMobileAds instance;

			static Builder()
			{
				AmazonMobileAdsImpl.Builder.instance = AmazonMobileAdsImpl.AmazonMobileAdsUnityAndroid.Instance;
			}

			public Builder()
			{
			}
		}

		protected delegate void CallbackDelegate(string jsonMessage);
	}
}