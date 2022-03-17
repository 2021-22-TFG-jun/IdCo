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
        public Guid[] FaceIds { get; set; }
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
        HttpClient httpClient;
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

            requestMessage.Content = new StreamContent(photo as Stream);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                Models.Face.Face[] faces = JsonConvert.DeserializeObject<Models.Face.Face[]>(response);
                return faces;
            }
            else
            {
                return default(Models.Face.Face[]);
            }
        }
        /// <summary>
        /// Encontrar la coincidencia más cercana de un conjunto de rostros.
        /// </summary>
        /// <param name="facesIds"></param>
        /// <param name="personGroupId"></param>
        /// <param name="maxNumOfCandidates"></param>
        /// <param name="confidenceThreshold"></param>
        /// <returns></returns>
        public async Task<Models.Face.Face[]> Identify(Guid[] facesIds, string personGroupId, int maxNumOfCandidates, double confidenceThreshold)
        {
            var request = $"{Settings.FaceEndPoint}/identify";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Settings.FaceEndPoint);

            requestMessage.RequestUri = new Uri(request);

            FaceRequestJson requestBody = new FaceRequestJson
            {
                PersonGroupId = Settings.FaceGroupID,
                FaceIds = facesIds,
                MaxNumOfCandidatesReturned = maxNumOfCandidates,
                ConfidenceThreshold = confidenceThreshold
            };

            var jsonBody = JsonConvert.SerializeObject(requestBody, Formatting.Indented);

            requestMessage.Content = new StringContent(jsonBody as string);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                Models.Face.Face[] faces = JsonConvert.DeserializeObject<Models.Face.Face[]>(response);
                return faces;
            }
            else
            {
                return default(Models.Face.Face[]);
            }
        }
    }
}
