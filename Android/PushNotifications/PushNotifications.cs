using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using KinderChat.ServerClient.Managers;
using KinderChat.ServerClient.Interfaces;
using Gcm.Client;
using Newtonsoft.Json;

namespace KinderChat
{
	[BroadcastReceiver(Permission=Constants.PERMISSION_GCM_INTENTS)]
	[IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted })] // Allow GCM on boot and when app is closed
	[IntentFilter(new [] { Constants.INTENT_FROM_GCM_MESSAGE },
		Categories = new [] { "@PACKAGE_NAME@" })]
	[IntentFilter(new [] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK },
		Categories = new [] { "@PACKAGE_NAME@" })]
	[IntentFilter(new [] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY },
		Categories = new [] { "@PACKAGE_NAME@" })]
	public class KinderGcmBroadcastReceiver : GcmBroadcastReceiverBase<KinderGcmService>
	{
		//Google API Console App Project Number
		public static string[] SenderIds = { "144208723707" };
    }

    [Service] //Must use the service tag
	public class KinderGcmService : GcmServiceBase
	{
		const string hubName = "inner6notifications";
		const string connectionPath = "Endpoint=sb://inner6notifications-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=Lu8wegPXUSHfRCqQ2Xfcv6WKF9s7Xai30r1PirMsdLU=";
		const string sharedAccessKey = "Lu8wegPXUSHfRCqQ2Xfcv6WKF9s7Xai30r1PirMsdLU=";
		const string ServiceBus = "sb://inner6notifications-ns.servicebus.windows.net";


		public static void Register(Context context)
		{
            // Check to ensure everything's set up right
            GcmClient.CheckDevice(context);
            GcmClient.CheckManifest(context);

            // Makes this easier to call from our Activity
            GcmClient.Register (context, KinderGcmBroadcastReceiver.SenderIds);
		}

		public KinderGcmService() : base(KinderGcmBroadcastReceiver.SenderIds)
		{
		}

		protected async override void OnRegistered (Context context, string registrationId)
		{
			//Receive registration Id for sending GCM Push Notifications to

			if (registrationId != Settings.NotificationRegId) {

				Settings.NotificationRegId = registrationId;
				try{
					var manager = new DeviceRegistrationManager ();
					await manager.RegisterAsync(Settings.NotificationRegId, new string[] { "username:" + Settings.UserDeviceId}, PlatformType.Android, null);
				}catch(Exception ex) {
					//App.Logger.Report (ex);
				}
			}

		}

		protected override void OnUnRegistered (Context context, string registrationId)
		{
			
		}

		protected override void OnMessage (Context context, Intent intent)
		{
			Console.WriteLine ("Received Notification");

			//if (Settings.InForeground)
				//return;

			//Push Notification arrived - print out the keys/values
			if (intent != null || intent.Extras != null) {

				var keyset = intent.Extras.KeySet ();

				foreach (var key in keyset)
					Console.WriteLine ("Key: {0}, Value: {1}", key, intent.Extras.GetString(key));

				ShowNotification (intent);
			}
		}



		private void ShowNotification(Intent intent)
		{
			const string key = "msg";

			if (!intent.Extras.ContainsKey(key))
				return;

			var message = intent.Extras.GetString(key);
				
			NotificationMessage notification;
			try
			{
				notification = JsonConvert.DeserializeObject<NotificationMessage>(message);
			}
			catch(Exception ex) {
				App.Logger.Report (ex);
				return;
			}

			if (notification == null || string.IsNullOrWhiteSpace (notification.Message))
				return;
	
			
			var notificationManager = NotificationManagerCompat.From (this);



			Intent notificationIntent = null;

			int id = notification.FromId;
			//if user id came in then let's send them to that page.
			if (id <= 0) {
				id = Settings.GetNotificationId ();
				notificationIntent = new Intent(this, typeof(WelcomeActivity));
			} else {
				notificationIntent = new Intent (this, typeof(ConversationActivity));
				notificationIntent.PutExtra (ConversationActivity.RecipientId, (long)id);
			}


			notificationIntent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask); 
			var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

			var wearableExtender =
				new NotificationCompat.WearableExtender ()
					.SetContentIcon (Resource.Drawable.ic_launcher);
			


			var builder = new NotificationCompat.Builder (this)
				.SetContentIntent (pendingIntent)
				.SetContentTitle ("Inner6")
				.SetAutoCancel (true)
				.SetSmallIcon(Resource.Drawable.ic_notification)
				.SetContentText(notification.Message)
				.Extend(wearableExtender);

			// Obtain a reference to the NotificationManager



				

			var notif = builder.Build ();
			notif.Defaults = NotificationDefaults.All; //add sound, vibration, led :)

			notificationManager.Notify(id, notif);
		}

		protected override bool OnRecoverableError (Context context, string errorId)
		{
			//Some recoverable error happened
			return true;
		}

		protected override void OnError (Context context, string errorId)
		{
		    var test = errorId;
		    //Some more serious error happened
		}
	}
}

