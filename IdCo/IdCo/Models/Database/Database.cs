using SQLite;
using System.Collections.Generic;
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
            person.Name = person.Name.ToUpper();
            person.LastName = person.LastName.ToUpper();
            Task<int> num = database.InsertAsync(person);
            return num.Result;
        }
        /// <summary>
        /// Actualizar un objeto persona em la base de datos.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public int UpdatePerson(Person.Person person)
        {
            Task<int> num = database.UpdateAsync(person);
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
        /// Buscar un objeto Persona/as por su nombre.
        /// </summary>
        /// <param name="name">Name del objeto Person a buscar</param>
        /// <returns>Person buscada</returns>
        public List<Person.Person> SearchPersonByName(string name)
        {
            string upperName = name.Trim().ToUpper();
            Task<List<Person.Person>> persons = database.Table<Person.Person>().Where(x => x.Name.Contains(upperName)).ToListAsync();
            return persons.Result;
        }
        /// <summary>
        /// Buscar una persona/as en funcion de su apellido.
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public List<Person.Person> SearchPersonByLastName(string lastName)
        {
            string upperLastName = lastName.Trim().ToUpper();
            Task<List<Person.Person>> persons = database.Table<Person.Person>().Where(x => x.LastName.Contains(upperLastName)).ToListAsync();
            return persons.Result;
        }
        /// <summary>
        /// Buscar una persona/as en función de su nombre y su apellido.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public List<Person.Person> SearchPersonByNameAndLastName(string name, string lastName)
        {
            string upperName = name.Trim().ToUpper();
            string upperLastName = lastName.Trim().ToUpper();
            Task<List<Person.Person>> persons = database.Table<Person.Person>().Where(x => x.Name.Contains(upperName)).Where(y => y.LastName.Contains(upperLastName)).ToListAsync();            
            return persons.Result;
        }
        /// <summary>
        /// Obtener todos los registros Person de la Base de Datos.
        /// </summary>
        /// <returns></returns>
        public List<Person.Person> SearchAllPersons()
        {
            Task<List<Person.Person>> persons = database.Table<Person.Person>().ToListAsync();
            return persons.Result;
        }
        /// <summary>
        /// Buscar un objeto Person por su PersonId.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Person.Person SearchPersonByPersonId(string personId)
        {
            Task<Person.Person> person = database.Table<Person.Person>().Where(x => x.PersonId == personId).FirstOrDefaultAsync();
            return person.Result;
        }
        /// <summary>
        /// Buscar un objeto Person por su FaceId.
        /// </summary>
        /// <param name="faceId"></param>
        /// <returns></returns>
        public Person.Person SearchPersonByFaceId(string faceId)
        {
            Task<Person.Person> person = database.Table<Person.Person>().Where(x => x.FaceId == faceId).FirstOrDefaultAsync();
            return person.Result;
        }
    }
}
