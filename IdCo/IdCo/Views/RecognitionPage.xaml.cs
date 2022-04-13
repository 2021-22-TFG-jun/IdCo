using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

using IdCo.Services.Face;
using IdCo.Models.Face;
using IdCo.Models.Person;
using Xamarin.Essentials;

namespace IdCo.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionPage : ContentPage
    {
        BackgroundWorker tarea;
        readonly FaceService faceService;
        bool procesado;
        /// <summary>
        /// Inicializador de la vista y del servicio.
        /// </summary>
        public RecognitionPage()
        {
            InitializeComponent();
            faceService = new FaceService();
        }

        /// <summary>
        /// Obtener la imagen capturada con sus propiedades (sin guardar en memoria)
        /// Empezar a hacer las operaciones de deteccion e identificacion y búsqueda en BD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void cameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            byte[] photo = e.ImageData.ToArray<byte>();
            using (var stream = new MemoryStream(photo))
            {
                Face[] detectFaces = await faceService.Detect(true, stream);
                if (detectFaces.Any())
                {
                    Guid[] facesIds = GetFacesIds(detectFaces);
                    Face[] identifyFaces = await faceService.Identify(facesIds);
                    if (identifyFaces.Any())
                    {
                        foreach (var rostro in identifyFaces)
                        {
                            foreach(var candidato in rostro.Candidates)
                            {
                                Person person = App.Database.SearchPersonByPersonId(candidato.PersonId.ToString());
                                await TextToSpeech.SpeakAsync(person.Name + " " + person.LastName);
                            }
                        }
                    }
                }
            }
            procesado = true;
        }
        /// <summary>
        /// Obtener los FacesId detectados.
        /// </summary>
        /// <param name="detectFaces"></param>
        /// <returns></returns>
        private Guid[] GetFacesIds(Face[] detectFaces)
        {
            Guid[] facesIds = new Guid[detectFaces.Length];
            int i = 0;
            foreach (Face face in detectFaces)
            {
                facesIds[i] = face.FaceId;
                i++;
            }

            return facesIds;
        }
        /// <summary>
        /// Volver a la vista inmediatamente anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            if(tarea != null)
            {
                tarea.CancelAsync();
            }
            
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Empezar la captura continua de imagenes en un proceso en segundo plano.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBtn_Clicked(object sender, EventArgs e)
        {
            BackgroudProcessJob();
            tarea.RunWorkerAsync(2000);
        }
        /// <summary>
        /// Capturar una imagen
        /// </summary>
        private void TakePhoto()
        {
            cameraView.Shutter();
        }
        /// <summary>
        /// Inicializar el proceso en segundo plano para la captura de imagenes.
        /// </summary>
        private void BackgroudProcessJob()
        {
            tarea = new BackgroundWorker();
            tarea.WorkerSupportsCancellation = true;
            tarea.DoWork += new DoWorkEventHandler(DoWork);
            tarea.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Finished);
            
        }
        /// <summary>
        /// Tareas que debe realizar el proceso en segundo plano
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eventArgs"></param>
        private void DoWork(object obj, DoWorkEventArgs eventArgs)
        {
            BackgroundWorker bw = obj as BackgroundWorker;
            TakePhotoPeriodically(bw);

            if (tarea.CancellationPending)
            {
                eventArgs.Cancel = true;
            }

        }
        /// <summary>
        /// Tomar fotos periodicamente
        /// </summary>
        /// <param name="bw"></param>
        private void TakePhotoPeriodically(BackgroundWorker bw)
        {
            while (!bw.CancellationPending)
            {
                procesado = false;
                int i = 0;
                TakePhoto();

                while (!procesado && i < 3)
                {
                    System.Threading.Thread.Sleep(30000); // dar tiempo para procesar la imagen
                    i = i + 1;
                }
            }
        }
        /// <summary>
        /// Tareas de finalizacion del proceso de segundo plano
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void Finished(object obj, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
#if DEBUG
                Console.WriteLine("Tarea cancelada");
#endif
            }
            else if(e.Error != null)
            {
#if DEBUG
                Console.WriteLine("A ocurrido un error: " + e.Error.Message);
#endif
            }
            else
            {
#if DEBUG
                Console.WriteLine("Se ha terminado correctamente la tarea: " + e.Result);
#endif
            }
        }
    }
}
