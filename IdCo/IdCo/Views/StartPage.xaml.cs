using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdCo.Services.Face;
using System.Threading.Tasks;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        PersonGroupService personGroupService;
        public StartPage()
        {
            InitializeComponent();

#if DEBUG
            Application.Current.Properties.Remove("ResourceName");
            Application.Current.Properties.Remove("ResourceKey");
#endif
            if (!Application.Current.Properties.ContainsKey("ResourceName") || !Application.Current.Properties.ContainsKey("ResourceKey"))
            {
                lbl_start.IsVisible = false;
                LoginDataPanel.IsVisible = true;
            }
            else
            {
                lbl_start.IsVisible = true;
                LoginDataPanel.IsVisible = false;
            }
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