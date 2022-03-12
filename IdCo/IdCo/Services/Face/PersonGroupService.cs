using System;
using System.Threading.Tasks;

namespace IdCo.Services.Face
{
    public class PersonGroupService : IPersonGroupService
    {
        public Task<TResponse> Create<TResponse>(string personGroupId, string name, string userData, string recognitionModel)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> Delete<TResponse>(string personGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> Train<TResponse>(string personGroupId)
        {
            throw new NotImplementedException();
        }
    }
}
