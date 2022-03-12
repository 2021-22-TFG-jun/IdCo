using SQLite;
using System.Threading.Tasks;

namespace IdCo.Models.Database
{
    public class Database : IDatabase
    {
        SQLiteAsyncConnection database;
        /// <summary>
        /// Inicializar la conexión de la base de datos.
        /// </summary>
        /// <param name="name">Nombre del path + base de datos</param>
        public Database(string name)
        {
            database = new SQLiteAsyncConnection(name);
        }
        /// <summary>
        /// Contar los elementos que hay dentro de la tabla Person en la base de datos.
        /// </summary>
        /// <returns>Numero de elementos de tipo Person en la BD</returns>
        public int Count()
        {
            Task<int> count = database.Table<Person.Person>().CountAsync();
            return count.Result;
        }
        /// <summary>
        /// Crear la tabla Person en la base de datos.
        /// </summary>
        public void CreateTable()
        {
            database.CreateTableAsync<Person.Person>().Wait();
        }
        /// <summary>
        /// Eliminar la tabla Person en la base de datos.
        /// </summary>
        public void DropTable()
        {
            database.DropTableAsync<Person.Person>().Wait();
        }
        /// <summary>
        /// Eliminar un objeto Person de la base de datos.
        /// </summary>
        /// <param name="person">Person a eliminar de la BD</param>
        /// <returns>Numero de filas de la BD eliminadas</returns>
        public int RemovePerson(Person.Person person)
        {
            Task<int> num = database.DeleteAsync(person);
            return num.Result;
        }
        /// <summary>
        /// Guardar un objeto Person en la base de datos.
        /// </summary>
        /// <param name="person">Guardar un objeto Person en la BD</param>
        /// <returns>numero de filas añadidas en la BD</returns>
        public int SavePerson(Person.Person person)
        {
            Task<int> num = database.InsertAsync(person);
            return num.Result;
        }
        /// <summary>
        /// Buscar un objeto Person por su id.
        /// </summary>
        /// <param name="id">Id del objeto Person a buscar</param>
        /// <returns>Person buscada</returns>
        public Person.Person SearchPersonById(int id)
        {
            Task<Person.Person> person = database.Table<Person.Person>().Where(x => x.ID == id).FirstOrDefaultAsync();
            return person.Result;
        }
        /// <summary>
        /// Buscar un objeto Person por su nombre.
        /// </summary>
        /// <param name="name">Name del objeto Person a buscar</param>
        /// <returns>Person buscada</returns>
        public Person.Person SearchPersonByName(string name)
        {
            Task<Person.Person> person = database.Table<Person.Person>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return person.Result;
        }

        public Person.Person SearchPersonByLastName(string lastName)
        {
            Task<Person.Person> person = database.Table<Person.Person>().Where(x => x.LastName == lastName).FirstOrDefaultAsync();
            return person.Result;
        }

        public Person.Person SearchPersonByNameAndLastName(string name, string lastName)
        {
            Task<Person.Person> person = database.Table<Person.Person>().Where(x => x.Name == name).Where(y => y.LastName == lastName).FirstOrDefaultAsync();            
            return person.Result;
        }
    }
}
