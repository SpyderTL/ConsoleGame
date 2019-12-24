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

						RpgGame.PartyWorld.X = 153;
						RpgGame.PartyWorld.Y = 165;

						RpgGame.DataWorld.Load();

						RpgGame.PartyWorld.Update();

						RpgWorld.Load();

						InputRpgWorld.Enable();
						RpgWorld.Enable();

						MapScreen.Show();

						RpgWorld.Disable();
						InputRpgWorld.Disable();
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
