using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdCo.Services.Face;

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
        
        private async void NextBtn_Clicked(object sender, EventArgs e)
        {
            string resourceName = ResourceNameEntry.Text;
            string resourceKey = ResourceKeyEntry.Text;
            bool flag1 = false;
            bool flag2 = false;

            if(!string.IsNullOrEmpty(resourceName) || !string.IsNullOrEmpty(resourceKey))
            {
                if (!string.IsNullOrEmpty(resourceName))
                {
                    Application.Current.Properties["ResourceName"] = resourceName;
                    await Application.Current.SavePropertiesAsync();
                    flag1 = true;
                }
                else
                {
                    await DisplayAlert("Error", "Introduce el nombre del recurso para continuar", "OK");
                }

                if (!string.IsNullOrEmpty(resourceKey))
                {
                    Application.Current.Properties["ResourceKey"] = resourceKey;
                    await Application.Current.SavePropertiesAsync();
                    flag2 = true;
                }
                else
                {
                    await DisplayAlert ("Error", "Introduce la clave del recurso para continuar", "OK");
                }
            }
            else
            {
                await DisplayAlert ("Error", "Rellena los campos", "OK");
            }

            if(flag1 && flag2)
            {
                personGroupService = new PersonGroupService();
                //TODO: Descomentar para eliminar el recurso del API (reiniciarlo)
                //await personGroupService.Delete();
                await personGroupService.Create();

                lbl_start.IsVisible = true;
                LoginDataPanel.IsVisible = false;
            }
            
        }
    }
}