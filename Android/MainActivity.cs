using System;
using Android.App;
using Android.Content;
using Android.OS;
using com.refractored;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Util;
using Android.Content.PM;
using Android.Views;
using Android.Widget;

namespace KinderChat
{
	[Activity (Label = "@string/app_name", Icon = "@drawable/ic_launcher", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
	public class MainActivity : BaseActivity, ViewPager.IOnPageChangeListener
	{

		protected override int LayoutResource {
			get { return Resource.Layout.activity_main; }
		}

		private MyPagerAdapter adapter;
		private ViewPager pager;
		private PagerSlidingTabStrip tabs;

		protected override void OnCreate (Bundle bundle)
		{
            base.OnCreate(bundle);
            adapter = new MyPagerAdapter(this, SupportFragmentManager);
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            tabs = FindViewById<PagerSlidingTabStrip>(Resource.Id.tabs);
            pager.Adapter = adapter;
            tabs.SetViewPager(pager);
            tabs.OnPageChangeListener = this;
            var pageMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, Resources.DisplayMetrics);
            pager.PageMargin = pageMargin;
            pager.OffscreenPageLimit = 4;
            if (Settings.FirstRun)
            {
                pager.CurrentItem = 2;
                App.MessageDialog.SendMessage(Resources.GetString(Resource.String.get_started_welcome),
                    Resources.GetString(Resource.String.welcome_to_kinderchat));
            }
            else
            {
                pager.CurrentItem = 0;
            }

            //IntentFilter connectivityIntentFilter = new IntentFilter();
            //connectivityIntentFilter.AddAction(Android.Net.ConnectivityManager.ConnectivityAction);
            //RegisterReceiver(new NetworkChangeReceiver(), connectivityIntentFilter);

            // Register for GCM
            KinderGcmService.Register(this);
        }

	    public void OnPageScrollStateChanged (int state)
		{
		}
		public void OnPageScrolled (int position, float positionOffset, int positionOffsetPixels)
		{
		}
		public void OnPageSelected (int position)
		{
			var tag = "android:switcher:" + pager.Id + ":" + position;
			var frag = SupportFragmentManager.FindFragmentByTag (tag) as BaseFragment;
			if (frag != null)
				frag.Init ();	
		}

		public class MyPagerAdapter : FragmentStatePagerAdapter, ICustomTabProvider
        {
			private string[] Titles;

            private readonly int[] icons =
            {
                Resource.Drawable.ic_tab_chats,
                Resource.Drawable.ic_tab_contacts,
                Resource.Drawable.ic_tab_profile
            };

            public MyPagerAdapter (Context context, Android.Support.V4.App.FragmentManager fm)
				: base (fm)
			{
				Titles = context.Resources.GetStringArray (Resource.Array.sections);
			}

			public override Java.Lang.ICharSequence GetPageTitleFormatted (int position)
			{
				return new Java.Lang.String (Titles [position]);
			}

			#region implemented abstract members of PagerAdapter

			public override int Count {
				get {
					return icons.Length;
				}
			}

			#endregion

			#region implemented abstract members of FragmentPagerAdapter

			public override Android.Support.V4.App.Fragment GetItem (int position)
			{
				switch (position) {
				case 0:
					var frag = ConversationsFragment.NewInstance ();
					return frag;
				case 1:
					var frag2 = FriendsFragment.NewInstance ();
					return frag2;
				case 2:
					var frag3 = ProfileFragment.NewInstance ();
					return frag3;
                }

				return null;
			}

            #endregion

            public View GetCustomTabView(ViewGroup parent, int position)
            {
                var tablayout =
                    (LinearLayout)
                        LayoutInflater.From(Application.Context).Inflate(Resource.Layout.tab_layout, parent, false);

                var tabImage = tablayout.FindViewById<ImageView>(Resource.Id.tabImage);

                tabImage.SetImageResource(icons[position]);

                return tablayout;
            }
        }
	}
}

