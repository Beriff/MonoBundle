using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle.UI
{
	public class VerticalList : Container
	{
		public int Spacing;
		public VerticalList(GUIController controller, Vector2 pos, int size_x, int spacing = 2)
			: base(controller, new(size_x, 0), pos)
		{
			Spacing = spacing;
			OnElementAdd = (component) =>
			{
				float size_y = 0;
				foreach (var item in ComponentList)
				{
					size_y += item.Size.Y + spacing;
				}
				component.Position = new Vector2(Position.X, Position.Y + size_y + spacing);
				size_y += component.Size.Y;
				Size = new(Size.X, size_y);
			};
			OnElementRemove = (component) =>
			{
				float y_index = 0;
				foreach (var item in ComponentList)
				{
					item.Position = new Vector2(item.Position.X, Position.Y + y_index);
					y_index += item.Size.Y + spacing;
				}
			};
		}
	}
	public class HorizontalList : Container
	{
		public int Spacing;
		public HorizontalList(GUIController controller, Vector2 pos, int size_x, int spacing = 2)
			: base(controller, new(size_x, 0), pos)
		{
			Spacing = spacing;
			OnElementAdd = (component) =>
			{
				float size_x = 0;
				foreach (var item in ComponentList)
				{
					size_x += item.Size.X + spacing;
				}
				component.Position = new Vector2(Position.X + size_x + spacing, Position.Y);
				size_x += component.Size.X;
				Size = new(size_x, Size.Y);
			};
			OnElementRemove = (component) =>
			{
				float x_index = 0;
				foreach (var item in ComponentList)
				{
					item.Position = new Vector2(Position.X + x_index, item.Position.Y);
					x_index += item.Size.X + spacing;
				}
			};
		}
	}
}
