using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartRecognitionPage : ContentPage
    {
        public StartRecognitionPage()
        {
            InitializeComponent();
        }

        private async void PlayBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecognitionPage());
        }
    }
}