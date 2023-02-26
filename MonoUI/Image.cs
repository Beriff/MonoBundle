using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle.UI
{
	public class Image : Component
	{
		public Texture2D ImageTexture;
		public Image(GUIController controller, Texture2D texture, Vector2 position, Vector2? size = null)
			: base(controller, size ?? texture.Bounds.Size.ToVector2(), position)
		{
			ImageTexture = texture;
		}
		public override void Render()
		{
			ParentController.SBatch.Draw(ImageTexture, Position, Color.White);
			base.Render();
		}
	}
}
