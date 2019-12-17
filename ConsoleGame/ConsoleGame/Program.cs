using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleGame
{
	public static class Program
	{
		static void Main(string[] args)
		{
			Screen.Enable();

			KeyboardInput.Enable();

			Keyboard.Enable();

			while (Application.Running)
			{
				switch (Game.Mode)
				{
					case Game.GameMode.Intro:
						InputIntro.Enable();

						IntroScreen.Show();

						InputIntro.Disable();

						Game.Mode = Game.GameMode.Menu;
						break;

					case Game.GameMode.Menu:
						Menu.Items = new[]
						{
							"World",
							"Battle",
							"Continue",
							"New",
							"Host",
							"Connect",
							"Quit"
						};

						Menu.Current = 0;

						InputMenu.Enable();

						MenuScreen.Title = "Main Menu";

						MenuScreen.Descriptions = new[]
						{
							"Test world map",
							"Test battle.",
							"Continue an existing game.",
							"Start a new single player game.",
							"Host a game.",
							"Connect to a remote host.",
							"Exit the game."
						};

						MenuScreen.Show();

						InputMenu.Disable();

						switch (Menu.Current)
						{
							case 0:
								// World
								Game.Mode = Game.GameMode.World;
								break;

							case 1:
								// Battle
								Game.Mode = Game.GameMode.Battle;
								break;

							case 2:
								// Continue
								Game.Mode = Game.GameMode.Continue;
								break;

							case 3:
								// New
								Game.Mode = Game.GameMode.New;
								break;

							case 4:
								// Host
								Game.Mode = Game.GameMode.Host;
								break;

							case 5:
								// Connect
								Game.Mode = Game.GameMode.Continue;
								break;

							default:
								// Quit
								Application.Running = false;
								break;
						}
						break;

					case Game.GameMode.World:
						Party.Characters[0] = new Party.Character { Name = "Alpha", Health = 54, MaxHealth = 102, Power = 12, MaxPower = 88 };
						Party.Characters[1] = new Party.Character { Name = "Beta", Health = 54, MaxHealth = 102, Power = 12, MaxPower = 88 };
						Party.Characters[2] = new Party.Character { Name = "Gamma", Health = 54, MaxHealth = 102, Power = 12, MaxPower = 88 };
						Party.Characters[3] = new Party.Character { Name = "Delta", Health = 54, MaxHealth = 102, Power = 12, MaxPower = 88 };

						//Map.Zones = new Map.Zone[]
						//{
						//	new Map.Zone { Bottom = 255, Right = 255, Character = '_' },
						//	new Map.Zone { Top = 100, Left = 100, Bottom = 200, Right = 200, Character = ',' },
						//	new Map.Zone { Top = 100, Left = 120, Bottom = 200, Right = 140 }
						//};

						Map.X = 152;
						Map.Y = 164;

						RpgGame.DataMap.Load(1, 0x0000);

						var zones = new List<Map.Zone>();

						for (var row = 0; row < RpgGame.Map.Rows.Length; row++)
						{
							var column = 0;

							foreach (var segment in RpgGame.Map.Rows[row].Segments)
							{
								zones.Add(new Map.Zone { Top = row, Left = column, Bottom = row, Right = column + segment.Width - 1, Character = Tiles[segment.Tile], Description = Descriptions[segment.Tile] });
								column += segment.Width;
							}
						}

						Map.Zones = zones.ToArray();

						InputWorld.Enable();

						MapScreen.Show();

						InputWorld.Disable();
						break;

					default:
						Game.Mode = Game.GameMode.Intro;
						break;
				}
			}

			Screen.Disable();

			Environment.Exit(0);
		}

		private static char[] Tiles = new char[]
		{
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',

			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'\0',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',

			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',

			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',

			'.',
			'.',
			'.',
			'.',
			'\0',
			'.',
			'\0',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',

			'\0',
			'\0',
			'.',
			'.',
			',',
			'~',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',

			',',
			',',
			'~',
			'~',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',

			',',
			',',
			'~',
			'~',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'.',
			'\0',
		};

		private static string[] Descriptions = new string[]
		{
			"Grass",
			"Castle 1 - Lower Left",
			"Castle 1 - Lower Right",
			"Forest - Top Left",
			"Forest - Top",
			"Forest - Top Right",
			"Grass - Bottom Right",
			"Grass - Bottom",
			"Grass - Bottom Left",
			"Castle 1 - Top Left",
			"Castle 1 - Top Right",
			"Castle 2 - Top Left",
			"Castle 2 - Top Right",
			"Tower of Illusion - Top",
			"Grotto 1",
			"Dock - Vertical Left",
			"Mountain - Top Left",
			"Mountain - Top",
			"Mountain - Top Right",
			"Forest - Left",
			"Forest - Bottom",
			"Forest - Right",
			"Grass - Right",
			"Ocean",
			"Grass - Left",
			"Castle 1 - Middle Left",
			"Castle 1 - Middle Right",
			"Castle 2 - Bottom Left",
			"Castle 2 - Bottom Right",
			"Tower of Illusion - Bottom",
			"Tower of Illusion - Shadow",
			"Dock - Vertical Right",
			"Mountain - Left",
			"Mountain",
			"Mountain - Right",
			"Forest - Bottom Left",
			"Forest - Bottom",
			"Forest - Bottom Right",
			"Grass - Top Right",
			"Grass - Top",
			"Grass - Top Left",
			"Castle 2 - Bottom Left",
			"Castle 2 - Bottom Right",
			"Grotto 2",
			"Village Fence - Top Left",
			"Village Fence - Top",
			"Village Fence - Top Right",
			"Grotto 3",
			"Mountain - Bottom Left",
			"Mountain - Bottom",
			"Grotto 4",
			"Mountain - Bottom Right",
			"Grotto 5",
			"Grotto 6",
			"Desert",
			"Desert",
			"Castle 2 - Bottom Left",
			"Castle 2 - Bottom Right",
			"Grotto 7",
			"Village Fence - Top Left Exterior",
			"Village Fence - Top Left Interior",
			"Village Area (gray)",
			"Village Fence - Top Right Interior",
			"Village Fence - Top Right Exterior",
			"River - Top Left",
			"River - Top Right",
			"Desert - Top Left",
			"Desert - Top Right",
			"River",
			"Desert",
			"Waterfall",
			"Castle Ruins - Top Left",
			"Castle Ruins - Top Right",
			"Village 1",
			"Village 2",
			"Village Fence - Middle Left",
			"Village 3",
			"Village 4",
			"Village 5",
			"Village Fence - Middle Right",
			"River - Bottom Left",
			"River - Bottom Right",
			"Desert - Bottom Left",
			"Desert - Bottom Right",
			"Tall Grass",
			"Swamp",
			"Castle Ruins - Bottom Left",
			"Castle Ruins - Bottom Middle Left",
			"Castle Ruins - Bottom Middle Right",
			"Castle Ruins - Bottom Right",
			"Village 6",
			"Village Fence - Middle Left 2",
			"Village Fence - Bottom Left",
			"Village 7",
			"Village Fence - Bottom Right",
			"Village Fence - Middle Right 2",
			"Tall Grass - Top Left",
			"Tall Grass - Top Right",
			"Swamp - Top Left",
			"Swamp - Top Right",
			"Volcano - Top Left",
			"Volcano - Top Right",
			"Hole 1",
			"Hole 2",
			"Hole 3",
			"Hole 4",
			"Hole 5",
			"Village Fence - Middle Left 3",
			"Hole 6",
			"Village 8",
			"Hole 7",
			"Village Fence - Middle Right 3",
			"Tall Grass - Bottom Left",
			"Tall Grass - Bottom Right",
			"Swamp - Bottom Left",
			"Swamp - Bottom Right",
			"Volcano - Bottom Left",
			"Volcano - Bottom Right",
			"Grass",
			"Dock - Bottom Right",
			"Dock - Bottom",
			"Dock - Bottom Left",
			"Dock - Top Left",
			"Village Fence - Bottom Left",
			"Village Wall 1",
			"Village Wall 2",
			"Ocean",
			"Unknown"
		};
	}
}
