using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using Android.Support.V4.Widget;
using Android.Content;

namespace KinderChat
{
	class ConversationsFragment : BaseFragment
    {
		
        public static ConversationsFragment NewInstance()
        {
            var f = new ConversationsFragment();
            var b = new Bundle();
            f.Arguments = b;
            return f;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
			RetainInstance = true;
        }

		readonly ConversationsViewModel viewModel = App.ConversationsViewModel;
		SwipeRefreshLayout refresher;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			var root = inflater.Inflate(Resource.Layout.fragment_conversations, container, false);
            var list = root.FindViewById<ListView>(Resource.Id.conversations_list);
            list.ItemClick += OnConversationClick;
			list.Adapter = new ConverstationAdapter(Activity, viewModel);

			refresher = root.FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);
			refresher.Refresh += (sender, e) => viewModel.ExecuteLoadConversationsCommand ();


            return root;
        }
		
        void OnConversationClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			
			var message = viewModel.Conversations [e.Position];

			var id = message.Sender == Settings.MyId ? message.Recipient : message.Sender;

			if (id == 0)
				return;

			var intent = new Intent (Activity, typeof(ConversationActivity));
			intent.PutExtra (ConversationActivity.RecipientId, id);
			Activity.StartActivity(intent);
        }
        
		void ViewModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Activity.RunOnUiThread (() => {
				switch (e.PropertyName) {
				case BaseViewModel.IsBusyPropertyName:
					refresher.Refreshing = viewModel.IsBusy;
					if (viewModel.IsBusy)
						AndroidHUD.AndHUD.Shared.Show (Activity, Resources.GetString(Resource.String.loading), maskType: AndroidHUD.MaskType.Clear);
					else
						AndroidHUD.AndHUD.Shared.Dismiss (Activity);
					break;
				}
			});
		}
			

		public override void OnResume ()
		{
			base.OnResume ();
			viewModel.PropertyChanged += ViewModelPropertyChanged;
			Init ();
		}

		public override void OnStop ()
		{
			base.OnStop ();
			viewModel.PropertyChanged -= ViewModelPropertyChanged;
		}

		public override void Init ()
		{
			base.Init ();
			if (!viewModel.Initialized || viewModel.IsDirty) {
				viewModel.Initialized = true;
				viewModel.ExecuteLoadConversationsCommand ();
			}
		}
    }
}