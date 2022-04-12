using System;
using System.IO;
using Plugin.Media.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IdCo.Models.Person;
using IdCo.Services.Face;
using IdCo.Models.Face;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace IdCo.Views
{ 
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonPage : ContentPage
    {
        MediaFile photo;
        PersonGroupPersonService personGroupPersonService;
        FaceService faceService;
        PersonGroupService personGroupService;

        public PersonPage(MediaFile photo)
        {
            InitializeComponent();
            this.photo = photo;

            personGroupPersonService = new PersonGroupPersonService();
            faceService = new FaceService();
            personGroupService = new PersonGroupService();
        }
        /// <summary>
        /// Cargar y comprobar los elementos antes de la visualizacion en la vista.
        /// </summary>
        protected override async void OnAppearing()
        {
            Face[] detectFaces = await faceService.Detect(true, photo.GetStream());
            if (!detectFaces.Any())
            {
                await DisplayAlert("Error", "No se ha detectado ningun rostro, vuelve a intentarlo", "OK");
                await Navigation.PopAsync();
            }

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
            bool checkInputs = await AreNameAndLastnameEntryCorrect();
            if (!checkInputs)
                return;

            try
            {
                await AddNewPersonToDBAndAPI_TrainGroup();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error: {ex.Message}");
#endif
            }
            finally
            {
                PhotoImg.Source = null;
                NameEntry.Text = string.Empty;
                LastNameEntry.Text = string.Empty;
            }

            await Navigation.PopAsync();
        }
        /// <summary>
        /// Comprobar si se han introducido los campos obligatorios (nombre y apellido)
        /// En caso negativo, mostrar una alerta.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> AreNameAndLastnameEntryCorrect()
        {
            if(string.IsNullOrEmpty(NameEntry.Text) || string.IsNullOrEmpty(LastNameEntry.Text))
            {
                if (string.IsNullOrEmpty(NameEntry.Text) && string.IsNullOrEmpty(LastNameEntry.Text))
                    await DisplayAlert("Error", "Introduce el nombre y el apellido de la persona", "OK");
                else if (string.IsNullOrEmpty(NameEntry.Text))
                    await DisplayAlert("Error", "Introduce el nombre de la persona", "OK");
                else 
                    await DisplayAlert("Error", "Introduce el apellido de la persona", "OK");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Añadir una nueva persona a la Base de Datos, al Face API Service y
        /// finalmente entrenar el conjunto.
        /// </summary>
        private async Task AddNewPersonToDBAndAPI_TrainGroup()
        {
            string name = NameEntry.Text;
            string lastName = LastNameEntry.Text;

            byte[] photoByte = this.ImageStreamToByteArray(photo.GetStream());
            var personId = await personGroupPersonService.Create(name, lastName);
            var faceId = await personGroupPersonService.AddFace(personId.PersonId.ToString(), photo.GetStream());

            Person person = new Person
            {
                Name = name,
                PersonId = personId.PersonId.ToString(),
                FaceId = faceId.PersistedFaceId.ToString(),
                LastName = lastName,
                Photo = photoByte
            };

            App.Database.SavePerson(person);

            await personGroupService.Train();
        }
    }
}