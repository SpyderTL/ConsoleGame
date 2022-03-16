using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpg;

namespace ConsoleGame
{
	internal static class BattleMenu
	{
		internal static int Character;
		internal static int ActivityType;
		internal static int Activity;

		internal static MenuItem[] Items = new MenuItem[0];

		internal static void Update()
		{
			if (Character == -1)
				Items = new MenuItem[0];
			else
			{
				switch (ActivityType)
				{
					case -1:
						Items = new MenuItem[]
						{
							new MenuItem { Text = "Attack", ActivityType = Battle.ActivityType.Attack, Activity = -1, Option = -1 },
							new MenuItem { Text = "Ability", ActivityType = Battle.ActivityType.Ability, Activity = -1, Option = -1 },
							new MenuItem { Text = "Spell", ActivityType = Battle.ActivityType.Spell, Activity = -1, Option = -1 },
							new MenuItem { Text = "Item", ActivityType = Battle.ActivityType.Item, Activity = -1, Option = -1 },
							new MenuItem { Text = "Run", ActivityType = Battle.ActivityType.Run, Activity = -1, Option = Enumerable.Range(0, Battle.Options[Character].Length).First(x => Battle.Options[Character][x].Type == Battle.ActivityType.Run) },
						};
						break;

					case (int)Battle.ActivityType.Attack:
						Items = Enumerable.Range(0, Battle.Options[Character].Length)
							.Where(x => Battle.Options[Character][x].Type == Battle.ActivityType.Attack)
							.Select(x => new MenuItem
							{
								Text = Battle.Options[Character][x].TargetType == Battle.TargetType.Enemy ? Battle.Enemies[Battle.Options[Character][x].Target].Name : Battle.Allies[Battle.Options[Character][x].Target].Name,
								ActivityType = Battle.ActivityType.Attack,
								Activity = -1,
								Option = x
							}).ToArray();
						break;

					case (int)Battle.ActivityType.Spell:
						if (Activity == -1)
						{
							Items = Enumerable.Range(0, Battle.Options[Character].Length)
								.Where(x => Battle.Options[Character][x].Type == Battle.ActivityType.Spell)
								.GroupBy(x => Battle.Options[Character][x].Value)
								.Select(x => new MenuItem
								{
									Text = x.Key.ToString(),
									ActivityType = Battle.ActivityType.Spell,
									Activity = x.Key,
									Option = -1
								}).ToArray();
						}
						else
						{
							Items = Enumerable.Range(0, Battle.Options[Character].Length)
								.Where(x => Battle.Options[Character][x].Type == Battle.ActivityType.Spell &&
									Battle.Options[Character][x].Value == Activity)
								.Select(x => new MenuItem
								{
									Text = Battle.Options[Character][x].TargetType == Battle.TargetType.Enemy ? Battle.Enemies[Battle.Options[Character][x].Target].Name : Battle.Allies[Battle.Options[Character][x].Target].Name,
									ActivityType = Battle.ActivityType.Spell,
									Activity = Battle.Options[Character][x].Value,
									Option = x
								}).ToArray();
						}
						break;
				}
			}
		}

		internal static void Select(int item)
		{
			if (item >= Items.Length)
				return;

			if (Items[item].Option != -1)
			{
				Battle.Actions[Character] = Items[item].Option;

				for (int ally = 0; ally <= Party.Characters.Length; ally++)
				{
					if (ally == Party.Characters.Length)
					{
						Character = -1;
						break;
					}

					if (ally <= Character)
						continue;

					if (Battle.Options[ally].Length == 0)
						continue;

					Character = ally;
					break;
				}

				ActivityType = -1;
				Activity = -1;
			}
			else
			{
				ActivityType = (int)Items[item].ActivityType;
				Activity = Items[item].Activity;
			}

			Update();

			BattleScreen.Draw();
			Screen.Update();

			RpgBattle.WriteActions();
		}

		internal struct MenuItem
		{
			internal string Text;
			internal Battle.ActivityType ActivityType;
			internal int Activity;
			internal int Option;
		}
	}
}
