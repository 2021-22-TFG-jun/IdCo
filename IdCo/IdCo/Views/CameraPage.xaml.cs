using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();
        }

        private async void CameraBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PersonPage());
        }
    }
}