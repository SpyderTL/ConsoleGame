using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RpgGame
{
	public static class DataMap
	{
		public static void Load(int map)
		{
			using (var reader = Data.Reader())
			{
				// Find Tileset
				reader.BaseStream.Position = Data.Address(0, 0xACC0 + map);

				var tileset = reader.ReadByte();

				// Load Tiles
				reader.BaseStream.Position = Data.Address(0, 0x8800 + (tileset * 256));

				for (var tile = 0; tile < 128; tile++)
				{
					var value = reader.ReadByte();
					var value2 = reader.ReadByte();

					Map.Tiles[tile] = new Map.Tile
					{
						Blocked = (value & 0x01) == 0x01,
						TileType = (Map.TileType)((value >> 1) & 0x0f),
						TeleportType = (Map.TeleportType)(value >> 6),
						Battle = (value & 0x20) == 0x20,
						Value = value2
					};
				}

				// Load Teleports
				reader.BaseStream.Position = Data.Address(0, 0xAD80);

				for (var teleport = 0; teleport < Map.Teleports.Length; teleport++)
					Map.Teleports[teleport].Map = reader.ReadByte();

				reader.BaseStream.Position = Data.Address(0, 0xAD00);

				for (var teleport = 0; teleport < Map.Teleports.Length; teleport++)
					Map.Teleports[teleport].X = reader.ReadByte();

				reader.BaseStream.Position = Data.Address(0, 0xAD40);

				for (var teleport = 0; teleport < Map.Teleports.Length; teleport++)
					Map.Teleports[teleport].Y = reader.ReadByte();

				// Load Exits
				reader.BaseStream.Position = Data.Address(0, 0xAC60);

				for (var exit = 0; exit < Map.Exits.Length; exit++)
					Map.Exits[exit].X = reader.ReadByte();

				reader.BaseStream.Position = Data.Address(0, 0xAC70);

				for (var exit = 0; exit < Map.Exits.Length; exit++)
					Map.Exits[exit].Y = reader.ReadByte();

				// Load Treasures
				reader.BaseStream.Position = Data.Address(0, 0xB100);

				for (var treasure = 0; treasure < Map.Treasures.Length; treasure++)
					Map.Treasures[treasure].Item = reader.ReadByte();

				// Load Items
				reader.BaseStream.Position = Data.Address(0x0a, 0xB700);

				var addresses = new int[256];

				for (var item = 0; item < Map.Items.Length; item++)
					addresses[item] = reader.ReadUInt16();

				for (var item = 0; item < Map.Items.Length; item++)
				{
					reader.BaseStream.Position = Data.Address(0x0a, addresses[item]);

					Map.Items[item].Name = reader.ReadText();

					if (item > 0x16)
						Map.Items[item].Type = Map.ItemType.Special;
					else if (item < 0x1C)
						Map.Items[item].Type = Map.ItemType.Item;
					else if (item < 0x44)
						Map.Items[item].Type = Map.ItemType.Weapon;
					else if (item < 0x54)
						Map.Items[item].Type = Map.ItemType.Armor;
					else if (item < 0x5d)
						Map.Items[item].Type = Map.ItemType.Shield;
					else if (item < 0x64)
						Map.Items[item].Type = Map.ItemType.Helmet;
					else if (item < 0x6c)
						Map.Items[item].Type = Map.ItemType.Gauntlet;
					else if (item < 0xb0)
						Map.Items[item].Type = Map.ItemType.Gold;
				}

				// Load Segments
				reader.BaseStream.Position = Data.Address(4, 0x8000 + (map * 2));

				var address = reader.ReadUInt16();

				var bank = 4 + (address >> 14);
				var offset = 0x8000 + (address & 0x3fff);

				reader.BaseStream.Position = Data.Address(bank, offset);

				var segments = new List<Map.Segment>();

				while (true)
				{
					var value = reader.ReadByte();

					if (value == 0xff)
						break;

					var count = 1;

					if ((value & 0x80) == 0x80)
					{
						value &= 0x7f;

						count = reader.ReadByte();

						if (count == 0)
							count = 256;
					}

					segments.Add(new Map.Segment { Tile = value, Count = count });
				}

				Map.Segments = segments.ToArray();

				// Load Objects
				reader.BaseStream.Position = Data.Address(0, 0xB400 + (map * 0x30));

				for (var obj = 0; obj < Map.Objects.Length; obj++)
				{
					var type = reader.ReadByte();
					var value = reader.ReadByte();
					var y = reader.ReadByte();

					var flags = value & 0xc0;
					var x = value & 0x3f;

					var mapObject = new Map.Object
					{
						Type = type,
						X = x,
						Y = y,
						Flags = flags
					};

					Map.Objects[obj] = mapObject;
				}

				// Load Dialogs
				reader.BaseStream.Position = Data.Address(0x0a, 0x8000);

				for (var dialog = 0; dialog < 256; dialog++)
				{
					addresses[dialog] = reader.ReadUInt16();
				};

				for (var dialog = 0; dialog < 256; dialog++)
				{
					reader.BaseStream.Position = Data.Address(0x0a, addresses[dialog]);

					Map.Dialogs[dialog] = reader.ReadText();
				}

				// Load Object Dialogs
				reader.BaseStream.Position = Data.Address(0x0e, 0x95D5);

				for (var obj = 0; obj < 208; obj++)
				{
					Map.ObjectDialogs[obj] = new int[4];

					for (var dialog = 0; dialog < 4; dialog++)
					{
						Map.ObjectDialogs[obj][dialog] = reader.ReadByte();
					}
				}
			}
		}

		private static string ReadText(this BinaryReader reader)
		{
			var builder = new StringBuilder(256);

			while (true)
			{
				var character = reader.ReadByte();

				if (character == 0)
					break;

				if (character == 0x01)
					builder.Append("[Next Line]");
				else if (character == 0x02)
					builder.Append("[Item Name]");
				else if (character == 0x03)
					builder.Append("[Character Name]");
				else if (character == 0x05)
					builder.Append("[New Line]");
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
