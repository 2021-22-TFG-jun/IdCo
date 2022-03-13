using System;
using System.IO;
using System.Threading.Tasks;

namespace IdCo.Services.Face
{
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
