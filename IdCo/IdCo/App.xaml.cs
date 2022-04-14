using System;
using System.IO;
using Xamarin.Forms;

using IdCo.Views;
using IdCo.Models.Database;
using IdCo.Helpers;
using IdCo.Services.Face;

namespace IdCo
{
    public partial class App : Application
    {
        static Database database;
        /// <summary>
        /// Inicializar la Base de Datos sino existe.
        /// </summary>
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Settings.BDName);
                    
                }
                database.CreateTable();
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            CarouselPage carousel = new CarouselPage();
            carousel.Children.Add(new StartPage());
            carousel.Children.Add(new StartRecognitionPage());
            carousel.Children.Add(new CameraPage());
            carousel.Children.Add(new StartSearchPage());

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
