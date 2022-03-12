using System.Threading.Tasks;

namespace IdCo.Services.Face
{
    public interface IPersonGroupService
    {
        /// <summary>
        /// Crear un PersonGroup
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="personGroupId"></param>
        /// <param name="name"></param>
        /// <param name="userData"></param>
        /// <param name="recognitionModel"></param>
        /// <returns>200 OK</returns>
        Task<TResponse> Create<TResponse>(string personGroupId, string name, string userData, string recognitionModel);
        /// <summary>
        /// Entrenar un PersonGroup
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="personGroupId"></param>
        /// <returns>202 OK</returns>
        Task<TResponse> Train<TResponse>(string personGroupId);
        /// <summary>
        /// Eliminar un PersonGroup
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="personGroupId"></param>
        /// <returns>200 OK</returns>
        Task<TResponse> Delete<TResponse>(string personGroupId);

    }
}
