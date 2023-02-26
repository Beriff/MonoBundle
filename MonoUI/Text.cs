using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle
{
	namespace UI
	{
		/// <summary>
		/// UI Component that represents a piece of text.
		/// To color specific parts of texts use <c>'°RGBcolored text'</c> notation. (° = ALT+248)
		/// °- resets the text to its default color
		/// </summary>
		public class Paragraph : Component
		{
			public string Text;

			public Paragraph(GUIController controller, Vector2 pos, string text) : base(controller, new(0), pos)
			{
				Text = text;
			}
			public override Vector2 Size { get => ParentController.DefaultFont.StringLengthPx(Text); set => base.Size = value; }
			public override Rectangle GetRect()
			{
				return new Rectangle(Position.ToPoint(), ParentController.DefaultFont.StringLengthPx(Text).ToPoint());
			}
			public static void RenderFormatted(GUIController controller, string text, Vector2 position)
			{
				Vector2 offset = new(0);
				Color current_color = controller.PrimaryColor;
				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					Vector2 pixel_offset = offset * controller.DefaultFont.SheetCellSize.ToVector2() + position;

					if (c == '\n') { offset.Y++; offset.X = 0; continue; }
					if (c == '°')
					{
						//parse the '°-' case
						if (i + 1 < text.Length && text[i + 1] == '-') { current_color = controller.PrimaryColor; i++; continue; }
						//parse the '°RGB' case
						else if (i + 1 < text.Length)
						{
							string r = $"{text[i + 1]}{text[i + 2]}{text[i + 3]}";
							string g = $"{text[i + 4]}{text[i + 5]}{text[i + 6]}";
							string b = $"{text[i + 7]}{text[i + 8]}{text[i + 9]}";
							i += 9;
							current_color = new Color(int.Parse(r), int.Parse(g), int.Parse(b));
							continue;
						}
					}
					else { controller.DefaultFont.RenderChar(controller.SBatch, c, pixel_offset, current_color); offset.X++; }
				}
			}
			public override void Render()
			{
				base.Render();
				RenderFormatted(ParentController, Text, Position);
			}
		}
	}
}
