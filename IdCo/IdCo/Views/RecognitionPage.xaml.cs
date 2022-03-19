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

namespace IdCo.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionPage : ContentPage
    {
        BackgroundWorker tarea = null;
        public RecognitionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Obtener la imagen capturada con sus propiedades (sin guardar en memoria)
        /// Empezar a hacer las operaciones de deteccion e identificacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            Console.WriteLine("MEDIA CAPTURED: ");
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
                Console.WriteLine("Cancelando tarea");
                tarea.CancelAsync();
                tarea.Dispose();
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
            tarea.DoWork += DoWork;
            
            tarea.RunWorkerAsync();
            Console.ReadLine();
        }
        /// <summary>
        /// Tareas que debe realizar el proceso en segundo plano
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eventArgs"></param>
        private void DoWork(object obj, DoWorkEventArgs eventArgs)
        {
            Console.WriteLine("Iniciando trabajo...");
            //System.Threading.Thread.Sleep(10000);
            TakePhoto();
            if (tarea.CancellationPending)
            {
                eventArgs.Cancel = true;
                return;
            }
            //System.Threading.Thread.Sleep(10000);
            tarea.RunWorkerCompleted += Finished;

        }
        /// <summary>
        /// Tareas de finalizacion del proceso de segundo plano
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void Finished(object obj, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;

            Console.WriteLine("Trabajo terminado....");   
        }
    }
}
