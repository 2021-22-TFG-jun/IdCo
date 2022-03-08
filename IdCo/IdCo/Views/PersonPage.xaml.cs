using System;
using System.IO;
using Plugin.Media.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IdCo.Models.Person;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonPage : ContentPage
    {
        MediaFile photo;
        public PersonPage(MediaFile photo)
        {
            InitializeComponent();
            this.photo = photo;

            PhotoImg.Source = ImageSource.FromStream(() =>
            {
                var stream = photo.GetStream();
                return stream;
            });
        }
        /// <summary>
        /// Convertir un stream a byte array.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>Imagen en byte array</returns>
        private byte[] ImageStreamToByteArray(Stream stream)
        {
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Reiniciar los elementos de la vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AspaBtn_Clicked(object sender, EventArgs e)
        {
            PhotoImg.Source = null;
            NameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;

            await Navigation.PopAsync();
        }
        /// <summary>
        /// Añadir un objeto Person (nombre, apellidos, foto) a la tabla de la Base de Datos 
        /// y reiniciar los elementos de la vista al terminar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TickBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NameEntry.Text) && !string.IsNullOrEmpty(LastNameEntry.Text))
                {
                    byte[] photoByte = this.ImageStreamToByteArray(photo.GetStream());
                    Person person = new Person
                    {
                        Name = NameEntry.Text,
                        LastName = LastNameEntry.Text,
                        Photo = photoByte
                    };

                    App.Database.SavePerson(person);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                PhotoImg.Source = null;
                NameEntry.Text = string.Empty;
                LastNameEntry.Text = string.Empty;
            }

            await Navigation.PopAsync();
        }
    }
}