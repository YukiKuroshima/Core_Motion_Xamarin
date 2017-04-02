using System;
using CoreGraphics;

using Foundation;
using UIKit;
using CoreMotion;

using AudioToolbox;
using AVFoundation;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CoreMotion
{
	public partial class CoreMotionViewController : UIViewController
	{
		private CMMotionManager motionManager;

		public CoreMotionViewController () : base ("CoreMotionViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		#region View lifecycle
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var shortSound = new SystemSound(1000);
			var longSound = new SystemSound(1031);
		
			motionManager = new CMMotionManager ();

			motionManager.DeviceMotionUpdateInterval = 0.01;
			motionManager.StartDeviceMotionUpdates(NSOperationQueue.CurrentQueue, (motion, error) =>
			{

				this.lblZ.Text = motion.RotationRate.z.ToString("0.00");
				this.lblX.Text = motion.RotationRate.x.ToString("0.00");
				this.lblY.Text = motion.RotationRate.y.ToString("0.00");//this

				Console.WriteLine(motion.RotationRate.z.ToString("0.00"));
				Console.WriteLine(motion.RotationRate.x.ToString("0.00"));
				Console.WriteLine(motion.RotationRate.y.ToString("0.00"));

				// If RotationRate is bigger than .25 or smaller than -.25
				// then consider the device is moving.

				if (motion.RotationRate.z < -0.25) // RotationRate of z axis is negative -> tilting to right.
				{
					Console.WriteLine("Tilting right");
					View.BackgroundColor = UIColor.Green;

					// checking if the device is parallel to the ground or vertical to the ground
					if (-0.04 < motion.Attitude.Pitch && motion.Attitude.Pitch < 0.04 ||
					    1.53 < motion.Attitude.Pitch && motion.Attitude.Pitch < 1.57 ||
					    -1.57 < motion.Attitude.Pitch && motion.Attitude.Pitch < -1.53)
					{
						shortSound.PlayAlertSound();
						SystemSound.Vibrate.PlayAlertSound();
						SystemSound.Vibrate.PlayAlertSound();
					}
				}
				else if (motion.RotationRate.z > 0.25)
				{
					Console.WriteLine("Tilting left");
					View.BackgroundColor = UIColor.Blue;
					if (-0.04 < motion.Attitude.Pitch && motion.Attitude.Pitch < 0.04 ||
						1.53 < motion.Attitude.Pitch && motion.Attitude.Pitch < 1.57 ||
						-1.57 < motion.Attitude.Pitch && motion.Attitude.Pitch < -1.53)
					{
						shortSound.PlayAlertSound();
						longSound.PlayAlertSound();
						SystemSound.Vibrate.PlayAlertSound();
					}
				}
				// If program comes here, the device is considered to be NOT moving.
				// and do not do anything.
	
			});
		}

		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		#endregion
	}
}

