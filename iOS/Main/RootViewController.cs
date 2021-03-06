﻿using System;
using System.Drawing;
using Foundation;
using UIKit;
using CoreGraphics;

namespace KinderChat.iOS
{
	public partial class RootViewController : UITabBarController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public RootViewController (IntPtr handle)
			: base (handle)
		{
			if (Settings.FirstRun) {
				Settings.FirstRun = false;
				this.SelectedIndex = (int)TabOrder.Profile;
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ApplyCurrentTheme ();
			Theme.ThemeChanged += OnThemeChanged;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			Theme.ThemeChanged -= OnThemeChanged;
		}

		#endregion

		void ApplyCurrentTheme()
		{
			CGSize gradientImageSize = new CGSize(TabBar.Frame.Size.Width, TabBar.Frame.Size.Height);
			UIImage gradientImage = ImageUtils.GetGradientImage(
				Theme.Current.MainGradientStartColor.CGColor, Theme.Current.MainGradientEndColor.CGColor, gradientImageSize);
			TabBar.TintColor = UIColor.FromPatternImage(gradientImage);
		}

		void OnThemeChanged (object sender, EventArgs e)
		{
			ApplyCurrentTheme ();
		}
	}
}