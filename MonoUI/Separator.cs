using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle.UI
{
	public class Separator : Component
	{
		string Header;

		public Separator(GUIController controller, string name, Vector2 position, Vector2 size)
			: base(controller, size, position)
		{
			Header = name;
			Size = new Vector2(Size.X, controller.DefaultFont.SheetCellSize.Y);
		}

		public override void Render()
		{
			base.Render();
			var offset = ParentController.DefaultFont.StringLengthPx(Header).X + Position.X;
			Paragraph.RenderFormatted(ParentController, Header, Position);
			Sb.Draw(ParentController.Rect,
				new Vector2(offset, Position.Y + Size.Y / 2), new Rectangle(new(0, 0), new((int)(Size.X), 1)), PrimaryColor);
		}
	}
}
