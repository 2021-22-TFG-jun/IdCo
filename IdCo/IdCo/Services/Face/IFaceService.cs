using System;
using System.IO;
using System.Threading.Tasks;

namespace IdCo.Services.Face
{
    public interface IFaceService
    {
        /// <summary>
        /// Detectar rostros.
        /// </summary>
        /// <param name="returnFaceId"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        Task<Models.Face.Face[]> Detect(bool returnFaceId, Stream photo);
        /// <summary>
        /// Encontrar la coincidencia más cercana de un conjunto de rostros.
        /// </summary>
        /// <param name="facesIds"></param>
        /// <param name="personGroupId"></param>
        /// <param name="maxNumOfCandidates"></param>
        /// <param name="confidenceThreshold"></param>
        /// <returns></returns>
        Task<Models.Face.Face[]> Identify(Guid[] facesIds, string personGroupId, int maxNumOfCandidates, double confidenceThreshold);
    }
}
