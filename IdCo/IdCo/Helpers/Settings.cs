using System;
using System.IO;

namespace IdCo.Helpers
{
    public class Settings
    {
        public static string Resource = ""; // Nombre de tu recurso
        public static readonly string FaceEndPoint = "https://"+Resource+ ".cognitiveservices.azure.com/face/v1.0";
        public static string FaceApiKey = ""; // Clave de acceso al recurso

        public static readonly string FaceGroupID = "Conocidos";
        public static readonly string FaceGroupName = "Mis Personas Conocidas";
        public static readonly string FaceGroupDescription = "Agrupación de los rostros de mis personas conocidas";

        public static readonly string BDName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PersonDB.db3");
    }
}
