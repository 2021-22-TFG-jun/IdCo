using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdCo.Services.Face;
using System.Threading.Tasks;
using System.IO;
using IdCo.Helpers;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        PersonGroupService personGroupService;
        string loginFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Login.txt");
        public StartPage()
        {
            InitializeComponent();

#if DEBUG
            Application.Current.Properties.Remove("ResourceName");
            Application.Current.Properties.Remove("ResourceKey");
#endif
            if (!Application.Current.Properties.ContainsKey("ResourceName") || !Application.Current.Properties.ContainsKey("ResourceKey"))
            {
                //lbl_start.IsVisible = false;
                //LoginDataPanel.IsVisible = true;
                //return;
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
                CreateLoginDataFile();
                personGroupService = new PersonGroupService();
                string status = await personGroupService.Create();
                if (string.Equals(status, "OK") || string.Equals(status, "Conflict"))
                {
                    lbl_start.IsVisible = true;
                    LoginDataPanel.IsVisible = false;
                }
                else if (string.Equals(status, "Unauthorized"))
                {
                    await DisplayAlert("Error", "La clave de subscripción es invalida o esta bloqueada. Contacte con su proveedor.", "OK");
                }else
                {
                    await DisplayAlert("Error", "Se ha producido un error. Contacte con su proveedor.", "OK");
                }
            }
        }
    }
}