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
    /// Clase para realizar las peticiones al API.
    /// </summary>
    public class FaceRequestJson
    {
        /// <summary>
        /// Id del personGroup.
        /// </summary>
        public string PersonGroupId { get; set; }
        /// <summary>
        /// Identificadores de los rostros.
        /// </summary>
        public string[] FaceIds { get; set; }
        /// <summary>
        /// Numero maximo de posibles candidatos devueltos.
        /// </summary>
        public int MaxNumOfCandidatesReturned { get; set; }
        /// <summary>
        /// Nivel de confianza en la identificación.
        /// </summary>
        public double ConfidenceThreshold { get; set; }
    }

    public class FaceService : IFaceService
    {
        readonly HttpClient httpClient;
        /// <summary>
        /// Inicializar el servicio.
        /// </summary>
        public FaceService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.FaceApiKey);
        }
        /// <summary>
        /// Realizar la detección de rostros en una imagen.
        /// </summary>
        /// <param name="returnFaceId"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        public async Task<Models.Face.Face[]> Detect(bool returnFaceId, Stream photo)
        {
            var recognitionModel = "recognition_03";
            var detectionModel = "detection_01";

            var request = $"{Settings.FaceEndPoint}/detect?returnFaceId={returnFaceId}&recognitionModel={recognitionModel}&detectionModel={detectionModel}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Settings.FaceEndPoint);

            requestMessage.RequestUri = new Uri(request);

            requestMessage.Content = new StreamContent(photo);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);
            Models.Face.Face[] faces = await ConvertHttpResponseToFace(httpResponseMessage).ConfigureAwait(true);
            return faces;
        }
        /// <summary>
        /// Encontrar la coincidencia más cercana de un conjunto de rostros.
        /// </summary>
        /// <param name="facesIds"></param>
        /// <param name="personGroupId"></param>
        /// <param name="maxNumOfCandidates"></param>
        /// <param name="confidenceThreshold"></param>
        /// <returns></returns>
        public async Task<Models.Face.Face[]> Identify(Guid[] facesIds, string personGroupId = null, int maxNumOfCandidates = 1, double confidenceThreshold = 0.5)
        {
            if (personGroupId == null)
            {
                personGroupId = Settings.FaceGroupID;
            }
                
            var request = $"{Settings.FaceEndPoint}/identify";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Settings.FaceEndPoint);
            requestMessage.RequestUri = new Uri(request);

            string[] f_id = ConvertGuidArrayToStringArray(facesIds);

            FaceRequestJson requestBody = new FaceRequestJson
            {
                PersonGroupId = personGroupId, FaceIds = f_id, MaxNumOfCandidatesReturned = maxNumOfCandidates,ConfidenceThreshold = confidenceThreshold
            };

            var jsonBody = JsonConvert.SerializeObject(requestBody);
            requestMessage.Content = new StringContent(jsonBody);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);
            Models.Face.Face[] faces = await ConvertHttpResponseToFace(httpResponseMessage).ConfigureAwait(true);
            return faces;
        }
        /// <summary>
        /// Covertir de Guid[] a String[].
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        private string[] ConvertGuidArrayToStringArray(Guid[] guids)
        {
            string[] stringArray = new string[guids.Length];
            int i = 0;
            foreach (Guid id in guids)
            {
                stringArray[i] = id.ToString();
                i = i + 1;
            }
            return stringArray;
        }
        /// <summary>
        /// Deserializar la respuesta http y convertirlo en Face[].
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        /// <returns></returns>
        private async Task<Models.Face.Face[]> ConvertHttpResponseToFace(HttpResponseMessage httpResponseMessage)
        {
            Models.Face.Face[] faces = default(Models.Face.Face[]);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                faces = JsonConvert.DeserializeObject<Models.Face.Face[]>(response);  
            }
            return faces;
        }
    }
}
