using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IdCo.Models.Person;
using System.IO;
using IdCo.Services.Face;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletePersonPage : ContentPage
    {
        Person person = null;
        PersonGroupPersonService personGroupPersonService;
        public DeletePersonPage(Person person)
        {
            InitializeComponent();
            this.person = person;
            personGroupPersonService = new PersonGroupPersonService();
        }

        /// <summary>
        /// Cargar la persona seleccionada.
        /// </summary>
        protected override void OnAppearing()
        {
            byte[] photo = person.Photo;
            MemoryStream memoryStream = new MemoryStream(photo);
            Image image = new Image { Source = ImageSource.FromStream(() => memoryStream) };

            FacePhoto.Source = image.Source;
            PersonNameLbl.Text = "Nombre: " + person.Name;
            PersonLastnameLbl.Text = "Apellido: " + person.LastName;
        }
        /// <summary>
        /// Volver a la pagina anterior sin realizar ninguna accion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        /// <summary>
        /// Eliminar a la persona y volver a la pagina anterior.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TrashBtn_Clicked(object sender, EventArgs e)
        {
            App.Database.RemovePerson(person);
            await personGroupPersonService.Delete(person.PersonId);

            await Navigation.PopAsync();
        }
    }
}