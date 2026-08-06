using Microsoft.Xna.Framework.Input;

namespace Liferoad
{
    public class Inputs
    {
        public static bool Up, Down, Left, Right, MouseDown, MousePress, MouseUp, Interact, InteractReset;
        public static int MouseX, MouseY;
        public static float Theta, AngleInDegrees;

        public static void Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                MouseDown = true;
                MousePress = true;
            }
            else
            {
                MouseDown = false;
            }
            if (MousePress && !MouseDown)
            {
                MouseUp = true;
                MousePress = false;
            }
            else
            {
                MouseUp = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Up = true;
            }
            else
            {
                Up = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Down = true;
            }
            else
            {
                Down = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Left = true;
            }
            else
            {
                Left = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Right = true;
            }
            else
            {
                Right = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                InteractReset = true;
            }
            if (InteractReset && !Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Interact = true;
                InteractReset = false;
            }
            else
            {
                Interact = false;
            }
        }
    }
}
