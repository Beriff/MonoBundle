using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBundle.Input;

namespace MonoBundle
{
	namespace UI
	{
		class MonoBundleException : Exception
		{
			public MonoBundleException() : base() { }
			public MonoBundleException(string message) : base(message) { }
		}
		public class GUIController
		{
			public Texture2D Rect;
			public SpriteBatch SBatch;

			public Color PrimaryColor;
			public Color SecondaryColor;
			public Color HighlightColor;

			public List<Component> Components;

			public SheetFont DefaultFont;

			public InputHandler DefaultInput;

			public bool DebugMode = false;

			public GUIController(SpriteBatch sb)
			{
				SBatch = sb;
				if (sb.GraphicsDevice.PresentationParameters.RenderTargetUsage != RenderTargetUsage.PreserveContents)
					throw new MonoBundleException("GraphicsDevice.PresentationParameters.RenderTargetUsage must be set to PreserveContents in Initialize()");
				Rect = new Texture2D(sb.GraphicsDevice, 1, 1);
				Rect.SetData(new Color[] { Color.White });
				Components = new();
				DefaultInput = new();
			}
			/// <summary>
			/// Changes the "Enabled" status of all elements with the same UIProfile
			/// </summary>
			/// <param name="profile_name">Targeted profile</param>
			/// <param name="status">Enabled status to set</param>
			public void SetProfileStatus(string profile_name, bool status)
			{
				foreach (var component in Components)
				{
					if (component.UIProfile == profile_name)
						component.Enabled = status;
				}
			}
			public void Render()
			{
				foreach (var c in Components)
				{
					if (c.Enabled)
						c.Render();
				}
			}
			public void Update(GameTime gt)
			{
				DefaultInput.Update();
				foreach (var component in Components)
				{
					if(component.Enabled)
						component.Update(gt, DefaultInput);
				}
			}
			public GUIController(SpriteBatch sb, Color primary, Color seconday, Color highlight) : this(sb)
			{
				PrimaryColor = primary;
				SecondaryColor = seconday;
				HighlightColor = highlight;
			}
		}
	}
}
