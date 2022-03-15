using SQLite;

namespace IdCo.Models.Person
{
    /// <summary>
    /// Estructura de datos para asociar un ID (único), personId, faceId, name, lastname y photo.
    /// </summary>
    public class Person
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        [Column("PersonId")]
        public string PersonId { get; set; }
        [Column("FaceId")]
        public string FaceId { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Column("Photo")]
        public byte[] Photo { get; set; }
    }
}
