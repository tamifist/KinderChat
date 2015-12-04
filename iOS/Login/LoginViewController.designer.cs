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
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIButton BoyButton { get; set; }

		[Outlet]
		UIKit.UIImageView BubbleImg { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint BubbleImgTopOffset { get; set; }

		[Outlet]
		UIKit.UIButton ContinueBtn { get; set; }

		[Outlet]
		UIKit.UIButton GirlButton { get; set; }

		[Outlet]
		UIKit.UITextField Input { get; set; }

		[Outlet]
		UIKit.UIView NavBarBlendView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint NavBarBlendViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UITextField NickName { get; set; }

		[Outlet]
		UIKit.UIButton SwitchSignUpType { get; set; }

		[Outlet]
		UIKit.UIView ThemeSelectorContainerView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (BoyButton != null) {
				BoyButton.Dispose ();
				BoyButton = null;
			}
			if (BubbleImg != null) {
				BubbleImg.Dispose ();
				BubbleImg = null;
			}
			if (BubbleImgTopOffset != null) {
				BubbleImgTopOffset.Dispose ();
				BubbleImgTopOffset = null;
			}
			if (ContinueBtn != null) {
				ContinueBtn.Dispose ();
				ContinueBtn = null;
			}
			if (GirlButton != null) {
				GirlButton.Dispose ();
				GirlButton = null;
			}
			if (Input != null) {
				Input.Dispose ();
				Input = null;
			}
			if (NavBarBlendView != null) {
				NavBarBlendView.Dispose ();
				NavBarBlendView = null;
			}
			if (NavBarBlendViewHeightConstraint != null) {
				NavBarBlendViewHeightConstraint.Dispose ();
				NavBarBlendViewHeightConstraint = null;
			}
			if (NickName != null) {
				NickName.Dispose ();
				NickName = null;
			}
			if (SwitchSignUpType != null) {
				SwitchSignUpType.Dispose ();
				SwitchSignUpType = null;
			}
			if (ThemeSelectorContainerView != null) {
				ThemeSelectorContainerView.Dispose ();
				ThemeSelectorContainerView = null;
			}
		}
	}
}
