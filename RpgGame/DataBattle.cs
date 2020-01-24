using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RpgGame
{
	public static class DataBattle
	{
		public static void Load()
		{
			using (var reader = Data.Reader())
			{
				reader.BaseStream.Position = Data.Address(0x0C, 0x8520);

				for (var enemy = 0; enemy < 127; enemy++)
				{
					Battle.EnemyTypes[enemy].Experience = reader.ReadUInt16();
					Battle.EnemyTypes[enemy].Gold = reader.ReadUInt16();
					Battle.EnemyTypes[enemy].Health = reader.ReadUInt16();
					Battle.EnemyTypes[enemy].Morale = reader.ReadByte();
					Battle.EnemyTypes[enemy].Logic = reader.ReadByte();
					Battle.EnemyTypes[enemy].Evade = reader.ReadByte();
					Battle.EnemyTypes[enemy].Absorb = reader.ReadByte();
					Battle.EnemyTypes[enemy].Hits = reader.ReadByte();
					Battle.EnemyTypes[enemy].Accuracy = reader.ReadByte();
					Battle.EnemyTypes[enemy].Damage = reader.ReadByte();
					Battle.EnemyTypes[enemy].Critical = reader.ReadByte();
					Battle.EnemyTypes[enemy].Reserved = reader.ReadByte();
					Battle.EnemyTypes[enemy].Attack = reader.ReadByte();
					Battle.EnemyTypes[enemy].Categories = (Battle.EnemyCategories)reader.ReadByte();
					Battle.EnemyTypes[enemy].MagicDefense = reader.ReadByte();
					Battle.EnemyTypes[enemy].Weak = (Battle.Elements)reader.ReadByte();
					Battle.EnemyTypes[enemy].Resist = (Battle.Elements)reader.ReadByte();
				}

				reader.BaseStream.Position = Data.Address(0x0C, 0x9020);

				for (var logic = 0; logic < 128; logic++)
				{
					Battle.LogicTypes[logic].Spell = reader.ReadByte();
					Battle.LogicTypes[logic].Ability = reader.ReadByte();

					Battle.LogicTypes[logic].Spells = new int[9];

					for (var magic = 0; magic < 9; magic++)
						Battle.LogicTypes[logic].Spells[magic] = reader.ReadByte();

					Battle.LogicTypes[logic].Abilities = new int[5];

					for (var ability = 0; ability < 5; ability++)
						Battle.LogicTypes[logic].Abilities[ability] = reader.ReadByte();
				}

				reader.BaseStream.Position = Data.Address(0x0B, 0x94E0);

				var addresses = new int[128];

				for (var enemy = 0; enemy < 128; enemy++)
				{
					addresses[enemy] = reader.ReadUInt16();
				}

				for (var enemy = 0; enemy < 128; enemy++)
				{
					reader.BaseStream.Position = Data.Address(0x0B, addresses[enemy]);

					Battle.EnemyTypes[enemy].Name = reader.ReadName();
				}

				reader.BaseStream.Position = Data.Address(0x0C, 0x81E0);

				for (var spell = 0; spell < 64; spell++)
				{
					Battle.SpellTypes[spell].Hit = reader.ReadByte();

					var value = reader.ReadByte();

					Battle.SpellTypes[spell].Elements = (Battle.Elements)reader.ReadByte();
					Battle.SpellTypes[spell].Target = (Battle.MagicTarget)reader.ReadByte();
					Battle.SpellTypes[spell].Effect = (Battle.MagicEffect)reader.ReadByte();
					Battle.SpellTypes[spell].Graphic = reader.ReadByte();
					Battle.SpellTypes[spell].Palette = reader.ReadByte();
					Battle.SpellTypes[spell].Reserved = reader.ReadByte();

					switch (Battle.SpellTypes[spell].Effect)
					{
						case Battle.MagicEffect.Resist:
						case Battle.MagicEffect.Weak:
							Battle.SpellTypes[spell].EffectElements = (Battle.Elements)value;
							break;

						case Battle.MagicEffect.Status:
						case Battle.MagicEffect.Status2:
						case Battle.MagicEffect.Cure:
						case Battle.MagicEffect.CureAll:
							Battle.SpellTypes[spell].EffectStatus = (Battle.Status)value;
							break;

						default:
							Battle.SpellTypes[spell].Value = value;
							break;
					}
				}

				for (var potion = 0; potion < 2; potion++)
				{
					Battle.PotionTypes[potion].Hit = reader.ReadByte();

					var value = reader.ReadByte();

					Battle.PotionTypes[potion].Elements = (Battle.Elements)reader.ReadByte();
					Battle.PotionTypes[potion].Target = (Battle.MagicTarget)reader.ReadByte();
					Battle.PotionTypes[potion].Effect = (Battle.MagicEffect)reader.ReadByte();
					Battle.PotionTypes[potion].Graphic = reader.ReadByte();
					Battle.PotionTypes[potion].Palette = reader.ReadByte();
					Battle.PotionTypes[potion].Reserved = reader.ReadByte();

					switch (Battle.PotionTypes[potion].Effect)
					{
						case Battle.MagicEffect.Resist:
						case Battle.MagicEffect.Weak:
							Battle.PotionTypes[potion].EffectElements = (Battle.Elements)value;
							break;

						case Battle.MagicEffect.Status:
						case Battle.MagicEffect.Status2:
						case Battle.MagicEffect.Cure:
						case Battle.MagicEffect.CureAll:
							Battle.PotionTypes[potion].EffectStatus = (Battle.Status)value;
							break;

						default:
							Battle.PotionTypes[potion].Value = value;
							break;
					}
				}

				for (var ability = 0; ability < 26; ability++)
				{
					Battle.AbilityTypes[ability].Hit = reader.ReadByte();

					var value = reader.ReadByte();

					Battle.AbilityTypes[ability].Elements = (Battle.Elements)reader.ReadByte();
					Battle.AbilityTypes[ability].Target = (Battle.MagicTarget)reader.ReadByte();
					Battle.AbilityTypes[ability].Effect = (Battle.MagicEffect)reader.ReadByte();
					Battle.AbilityTypes[ability].Graphic = reader.ReadByte();
					Battle.AbilityTypes[ability].Palette = reader.ReadByte();
					Battle.AbilityTypes[ability].Reserved = reader.ReadByte();

					switch (Battle.AbilityTypes[ability].Effect)
					{
						case Battle.MagicEffect.Resist:
						case Battle.MagicEffect.Weak:
							Battle.AbilityTypes[ability].EffectElements = (Battle.Elements)value;
							break;

						case Battle.MagicEffect.Status:
						case Battle.MagicEffect.Status2:
						case Battle.MagicEffect.Cure:
						case Battle.MagicEffect.CureAll:
							Battle.AbilityTypes[ability].EffectStatus = (Battle.Status)value;
							break;

						default:
							Battle.AbilityTypes[ability].Value = value;
							break;
					}
				}
			}
		}

		public static void LoadFormation(int formation, bool alternate)
		{
			using (var reader = Data.Reader())
			{
				reader.BaseStream.Position = Data.Address(0x0B, 0x8400 + (formation * 16));

				var data = reader.ReadBytes(16);

				var enemy1 = data[2];
				var enemy2 = data[3];
				var enemy3 = data[4];
				var enemy4 = data[5];

				var minimum1 = data[6] >> 4;
				var maximum1 = data[6] & 0x0f;
				var minimum2 = data[7] >> 4;
				var maximum2 = data[7] & 0x0f;
				var minimum3 = data[8] >> 4;
				var maximum3 = data[8] & 0x0f;
				var minimum4 = data[9] >> 4;
				var maximum4 = data[9] & 0x0f;

				var alternateMinimum1 = data[14] >> 4;
				var alternateMaximum1 = data[14] & 0x0f;
				var alternateMinimum2 = data[15] >> 4;
				var alternateMaximum2 = data[15] & 0x0f;

				var enemies = new List<Battle.Enemy>();

				var random = new Random();

				if (!alternate)
				{
					var count1 = random.Next(minimum1, maximum1 + 1);
					var count2 = random.Next(minimum2, maximum2 + 1);
					var count3 = random.Next(minimum3, maximum3 + 1);
					var count4 = random.Next(minimum4, maximum4 + 1);

					for (var x = 0; x < count1 && enemies.Count < 9; x++)
						enemies.Add(new Battle.Enemy { Type = enemy1, Health = Battle.EnemyTypes[enemy1].Health, MaxHealth = Battle.EnemyTypes[enemy1].Health });

					for (var x = 0; x < count2 && enemies.Count < 9; x++)
						enemies.Add(new Battle.Enemy { Type = enemy2, Health = Battle.EnemyTypes[enemy2].Health, MaxHealth = Battle.EnemyTypes[enemy2].Health });

					for (var x = 0; x < count3 && enemies.Count < 9; x++)
						enemies.Add(new Battle.Enemy { Type = enemy3, Health = Battle.EnemyTypes[enemy3].Health, MaxHealth = Battle.EnemyTypes[enemy3].Health });

					for (var x = 0; x < count4 && enemies.Count < 9; x++)
						enemies.Add(new Battle.Enemy { Type = enemy4, Health = Battle.EnemyTypes[enemy4].Health, MaxHealth = Battle.EnemyTypes[enemy4].Health });
				}
				else
				{
					var alternateCount1 = random.Next(alternateMinimum1, alternateMaximum1 + 1);
					var alternateCount2 = random.Next(alternateMinimum2, alternateMaximum2 + 1);

					for (var x = 0; x < alternateCount1 && enemies.Count < 9; x++)
						enemies.Add(new Battle.Enemy { Type = enemy1, Health = Battle.EnemyTypes[enemy1].Health, MaxHealth = Battle.EnemyTypes[enemy1].Health });

					for (var x = 0; x < alternateCount2 && enemies.Count < 9; x++)
						enemies.Add(new Battle.Enemy { Type = enemy2, Health = Battle.EnemyTypes[enemy2].Health, MaxHealth = Battle.EnemyTypes[enemy2].Health });
				}

				Battle.Enemies = enemies.ToArray();
			}
		}

		private static string ReadName(this BinaryReader reader)
		{
			var builder = new StringBuilder(256);

			while (true)
			{
				var character = reader.ReadByte();

				if (character == 0)
					break;

				if (character == 0x01)
					builder.Append("\\n");
				else if (character == 0x02)
					builder.Append("[Item Name]");
				else if (character == 0x03)
					builder.Append("[Character Name]");
				else if (character == 0x05)
					builder.Append("\\r\\n");
				else
					builder.Append(Characters[character]);
			}

			return builder.ToString();
		}

		private static string[] Characters = new string[]
		{
			// 0x00
			"[0x00]",
			"[0x01]",
			"[0x02]",
			"[0x03]",
			"[0x04]",
			"[0x05]",
			"[0x06]",
			"[0x07]",
			"[0x08]",
			"[0x09]",
			"[0x0a]",
			"[0x0b]",
			"[0x0c]",
			"[0x0d]",
			"[0x0e]",
			"[0x0f]",

			// 0x10
			"[0x10]",
			"[0x11]",
			"[0x12]",
			"[0x13]",
			"[0x14]",
			"[0x15]",
			"[0x16]",
			"[0x17]",
			"[0x18]",
			"[0x19]",
			"e ",
			" t",
			"th",
			"he",
			"s ",
			"in",

			// 0x20
			" a",
			"t ",
			"an",
			"re",
			" s",
			"er",
			"ou",
			"d ",
			"to",
			"n ",
			"ng",
			"ea",
			"es",
			" i",
			"o ",
			"ar",

			// 0x30
			"is",
			" b",
			"ve",
			" w",
			"me",
			"or",
			" o",
			"st",
			" c",
			"at",
			"en",
			"nd",
			"on",
			"hi",
			"se",
			"as",

			// 0x40
			"ed",
			"ha",
			" m",
			" f",
			"r ",
			"le",
			"ow",
			"g ",
			"ce",
			"om",
			"GI",
			"y ",
			"of",
			"ro",
			"ll",
			" p",

			// 0x50
			" y",
			"ca",
			"MA",
			"te",
			"f ",
			"ur",
			"yo",
			"ti",
			"l ",
			" h",
			"ne",
			"it",
			"ri",
			"wa",
			"ac",
			"al",

			// 0x60
			"we",
			"il",
			"be",
			"rs",
			"u ",
			" l",
			"ge",
			" d",
			"li",
			"....",
			"ne",
			"it",
			"ri",
			"wa",
			"ac",
			"al",

			// 0x70
			"[0x70]",
			"[0x71]",
			"[0x72]",
			"[0x73]",
			"[0x74]",
			"[0x75]",
			"[0x76]",
			"[0x77]",
			"[0x78]",
			"[0x79]",
			"/",
			"[0x7b]",
			"[0x7c]",
			"[0x7d]",
			"[0x7e]",
			"[0x7f]",

			// 0x80
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"A",
			"B",
			"C",
			"D",
			"E",
			"F",

			// 0x90
			"G",
			"H",
			"I",
			"J",
			"K",
			"L",
			"M",
			"N",
			"O",
			"P",
			"Q",
			"R",
			"S",
			"T",
			"U",
			"V",

			// 0xa0
			"W",
			"X",
			"Y",
			"Z",
			"a",
			"b",
			"c",
			"d",
			"e",
			"f",
			"g",
			"h",
			"i",
			"j",
			"k",
			"l",

			// 0xb0
			"m",
			"n",
			"o",
			"p",
			"q",
			"r",
			"s",
			"t",
			"u",
			"v",
			"w",
			"x",
			"y",
			"z",
			"'",
			",",

			// 0xc0
			".",
			"[0xc1]",
			"-",
			"..",
			"!",
			"?",
			"[0xc6]",
			"[0xc7]",
			"ee",
			"[0xc9]",
			"[0xca]",
			"[0xcb]",
			"[0xcc]",
			"[0xcd]",
			"[0xce]",
			"[0xcf]",

			// 0xd0
			"[0xd0]",
			"[0xd1]",
			"[0xd2]",
			"[0xd3]",
			"[Sword]",
			"[Hammer]",
			"[Dagger]",
			"[Axe]",
			"[Staff]",
			"[Nunchuck]",
			"[Armor]",
			"[Shield]",
			"[Helmet]",
			"[Gauntlet]",
			"[Bracelet]",
			"[Robe]",
			// 0xe0
			"%",
			"[Potion]",
			"[0xe2]",
			"[0xe3]",
			"[0xe4]",
			"[0xe5]",
			"[0xe6]",
			"[0xe7]",
			"[0xe8]",
			"[0xe9]",
			"[0xea]",
			"[0xeb]",
			"[0xec]",
			"[0xed]",
			"[0xee]",
			"[0xef]",

			// 0xf0
			"[0xf0]",
			"[0xf1]",
			"[0xf2]",
			"[0xf3]",
			"[0xf4]",
			"[0xf5]",
			"[0xf6]",
			"[0xf7]",
			"[0xf8]",
			"[0xf9]",
			"[0xfa]",
			"[0xfb]",
			"[0xfc]",
			"[0xfd]",
			"[0xfe]",
			" ",
		};
	}
}
