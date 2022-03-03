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
        /// <param name="name"> Nombre del fichero de la foto capturada </param>
        /// <param name="photoSize">Tamaño de la foto: Small, Medium, Large, Full, Custom, MaxWidthHeight </param>
        /// <param name="directory"> Directorio de la imagen </param>
        /// <returns></returns>
        StoreCameraMediaOptions StoreCameraOptions(string name = null, PhotoSize photoSize = PhotoSize.Small, string directory = null);
        /// <summary>
        /// Captura de imagen con la camara.
        /// </summary>
        /// <param name="storeOptions"> Opciones de almacenamiento para las imagenes capturadas </param>
        /// <returns></returns>
        Task<MediaFile> TakePhoto(StoreCameraMediaOptions storeOptions);
    }
}
