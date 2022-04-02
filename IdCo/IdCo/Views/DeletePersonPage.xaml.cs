using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IdCo.Models.Person;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletePersonPage : ContentPage
    {
        Person person = null;
        public DeletePersonPage(Person person)
        {
            InitializeComponent();
            this.person = person;
        }
    }
}