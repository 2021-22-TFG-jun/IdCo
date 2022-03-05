using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdCo.Views;

namespace IdCo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Nuevo estilo de navegabilidad, desplazar vistas de izq - der y der - izq
            CarouselPage carousel = new CarouselPage();
            carousel.Children.Add(new StartPage());
            carousel.Children.Add(new CameraPage());
            carousel.Children.Add(new PersonPage());

            MainPage = new NavigationPage(carousel);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
