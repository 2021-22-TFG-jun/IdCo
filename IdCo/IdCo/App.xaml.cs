﻿using System;
using System.IO;
using Xamarin.Forms;

using IdCo.Views;
using IdCo.Models.Database;
using IdCo.Helpers;

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
                //TODO: Descomentar para reiniciar la tabla Person en BD
                //database.DropTable();
                database.CreateTable();
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            // Nuevo estilo de navegabilidad, desplazar vistas de izq - der y der - izq
            CarouselPage carousel = new CarouselPage();
            carousel.Children.Add(new StartPage());
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
