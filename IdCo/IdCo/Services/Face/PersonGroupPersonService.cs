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
        readonly HttpClient httpClient;
        /// <summary>
        /// Inicializar el servicio.
        /// </summary>
        public PersonGroupPersonService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.FaceApiKey);
        }
        /// <summary>
        /// Añadir un face a un Person ya creado.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="photo"></param>
        /// <param name="personGroupId"></param>
        /// <param name="detectionModel"></param>
        /// <returns></returns>
        public async Task<Models.Face.Face> AddFace(string personId, Stream photo, string personGroupId = null,  string detectionModel = null)
        {
            if (personGroupId == null)
            {
                personGroupId = Settings.FaceGroupID;
            }
            if (detectionModel == null)
            {
                detectionModel = "detection_01";
            }

            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}/persons/{personId}/persistedFaces?detectionModel={detectionModel}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Settings.FaceEndPoint);

            requestMessage.RequestUri = new Uri(request);

            requestMessage.Content = new StreamContent(photo);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);
            Models.Face.Face face = await ConvertHttpResponseToFace(httpResponseMessage).ConfigureAwait(true);
            return face;
        }
        /// <summary>
        /// Crear un Person en un PersonGroup.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userData"></param>
        /// <param name="personGroupId"></param>
        /// <returns></returns>
        public async Task<Models.Face.Face> Create(string name, string userData, string personGroupId = null)
        {
            if (personGroupId == null)
            {
                personGroupId = Settings.FaceGroupID;
            }

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
            Models.Face.Face face = await ConvertHttpResponseToFace(httpResponseMessage).ConfigureAwait(true);
            return face;
        }
        /// <summary>
        /// Eliminar un Person de un PersonGroup.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="personGroupId"></param>
        /// <returns></returns>
        public async Task<string> Delete(string personId, string personGroupId = null)
        {
            if (personGroupId == null)
            {
                personGroupId = Settings.FaceGroupID;
            }

            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}/persons/{personId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, Settings.FaceEndPoint);

            requestMessage.RequestUri = new Uri(request);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            return httpResponseMessage.StatusCode.ToString();
        }
        /// <summary>
        /// Eliminar un rostro de un Person.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="faceId"></param>
        /// <param name="personGroupId"></param>
        /// <returns></returns>
        public async Task<string> DeleteFace(string personId, string faceId, string personGroupId = null)
        {
            if (personGroupId == null)
            {
                personGroupId = Settings.FaceGroupID;
            }

            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}/persons/{personId}/persistedFaces/{faceId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, Settings.FaceEndPoint);

            requestMessage.RequestUri = new Uri(request);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            return httpResponseMessage.StatusCode.ToString();
        }
        /// <summary>
        /// Convertir una respuesta http en un objeto Face.
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        /// <returns></returns>
        private async Task<Models.Face.Face> ConvertHttpResponseToFace(HttpResponseMessage httpResponseMessage)
        {
            Models.Face.Face face = default(Models.Face.Face);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                face = JsonConvert.DeserializeObject<Models.Face.Face>(response);
            }
            return face;
        }
    }
}
