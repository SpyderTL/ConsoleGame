using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class Game
	{
		internal static GameMode Mode;

		internal enum GameMode
		{
			Intro,
			Menu,
			New,
			Continue,
			Host,
			Join,
			World,
			Map,
			Battle
		}
	}
}
