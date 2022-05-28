using System.Collections.Generic;
using Holoville.HOTween.Plugins.Core;

namespace Holoville.HOTween.Core
{
	internal class OverwriteManager
	{
		internal bool enabled;

		internal bool logWarnings;

		private readonly List<Tweener> runningTweens;

		public OverwriteManager()
		{
			runningTweens = new List<Tweener>();
		}

		public void AddTween(Tweener p_tween)
		{
			if (enabled)
			{
				List<ABSTweenPlugin> plugins = p_tween.plugins;
				int num = runningTweens.get_Count() - 1;
				int count = plugins.get_Count();
				for (int num2 = num; num2 > -1; num2--)
				{
					Tweener tweener = runningTweens.get_Item(num2);
					List<ABSTweenPlugin> plugins2 = tweener.plugins;
					int num3 = plugins2.get_Count();
					if (tweener.target == p_tween.target)
					{
						for (int i = 0; i < count; i++)
						{
							ABSTweenPlugin aBSTweenPlugin = plugins.get_Item(i);
							for (int num4 = num3 - 1; num4 > -1; num4--)
							{
								ABSTweenPlugin aBSTweenPlugin2 = plugins2.get_Item(num4);
								if (aBSTweenPlugin2.propName == aBSTweenPlugin.propName && (aBSTweenPlugin.pluginId == -1 || aBSTweenPlugin2.pluginId == -1 || aBSTweenPlugin2.pluginId == aBSTweenPlugin.pluginId))
								{
									if (tweener.isSequenced && p_tween.isSequenced && tweener.contSequence == p_tween.contSequence)
									{
										goto end_IL_0258;
									}
									if (!tweener._isPaused && (!tweener.isSequenced || !tweener.isComplete))
									{
										plugins2.RemoveAt(num4);
										num3--;
										if (HOTween.isEditor && HOTween.warningLevel == WarningLevel.Verbose)
										{
											string text = ((object)((object)aBSTweenPlugin).GetType()).ToString();
											text = text.Substring(text.LastIndexOf(".") + 1);
											string text2 = ((object)((object)aBSTweenPlugin2).GetType()).ToString();
											text2 = text2.Substring(text2.LastIndexOf(".") + 1);
											if (logWarnings)
											{
												TweenWarning.Log(string.Concat(new object[7] { text, " is overwriting ", text2, " for ", tweener.target, ".", aBSTweenPlugin2.propName }));
											}
										}
										if (num3 == 0)
										{
											if (tweener.isSequenced)
											{
												tweener.contSequence.Remove(tweener);
											}
											runningTweens.RemoveAt(num2);
											tweener.Kill(false);
										}
										if (tweener.onPluginOverwritten != null)
										{
											tweener.onPluginOverwritten();
										}
										else if (tweener.onPluginOverwrittenWParms != null)
										{
											tweener.onPluginOverwrittenWParms(new TweenEvent(tweener, tweener.onPluginOverwrittenParms));
										}
										if (tweener.destroyed)
										{
											goto end_IL_0258;
										}
									}
								}
							}
							continue;
							end_IL_0258:
							break;
						}
					}
				}
			}
			runningTweens.Add(p_tween);
		}

		public void RemoveTween(Tweener p_tween)
		{
			int count = runningTweens.get_Count();
			for (int i = 0; i < count; i++)
			{
				if (runningTweens.get_Item(i) == p_tween)
				{
					runningTweens.RemoveAt(i);
					break;
				}
			}
		}
	}
}
