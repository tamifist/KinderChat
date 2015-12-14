using System;
using Foundation;
using UIKit;
using System.IO;
using System.Linq;
using KinderChat.ServerClient.Managers;
using KinderChat.ServerClient.Interfaces;

namespace KinderChat.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate, INotificationsHub
	{
		const string StartViewControllerId = "StartViewControllerId";
		const string SignUpViewControllerId = "SignUpViewControllerId";

		public Theme BlueTheme { get; private set; }

		public Theme RedTheme { get; private set; }

		public Theme BlackTheme { get; private set; }

		UIWindow window;

		UIStoryboard MainStoryboard {
			get {
				return UIStoryboard.FromName ("MainStoryboard", NSBundle.MainBundle);
			}
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			Xamarin.Insights.Initialize (App.InsightsKey);
			Xamarin.Insights.ForceDataTransmission = true;
			ServiceContainer.Register<IMessageDialog> (() => new MessageDialog ());
			var dbLocation = "kinder.db3";
			var docsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var libraryPath = Path.Combine (docsPath, "../Library/");
			dbLocation = Path.Combine (libraryPath, dbLocation);
			KinderDatabase.DatabaseLocation = dbLocation;

			ServiceContainer.Register<IUIThreadDispacher> (() => new UiThreadDispacher ());
			ServiceContainer.Register<ILiveConnection> (() => new WebSocketConnection ());
			App.Init ();

			SetupAppearance ();
			Theme.ThemeChanged += OnThemeChanged;

			window.RootViewController = InitStartViewController ();
			window.MakeKeyAndVisible ();

			App.ConnectionManager.TryKeepConnectionAsync();

			App.NotificationsHub = this;
			RegisterForPushNotifications ();

			return false;
		}

		public void RegisterForPushNotifications() 
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes (
					UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
					new NSSet ());

				UIApplication.SharedApplication.RegisterUserNotificationSettings (pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications ();
			} else {
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes (notificationTypes);
			}

			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		public async override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			if(string.IsNullOrWhiteSpace(Settings.UserDeviceId)) 
			{
				return;
			}
				
			var registrationId = deviceToken.Description;
			if (!string.IsNullOrWhiteSpace(registrationId)) {
				registrationId = registrationId.Trim('<').Trim('>').Replace(" ", "");
			}
			if(Settings.NotificationRegId != registrationId)
			{
				Settings.NotificationRegId = registrationId;
				try 
				{
					var manager = new DeviceRegistrationManager();
					await manager.RegisterAsync(Settings.NotificationRegId, new string[] { "username:" + Settings.UserDeviceId }, PlatformType.iOS, null);
				} 
				catch(Exception ex) {
					App.Logger.Report(ex);
				}
			}
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
			//ProcessNotification(userInfo, false);
		}

		public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
		{
			var test = error;
		}

		void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
		{
			// Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
			if (null != options && options.ContainsKey(new NSString("aps")))
			{
				//Get the aps dictionary
				NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

				string alert = string.Empty;

				//Extract the alert text
				// NOTE: If you're using the simple alert by just specifying
				// "  aps:{alert:"alert msg here"}  ", this will work fine.
				// But if you're using a complex alert with Localization keys, etc.,
				// your "alert" object from the aps dictionary will be another NSDictionary.
				// Basically the JSON gets dumped right into a NSDictionary,
				// so keep that in mind.
				if (aps.ContainsKey(new NSString("alert")))
					alert = (aps [new NSString("alert")] as NSString).ToString();

				//If this came from the ReceivedRemoteNotification while the app was running,
				// we of course need to manually process things like the sound, badge, and alert.
				if (!fromFinishedLaunching)
				{
					//Manually show an alert
					if (!string.IsNullOrEmpty(alert))
					{
						UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
						avAlert.Show();
					}
				}
			}
		}

		void SetupAppearance ()
		{
			SetupCurrentTheme ();
			ApplyCurrentTheme ();
		}

		void OnThemeChanged (object sender, EventArgs e)
		{
			ApplyCurrentTheme ();
		}

		void SetupCurrentTheme ()
		{
			Theme.AvailableThemes.Add (BlueTheme = CreateBlueTheme ());
			//Theme.AvailableThemes.Add (BlackTheme = CreateBlackTheme ());
			Theme.AvailableThemes.Add (RedTheme = CreateRedTheme ());

			switch (Settings.AppTheme) {
			case AppTheme.Blue:
				Theme.Current = BlueTheme;
				break;

			case AppTheme.Red:
				Theme.Current = RedTheme;
				break;

			case AppTheme.Black:
				Theme.Current = BlackTheme;
				break;

			default:
				Theme.Current = BlueTheme;
				break;
			}
		}

		Theme CreateBlueTheme ()
		{
			IColorTheme colorTheme = BlueColorTheme.Instance;
			IFontTheme fontTheme = BlueFontTheme.Instance;
			IImagesTheme imgTheme = BlueImagesTheme.Instance;

			return new Theme (colorTheme, fontTheme, imgTheme);
		}

		Theme CreateBlackTheme ()
		{
			IColorTheme colorTheme = BlackColorTheme.Instance;
			IFontTheme fontTheme = BlackFontTheme.Instance;
			IImagesTheme imgTheme = BlackImagesTheme.Instance;

			return new Theme (colorTheme, fontTheme, imgTheme);
		}

		Theme CreateRedTheme ()
		{
			IColorTheme colorTheme = RedColorTheme.Instance;
			IFontTheme fontTheme = RedFontTheme.Instance;
			IImagesTheme imgTheme = RedImagesTheme.Instance;

			return new Theme (colorTheme, fontTheme, imgTheme);
		}

		void ApplyCurrentTheme ()
		{
			UIApplication.SharedApplication.StatusBarStyle = Theme.Current.StatusBarStyle;

			UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes {
				ForegroundColor = Theme.Current.ScreenTitleColor,
				Font = Theme.Current.ScreenTitleFont
			};
			// Set back button chevron color
		    UINavigationBar.Appearance.TintColor = Theme.Current.ScreenTitleColor;
            
            UIBarButtonItem.Appearance.SetTitleTextAttributes (new UITextAttributes {
				TextColor = Theme.Current.ScreenTitleColor,
				Font = Theme.Current.NavigationButtonTitleFont
			}, UIControlState.Normal);

			UITabBarItem.Appearance.SetTitleTextAttributes (new UITextAttributes {
				Font = Theme.Current.TabBarItemTitle
			}, UIControlState.Normal);

			var old = window.RootViewController;
			window.RootViewController = null;
			window.RootViewController = old;
		}

		UIViewController InitStartViewController ()
		{
			if (!Settings.IsLoggedIn || App.ForceSignup) {
				return MainStoryboard.InstantiateViewController (SignUpViewControllerId);
			} else {
				return MainStoryboard.InstantiateViewController (StartViewControllerId);
			}
		}

		public void GoToMainScreen ()
		{
			var vc = MainStoryboard.InstantiateViewController (StartViewControllerId);
			window.RootViewController = vc;
		}

		public override void OnResignActivation (UIApplication application)
		{
		}

		public override void DidEnterBackground (UIApplication application)
		{
			Settings.InForeground = false;
			App.ConnectionManager.HandlePause();
		}

		public override void WillEnterForeground (UIApplication application)
		{
			Settings.InForeground = true;
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
			App.ConnectionManager.HandleResume ();
		}

		public override void WillTerminate (UIApplication application)
		{
		}

		public async override void HandleWatchKitExtensionRequest (UIApplication application, NSDictionary userInfo, Action<NSDictionary> reply)
		{
			if (userInfo ["Profile"] != null && userInfo ["Profile"].ToString () == "Profile") {
				var conversationsViewModel = App.ConversationsViewModel;
				await conversationsViewModel.ExecuteLoadConversationsCommand ();
				var conversationsList = conversationsViewModel.Conversations.ToList ();
				ConversationsStore.Save (conversationsList);

				foreach (var conversation in conversationsList) {
					var conversationViewModel = new ConversationViewModel (conversation.Sender);
					await conversationViewModel.ExecuteLoadMessagesCommand ();
					ConversationsStore.Save (conversationViewModel.Messages.Select(i => i.UnderlyingMessage).ToList (), string.Format ("conversation-{0}.xml", conversation.Sender));
				}

				var viewModel = App.ProfileViewModel;
				NSData avatarImage = !string.IsNullOrEmpty (viewModel.AvatarUrl) ? ImageUtils.GetImage (viewModel.AvatarUrl, url => url == viewModel.AvatarUrl).AsPNG () : new NSData ();

				reply (new NSDictionary ("Username", viewModel.NickName, "Avatar", avatarImage));
			}

			if (userInfo ["TextInput"] != null && userInfo ["TextInput"].ToString () != string.Empty) {
				var number = userInfo ["SenderID"] as NSNumber;
				var conversationViewModel = new ConversationViewModel (number.Int32Value);
				await conversationViewModel.ExecuteLoadMessagesCommand ();
				await conversationViewModel.SendMessage (userInfo ["TextInput"].ToString ());

				reply (new NSDictionary ("Confirmation", "Text was received"));
			}
		}
	}
}
