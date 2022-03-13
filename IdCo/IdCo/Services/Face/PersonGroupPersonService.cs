using System;
using System.IO;
using System.Threading.Tasks;

namespace IdCo.Services.Face
{
    /// <summary>
    /// Clase que identifica las características obligatorias de una Persona.
    /// </summary>
    public class PersonGroupPerson
    {
        /// <summary>
        /// Nombre de la persona
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Datos adicionales sobre la persona.
        /// </summary>
        public string UserData { get; set; }
    }
    public class PersonGroupPersonService : IPersonGroupPersonService
    {
        public Task<string> AddFace(string personId, Stream photo, string personGroupId = null,  string detectionModel = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> Create(string name, string userData, string personGroupId = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> Delete(string personId, string personGroupId = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteFace(string personId, string faceId, string personGroupId = null)
        {
            throw new NotImplementedException();
        }
    }
}
