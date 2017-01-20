
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;

using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
using FlatRedBall.Localization;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace BrakeNeck.Screens
{
    public enum InputDevice
    {
        Keyboard,
        Controller
    }

	public partial class MainMenu
	{
        public static InputDevice? SelectedInputDevice
        {
            get;
            set;
        } = null;

        public static int SelectedNumberOfPlayers
        {
            get;
            set;
        }

		void CustomInitialize()
		{


		}

		void CustomActivity(bool firstTimeCalled)
        {
            bool didProceed = InputDeviceSelectionActivity();

            if(!didProceed)
            {
                MultiPlayerSelectionActivity();
            }
        }

        private bool InputDeviceSelectionActivity()
        {
            bool didProceed = false;

            if (SelectedInputDevice == null)
            {
                if (InputManager.Keyboard.AnyKeyPushed())
                {
                    SelectedInputDevice = InputDevice.Keyboard;
                    MoveToScreen(typeof(GameScreen));
                    didProceed = true;

                }
                else
                {
                    bool proceededByGamePad = false;
                    foreach (var gamePad in InputManager.Xbox360GamePads)
                    {
                        if (gamePad.IsConnected && gamePad.AnyButtonPushed())
                        {
                            SelectedInputDevice = InputDevice.Controller;
                            this.MainMenuGumRuntime.CurrentVariableState = 
                                GumRuntimes.MainMenuGumRuntime.VariableState.PlayerSelection;
                            didProceed = true;
                            proceededByGamePad = true;
                        }

                    }

                    if(proceededByGamePad)
                    {
                        bool shouldAskForMultiPlayer = InputManager.NumberOfConnectedGamePads > 1;
                        if(shouldAskForMultiPlayer == false)
                        {
                            MoveToScreen(typeof(GameScreen));
                        }
                    }
                }
            }

            return didProceed;
        }

        private void MultiPlayerSelectionActivity()
        {
            if(this.MainMenuGumRuntime.CurrentVariableState == GumRuntimes.MainMenuGumRuntime.VariableState.PlayerSelection)
            {
                IndexChangingActivity();

                ControllerSelectingActivity();
            }
        }

        private void ControllerSelectingActivity()
        {
            var controllers = InputManager.Xbox360GamePads;
            if (controllers.Any(item =>
                 item.ButtonPushed(Xbox360GamePad.Button.Start) ||
                 item.ButtonPushed(Xbox360GamePad.Button.A)
                ))
            {
                int index = PlayerSelectionContainer.Children.IndexOf(SelectionMarker.Parent);

                SelectedNumberOfPlayers = index + 1;

                MoveToScreen(typeof(GameScreen));
            }
        }

        private void IndexChangingActivity()
        {
            var controllers = InputManager.Xbox360GamePads;
            bool movedUp = controllers.Any(item => item.ButtonPushed(Xbox360GamePad.Button.DPadUp) ||
                item.LeftStick.AsDPadPushed(Xbox360GamePad.DPadDirection.Up));

            bool movedDown = controllers.Any(item => item.ButtonPushed(Xbox360GamePad.Button.DPadDown) ||
                item.LeftStick.AsDPadPushed(Xbox360GamePad.DPadDirection.Down));

            int index = PlayerSelectionContainer.Children.IndexOf(SelectionMarker.Parent);

            if (movedUp)
            {
                index--;
            }
            if (movedDown)
            {
                index++;
            }

            if (index < 0)
            {
                index = PlayerSelectionContainer.Children.Count - 1;
            }
            if (index >= PlayerSelectionContainer.Children.Count)
            {
                index = 0;
            }

            SelectionMarker.Parent = PlayerSelectionContainer.Children[index];
        }

        void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
