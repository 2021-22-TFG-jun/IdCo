using System.IO;
using System.Threading.Tasks;

namespace IdCo.Services.Face
{
    public interface IPersonGroupPersonService
    {
        /// <summary>
        /// Crear un Person en un PersonGroup.
        /// </summary>
        /// <param name="personGroupId">Id del PersonGroup</param>
        /// <param name="name">Nombre del Person</param>
        /// <param name="userData">Datos adicionales del Person</param>
        /// <returns>200 OK: personId de la Person creada</returns>
        Task<string> Create(string personGroupId, string name, string userData);
        /// <summary>
        /// Añadir un face a un Person ya creado.
        /// </summary>
        /// <param name="personId">Id del Person</param>
        /// <param name="photo">Imagen del rostro a vincular con el Person</param>
        /// <param name="personGroupId">Id del PersonGroup</param>
        /// <param name="detectionModel">Modelo de detección a usar</param>
        /// <returns>200 OK: persistedFaceId, id del rostro añadido</returns>
        Task<string> AddFace(string personId, Stream photo, string personGroupId, string detectionModel);
        /// <summary>
        /// Eliminar un Person de un PersonGroup.
        /// </summary>
        /// <param name="personGroupId">Id del PersonGroup</param>
        /// <param name="personId">Id del Person a eliminar</param>
        /// <returns>200 OK</returns>
        Task<string> Delete(string personGroupId, string personId);
        /// <summary>
        /// Eliminar un rostro de un Person.
        /// </summary>
        /// <param name="personGroupId">Id del PersonGroup</param>
        /// <param name="personId">Id del Person</param>
        /// <param name="faceId">Id del rostro a eliminar</param>
        /// <returns>200 OK</returns>
        Task<string> DeleteFace(string personGroupId, string personId, string faceId);
    }
}
