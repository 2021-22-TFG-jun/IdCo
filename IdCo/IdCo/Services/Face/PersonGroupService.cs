using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using IdCo.Helpers;

namespace IdCo.Services.Face
{
    /// <summary>
    /// Clase que identifica las características obligatorias de un PersonGroup.
    /// </summary>
    public class PersonGroup
    {
        /// <summary>
        /// Nombre del PersonGroup.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Datos descriptivos del PersonGroup.
        /// </summary>
        public string UserData { get; set; }
        /// <summary>
        /// Modelo utilizado para el reconocimiento en el PersonGroup.
        /// </summary>
        public string RecognitionModel { get; set; }
    }

    /// <summary>
    /// Clase que implementa las funcionalidades relativas a un PersonGroup.
    /// Crear/Eliminar/Entrenar
    /// </summary>
    public class PersonGroupService : IPersonGroupService
    {
        readonly HttpClient httpClient;
        /// <summary>
        /// Inicialización del cliente Https y asignación de la clave del API.
        /// </summary>
        public PersonGroupService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.FaceApiKey);
        }
        /// <summary>
        /// Sobrecarga de método. Crear un PersonGroup.
        /// </summary>
        /// <returns></returns>
        public async Task<string> Create()
        {
            string personGroupId = Settings.FaceGroupID;
            string name = Settings.FaceGroupName;
            string userData = Settings.FaceGroupDescription;
            string recognitionModel = "recognition_03";

            return await Create(personGroupId, name, userData, recognitionModel).ConfigureAwait(true);
        }
        /// <summary>
        /// Crear un PersonGroup
        /// </summary>
        /// <param name="personGroupId">Id del PersonGroup</param>
        /// <param name="name">Name del PersonGroup</param>
        /// <param name="userData">Descripcion del PersonGroup</param>
        /// <param name="recognitionModel">Modelo de reconocimiento</param>
        /// <returns>Código del estado de la respuesta http</returns>
        public async Task<string> Create(string personGroupId, string name, string userData, string recognitionModel)
        {
            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, Settings.FaceEndPoint);

            PersonGroup requestBody = new PersonGroup
            {
                Name = name,
                UserData = userData,
                RecognitionModel = recognitionModel
            };

            requestMessage.RequestUri = new Uri(request);
            var jsonBody = JsonConvert.SerializeObject(requestBody, Formatting.Indented);

            requestMessage.Content = new StringContent(jsonBody as string);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            return httpResponseMessage.StatusCode.ToString();
        }
        /// <summary>
        /// Sobrecarga de método. Eliminar un PersonGroup.
        /// </summary>
        /// <returns></returns>
        public async Task<string> Delete()
        {
            string personGroupId = Settings.FaceGroupID;
            return await Delete(personGroupId).ConfigureAwait(true);
        }
        /// <summary>
        /// Eliminar un PersonGroup
        /// </summary>
        /// <param name="personGroupId">Id del PersonGroup</param>
        /// <returns>Código del estado de la respuesta http</returns>
        public async Task<string> Delete(string personGroupId)
        {
            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, Settings.FaceEndPoint);
            requestMessage.RequestUri = new Uri(request);

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            return httpResponseMessage.StatusCode.ToString();
        }
        /// <summary>
        /// Sobrecarga de método. Entrenar un PersonGroup.
        /// </summary>
        /// <returns></returns>
        public async Task<string> Train()
        {
            string personGroupId = Settings.FaceGroupID;
            return await Train(personGroupId).ConfigureAwait(true);
        }
        /// <summary>
        /// Entrenar un PersonGroup
        /// </summary>
        /// <param name="personGroupId">Id Del PersonGroup</param>
        /// <returns>Código del estado de la respuesta http</returns>
        public async Task<string> Train(string personGroupId)
        {
            var request = $"{Settings.FaceEndPoint}/persongroups/{personGroupId}/train";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Settings.FaceEndPoint);
            requestMessage.RequestUri = new Uri(request);

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            return httpResponseMessage.StatusCode.ToString();
        }
    }
}
