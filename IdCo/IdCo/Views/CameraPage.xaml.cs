﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdCo.Services.Camera;
using Plugin.Media.Abstractions;
using IdCo.Helpers;

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
            if (!CheckSettings.CorrectDBAccess || !CheckSettings.CorrectResourceAccess)
            {
                await DisplayAlert("Acceso denegado", "Complete la configuración inicial para acceder a este servicio", "OK");
                return;
            }
            
            StoreCameraMediaOptions options = camera.StoreCameraOptions();
            MediaFile photo = null;
            try
            {
                photo = await camera.TakePhoto(options);
            }catch(ArgumentNullException ex)
            {
                await DisplayAlert(" ", "La cámara no se encuentra disponible", "OK");
#if DEBUG
                Console.WriteLine($"Error: {ex.Message} - {ex.StackTrace}");
#endif
            }

            if(photo != null)
            {
                await Navigation.PushAsync(new PersonPage(photo));
            }
        }
    }
}