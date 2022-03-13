using System.Threading.Tasks;

namespace IdCo.Services.Face
{
    public interface IPersonGroupService
    {
        /// <summary>
        /// Crear un PersonGroup
        /// </summary>
        /// <param name="personGroupId"></param>
        /// <param name="name"></param>
        /// <param name="userData"></param>
        /// <param name="recognitionModel"></param>
        /// <returns>200 OK</returns>
        Task<string> Create(string personGroupId, string name, string userData, string recognitionModel);
        /// <summary>
        /// Entrenar un PersonGroup
        /// </summary>
        /// <param name="personGroupId"></param>
        /// <returns>202 OK</returns>
        Task<string> Train(string personGroupId);
        /// <summary>
        /// Eliminar un PersonGroup
        /// </summary>
        /// <param name="personGroupId"></param>
        /// <returns>200 OK</returns>
        Task<string> Delete(string personGroupId);

    }
}
