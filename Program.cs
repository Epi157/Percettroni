using System;
using System.IO;
using System.Globalization;

class Program
{
    const int FEATURES = 5;
    const double THRESHOLD = 0.5;

    // Funzione di attivazione (step)
    static int Activation(double x)
    {
        if (x > THRESHOLD) return 1;
        else return 0;
    }

    // Carica pesi e bias da file
    static bool CaricaPesi(string filename, out double[] weights, out double bias)
    {
        weights = new double[FEATURES];
        bias = 0.0;

        if (!File.Exists(filename))
        {
            Console.WriteLine($"Errore: file {filename} non trovato!");
            return false;
        }

        string[] lines = File.ReadAllLines(filename);
        if (lines.Length < FEATURES + 1)
        {
            Console.WriteLine("Errore: il file non contiene abbastanza righe!");
            return false;
        }

        try
        {
            for (int i = 0; i < FEATURES; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    weights[i] = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
                }
                else
                {
                    Console.WriteLine($"Errore nella lettura del peso {i + 1}");
                    return false;
                }
            }

            string biasLine = lines[FEATURES];
            string[] biasParts = biasLine.Split(':');
            if (biasParts.Length == 2)
            {
                bias = double.Parse(biasParts[1].Trim(), CultureInfo.InvariantCulture);
            }
            else
            {
                Console.WriteLine("Errore nella lettura del bias");
                return false;
            }

            return true;
        }
        catch (FormatException)
        {
            Console.WriteLine("Errore di formato durante la lettura dei pesi o del bias.");
            return false;
        }
    }

    // Funzione di previsione
    static int Prevedi(double[] weights, double bias, int[] input)
    {
        double somma = bias;
        for (int i = 0; i < FEATURES; i++)
        {
            somma += input[i] * weights[i];
        }
        return Activation(somma);
    }

    static void Main(string[] args)
    {
        if (!CaricaPesi("pesi_concerto.txt", out double[] weights, out double bias))
        {
            return;
        }

        Console.WriteLine("Inserisci i dati:");
        int[] input = new int[FEATURES];

        Console.Write("Artista famoso? (1=Si, 0=No): ");
        _ = int.TryParse(Console.ReadLine(), out input[0]);

        Console.Write("Bel meteo? (1=Si, 0=No): ");
        _ = int.TryParse(Console.ReadLine(), out input[1]);

        Console.Write("Amici presenti? (1=Si, 0=No): ");
        _ = int.TryParse(Console.ReadLine(), out input[2]);

        Console.Write("Cibo buono? (1=Si, 0=No): ");
        _ = int.TryParse(Console.ReadLine(), out input[3]);

        Console.Write("Alcool disponibile? (1=Si, 0=No): ");
        _ = int.TryParse(Console.ReadLine(), out input[4]);

        int decisione = Prevedi(weights, bias, input);

        if (decisione == 1)
        {
            Console.WriteLine("\n=> Vai al concerto!");
        }
        else
        {
            Console.WriteLine("\n=> Resta a casa!");
        }
    }
}
