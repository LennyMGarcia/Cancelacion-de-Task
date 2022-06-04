using System;
using System.Threading;
using System.Threading.Tasks;
/// <summary>
/// Para cancelar un task, se crea el token de cancelacion, se propaga a traves de; metodo y de la clase y luego se le dice que debe hacer en caso de cancelarse
/// </summary>
namespace Cancelacion_de_Task
{
    
    class Program
    {
        static int acumulador = 0;

        static void Main(string[] args)
        {
            //Este senala quien es cancelation token que debe cancelar la tarea
            CancellationTokenSource miToken = new CancellationTokenSource();
            //este es el que la cancela
            CancellationToken cancelaToken = miToken.Token;

            Task tarea = Task.Run(() => RealizarTarea(cancelaToken));

            for (int i = 0; i < 100; i++)
            {
                acumulador += 30;

                Thread.Sleep(1000);

                if(acumulador>100)
                {
                    //propaga la cancelacion de la tarea
                    miToken.Cancel();
                    //esto dice que se salga
                    break;
                }
                
            }
            Thread.Sleep(1000);

            Console.WriteLine("Valor de; acumulador " + acumulador);

            Console.ReadLine();
        }
        //Se necesita este token para que pueda recibirlo, recibe el objeto de la cancelacion
        static void RealizarTarea(CancellationToken token)
        {
            for (int i = 0; i<100; i++)
            {
                acumulador++;
                var miThreth = Thread.CurrentThread.ManagedThreadId;

                Thread.Sleep(1000);

                Console.WriteLine("Ejecutando el thread:" + miThreth);
                Console.WriteLine(acumulador);
                //si recibe la peticion de la cancelacion
                if (token.IsCancellationRequested)
                {
                    acumulador = 0;
                    //devuelve el flujo de ejecucion donde se encontraba antes, al hilo principal
                    return;
                }
            }
        }
    }
}
