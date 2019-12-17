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

						Map.X = 128;
						Map.Y = 128;

						RpgGame.DataMap.Load(1, 0x0000);

						var zones = new List<Map.Zone>();

						for (var row = 0; row < RpgGame.Map.Rows.Length; row++)
						{
							var column = 0;

							foreach (var segment in RpgGame.Map.Rows[row].Segments)
							{
								zones.Add(new Map.Zone { Top = row, Left = column, Bottom = row, Right = column + segment.Width - 1, Character = (char)(segment.Tile + 'A') });
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
	}
}
