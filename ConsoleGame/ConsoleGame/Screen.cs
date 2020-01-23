using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class Screen
	{
		internal const int Width = 40;
		internal const int Height = 25;

		internal static char[] Characters = new char[Width * Height];

		private static bool CursorVisible;
		private static int WindowWidth;
		private static int WindowHeight;
		private static int BufferWidth;
		private static int BufferHeight;

		internal static void Enable()
		{
			CursorVisible = Console.CursorVisible;
			WindowWidth = Console.WindowWidth;
			WindowHeight = Console.WindowHeight;
			BufferWidth = Console.BufferWidth;
			BufferHeight = Console.BufferHeight;

			Console.CursorVisible = false;

			Console.SetWindowSize(Width, Height + 1);
			Console.SetBufferSize(Width, Height + 1);

			Console.Title = "SoulAge: Band of Warriors";
		}

		internal static void Disable()
		{
			Console.SetWindowSize(WindowWidth, WindowHeight);
			Console.SetBufferSize(BufferWidth, BufferHeight);

			Console.CursorVisible = CursorVisible;
		}

		internal static void Update()
		{
			Console.SetCursorPosition(0, 0);

			Console.Write(Characters);
		}

		internal static void Clear()
		{
			Array.Clear(Characters, 0, Characters.Length);
		}

		internal static void Fill(char value)
		{
			Array.Fill(Characters, value);
		}

		internal static void FillRectangle(char value, int top, int left, int bottom, int right)
		{
			var position = (top * Width) + left;
			var width = right - left + 1;

			for (var row = top; row <= bottom; row++)
			{
				Array.Fill(Characters, value, position, width);
				position += Width;
			}
		}

		internal static void DrawRectangle(char value, int top, int left, int bottom, int right)
		{
			DrawHorizontalLine(value, top, left, right);
			DrawHorizontalLine(value, bottom, left, right);
			DrawVerticalLine(value, left, top, bottom);
			DrawVerticalLine(value, right, top, bottom);
		}

		internal static void DrawVerticalLine(char value, int x, int top, int bottom)
		{
			var position = (top * Width) + x;

			for (var row = top; row <= bottom; row++)
			{
				Characters[position] = value;
				position += Width;
			}
		}

		internal static void DrawHorizontalLine(char value, int y, int left, int right)
		{
			var position = (y * Width) + left;

			Array.Fill(Characters, value, position, right - left + 1);
		}

		internal static void DrawString(string text, int x, int y)
		{
			var position = (y * Width) + x;

			foreach (var character in text)
				Characters[position++] = character;
		}
	}
}
