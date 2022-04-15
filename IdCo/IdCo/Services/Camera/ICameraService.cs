using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace IdCo.Services.Camera
{
    /// <summary>
    /// Interfaz para el servicio de la Camara Android
    /// </summary>
    public interface ICameraService
    {
        /// <summary>
        /// Forma de almacenamiento de las imagenes capturas.
        /// </summary>
        /// <returns></returns>
        StoreCameraMediaOptions StoreCameraOptions();
        /// <summary>
        /// Captura de imagen con la camara.
        /// </summary>
        /// <param name="storeOptions"> Opciones de almacenamiento para las imagenes capturadas </param>
        /// <returns></returns>
        Task<MediaFile> TakePhoto(StoreCameraMediaOptions storeOptions);
    }
}
