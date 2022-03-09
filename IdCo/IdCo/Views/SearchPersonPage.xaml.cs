using IdCo.Models.Person;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPersonPage : ContentPage
    {
        public SearchPersonPage()
        {
            InitializeComponent();
        }

        private async void SearchBtn_Clicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text;
            Person person = null;

            try
            {
                //TODO: Validar el apellido en la BD
                if (!string.IsNullOrEmpty(name))
                {
                    person = App.Database.SearchPersonByName(name);
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
                await DisplayAlert("Error", $"No existe ningún registro con el Nombre \"{name}\"", "OK");
            }
        }

        private async void TrashBtn_Clicked(object sender, EventArgs e)
        {
            NameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            PhotoBD.Source = null;

            await Navigation.PopAsync();
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            NameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            PhotoBD.Source = null;

            await Navigation.PopAsync();
        }
    }
}