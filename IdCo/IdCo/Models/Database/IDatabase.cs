using System.Collections.Generic;

namespace IdCo.Models.Database
{
    /// <summary>
    /// Interfaz para la Base de Datos.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Crear una tabla sino existe en la BD
        /// </summary>
        void CreateTable();
        /// <summary>
        /// Eliminar una tabla de la BD, siempre y cuando esta exista.
        /// </summary>
        void DropTable();
        /// <summary>
        /// Contar el numero de elementos que tiene una tabla en una BD.
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// Guardar una persona en la base de datos.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        int SavePerson(Person.Person person);
        /// <summary>
        /// Eliminar una persona de la base de datos.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        int RemovePerson(Person.Person person);
        /// <summary>
        /// Actualizar una persona de la base de datos.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        int UpdatePerson(Person.Person person);
        /// <summary>
        /// Buscar una persona en funcion de su Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Person.Person SearchPersonById(int id);
        /// <summary>
        /// Buscar una persona/as en funcion de su nombre.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Person.Person> SearchPersonByName(string name);
        /// <summary>
        /// Buscar una persona/as en funcion de su apellido.
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        List<Person.Person> SearchPersonByLastName(string lastName);
        /// <summary>
        /// Buscar una persona/as en función de su nombre y su apellido.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        List<Person.Person> SearchPersonByNameAndLastName(string name, string lastName);
        /// <summary>
        /// Buscar todas las personas que estan en la base de datos.
        /// </summary>
        /// <returns></returns>
        List<Person.Person> SearchAllPersons();
        /// <summary>
        /// Buscar un objeto Person por su PersonId.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Person.Person SearchPersonByPersonId(string personId);
        /// <summary>
        /// Buscar un objeto Person por su FaceId.
        /// </summary>
        /// <param name="faceId"></param>
        /// <returns></returns>
        Person.Person SearchPersonByFaceId(string faceId);
    }
}
