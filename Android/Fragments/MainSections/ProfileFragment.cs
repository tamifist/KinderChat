using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;

namespace KinderChat
{
	class ProfileFragment : BaseFragment
    {
		public static ProfileFragment NewInstance()
        {
			var f = new ProfileFragment();
            var b = new Bundle();
            f.Arguments = b;
            return f;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
			RetainInstance = true;
			HasOptionsMenu = true;
        }


		readonly ProfileViewModel viewModel = App.ProfileViewModel;
		TextView nickName;
		ImageView avatar;
		ImageView avatar_mask;
		ImageView btn_take_photo;
		GridView avatarGrid;
	    private ImageButton edit_profile_button;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			var root = inflater.Inflate(Resource.Layout.fragment_profile, container, false);
           

			avatarGrid = root.FindViewById<GridView> (Resource.Id.grid);
			avatarGrid.Adapter = new AvatarAdapter (Activity, viewModel);

			avatarGrid.ItemClick += (sender, e) => {
				var selected = viewModel.Avatars[e.Position];
				viewModel.Avatar = selected.Location;
			};


			nickName = root.FindViewById<TextView> (Resource.Id.nickname);
            nickName.Text = viewModel.NickName;

            avatar_mask = root.FindViewById<ImageView>(Resource.Id.avatar_mask);
            avatar_mask.SetImageResource(Settings.AppTheme == AppTheme.Blue
                ? Resource.Drawable.ic_blue_circle
                : Resource.Drawable.ic_red_circle);

            Func<string, Stream> resizeAndRotateFunc = fileName =>
            {
                Bitmap bitmap = fileName.LoadAndResizeBitmap(1280, 720);
                byte[] bitmapData;
                using (var stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    bitmapData = stream.ToArray();
                }

                return new MemoryStream(bitmapData);
            };
                /*btn_take_photo = root.FindViewById<ImageView> (Resource.Id.btn_take_photo);
            btn_take_photo.Clickable = true;
            btn_take_photo.Click += (sender, args) => App.MessageDialog.SelectOption("New Avatar", new[] {
                "Pick Photo from Gallery",
                "Take Photo"
            }, which => {
                if (which == 0)
                    viewModel.PickPhoto();
                else if (which == 1)
                    viewModel.TakePhoto(resizeAndRotateFunc);
            });*/

            avatar = root.FindViewById<ImageView> (Resource.Id.avatar);
			avatar.Clickable = true;
			avatar.Click += (sender, e) => App.MessageDialog.SelectOption ("New Avatar", new[] {
				"Pick Photo from Gallery",
				"Take Photo"
			}, which => {
				if (which == 0)
					viewModel.PickPhoto ();
				else if(which == 1)
					viewModel.TakePhoto (resizeAndRotateFunc);
			});

            edit_profile_button = root.FindViewById<ImageButton>(Resource.Id.edit_profile_button);
            edit_profile_button.Clickable = true;
            edit_profile_button.Click += (sender, args) =>
            {
                App.MessageDialog.AskForString(Resources.GetString(Resource.String.enter_name),
                    Resources.GetString(Resource.String.edit_profile),
                    newNickName =>
                    {
                        if (newNickName != viewModel.NickName)
                        {
                            nickName.Text = newNickName;
                            viewModel.NickName = newNickName;
                            viewModel.ExecuteSaveProfileCommand();
                        }
                    });
            };

            /*if (viewModel.AvatarUrl.EndsWith("avatar-lion-1.png"))
            {
                viewModel.AvatarUrl = null;
            }
            
            if (!string.IsNullOrWhiteSpace(viewModel.AvatarUrl))
            {
                avatar.Visibility = ViewStates.Visible;
                Koush.UrlImageViewHelper.SetUrlDrawable(avatar, viewModel.AvatarUrl);
            }
            else
            {
                avatar.Visibility = ViewStates.Gone;
            }*/

            Koush.UrlImageViewHelper.SetUrlDrawable(avatar, viewModel.AvatarUrl);
            viewModel.PropertyChanged += ViewModelPropertyChanged;
            return root;
        }

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate (Resource.Menu.profile, menu);
			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override void OnResume ()
		{
			base.OnResume ();
			if (Settings.FirstRun) {
				Settings.FirstRun = false;
				if (!viewModel.Initialized) {
					viewModel.Initialized = true;
					viewModel.ExecuteLoadAvatarsCommand ();
				}
			}
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			viewModel.PropertyChanged -= ViewModelPropertyChanged;
		}

		public override void Init ()
		{
			base.Init ();
			if (!viewModel.Initialized) {
				viewModel.Initialized = true;
				viewModel.ExecuteLoadAvatarsCommand ();
			}
		}
        
		void ViewModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Activity.RunOnUiThread (() => {
				switch (e.PropertyName) {
				case BaseViewModel.IsBusyPropertyName:
					if (viewModel.IsBusy)
						AndroidHUD.AndHUD.Shared.Show (Activity, viewModel.LoadingMessage, maskType: AndroidHUD.MaskType.Clear);
					else
						AndroidHUD.AndHUD.Shared.Dismiss (Activity);

					break;
				case ProfileViewModel.AvatarUrlName:
                        
                        Koush.UrlImageViewHelper.SetUrlDrawable(avatar, viewModel.AvatarUrl);

                        break;
				}
			});
		}

    }
}