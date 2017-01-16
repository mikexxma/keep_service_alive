using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using Android.Icu.Util;
using Android.Runtime;
using System.Threading.Tasks;

namespace Gesture_Demo
{
    [Activity(Label = "Gesture_Demo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button bt1;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            bt1 = FindViewById<Button>(Resource.Id.button1);
            bt1.Click += Bt1_Click;
        }

        private void Bt1_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(NotifyEvent));
            StartService(intent);
        }

        //protected override void OnDestroy()
        //{
        //    //Notificator not = new Notificator();
        //    //not.ShowNotification(this);

        //    Intent intent = new Intent(this, typeof(NotifyEvent));
        //    StartService(intent);
        //    System.Console.WriteLine("OnDestroy");
        //    base.OnDestroy();
        //}


    }


    [Service]
    class NotifyEvent : Service
    {
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            new Task(() => {

                PendingIntent pIntent = PendingIntent.GetActivity(this, 0, intent, 0);
                Notification.Builder builder = new Notification.Builder(this);
                builder.SetContentTitle("hello");
                builder.SetContentText("hello");
                builder.SetSmallIcon(Resource.Drawable.Icon);
                builder.SetPriority(1);
                builder.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate);
                builder.SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis());
                Notification notifikace = builder.Build();
                NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notifikace);

            }).Start();
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override void OnTaskRemoved(Intent rootIntent)
        {
            Intent restartService = new Intent(ApplicationContext, typeof(NotifyEvent));
            restartService.SetPackage(PackageName);
            var pendingServiceIntent = PendingIntent.GetService(ApplicationContext, 0, restartService, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarm = (AlarmManager)ApplicationContext.GetSystemService(Context.AlarmService);
            alarm.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 1000, pendingServiceIntent);

            System.Console.WriteLine("service OnTaskRemoved");
            base.OnTaskRemoved(rootIntent);
        }
    }

    public class Notificator
    {
        public void ShowNotification(Context context)
        {
            Intent intent = new Intent(context, typeof(NotifyEvent));
            var pendingServiceIntent = PendingIntent.GetService(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarm = (AlarmManager)context.GetSystemService(Context.AlarmService);
            alarm.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 1000, pendingServiceIntent);
        }
    }

    
}

