using System;
using Android.App;
using Android.OS;
using System.IO;
using Plugin.CurrentActivity;
using KinderChat.Infrastructure;

using Environment = System.Environment;

namespace KinderChat
{
	[Application]
	public class KinderApplication: Application, Application.IActivityLifecycleCallbacks
	{
		
		public KinderApplication(IntPtr handle, global::Android.Runtime.JniHandleOwnership transer)
			:base(handle, transer)
		{

		}

		public override void OnCreate()
		{
			base.OnCreate();
			RegisterActivityLifecycleCallbacks(this);

			Xamarin.Insights.Initialize (App.InsightsKey, this);
			Xamarin.Insights.ForceDataTransmission = true;
			ServiceContainer.Register<IMessageDialog> (() => new MessageDialog ());
			var dbLocation = "kinder.db3";

			var library = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			dbLocation = Path.Combine(library, dbLocation);
			KinderDatabase.DatabaseLocation = dbLocation;

			ServiceContainer.Register<IUIThreadDispacher>(() => new UIThreadDispacher());
			ServiceContainer.Register<ILiveConnection>(() => new WebSocketConnection());
			App.Init ();
		}

		public override void OnTerminate()
		{
			base.OnTerminate();
			UnregisterActivityLifecycleCallbacks(this);
		}

		public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivityDestroyed(Activity activity)
		{
		}

		public void OnActivityPaused(Activity activity)
		{
		}

		public void OnActivityResumed(Activity activity)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
		{
		}

		public void OnActivityStarted(Activity activity)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivityStopped(Activity activity)
		{
		}
	}
}

