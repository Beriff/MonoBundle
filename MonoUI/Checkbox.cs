using Microsoft.Xna.Framework;
using MonoBundle.Input;
using MonoBundle.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle.UI
{
	public class Checkbox : Component, IClickable
	{
		public string Label;
		public bool Checkmark;
		protected Color? _CheckboxColor;
		public Color CheckboxColor { get => _CheckboxColor ?? PrimaryColor; set => _CheckboxColor = value; }
		public Action OnClick { get; set; }

		public Checkbox(GUIController controller, Vector2 size, Vector2 position, string label) : base(controller, size, position)
		{
			Label = label;
			Checkmark = false;
			OnClick = () => { };
		}

		public Rectangle GetCheckboxBounds()
		{
			return new Rectangle(Position.ToPoint(), (Size * new Vector2(.2f, 1)).ToPoint());
		}

		public override void Render()
		{
			base.Render();

			//render checkbox
			var batch = ParentController.SBatch;
			var chbounds = GetCheckboxBounds();
			batch.Draw(ParentController.Rect, chbounds, CheckboxColor);
			if(Checkmark)
			{
				Rectangle rect = new(
					(chbounds.Center.ToVector2() - chbounds.Size.ToVector2() / 4).ToPoint(),
					(chbounds.Size.ToVector2() / 2).ToPoint());
				batch.Draw(ParentController.Rect, rect, Color.Black);
			}
			//render label
			Paragraph.RenderFormatted(ParentController, Label, new(Position.X + Size.X * .2f + 2, Position.Y + Size.Y * .25f));
				
		}
		public override void Update(GameTime gt, InputHandler input)
		{
			var rect = GetCheckboxBounds();
			var mousepos_prev = input.PreviousMState.Position;
			var mousepos_curr = input.CurrentMState.Position;
			if (rect.Contains(mousepos_curr) && input.GetM1State() == KeyState.E_Released)
			{ 
				OnClick(); 
				Checkmark = !Checkmark; 
			}
		}
	}
}
