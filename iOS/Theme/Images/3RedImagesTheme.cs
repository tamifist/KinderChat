using System;

using UIKit;

namespace KinderChat.iOS
{
	public class RedImagesTheme : IImagesTheme
	{
		public static RedImagesTheme Instance { get; private set; }

		static RedImagesTheme()
		{
			Instance = new RedImagesTheme ();
		}

		private RedImagesTheme()
		{
		}

		public UIImage SignUpIcon {
			get {
				return UIImage.FromBundle ("inner6Icon");
			}
		}

		public UIImage ApplyEffects (UIImage image)
		{
			return image;
		}
	}
}

