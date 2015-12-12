
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.App;
using Android.Content.PM;

namespace KinderChat
{
	public abstract class BaseActivity : AppCompatActivity
	{

		public static Activity CurrentActivity {get;set;}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			CurrentActivity = this;
			SetTheme (Settings.AppTheme == AppTheme.Blue ? Resource.Style.MyTheme : Resource.Style.MyThemeRed);
			base.OnCreate (savedInstanceState);
			SetContentView (LayoutResource);
		}

		protected override void OnPause ()
        {

			base.OnPause ();
			Settings.InForeground = false;
			App.ConnectionManager.HandlePause();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			CurrentActivity = this;
			Settings.InForeground = true;
			App.ConnectionManager.HandleResume();
		}

		protected override void OnStop ()
		{
			base.OnStop ();
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected abstract int LayoutResource {
			get;
		}
	}

}