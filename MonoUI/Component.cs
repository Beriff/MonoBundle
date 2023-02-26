using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBundle.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle
{
	namespace UI
	{
		public interface IClickable
		{
			public Action OnClick { get; set;}
		}
		public interface IMouseResponsive : IClickable
		{
			public Action OnMouseEnter { get; set; }
			public Action OnMouseLeave { get; set; }
		}
		/// <summary>
		/// Base UI Component class
		/// </summary>
		public abstract class Component
		{
			public Vector2 _Size;
			public Vector2 _Position;

			public Vector2 Anchor = new(0);

			public Action<Vector2> OnResize = (v) => { };
			public Action<Vector2> OnReposition = (v) => { };

			public string Name;

			protected bool _Enabled = true;
			public Action<bool> EnableToggle = (s) => { };

			public List<Overlay> Overlays;

			/// <summary>
			/// Controls whether the component will be rendered and updated or not
			/// </summary>
			public bool Enabled { get => _Enabled; set { EnableToggle(value); _Enabled = value; } }

			/// <summary>
			/// Component's UI profile that determines the logic group of the component
			///
			/// </summary>
			/// <remarks> Ex. "Menu" profile could unite main menu elements. Opening Settings window can close all of the other
			/// "Menu" profile components.</remarks>
			public string? UIProfile = null;

			public GUIController ParentController;
			protected SpriteBatch Sb { get => ParentController.SBatch; }

			// Color overrides
			// if no color is specified (null), then GUIController color defaults will be used
			// (which is preferrable, to be able to change whole UI theme from single controller)
			protected Color? _PrimaryColor;
			protected Color? _SecondaryColor;
			protected Color? _HighlightColor;
			public Color PrimaryColor { get => _PrimaryColor ?? ParentController.PrimaryColor; set => _PrimaryColor = value; }
			public Color SecondaryColor { get => _SecondaryColor ?? ParentController.SecondaryColor; set => _SecondaryColor = value; }
			public Color HighlightColor { get => _HighlightColor ?? ParentController.HighlightColor; set => _HighlightColor = value; }

			public virtual Vector2 Size { get => _Size; set { OnResize(value); _Size = value; } }
			public virtual Vector2 Position { get => _Position; set { OnReposition(value); _Position = value; } }
			public virtual Rectangle GetRect() => new Rectangle(Position.ToPoint(), Size.ToPoint());

			/// <summary>
			/// Render UI component using Component.ParentController.SBatch
			/// </summary>
			public virtual void Render()
			{
				foreach (var overlay in Overlays)
					overlay.Render(this);

				if (ParentController.DebugMode)
					ParentController.SBatch.Draw(ParentController.Rect, GetRect(), new Color(255, 0, 0, 137));
			}
			public virtual void Render(Vector2 position)
			{
				var oldpos = Position;
				_Position = position; //avoid firing OnReposition event
				Render();
				_Position = oldpos;
			}
			/// <summary>
			/// Updates UI component. Primarly used for interactive elements, such as buttons
			/// </summary>
			/// <param name="gt">Game time passed from Game.Update()</param>
			public virtual void Update(GameTime gt, InputHandler input) { }
			public Component(GUIController controller, Vector2 size, Vector2 pos)
			{
				ParentController = controller;
				Size = size;
				Position = pos;
				ParentController.Components.Add(this);
				Overlays = new();
			}

			/// <summary>
			/// Place component relative to the provided component.
			/// </summary>
			/// <remarks>Component.Anchor is taken into account.</remarks>
			/// <param name="component">Relative component</param>
			/// <param name="multiplier">Placement multiplier. Ex. (.5,.5) would place in the center of the component</param>
			public void Place(Component component, Vector2 multiplier)
			{
				Position = component.Position + (component.Size * multiplier) - Size * Anchor;
			}
			/// <summary>
			/// Set new size of the component, relative to the provided component's size
			/// </summary>
			/// <param name="component">Relative component</param>
			/// <param name="multiplier">Size multiplier. (1,1) is the size of provided component</param>
			public void Resize(Component component, Vector2 multiplier)
			{
				Size = component.Size * multiplier;
			}
			/// <summary>
			/// Place component relative to the viewport
			/// </summary>
			/// <param name="multiplier">Placement multiplier</param>
			public void ViewportPlace(Vector2 multiplier)
			{
				var viewport = ParentController.SBatch.GraphicsDevice.Viewport;
				Position = new Vector2(viewport.Width, viewport.Height) * multiplier - Size * Anchor;
			}

			public static Component operator + (Component self, Overlay other)
			{
				self.Overlays.Add(other);
				return self;
			}
		}

		public class Placeholder : Component
		{
			public Placeholder(GUIController controller, Vector2 position, Vector2? size = null)
				: base(controller, size ?? new(50), position)
			{

			}
		}
	}
}
