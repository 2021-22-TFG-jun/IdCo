using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdCo.Services.Face;
using System.Threading.Tasks;
using System.IO;
using IdCo.Helpers;
using Xamarin.Essentials;
using System.Collections.Generic;
using IdCo.Models.Person;
using IdCo.Models.Face;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        PersonGroupService personGroupService;
        PersonGroupPersonService personGroupPersonService;
        FaceService faceService;

        readonly string loginFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Login.txt");
        public StartPage()
        {
            InitializeComponent();

#if DEBUG
            Application.Current.Properties.Remove("ResourceName");
            Application.Current.Properties.Remove("ResourceKey");
#endif
            if (!Application.Current.Properties.ContainsKey("ResourceName") || !Application.Current.Properties.ContainsKey("ResourceKey"))
            {
                if (!ExistLoginDataFile())
                {
                    lbl_start.IsVisible = false;
                    LoginDataPanel.IsVisible = true;
                    return;
                }
                else
                {
                    ReadLoginDataFile();
                }
            }
            lbl_start.IsVisible = true;
            LoginDataPanel.IsVisible = false;
        }
        /// <summary>
        /// Comprobar si el campo del nombre del recurso esta completado.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckResourceNameField()
        {
            string resourceName = ResourceNameEntry.Text;

            if (!string.IsNullOrEmpty(resourceName) || !string.IsNullOrWhiteSpace(resourceName))
            {
                Application.Current.Properties["ResourceName"] = resourceName;
                await Application.Current.SavePropertiesAsync();
                return true;
            }
            else
            {
                await DisplayAlert("Error", "Introduce el nombre del recurso para continuar", "OK");
            }

            return false;
        }
        /// <summary>
        /// Comprobar si el campo de la clave del recurso esta completado.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckResourceKeyField()
        {
            string resourceKey = ResourceKeyEntry.Text;

            if (!string.IsNullOrEmpty(resourceKey))
            {
                Application.Current.Properties["ResourceKey"] = resourceKey;
                await Application.Current.SavePropertiesAsync();
                return true;
            }
            else
            {
                await DisplayAlert("Error", "Introduce la clave del recurso para continuar", "OK");
            }

            return false;
        }
        /// <summary>
        /// Crear un fichero para el almacenamiento de los datos de inicio de sesion en el recurso.
        /// </summary>
        private void CreateLoginDataFile()
        {
            string resourceName = (string)Application.Current.Properties["ResourceName"];
            string resourceKey = (string)Application.Current.Properties["ResourceKey"];
            if (!ExistLoginDataFile())
            {
                StreamWriter writer = new StreamWriter(loginFilePath);
                writer.WriteLine(resourceName);
                writer.WriteLine(resourceKey);
                writer.Close();
            }
        }
        /// <summary>
        /// Leer el archivo de inicio de sesion si existe
        /// </summary>
        private void ReadLoginDataFile()
        {
            if (ExistLoginDataFile())
            {
                StreamReader reader = new StreamReader(loginFilePath);
                Application.Current.Properties["ResourceName"] = reader.ReadLine();
                Application.Current.Properties["ResourceKey"] = reader.ReadLine();
                reader.Close();
            }
        }
        /// <summary>
        /// Determinar si existe el fichero de inicio de sesión.
        /// </summary>
        /// <returns></returns>
        private bool ExistLoginDataFile()
        {
            
            if (!File.Exists(loginFilePath))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Pasar a la siguiente vista siempre y cuando se haya rellenado los campos
        /// obligatorios del recurso.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NextBtn_Clicked(object sender, EventArgs e)
        {
            bool flag1 = await CheckResourceNameField().ConfigureAwait(true);
            bool flag2 = await CheckResourceKeyField().ConfigureAwait(true);

            if (flag1 && flag2)
            {
                bool access = await ResourceAccess().ConfigureAwait(true);
                if (access)
                {
                    CreateLoginDataFile();
                    LoginDataPanel.IsVisible = false;
                    ImportDataPanel.IsVisible = true;
                    ImportDBFrame.IsVisible = true;
                }
            }
        }
        /// <summary>
        /// Determinar si las claves de acceso con correctas
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ResourceAccess()
        {
            personGroupService = new PersonGroupService();
            string status = await personGroupService.Create();
            if (string.Equals(status, "OK") || string.Equals(status, "Conflict"))
            {
                CheckSettings.CorrectResourceAccess = true;
                return true;
            }
            else if (string.Equals(status, "Unauthorized"))
            {
                await DisplayAlert("Error", "La clave de subscripción es invalida o esta bloqueada. Contacte con su proveedor.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Se ha producido un error. Contacte con su proveedor.", "OK");
            }
            return false;
        }
        /// <summary>
        /// Comprobar si ya existe una base de datos creada.
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private bool IsDataBaseImported(string ruta)
        {
            if (File.Exists(ruta))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Seleccionar un archivo de base de datos .db3 e importarlo a un directorio
        /// interno de la aplicación
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private async Task<bool> ImportDataBase(string ruta)
        {
            try
            {
                var file = await FilePicker.PickAsync();

                if (file != null)
                {
                    if (file.FileName.EndsWith("db3", StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(file.FullPath, ruta);
                        Settings.BDName = ruta;
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        /// <summary>
        /// Boton para controlar el evento de importación de base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ImportBtn_Clicked(object sender, EventArgs e)
        {
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db3");

            if (IsDataBaseImported(ruta))
            {
                await DisplayAlert("", "Ya existe una base datos creada, se omitira este paso.", "OK");
                CheckSettings.CorrectDBAccess = true;
            }
            else
            {
                bool checkImport = await ImportDataBase(ruta).ConfigureAwait(true);
                if (!checkImport)
                {
                    await DisplayAlert("", "Se a producido un error al importar la base de datos. Vuelve a intentarlo.", "Ok");
                }
                else
                {
                    ShowActivityIndicator(false);
                    await SyncDBwithAPI().ConfigureAwait(true);
                    CheckSettings.CorrectDBAccess = true;
                    ShowActivityIndicator(true);
                }
            }
        }
        /// <summary>
        /// Mostrar el indicados de carga de actividad
        /// </summary>
        /// <param name="finish"></param>
        private void ShowActivityIndicator(bool finish)
        {
            ImportDataPanel.IsVisible = false;
            ImportDBFrame.IsVisible = false;
            if (!finish)
            {
                ActivityIndicatorPanel.IsVisible = true;
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;
            }
            else
            {
                ActivityIndicatorPanel.IsVisible = false;
                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;
                lbl_start.IsVisible = true;
            }
        }
        /// <summary>
        /// Sincronizar la base de datos con el servicio API.
        /// </summary>
        private async Task SyncDBwithAPI()
        {
            List<Person> people = App.Database.SearchAllPersons();
            faceService = new FaceService();
            personGroupPersonService = new PersonGroupPersonService();
            foreach(Person person in people)
            {
                Face[] detectFaces = await faceService.Detect(true, new MemoryStream(person.Photo));
                System.Threading.Thread.Sleep(3000);
                if (detectFaces.Length.Equals(1))
                {
                    var personId = await personGroupPersonService.Create(person.Name, person.LastName);
                    System.Threading.Thread.Sleep(3000);
                    var faceId = await personGroupPersonService.AddFace(personId.PersonId.ToString(), new MemoryStream(person.Photo));
                    System.Threading.Thread.Sleep(3000);
                    person.PersonId = personId.PersonId.ToString();
                    person.FaceId = faceId.PersistedFaceId.ToString();
                    App.Database.UpdatePerson(person);
                }
                else
                {
                    App.Database.RemovePerson(person);
                } 
            }
            await personGroupService.Train().ConfigureAwait(true);
        }
        /// <summary>
        /// Controlar el evento de no importación de base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.BDName))
            {
                Settings.BDName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "People.db3");
            }

            lbl_start.IsVisible = true;
            ImportDataPanel.IsVisible = false;
            ImportDBFrame.IsVisible = false;
            CheckSettings.CorrectDBAccess = true;
        }
    }
}