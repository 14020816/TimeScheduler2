using System;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace TimeSccheduler
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Intent TimeSchedulerService;
        bool isStarted = false;
        Button stopServiceButton;
        Button startServiceButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;
            Button button = FindViewById<Button>(Resource.Id.button2);
            button.Click += delegate { button.Text = "You clicked me"; };
            AudioManager am = (AudioManager)this.GetSystemService(Context.AudioService);
            am.RingerMode = RingerMode.Silent;
            this.ChangeSchedulerBaseOnTime();
            TimeSchedulerService = new Intent(this, typeof(TimestampService));
            stopServiceButton = FindViewById<Button>(Resource.Id.button4);
            startServiceButton = FindViewById<Button>(Resource.Id.button5);

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(changeTone);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

            if (isStarted)
            {
                stopServiceButton.Click += StopServiceButton_Click;
                stopServiceButton.Enabled = true;
                startServiceButton.Enabled = false;
            }
            else
            {
                startServiceButton.Click += StartServiceButton_Click;
                startServiceButton.Enabled = true;
                stopServiceButton.Enabled = false;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
        private void ChangeSchedulerBaseOnTime()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            string currentTime = (string.Format("Current Time: {0}", now));
            AudioManager am = (AudioManager)this.GetSystemService(Context.AudioService);
            
            if (now.Hour > 8 || now.Hour < 18)
            {
                am.RingerMode = RingerMode.Silent;
            }
            else
            {
                am.RingerMode = RingerMode.Normal;
            }
        }
        void StopServiceButton_Click(object sender, System.EventArgs e)
        {
            stopServiceButton.Click -= StopServiceButton_Click;
            stopServiceButton.Enabled = false;

            //Log.Info(TAG, "User requested that the service be stopped.");
            StopService(TimeSchedulerService);
            isStarted = false;

            startServiceButton.Click += StartServiceButton_Click;
            startServiceButton.Enabled = true;
        }

        void StartServiceButton_Click(object sender, System.EventArgs e)
        {
            startServiceButton.Enabled = false;
            startServiceButton.Click -= StartServiceButton_Click;

            StartService(TimeSchedulerService);
            //Log.Info(TAG, "User requested that the service be started.");

            isStarted = true;
            stopServiceButton.Click += StopServiceButton_Click;

            stopServiceButton.Enabled = true;
        }
        private  void  changeTone(object source, ElapsedEventArgs e)
        {
            AudioManager am = (AudioManager)this.GetSystemService(Context.AudioService);
            if(am.RingerMode == RingerMode.Normal)
            {
                am.RingerMode = RingerMode.Silent;
            }
            else
            {
                am.RingerMode = RingerMode.Normal;
            }
        }
    }
}

