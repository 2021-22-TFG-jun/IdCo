using IdCo.Models.Person;
using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IdCo.Services.Face;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPersonPage : ContentPage
    {
        Person person = null;
        PersonGroupPersonService personGroupPersonService;
        public SearchPersonPage()
        {
            InitializeComponent();
            personGroupPersonService = new PersonGroupPersonService();
        }

        /// <summary>
        /// Buscar una persona en la BD, por su nombre y/o apellido.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchBtn_Clicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text;
            string lastName = LastNameEntry.Text;

            try
            {
                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(lastName))
                {
                    if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(lastName))
                    {
                        person = App.Database.SearchPersonByNameAndLastName(name.Trim().ToLower(), lastName.Trim().ToLower());
                    }
                    else if(!string.IsNullOrEmpty(name))
                    {
                        person = App.Database.SearchPersonByName(name.Trim().ToLower());
                    }
                    else
                    {
                        person = App.Database.SearchPersonByLastName(lastName.Trim().ToLower());
                    }
                        
                    byte[] photo = person.Photo;
                    MemoryStream memoryStream = new MemoryStream(photo);
                    Image image = new Image { Source = ImageSource.FromStream(() => memoryStream) };
                    PhotoBD.IsVisible = true;
                    PhotoBD.Source = image.Source;

                    FirstStack.IsVisible = false;
                    SecondStack.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Error", "Introduce algún criterio de búsqueda", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Error", $"No existe ningún registro para los criterios de búqueda introducidos", "OK");
            }
        }
        /// <summary>
        /// Eliminar un objeto Person anteriormente buscado de la BD permanentemente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TrashBtn_Clicked(object sender, EventArgs e)
        {
            App.Database.RemovePerson(person);
            await personGroupPersonService.Delete(person.PersonId);

            NameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            PhotoBD.Source = null;

            await Navigation.PopAsync();
        }
        /// <summary>
        /// Volver a la ventana anterior sin realizar ningún tipo de cambio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            NameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            PhotoBD.Source = null;

            await Navigation.PopAsync();
        }
    }
}