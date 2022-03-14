using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class ClassData
	{
		public const int ClassCount = 6;

		public static ClassType[] Classes = new ClassType[ClassCount];

		private const int ClassBank = 0x00;
		private const int ClassAddress = 0xb040;

		public static void Load()
		{
			using (var reader = Data.Reader())
			{
				reader.BaseStream.Position = Data.Position(ClassBank, ClassAddress);

				for (var i = 0; i < ClassCount; i++)
				{
					Classes[i].Id = reader.ReadByte();
					Classes[i].Health = reader.ReadByte();
					Classes[i].Strength = reader.ReadByte();
					Classes[i].Agility = reader.ReadByte();
					Classes[i].Intelligence = reader.ReadByte();
					Classes[i].Vitality = reader.ReadByte();
					Classes[i].Luck = reader.ReadByte();
					Classes[i].Damage = reader.ReadByte();
					Classes[i].Accuracy = reader.ReadByte();
					Classes[i].Evade = reader.ReadByte();
					Classes[i].MagicDefense = reader.ReadByte();

					reader.ReadBytes(5);
				};
			}
		}

		public struct ClassType
		{
			public int Id;
			public int Health;
			public int Strength;
			public int Agility;
			public int Intelligence;
			public int Vitality;
			public int Luck;
			public int Damage;
			public int Accuracy;
			public int Evade;
			public int MagicDefense;
		}
	}
}
