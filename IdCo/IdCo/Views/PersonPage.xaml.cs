using System;
using System.IO;
using Plugin.Media.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IdCo.Models.Person;
using IdCo.Services.Face;
using IdCo.Models.Face;
using System.Linq;

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

            PhotoImg.Source = ImageSource.FromStream(() =>
            {
                var stream = photo.GetStream();
                return stream;
            });

            personGroupPersonService = new PersonGroupPersonService();
            faceService = new FaceService();
            personGroupService = new PersonGroupService();
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
                Face[] detectFaces = await faceService.Detect(true, photo.GetStream());
                if (detectFaces.Any())
                {
                    if (!string.IsNullOrEmpty(NameEntry.Text) && !string.IsNullOrEmpty(LastNameEntry.Text))
                    {
                        string name = NameEntry.Text;
                        string lowerName = name.Trim().ToLower();
                        string lastName = LastNameEntry.Text;
                        string lowerLastName = lastName.Trim().ToLower();
                        
                        byte[] photoByte = this.ImageStreamToByteArray(photo.GetStream());
                        var personId = await personGroupPersonService.Create(lowerName, lowerLastName);
                        var faceId = await personGroupPersonService.AddFace(personId.PersonId.ToString(), photo.GetStream());

                        Person person = new Person
                        {
                            Name = lowerName,
                            PersonId = personId.PersonId.ToString(),
                            FaceId = faceId.PersistedFaceId.ToString(),
                            LastName = lowerLastName,
                            Photo = photoByte
                        };

                        App.Database.SavePerson(person);

                        await personGroupService.Train();
                    }
                    else
                    {
                        await DisplayAlert("Error","Introduce el nombre y el apellido de la persona", "OK");
                    }
                }
                else
                {
                    Console.WriteLine("No se ha detectado ningun rostro");
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