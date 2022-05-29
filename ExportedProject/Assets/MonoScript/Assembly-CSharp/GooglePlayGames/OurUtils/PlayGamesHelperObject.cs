using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	public class PlayGamesHelperObject : MonoBehaviour
	{
		private static PlayGamesHelperObject instance;

		private static bool sIsDummy;

		private static List<Action> sQueue;

		private List<Action> localQueue = new List<Action>();

		private static volatile bool sQueueEmpty;

		private static List<Action<bool>> sPauseCallbackList;

		private static List<Action<bool>> sFocusCallbackList;

		static PlayGamesHelperObject()
		{
			PlayGamesHelperObject.instance = null;
			PlayGamesHelperObject.sIsDummy = false;
			PlayGamesHelperObject.sQueue = new List<Action>();
			PlayGamesHelperObject.sQueueEmpty = true;
			PlayGamesHelperObject.sPauseCallbackList = new List<Action<bool>>();
			PlayGamesHelperObject.sFocusCallbackList = new List<Action<bool>>();
		}

		public PlayGamesHelperObject()
		{
		}

		public static void AddFocusCallback(Action<bool> callback)
		{
			if (!PlayGamesHelperObject.sFocusCallbackList.Contains(callback))
			{
				PlayGamesHelperObject.sFocusCallbackList.Add(callback);
			}
		}

		public static void AddPauseCallback(Action<bool> callback)
		{
			if (!PlayGamesHelperObject.sPauseCallbackList.Contains(callback))
			{
				PlayGamesHelperObject.sPauseCallbackList.Add(callback);
			}
		}

		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public static void CreateObject()
		{
			if (PlayGamesHelperObject.instance != null)
			{
				return;
			}
			if (!Application.isPlaying)
			{
				PlayGamesHelperObject.instance = new PlayGamesHelperObject();
				PlayGamesHelperObject.sIsDummy = true;
			}
			else
			{
				GameObject gameObject = new GameObject("PlayGames_QueueRunner");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				PlayGamesHelperObject.instance = gameObject.AddComponent<PlayGamesHelperObject>();
			}
		}

		public void OnApplicationFocus(bool focused)
		{
			foreach (Action<bool> action in PlayGamesHelperObject.sFocusCallbackList)
			{
				try
				{
					action(focused);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Debug.LogError(string.Concat("Exception in OnApplicationFocus:", exception.Message, "\n", exception.StackTrace));
				}
			}
		}

		public void OnApplicationPause(bool paused)
		{
			foreach (Action<bool> action in PlayGamesHelperObject.sPauseCallbackList)
			{
				try
				{
					action(paused);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Debug.LogError(string.Concat("Exception in OnApplicationPause:", exception.Message, "\n", exception.StackTrace));
				}
			}
		}

		public void OnDisable()
		{
			if (PlayGamesHelperObject.instance == this)
			{
				PlayGamesHelperObject.instance = null;
			}
		}

		public static bool RemoveFocusCallback(Action<bool> callback)
		{
			return PlayGamesHelperObject.sFocusCallbackList.Remove(callback);
		}

		public static bool RemovePauseCallback(Action<bool> callback)
		{
			return PlayGamesHelperObject.sPauseCallbackList.Remove(callback);
		}

		public static void RunCoroutine(IEnumerator action)
		{
			if (PlayGamesHelperObject.instance != null)
			{
				PlayGamesHelperObject.RunOnGameThread(() => PlayGamesHelperObject.instance.StartCoroutine(action));
			}
		}

		public static void RunOnGameThread(Action action)
		{
			// 
			// Current member / type: System.Void GooglePlayGames.OurUtils.PlayGamesHelperObject::RunOnGameThread(System.Action)
			// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp.dll
			// 
			// Product version: 0.9.2.0
			// Exception in: System.Void RunOnGameThread(System.Action)
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

		public void Update()
		{
			if (PlayGamesHelperObject.sIsDummy || PlayGamesHelperObject.sQueueEmpty)
			{
				return;
			}
			this.localQueue.Clear();
			List<Action> actions = PlayGamesHelperObject.sQueue;
			Monitor.Enter(actions);
			try
			{
				this.localQueue.AddRange(PlayGamesHelperObject.sQueue);
				PlayGamesHelperObject.sQueue.Clear();
				PlayGamesHelperObject.sQueueEmpty = true;
			}
			finally
			{
				Monitor.Exit(actions);
			}
			for (int i = 0; i < this.localQueue.Count; i++)
			{
				this.localQueue[i]();
			}
		}
	}
}