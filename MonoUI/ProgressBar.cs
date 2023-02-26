using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle
{
	namespace UI
	{
		public class ProgressBar : Component
		{
			public float Progress;
			public bool Clamped;
			//string text
			public void SetProgress(float progress)
			{
				progress = Clamped ? Math.Clamp(progress, 0f, 1f) : progress;
				Progress = progress;
			}
			public override void Render()
			{
				base.Render();
				var rect = GetRect();
				var progress_rect = rect;
				progress_rect.Width = (int)(progress_rect.Width * Progress);

				Sb.Draw(ParentController.Rect, rect, SecondaryColor);
				Sb.Draw(ParentController.Rect, progress_rect, HighlightColor);
			}
			public ProgressBar(Vector2 size, Vector2 position, GUIController controller) : base(controller, size, position)
			{
				Progress = .5f;
				Clamped = true;
			}
		}
	}
}
