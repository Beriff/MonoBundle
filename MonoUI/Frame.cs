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
		/// Container with a definite render frame, outside of which elements are not rendered
		/// </summary>
		public class Frame : Container
		{
			protected RenderTarget2D FrameTexture;
			public Frame(GUIController controller, Vector2 position, Vector2 size) : base(controller, size, position) 
			{
				var gd = controller.SBatch.GraphicsDevice;
				FrameTexture = new RenderTarget2D(
					ParentController.SBatch.GraphicsDevice, 
					ParentController.SBatch.GraphicsDevice.Viewport.Width, 
					ParentController.SBatch.GraphicsDevice.Viewport.Height, true,
					gd.PresentationParameters.BackBufferFormat,
					gd.PresentationParameters.DepthStencilFormat,
					gd.PresentationParameters.MultiSampleCount,
					RenderTargetUsage.PreserveContents);
			}
			public override void Render()
			{
				
				ParentController.SBatch.End();

				ParentController.SBatch.GraphicsDevice.SetRenderTarget(FrameTexture);

				ParentController.SBatch.GraphicsDevice.Clear(Color.Black);

				ParentController.SBatch.Begin();
				ParentController.SBatch.Draw(ParentController.Rect, GetRect(), SecondaryColor);
				foreach (var c in ComponentList) 
					c.Render();
				ParentController.SBatch.End();

				ParentController.SBatch.GraphicsDevice.SetRenderTarget(null);
				ParentController.SBatch.Begin();
				ParentController.SBatch.Draw(FrameTexture, Position, GetRect(), Color.White);

			}
			/// <summary>
			/// Return a frame with a close button and a title
			/// </summary>
			/// <param name="controller">parent controller</param>
			/// <param name="title">window title</param>
			/// <param name="pos">window position</param>
			/// <param name="size">window size</param>
			/// <returns>Assembled window</returns>
			public static Frame Window(GUIController controller, string title, Vector2 pos, Vector2 size)
			{
				var frame = new Frame(controller, pos, size);
				var para = new Paragraph(controller, frame.Position, title);
				var closebtn = new Button(controller,
					new(15, controller.DefaultFont.SheetCellSize.Y),
					new(frame.Position.X + frame.Size.X - 15, frame.Position.Y), "X")
				.WithOnClick(() => { frame.Enabled = false; });
				frame.Add(para);
				frame.Add(closebtn);
				return frame;
			}
		}
	}
}
