using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MonoBundle.Input;

namespace MonoBundle.UI
{
	public class ScrollFrame : Frame
	{
		public float Scroll;
		public ScrollFrame(GUIController controller, Vector2 position, Vector2 size)
			: base(controller, position, size)
		{

		}

		protected void DisplaceComponents()
		{
			float total_size_y = 0;

			Component minitem = null;
			Component maxitem = null;
			foreach (var item in ComponentList)
			{
				if (maxitem == null || minitem == null)
					maxitem = minitem = item;
				if (item.Position.Y < minitem.Position.Y)
					minitem = item;
				if (item.Position.Y > maxitem.Position.Y)
					maxitem = item;

			}

			total_size_y = maxitem.Position.Y + maxitem.Size.Y - (minitem.Position.Y + minitem.Size.Y);
			float topitem_pos = MathHelper.Lerp(Position.Y - (total_size_y - Size.Y), Position.Y, Scroll);
			float overall_shift = minitem.Position.Y - topitem_pos;
			foreach (var item in ComponentList)
			{
				item.Position = new Vector2(item.Position.X, item.Position.Y - overall_shift);
			}
		}

		public override void Update(GameTime gt, InputHandler input)
		{
			base.Update(gt, input);
			if(GetRect().Contains(input.CurrentMState.Position))
			{
				var scroll = input.PreviousMState.ScrollWheelValue - input.CurrentMState.ScrollWheelValue;
				Scroll = Math.Clamp(Scroll + scroll * .001f, 0f, 1f);
			}
			DisplaceComponents();
			
		}

		public override void Render()
		{
			base.Render();
		}
	}
}
