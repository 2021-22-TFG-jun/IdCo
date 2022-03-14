using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

using IdCo.Helpers;

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
        HttpClient httpClient;
        public PersonGroupPersonService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.FaceApiKey);
        }
        public async Task<Models.Face.Face> AddFace(string personId, Stream photo, string personGroupId = null,  string detectionModel = null)
        {
            if (personGroupId == null)
                personGroupId = Settings.FaceGroupID;
            if (detectionModel == null)
                detectionModel = "detection_01";

            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}/persons/{personId}/persistedFaces?detectionModel={detectionModel}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Settings.FaceEndPoint);

            requestMessage.RequestUri = new Uri(request);

            requestMessage.Content = new StreamContent(photo as Stream);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);
            

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                Models.Face.Face faceId = JsonConvert.DeserializeObject< Models.Face.Face>(response);

                return faceId;
            }
            else
            {
                return default(Models.Face.Face);
            }
        }

        public async Task<Models.Face.Face> Create(string name, string userData, string personGroupId = null)
        {
            if (personGroupId == null)
                personGroupId = Settings.FaceGroupID;

            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}/persons";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Settings.FaceEndPoint);

            PersonGroupPerson requestBody = new PersonGroupPerson
            {
                Name = name,
                UserData =  userData
            };

            requestMessage.RequestUri = new Uri(request);
            var jsonBody = JsonConvert.SerializeObject(requestBody, Formatting.Indented);

            requestMessage.Content = new StringContent(jsonBody as string);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                Models.Face.Face personId = JsonConvert.DeserializeObject<Models.Face.Face>(response);

                return personId;
            }
            else
            {
                return default(Models.Face.Face);
            }

        }

        public async Task<string> Delete(string personId, string personGroupId = null)
        {
            if (personGroupId == null)
                personGroupId = Settings.FaceGroupID;

            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}/persons/{personId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, Settings.FaceEndPoint);

            requestMessage.RequestUri = new Uri(request);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            return httpResponseMessage.StatusCode.ToString();
        }

        public async Task<string> DeleteFace(string personId, string faceId, string personGroupId = null)
        {
            if (personGroupId == null)
                personGroupId = Settings.FaceGroupID;

            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}/persons/{personId}/persistedFaces/{faceId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, Settings.FaceEndPoint);

            requestMessage.RequestUri = new Uri(request);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            return httpResponseMessage.StatusCode.ToString();
        }
    }
}
