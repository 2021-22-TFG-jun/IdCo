using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartSearchPage : ContentPage
    {
        public StartSearchPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Redireccionar a la página de busqueda de personas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SearchPersonPage());
        }
    }
}