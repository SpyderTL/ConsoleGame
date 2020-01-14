using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
						new MenuItem { Text = "Attack", ActivityType = Battle.ActivityType.Attack, Option = -1 },
						new MenuItem { Text = "Special", ActivityType = Battle.ActivityType.Special, Option = -1 },
						new MenuItem { Text = "Magic", ActivityType = Battle.ActivityType.Magic, Option = -1 },
						new MenuItem { Text = "Item", ActivityType = Battle.ActivityType.Item, Option = -1 },
						new MenuItem { Text = "Run", ActivityType = Battle.ActivityType.Run, Option = Enumerable.Range(0, Battle.Options[Character].Length).First(x => Battle.Options[Character][x].Type == Battle.ActivityType.Run) },
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

					case (int)Battle.ActivityType.Magic:
						if (Activity == -1)
						{
							Items = Enumerable.Range(0, Battle.Options[Character].Length)
								.Where(x => Battle.Options[Character][x].Type == Battle.ActivityType.Magic)
								.GroupBy(x => Battle.Options[Character][x].Value)
								.Select(x => new MenuItem
								{
									Text = x.Key.ToString(),
									ActivityType = Battle.ActivityType.Magic,
									Activity = x.Key,
									Option = -1
								}).ToArray();
						}
						else
						{
							Items = Enumerable.Range(0, Battle.Options[Character].Length)
								.Where(x => Battle.Options[Character][x].Type == Battle.ActivityType.Magic &&
									Battle.Options[Character][x].Value == Activity)
								.Select(x => new MenuItem
								{
									Text = Battle.Options[Character][x].TargetType == Battle.TargetType.Enemy ? Battle.Enemies[Battle.Options[Character][x].Target].Name : Battle.Allies[Battle.Options[Character][x].Target].Name,
									ActivityType = Battle.ActivityType.Magic,
									Activity = Battle.Options[Character][x].Value,
									Option = x
								}).ToArray();
						}
						break;
				}
			}
		}

		internal static void Select(int index)
		{
			if (index >= Items.Length)
				return;

			if (Items[index].Option != -1)
			{
				Battle.Actions[Character] = Items[index].Option;

				RpgBattle.UpdateActions();

				var remaining = Enumerable.Range(0, Battle.Actions.Length).Where(x => Battle.Actions[x] == -1).ToArray();

				if (remaining.Length == 0)
					Character = -1;
				else if (remaining.Any(x => x > Character))
					Character = remaining.First(x => x > Character);
				else
					Character = remaining.First();

				ActivityType = -1;
				Activity = -1;
			}
			else
			{
				ActivityType = (int)Items[index].ActivityType;
				Activity = Items[index].Activity;
			}

			Update();
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
