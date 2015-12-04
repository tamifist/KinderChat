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
	[Register ("ConfirmationViewController")]
	partial class ConfirmationViewController
	{
		[Outlet]
		UIKit.UIButton ContinueBtn { get; set; }

		[Outlet]
		UIKit.UILabel DescriptionBottomlbl { get; set; }

		[Outlet]
		UIKit.UILabel DescriptionTopLbl { get; set; }

		[Outlet]
		UIKit.UITextField Input { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ContinueBtn != null) {
				ContinueBtn.Dispose ();
				ContinueBtn = null;
			}
			if (DescriptionBottomlbl != null) {
				DescriptionBottomlbl.Dispose ();
				DescriptionBottomlbl = null;
			}
			if (DescriptionTopLbl != null) {
				DescriptionTopLbl.Dispose ();
				DescriptionTopLbl = null;
			}
			if (Input != null) {
				Input.Dispose ();
				Input = null;
			}
		}
	}
}
