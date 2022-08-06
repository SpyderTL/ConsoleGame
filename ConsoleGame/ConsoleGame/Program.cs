using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleGame
{
	public static class Program
	{
		static void Main(string[] args)
		{
			RpgGame.BattleData.Load();
			RpgGame.ClassData.Load();

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
								RpgGame.Party.Characters = new[] { "Alpha", "Beta", "Gamma", "Delta" }
									.Select(x => new RpgGame.Party.Character
									{
										Name = x,
										Type = RpgGame.Party.CharacterType.Fighter,
										Health = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Health,
										MaxHealth = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Health,
										Strength = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Strength,
										Agility = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Agility,
										Intelligence = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Intelligence,
										Vitality = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Vitality,
										Luck = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Luck,
										Damage = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Damage,
										Accuracy = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Accuracy,
										Evade = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Evade,
										//MagicDefense = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].MagicDefense

										Hits = (RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Accuracy >> 5) + 1,
										Level = 1,
										Experience = 0,
										Absorb = 0
									}).ToArray();

								//RpgGame.Party.Characters[0] = new RpgGame.Party.Character { Name = "Alpha", Type = RpgGame.Party.CharacterType.Fighter, Health = 100, MaxHealth = 100, Power = 100, MaxPower = 100, Hits = 1 };
								//RpgGame.Party.Characters[1] = new RpgGame.Party.Character { Name = "Beta", Type = RpgGame.Party.CharacterType.Fighter, Health = 100, MaxHealth = 100, Power = 100, MaxPower = 100, Hits = 1 };
								//RpgGame.Party.Characters[2] = new RpgGame.Party.Character { Name = "Gamma", Type = RpgGame.Party.CharacterType.Fighter, Health = 100, MaxHealth = 100, Power = 100, MaxPower = 100, Hits = 1 };
								//RpgGame.Party.Characters[3] = new RpgGame.Party.Character { Name = "Delta", Type = RpgGame.Party.CharacterType.Fighter, Health = 100, MaxHealth = 100, Power = 100, MaxPower = 100, Hits = 1 };

								RpgGame.PartyWorld.X = 153;
								RpgGame.PartyWorld.Y = 165;

								RpgGame.WorldData.Load();

								RpgGame.PartyWorld.Refresh();

								RpgParty.Refresh();

								Game.Mode = Game.GameMode.World;
								break;

							case 1:
								// Battle
								RpgGame.Party.Characters = new[] { "Alpha", "Beta", "Gamma", "Delta" }
									.Select(x => new RpgGame.Party.Character
									{
										Name = x,
										Type = RpgGame.Party.CharacterType.Fighter,
										Health = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Health + 1000,
										MaxHealth = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Health + 1000,
										Strength = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Strength,
										Agility = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Agility,
										Intelligence = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Intelligence,
										Vitality = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Vitality,
										Luck = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Luck,
										Damage = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Damage,
										Accuracy = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Accuracy,
										Evade = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Evade,
										//MagicDefense = RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].MagicDefense

										Hits = (RpgGame.ClassData.Classes[(int)RpgGame.Party.CharacterType.Fighter].Accuracy >> 5) + 1,
										Level = 1,
										Experience = 0,
										Absorb = 0
									}).ToArray();

								RpgGame.PartyWorld.X = 153;
								RpgGame.PartyWorld.Y = 165;

								RpgGame.WorldData.Load();

								RpgGame.PartyWorld.Refresh();

								RpgParty.Refresh();

								var spellCasters = Enumerable.Range(0, 128)
									.Where(x => RpgGame.Battle.Formations[x]
										.Any(y => y.Minimum != 0 &&
											RpgGame.Battle.EnemyTypes[y.Type].Logic != 255))
									.ToArray();

								var random = new Random();

								RpgGame.BattleData.LoadFormation(spellCasters[random.Next(0, spellCasters.Length)], false);
								//RpgGame.BattleData.LoadFormation(random.Next(0x6e, 0x6f), false);

								RpgBattle.ReadData();
								RpgBattle.ReadCharacters();

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
						RpgWorld.Load();

						InputRpgWorld.Enable();
						RpgWorld.Enable();

						MapScreen.Show();

						RpgWorld.Disable();
						InputRpgWorld.Disable();
						break;

					case Game.GameMode.Map:
						RpgMap.Load();

						InputRpgMap.Enable();
						RpgMap.Enable();

						MapScreen.Show();

						RpgMap.Disable();
						InputRpgMap.Disable();
						break;

					case Game.GameMode.Battle:
						RpgBattle.ReadData();
						RpgBattle.ReadCharacters();

						InputBattle.Enable();
						RpgBattle.Enable();

						RpgGame.Battle.Enable();

						BattleScreen.Show();

						RpgGame.Battle.Disable();

						RpgBattle.Disable();
						InputBattle.Disable();
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
