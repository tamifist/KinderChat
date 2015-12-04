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
	[Register ("PointsViewController")]
	partial class PointsViewController
	{
		[Outlet]
		UIKit.UIView BlendNavBarView { get; set; }

		[Outlet]
		UIKit.UICollectionView CollectionView { get; set; }

		[Outlet]
		UIKit.UILabel CollectionViewTitle { get; set; }

		[Outlet]
		UIKit.UIView LeftBadgeContainer { get; set; }

		[Outlet]
		UIKit.UILabel LeftBadgeDescriptionLbl { get; set; }

		[Outlet]
		UIKit.UILabel LeftBadgeValueLbl { get; set; }

		[Outlet]
		UIKit.UIView RightBadgeContainder { get; set; }

		[Outlet]
		UIKit.UILabel RightBadgeDescriptionLbl { get; set; }

		[Outlet]
		UIKit.UILabel RightBadgeValueLbl { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView Spinner { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (BlendNavBarView != null) {
				BlendNavBarView.Dispose ();
				BlendNavBarView = null;
			}
			if (CollectionView != null) {
				CollectionView.Dispose ();
				CollectionView = null;
			}
			if (CollectionViewTitle != null) {
				CollectionViewTitle.Dispose ();
				CollectionViewTitle = null;
			}
			if (LeftBadgeContainer != null) {
				LeftBadgeContainer.Dispose ();
				LeftBadgeContainer = null;
			}
			if (LeftBadgeDescriptionLbl != null) {
				LeftBadgeDescriptionLbl.Dispose ();
				LeftBadgeDescriptionLbl = null;
			}
			if (LeftBadgeValueLbl != null) {
				LeftBadgeValueLbl.Dispose ();
				LeftBadgeValueLbl = null;
			}
			if (RightBadgeContainder != null) {
				RightBadgeContainder.Dispose ();
				RightBadgeContainder = null;
			}
			if (RightBadgeDescriptionLbl != null) {
				RightBadgeDescriptionLbl.Dispose ();
				RightBadgeDescriptionLbl = null;
			}
			if (RightBadgeValueLbl != null) {
				RightBadgeValueLbl.Dispose ();
				RightBadgeValueLbl = null;
			}
			if (Spinner != null) {
				Spinner.Dispose ();
				Spinner = null;
			}
		}
	}
}
