using IdCo.Models.Person;
using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IdCo.Services.Face;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace IdCo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPersonPage : ContentPage
    {
        public List<Person> peopleView { get; set; }
        public SearchPersonPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Cargar todos los elementos de la Base de Datos
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var count = App.Database.Count();
            peopleView = App.Database.SearchAllPersons();
            CountLbl.Text = "Total: " + count;
            listViewPanel.ItemsSource = peopleView;
        }
        /// <summary>
        /// Buscar una persona en la BD, por su nombre y/o apellido.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBtn_Clicked(object sender, EventArgs e)
        {
            listViewPanel.IsVisible = false;
            CountLbl.IsVisible = false;

        }
        /// <summary>
        /// Redirigir una vista de eliminación con la persona seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listViewPanel_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Person person = (Person)e.Item;
            await Navigation.PushAsync(new DeletePersonPage(person));
        }
        /// <summary>
        /// Búsqueda de personas en la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCriteraSb_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool filterByName = NameRBtn.IsChecked;
            bool filterByLastname = LastNameRBtn.IsChecked;
            List<Person> people = null;
            string filtro = e.NewTextValue;
            string[] delimitadores = { " " };
            
            if (string.IsNullOrWhiteSpace(filtro))
            {
                people = App.Database.SearchAllPersons();
            }
            else
            {
                string[] fullname = filtro.Split(delimitadores, System.StringSplitOptions.RemoveEmptyEntries);      
                if (filterByName)
                {
                    people = App.Database.SearchPersonByName(fullname[0]);
                }else if (filterByLastname)
                {
                    people = App.Database.SearchPersonByLastName(fullname[0]);
                }
                else
                {
                    people = App.Database.SearchAllPersons();
                }
            }

            listViewPanel.ItemsSource = people;
            CountLbl.Text = "Total: " + people.Count;
        }
    }
}