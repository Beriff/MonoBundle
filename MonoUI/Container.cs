using Microsoft.Xna.Framework;
using MonoBundle.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle
{
	namespace UI
	{
		public abstract class Container : Component
		{
			public List<Component> ComponentList;
			public Action<Component> OnElementAdd;
			public Action<Component> OnElementRemove;
			public Container(GUIController controller, Vector2 size, Vector2 position, Action<Component>? onadd = null, Action<Component>? onremove = null) 
				: base(controller, size, position) 
			{
				ComponentList = new();
				OnElementAdd = onadd ?? ( (_) => { } );
				OnElementRemove = onremove ?? ((_) => { });
				OnReposition = (newpos) =>
				{
					var shift = newpos - Position;
					foreach (var c in ComponentList)
						c._Position += shift; //avoid firing OnReposition() for child components
				};
				//When enabled status is changed, change it for every child element
				EnableToggle = (status) =>
				{
					foreach (var c in ComponentList)
						c.Enabled = status;
				};
			}
			public override void Render()
			{
				base.Render();
				foreach(var component in ComponentList) { if (Enabled) { component.Render(); } }
			}
			public override void Update(GameTime gt, InputHandler input)
			{
				if(Enabled) { foreach (var component in ComponentList) { if (Enabled) { component.Update(gt, input); } } }
			}
			public Component? FindChild(string name)
			{
				foreach(var component in ComponentList)
				{
					if (component.Name == name)
						return component;
				} return null;
			}
			public void RemoveChild(string name)
			{
				foreach(var component in ComponentList)
				{
					if (component.Name == name)
						ComponentList.Remove(component);
				}
			}
			public void RemoveChild(Component component) { OnElementRemove(component); ComponentList.Remove(component); }
			public void Add(Component component)
			{
				OnElementAdd(component);
				ComponentList.Add(component);
				ParentController.Components.Remove(component);
			}

			public static Container operator + (Container self, Component other)
			{
				self.Add(other);
				return self;
			}
		}
	}
}
