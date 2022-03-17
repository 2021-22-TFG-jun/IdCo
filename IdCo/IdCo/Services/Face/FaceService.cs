using System;
using System.IO;
using System.Threading.Tasks;

namespace IdCo.Services.Face
{
    public class FaceService : IFaceService
    {
        public Task<Models.Face.Face[]> Detect(bool returnFaceId, Stream photo)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Face.Face[]> Identify(Guid[] facesIds, string personGroupId, int maxNumOfCandidates, double confidenceThreshold)
        {
            throw new NotImplementedException();
        }
    }
}
