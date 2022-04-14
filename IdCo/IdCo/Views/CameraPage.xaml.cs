using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdCo.Services.Camera;
using Plugin.Media.Abstractions;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {

        readonly ICameraService camera;

        /// <summary>
        /// Inicializar el servicio de la camara
        /// </summary>
        public CameraPage()
        {
            InitializeComponent();
            camera = new CameraService();
        }

        /// <summary>
        /// Acceder a la camara, sacar foto y mandar la foto a otra ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CameraBtn_Clicked(object sender, EventArgs e)
        {
            StoreCameraMediaOptions options = camera.StoreCameraOptions();
            MediaFile photo = await camera.TakePhoto(options);

            if(photo != null)
            {
                await Navigation.PushAsync(new PersonPage(photo));
            }
        }
    }
}