using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using MonoBundle.Input;

namespace MonoBundle
{
	namespace UI
	{
		public class Button : Component, IMouseResponsive
		{
			public string Label;
			public Action OnMouseEnter { get; set; }
			public Action OnMouseLeave { get; set; }
			public Action OnClick { get; set; }
			public Button(GUIController controller, Vector2 size, Vector2 position, string text) : base(controller, size, position)
			{
				Label = text;
				OnMouseEnter = () =>
				{
					
					PrimaryColor = new Color(PrimaryColor.R + 30, PrimaryColor.G + 30, PrimaryColor.B + 30, PrimaryColor.A);
				};
				OnMouseLeave = () =>
				{
					PrimaryColor = new Color(PrimaryColor.R - 30, PrimaryColor.G - 30, PrimaryColor.B - 30, PrimaryColor.A);
				};
				OnClick = () => { };
			}
			public override void Update(GameTime gt, InputHandler input)
			{
				var rect = GetRect();
				var mousepos_prev = input.PreviousMState.Position;
				var mousepos_curr = input.CurrentMState.Position;
				//System.Diagnostics.Debug.WriteLine("eh");

				if (!rect.Contains(mousepos_prev) && rect.Contains(mousepos_curr))
					OnMouseEnter();
				else if (rect.Contains(mousepos_prev) && !rect.Contains(mousepos_curr))
					OnMouseLeave();

				if (rect.Contains(mousepos_curr) && input.GetM1State() == Input.KeyState.E_Released)
					OnClick();

			}
			public Button WithOnClick(Action onclick)
			{
				OnClick = onclick;
				return this;
			}
			public override void Render()
			{
				
				ParentController.SBatch.Draw(ParentController.Rect, GetRect(), SecondaryColor);
				base.Render();
				//Draw centered label
				var labelpos = GetRect().Center.ToVector2() - ParentController.DefaultFont.StringLengthPx(Label) / 2;
				//labelpos += new Vector2(Size.X, 0);
				Paragraph.RenderFormatted(ParentController, Label, labelpos);
			}
		}
	}
}
