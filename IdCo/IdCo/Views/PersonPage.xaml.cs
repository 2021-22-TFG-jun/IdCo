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

        }
    }
}