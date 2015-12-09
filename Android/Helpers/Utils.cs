using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;

namespace KinderChat
{
	public static class Utils
	{

		public static Bitmap DrawableToBitmap(Drawable drawable)
		{
			if (drawable is BitmapDrawable)
			{
				return ((BitmapDrawable)drawable).Bitmap;
			}

			if (drawable.IntrinsicHeight == -1 || drawable.IntrinsicHeight == -1)
				return null;

			Bitmap bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas(bitmap);
			drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
			drawable.Draw(canvas);

			return bitmap;
		}

        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                        : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            // Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
            Matrix mtx = new Matrix();
            ExifInterface exif = new ExifInterface(fileName);
            var orientation = (Orientation)exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Undefined);

            switch (orientation)
            {
                case Orientation.Undefined: // Nexus 7 landscape...
                    break;
                case Orientation.Normal: // landscape
                    break;
                case Orientation.FlipHorizontal:
                    break;
                case Orientation.Rotate180:
                    mtx.PreRotate(180);
                    resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                    mtx.Dispose();
                    mtx = null;
                    break;
                case Orientation.FlipVertical:
                    break;
                case Orientation.Transpose:
                    break;
                case Orientation.Rotate90: // portrait
                    mtx.PreRotate(90);
                    resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                    mtx.Dispose();
                    mtx = null;
                    break;
                case Orientation.Transverse:
                    break;
                case Orientation.Rotate270: // might need to flip horizontally too...
                    mtx.PreRotate(270);
                    resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                    mtx.Dispose();
                    mtx = null;
                    break;
                default:
                    mtx.PreRotate(90);
                    resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                    mtx.Dispose();
                    mtx = null;
                    break;
            }
            
            return resizedBitmap;
        }
    }
}

