using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdCo.Services.Camera;
using Plugin.Media.Abstractions;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        ICameraService camera;
        public CameraPage()
        {
            InitializeComponent();
            camera = new CameraService();
        }

        private async void CameraBtn_Clicked(object sender, System.EventArgs e)
        {
            StoreCameraMediaOptions options = camera.StoreCameraOptions();
            MediaFile photo = await camera.TakePhoto(options);
            await Navigation.PushAsync(new PersonPage());
        }
    }
}