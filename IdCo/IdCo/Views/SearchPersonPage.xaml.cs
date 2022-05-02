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
            if (App.Database != null)
            {
                peopleView = App.Database.SearchAllPersons();
            }
            RefreshPeopleList(peopleView);
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
            List<Person> people = null;
            string filtro = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(filtro))
            {
                people = App.Database.SearchAllPersons();
            }
            else
            {
                people = SearchInDBByCriteriaField(filtro);
            }

            RefreshPeopleList(people);
        }
        /// <summary>
        /// Búsqueda de personas en la Base de Datos que conincidan con
        /// un filtro y campo especifico.
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        private List<Person> SearchInDBByCriteriaField(string filtro)
        {
            List<Person> people = null;
            string[] delimitadores = { " " };
            string[] fullname = filtro.Split(delimitadores, System.StringSplitOptions.RemoveEmptyEntries);
            bool filterByName = NameRBtn.IsChecked;
            bool filterByLastname = LastNameRBtn.IsChecked;

            if (filterByName)
            {
                people = App.Database.SearchPersonByName(fullname[0]);
            }
            else if (filterByLastname)
            {
                people = App.Database.SearchPersonByLastName(fullname[0]);
            }
            else
            {
                people = App.Database.SearchAllPersons();
            }
            return people;
        }
        /// <summary>
        /// Refrescar la lista con los nuevos elementos filtrados.
        /// </summary>
        /// <param name="people"></param>
        private void RefreshPeopleList(List<Person> people)
        {
            listViewPanel.ItemsSource = people;
            CountLbl.Text = "Total: " + people.Count;
        }
        /// <summary>
        /// Volver a la vista anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        /// <summary>
        /// Realizar la búsqueda cuando se cambie el tipo de filtro entre nombre y apellido.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FiltersRadioBtn_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string filtro = SearchCriteraSb.Text;
            if(!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                List<Person> people = SearchInDBByCriteriaField(filtro);
                RefreshPeopleList(people);
            }
        }
    }
}