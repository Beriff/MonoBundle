using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle
{
	namespace Input
	{
		public enum KeyState
		{
			Pressed,
			Released,
			E_Pressed,
			E_Released
		}
		public class InputHandler
		{
			public KeyboardState PreviousKState;
			public MouseState PreviousMState;
			public KeyboardState CurrentKState;
			public MouseState CurrentMState;

			public KeyState GetKeyState(Keys key)
			{
				if (CurrentKState.IsKeyDown(key))
				{
					if (PreviousKState.IsKeyDown(key))
						return KeyState.Pressed;
					else
						return KeyState.E_Pressed;
				}
				else
				{
					if (PreviousKState.IsKeyDown(key))
						return KeyState.E_Released;
					else
					{
						return KeyState.Released;
					}
				}
			}
			public KeyState GetM1State()
			{
				var m1_prevpress = PreviousMState.LeftButton == ButtonState.Pressed;
				var m1_currpress = CurrentMState.LeftButton == ButtonState.Pressed;

				if (m1_prevpress && m1_currpress)
					return KeyState.Pressed;
				else if (!m1_prevpress && !m1_currpress)
					return KeyState.Released;
				else if (m1_prevpress && !m1_currpress)
					return KeyState.E_Released;
				else
					return KeyState.E_Pressed;
			}
			public void Update()
			{
				PreviousKState = CurrentKState;
				CurrentKState = Keyboard.GetState();

				PreviousMState = CurrentMState;
				CurrentMState = Mouse.GetState();
			}

			public InputHandler()
			{
				PreviousKState = new();
				PreviousMState = new();
				CurrentKState = new();
				CurrentMState = new();
			}
		}
	}
}
