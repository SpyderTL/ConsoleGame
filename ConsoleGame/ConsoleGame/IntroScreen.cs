using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	public static class IntroScreen
	{
		public static void Show()
		{
			Intro.Playing = true;

			var animation = AnimateText().GetEnumerator();

			while (Intro.Playing)
				animation.MoveNext();

			Screen.Clear();
			Screen.Update();
		}

		private static IEnumerable AnimateText()
		{
			Screen.Clear();
			Screen.Update();

			var start = Environment.TickCount;

			while (Environment.TickCount - start < 1000)
				yield return null;

			Screen.DrawString("Aftercast Games", 12, 12);
			Screen.Update();

			while (Environment.TickCount - start < 4000)
				yield return null;

			Screen.Clear();
			Screen.Update();

			while (Environment.TickCount - start < 6000)
				yield return null;

			Screen.DrawString("Presents", 16, 14);
			Screen.Update();

			while (Environment.TickCount - start < 10000)
				yield return null;

			Screen.Clear();
			Screen.Update();

			while (Environment.TickCount - start < 12000)
				yield return null;

			Screen.DrawString("SoulAge", 10, 10);
			Screen.Update();

			while (Environment.TickCount - start < 14000)
				yield return null;

			Screen.DrawString("Band Of Warriors", 12, 12);
			Screen.Update();

			while (Environment.TickCount - start < 16000)
				yield return null;

			Screen.DrawString("Universal Edition", 20, 24);
			Screen.Update();

			while (Environment.TickCount - start < 20000)
				yield return null;

			Screen.Clear();
			Screen.Update();

			while (Environment.TickCount - start < 22000)
				yield return null;

			Intro.Playing = false;
		}
	}
}