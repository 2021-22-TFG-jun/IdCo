using System;
using System.IO;
using Xamarin.Forms;

namespace IdCo.Helpers
{
    public class Settings
    {
        public static readonly string Resource = Application.Current.Properties["ResourceName"].ToString();
        public static readonly string FaceEndPoint = "https://"+ Resource + ".cognitiveservices.azure.com/face/v1.0";
        public static readonly string FaceApiKey = Application.Current.Properties["ResourceKey"].ToString();

        public static readonly string FaceGroupID = "conocidos";
        public static readonly string FaceGroupName = "Mis Personas Conocidas";
        public static readonly string FaceGroupDescription = "Agrupación de los rostros de mis personas conocidas";
        public static string BDName { get; set; }
    }

    public class CheckSettings
    {
        public static bool CorrectResourceAccess = false;
        public static bool CorrectDBAccess = false;
    }
}
