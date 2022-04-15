using System;

using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace IdCo.Services.Camera
{
    /// <summary>
    /// Controlar las principales funcionalidades de una camara Andorid.
    /// </summary>
    public class CameraService : ICameraService
    {
        /// <summary>
        /// Inicializar el plugin para el uso de la camara.
        /// </summary>
        public CameraService()
        {
            CrossMedia.Current.Initialize();
        }

        /// <summary>
        /// Establecer la forma de almacenamiento de las fotos.
        /// </summary>
        /// <param name="name"> Nombre del archivo de imagen </param>
        /// <param name="photoSize"> Tamaño de la imagen </param>
        /// <param name="directory"> Nombre del directorio de almacenamiento </param>
        /// <returns> Opcion de almacenamiento para una imagen </returns>
        public StoreCameraMediaOptions StoreCameraOptions(string name, PhotoSize photoSize, string directory)
        {
            return new StoreCameraMediaOptions { Name = name, PhotoSize = photoSize, Directory = directory};
        }
        /// <summary>
        /// Almacenamiento de las imágenes por defecto.
        /// </summary>
        /// <returns></returns>
        public StoreCameraMediaOptions StoreCameraOptions()
        {
            string name = "photo_" + DateTime.Now.ToString("dd-MM-yyyy HH.mm");
            PhotoSize photoSize = PhotoSize.Small;
            string directory = "IdCoGalery";

            return StoreCameraOptions(name, photoSize, directory);
        }

        /// <summary>
        /// Capturar una imagen con la camara, siempre y cuando la camara este disponible.
        /// </summary>
        /// <param name="storeOptions"></param>
        /// <returns> Imagen capturada con la camara </returns>
        public async Task<MediaFile> TakePhoto(StoreCameraMediaOptions storeOptions)
        {
            MediaFile photo;

            if (!CrossMedia.Current.IsCameraAvailable)
            {
                throw new ArgumentNullException("Error: La cámara no está disponible.");
            }

            photo = await CrossMedia.Current.TakePhotoAsync(storeOptions);
            return photo;
        }
    }
}
