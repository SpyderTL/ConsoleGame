using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGame
{
	internal static class RpgParty
	{
		internal static void Refresh()
		{
			Party.Characters = RpgGame.Party.Characters.Select(x => new Party.Character
			{
				Name = x.Name,
				Health = x.Health,
				MaxHealth = x.MaxHealth,
				Power = x.Power,
				MaxPower = x.MaxPower
			}).ToArray();
		}
	}
}
