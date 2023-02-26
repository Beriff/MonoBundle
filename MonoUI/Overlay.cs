using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle.UI
{
	public abstract class Overlay
	{
		public abstract void Render(Component parent);
	}
	
	public class BorderOverlay : Overlay
	{
		public Color BorderColor;
		public int BorderWidth;
		public BorderOverlay(Color color, int width = 1)
		{
			BorderColor = color;
			BorderWidth = width;
		}
		public override void Render(Component parent)
		{
			var controller = parent.ParentController;

			controller.SBatch.Draw(controller.Rect, parent.Position,
				new Rectangle(new(0), new((int)parent.Size.X, BorderWidth)), BorderColor);
			controller.SBatch.Draw(controller.Rect, parent.Position + new Vector2(0, parent.Size.Y),
				new Rectangle(new(0), new((int)parent.Size.X, BorderWidth)), BorderColor);

			controller.SBatch.Draw(controller.Rect, parent.Position,
				new Rectangle(new(0), new(BorderWidth, (int)parent.Size.Y)), BorderColor);
			controller.SBatch.Draw(controller.Rect, parent.Position + new Vector2(parent.Size.X, 0),
				new Rectangle(new(0), new(BorderWidth, (int)parent.Size.Y)), BorderColor);
		}
	}
}
