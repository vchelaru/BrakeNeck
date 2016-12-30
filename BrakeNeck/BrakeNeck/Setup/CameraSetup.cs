using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using Microsoft.Xna.Framework;

#if !FRB_MDX
using System.Linq;
#endif

namespace BrakeNeck
{
	internal static class CameraSetup
	{
			internal static void SetupCamera (Camera cameraToSetUp, GraphicsDeviceManager graphicsDeviceManager)
			{
				SetupCamera(cameraToSetUp, graphicsDeviceManager, 1600, 900);
			}
			internal static void SetupCamera (Camera cameraToSetUp, GraphicsDeviceManager graphicsDeviceManager, int width, int height)
			{
				#if WINDOWS
				FlatRedBall.FlatRedBallServices.GraphicsOptions.SetResolution(width, height);
				#elif IOS || ANDROID
				FlatRedBall.FlatRedBallServices.GraphicsOptions.SetFullScreen(FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionWidth, FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionHeight);
				#endif
				#if WINDOWS_PHONE || WINDOWS_8 || IOS || ANDROID
				if (height > width)
				{
					graphicsDeviceManager.SupportedOrientations = DisplayOrientation.Portrait;
				}
				else
				{
					graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
				}
				#endif
				cameraToSetUp.UsePixelCoordinates(false, 1920, 1080);
			}
			internal static void ResetCamera (Camera cameraToReset)
			{
				cameraToReset.X = 0;
				cameraToReset.Y = 0;
				cameraToReset.XVelocity = 0;
				cameraToReset.YVelocity = 0;
				// Glue does not generate a detach call because the camera may be attached by this point
			}

	}
}
