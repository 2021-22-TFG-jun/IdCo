using Android.App;
using Android.OS;


namespace IdCo.Droid
{
    [Activity(Theme = "@style/MyTheme.Start", Icon = "@drawable/logo", MainLauncher = true, NoHistory = true, Label = "Identificación de conocidos")]
    public class StartActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            System.Threading.Thread.Sleep(500);
            StartActivity(typeof(MainActivity));
        }
    }
}