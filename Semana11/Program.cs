using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Matrices
{
  internal class Program
  {
    static int max_estudiantes = 40;
    static int max_cursos = 4;
    static string[] nombre_de_cursos = { "Matematicas", "Programación", "Base de datos", "Fisica" };
    static string[] estudiantes = new string[max_estudiantes];
    static double[,] matrizNotas = new double[max_estudiantes, max_cursos];
    static int total_estudiantes = 0;
    static string rutaArchivo = "notas_curso.txt";
    public static void RegistrarEstudiantes()
    {
      Console.WriteLine("Registro de estudiante");

      if (total_estudiantes >= max_estudiantes)
      {
        Console.WriteLine("Capacidad máxima de estudiantes alcanzada");
        return;
      }
      string nombre = Validador.LeerTexto("Ingresar nombre y apellidos: ").ToUpper();
      estudiantes[total_estudiantes] = nombre;

      Console.WriteLine("Ingresar notas para curso(0-20)");
      for (int i = 0; i < max_cursos; i++)
      {
        matrizNotas[total_estudiantes, i] = Validador.LeerDecimal($"-Ingrese la nota de {nombre_de_cursos[i]}: ");
      }
      total_estudiantes++;
      Console.WriteLine("Estudiante registrado....!");
    }
    public static void GuardarArchivo()
    {
      Console.WriteLine("GUARDAR ARCHIVO");
      if (total_estudiantes == 0)
      {
        Console.WriteLine("No hay registros por guardar");
        return;
      }
      using (StreamWriter escribir = new StreamWriter(rutaArchivo))
      {
        for (int i = 0; i < total_estudiantes; i++)
        {
          string linea = estudiantes[i];
          // CORREGIDO: la condicion era "i < total_estudiantes"
          for (int j = 0; j < max_cursos; j++)
          {
            linea += $",{matrizNotas[i, j]}";
          }
          escribir.WriteLine(linea);
        }
      }
      Console.WriteLine("Registros guardados");
    }

    public static void CargarRegistros()
    {
      Console.WriteLine("Cargar registros desde archivo");
      if (!File.Exists(rutaArchivo))
      {
        Console.WriteLine("No encontro archivo...");
        return;
      }
      total_estudiantes = 0;
      using (StreamReader lector = new StreamReader(rutaArchivo))
      {
        string linea;
        while ((linea = lector.ReadLine()) != null && total_estudiantes < max_estudiantes)
        {
          string[] partes = linea.Split(',');
          if (partes.Length == max_cursos + 1)
          {
            estudiantes[total_estudiantes] = partes[0];
            for (int i = 0; i < max_cursos; i++)
            {
              matrizNotas[total_estudiantes, i] = Convert.ToDouble(partes[i + 1]);
            }
            total_estudiantes++;
          }
        }
      }
      Console.WriteLine("Registros cargados");
    }

    static public void mostrarRegistros()
    {
      Console.WriteLine(new string('=', 100));
      Console.WriteLine("\t\tReporte General De Notas Por Cursos");
      Console.WriteLine(new string('=', 100));
      if (total_estudiantes == 0)
      {
        Console.WriteLine("No hay registros");
        return;
      }
      Console.Write($"{"Estudiante",-25}|");
      for (int i = 0; i < max_cursos; i++)
      {
        Console.Write($"{nombre_de_cursos[i],-14}|");
      }
      Console.WriteLine("PROMEDIO");
      Console.WriteLine(new string('-', 100));
      for (int i = 0; i < total_estudiantes; i++)
      {
        Console.Write($"{estudiantes[i],-25}|");
        double suma = 0;
        for (int j = 0; j < max_cursos; j++)
        {
          Console.Write($"{matrizNotas[i, j],-14:F1}|");
          suma += matrizNotas[i, j];
        }
        double promedio = suma / max_cursos;
        string estado = promedio >= 12 ? "Aprobado" : "Reprobado";
        Console.WriteLine($"{promedio,-10:F1} | {estado}");
      }
    }

    static void Main(string[] args)
    {
      int opc = 0;
      while (opc != 5)
      {
        Console.Clear();
        Console.WriteLine("[1] Registrar estudiante");
        Console.WriteLine("[2] Guardar en Archivo");
        Console.WriteLine("[3] Cargar desde Archivo");
        Console.WriteLine("[4] Mostrar Registros");
        Console.WriteLine("[5] Salir");
        opc = Validador.LeerEnteros("Ingrese una opcion(1-5): ");
        switch (opc)
        {
          case 1:
            RegistrarEstudiantes();
            break;
          case 2:
            GuardarArchivo();
            break;
          case 3:
            CargarRegistros();
            break;
          case 4:
            mostrarRegistros();
            break;
          case 5:
            Console.WriteLine("Saliendo del programa...");
            break;
          default:
            Console.WriteLine("Opcion invalida, intente nuevamente.");
            break;
        }

        Console.ReadKey();
      }
    }
  }
}