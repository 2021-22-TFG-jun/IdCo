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
        public Task<string> AddFace(string personGroupId, string personId, string detectionModel, Stream photo)
        {
            throw new NotImplementedException();
        }

        public Task<string> Create(string personGroupId, string name, string userData)
        {
            throw new NotImplementedException();
        }

        public Task<string> Delete(string personGroupId, string personId)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteFace(string personGroupId, string personId, string faceId)
        {
            throw new NotImplementedException();
        }
    }
}
