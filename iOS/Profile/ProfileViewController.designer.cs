// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace KinderChat.iOS
{
	[Register ("ProfileViewController")]
	partial class ProfileViewController
	{
		[Outlet]
		UIKit.UIImageView AvatarImg { get; set; }

		[Outlet]
		UIKit.UIView BlendNavBarView { get; set; }

		[Outlet]
		UIKit.UICollectionView CollectionView { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView Spinner { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (AvatarImg != null) {
				AvatarImg.Dispose ();
				AvatarImg = null;
			}
			if (BlendNavBarView != null) {
				BlendNavBarView.Dispose ();
				BlendNavBarView = null;
			}
			if (CollectionView != null) {
				CollectionView.Dispose ();
				CollectionView = null;
			}
			if (Spinner != null) {
				Spinner.Dispose ();
				Spinner = null;
			}
		}
	}
}
