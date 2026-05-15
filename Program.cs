using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
namespace Prueba_de_concepto_con_Task
{

    class Archivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }

    class Program
    {
        static ConcurrentDictionary<int, string> estadoDescargas =
            new ConcurrentDictionary<int, string>();

        static Random random = new Random();

        static async Task DescargarArchivo(Archivo archivo)
        {
            Console.WriteLine($" Iniciando: {archivo.Nombre} ({archivo.Tipo})");

            estadoDescargas[archivo.Id] = "Descargando";

            int tiempo = archivo.Tipo switch
            {
                "Video" => random.Next(3000, 6000),
                "Imagen" => random.Next(1500, 3000),
                "Documento" => random.Next(1000, 2000),
                _ => 2000
            };

            await Task.Delay(tiempo);

            estadoDescargas[archivo.Id] = "Completado";

            Console.WriteLine($" Finalizado: {archivo.Nombre}");
        }

        static async Task Main()
        {
            Console.WriteLine("=== GESTOR DE DESCARGAS CONCURRENTE ===\n");

            var archivos = new[]
            {
                new Archivo { Id = 1, Nombre = "Video Curso C#", Tipo = "Video" },
                new Archivo { Id = 2, Nombre = "Imagen Proyecto", Tipo = "Imagen" },
                new Archivo { Id = 3, Nombre = "PDF Investigación", Tipo = "Documento" },
                new Archivo { Id = 4, Nombre = "Video Tutorial", Tipo = "Video" },
                new Archivo { Id = 5, Nombre = "Guía Rápida", Tipo = "Documento" }
            };

            Task[] tareas = new Task[archivos.Length];

            for (int i = 0; i < archivos.Length; i++)
            {
                estadoDescargas[archivos[i].Id] = "En cola";
                tareas[i] = DescargarArchivo(archivos[i]);
            }

            await Task.WhenAll(tareas);

            Console.WriteLine("\n=== ESTADO FINAL ===");

            foreach (var item in estadoDescargas)
            {
                Console.WriteLine($"Archivo {item.Key}: {item.Value}");
            }
        }
    }
}