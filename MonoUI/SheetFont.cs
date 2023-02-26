using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle
{
	namespace UI
	{
		/// <summary>
		/// Class representing a monospace bitmap font.
		/// Use UI.Paragraph for integration with UI system
		/// </summary>
		public class SheetFont
		{
			public static char[] Characters = new char[]
			{
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
			'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
			'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
			'Y', 'Z', '!', '@', '#', '$', '%', '^', '&', '*',
			'(', ')', '1', '2', '3', '4', '5', '6', '7', '8',
			'9', '0', '`', '~', ':', ';', '\\', '|', '/', '.',
			',', '\'', '"', '-', '_', '+', '=', '[', ']', '{',
			'}', '>', '?', '♦', ' ', '°', '<', '░', '▒', '▓'
			};
			public readonly Dictionary<char, Point> CharDict;

			public Texture2D Sheet;
			public Point SheetCellSize;
			protected int RowSize = 10;

			/// <summary>
			/// Renders a given string of text on the screen
			/// </summary>
			/// <param name="sb">Active sprite batch</param>
			/// <param name="text">A string of text to render</param>
			/// <param name="m_offset">Rendering offset in pixels</param>
			/// <param name="color">Color of the text</param>
			/// <param name="scale">Scale of each character</param>
			public void RenderString(SpriteBatch sb, string text, Vector2 m_offset, Color color, int scale = 1)
			{
				Vector2 offset = new(0);
				foreach (var c in text)
				{
					if (c == '\n') { offset.Y++; offset.X = 0; continue; }

					var pos = CharDict[c] * SheetCellSize;
					//render the cell of the texture
					var pixel_offset = new Vector2(offset.X * SheetCellSize.X * scale, offset.Y * SheetCellSize.Y * scale) + m_offset;
					sb.Draw(Sheet, pixel_offset, new Rectangle(pos, SheetCellSize), color);
					offset.X++;
				}
			}
			/// <summary>
			/// Render provided character
			/// </summary>
			/// <param name="sb">Active sprite batch</param>
			/// <param name="c">Character to render</param>
			/// <param name="offset">Pixel offset of character</param>
			/// <param name="color">Color of the character</param>
			public void RenderChar(SpriteBatch sb, char c, Vector2 offset, Color color)
			{
				sb.Draw(Sheet, offset, new Rectangle(CharDict[c] * SheetCellSize, SheetCellSize), color);
			}
			/// <summary>
			/// Get length of provided string of text in pixels
			/// </summary>
			/// <param name="text">A string of text to measure</param>
			/// <returns></returns>
			public Vector2 StringLengthPx(string text)
			{
				int newline_count = 1;
				int max_width = 0;
				int current_width = 0;
				for(int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					if(c == '°') { i += 9; continue; }
					if(c == '\n') 
					{ 
						newline_count++; 
						max_width = Math.Max(max_width, current_width); 
						current_width = 0; 
						continue; 
					}
					current_width++;
				}
				max_width = Math.Max(max_width, current_width);
				return new(SheetCellSize.X * max_width, newline_count * SheetCellSize.Y);
			}

			/// <summary>
			/// Wrap the string based on the sheet font and passed max width
			/// </summary>
			/// <param name="text">String to be formatted</param>
			/// <param name="max_width">Maximal width of a single line</param>
			/// <returns></returns>
			public string WrapString(string text, int max_width)
			{
				int current_width = 0;
				string[] words = text.Split(' ');
				var string_result = new StringBuilder();
				foreach(var word in words)
				{
					current_width += (int)StringLengthPx($"{word} ").X;
					
					if (current_width >= max_width)
					{
						string_result.Append($"\n{word} ");
						current_width = 0;
					} else { string_result.Append($"{word} "); }
				}
				return string_result.ToString();
			}
			/// <summary>
			/// Create a new instance of bitmap font
			/// </summary>
			/// <param name="texture">Texture of the font</param>
			/// <param name="cellsize">How many pixels (width & height) does single character take</param>
			/// <param name="rowsize">How many there are characters in a single row of texture</param>
			public SheetFont(Texture2D texture, int cellsize = 8, int rowsize = 10)
			{
				Sheet = texture;
				SheetCellSize = new(cellsize);
				CharDict = new();
				RowSize = 10;
				for (int i = 0; i < Characters.Length; i++)
				{
					char c = Characters[i];
					int x = i % RowSize;
					int y = (int)Math.Floor(i / (float)RowSize);
					CharDict[c] = new Point(x, y);
				}
			}
		}
	}
}
