using Plugin.Media.Abstractions;

namespace IdCo.Services.Camera
{
    public interface ICameraService
    {
        StoreCameraMediaOptions StoreCameraOptions(string name, PhotoSize photoSize, string directory);
        MediaFile TakePhoto();
    }
}
