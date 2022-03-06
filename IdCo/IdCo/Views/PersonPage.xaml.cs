using Plugin.Media.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonPage : ContentPage
    {
        MediaFile photo;
        public PersonPage(MediaFile photo)
        {
            InitializeComponent();
            this.photo = photo;

            PhotoImg.Source = ImageSource.FromStream(() =>
            {
                var stream = photo.GetStream();
                return stream;
            });
        }

        private void AspaBtn_Clicked(object sender, System.EventArgs e)
        {

        }

        private void TickBtn_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}