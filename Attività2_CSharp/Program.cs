using System;
using System.Globalization;
using System.IO;

class Program
{
    const int N = 8;     // numero di esempi
    const int EPOCHS = 100;
    const double LEARNING_RATE = 0.1;

    // Funzione di attivazione a gradino con soglia 0.5
    static int Activation(double sum)
    {
        if (sum >= 0.5)
        {
            return 1;
        }
        return 0;
    }

    static void Main()
    {
        // Dati di addestramento: sole, tempo, stanco
        int[,] input = new int[N, 3] {
            {0, 0, 0}, {0, 0, 1},
            {0, 1, 0}, {0, 1, 1},
            {1, 0, 0}, {1, 0, 1},
            {1, 1, 0}, {1, 1, 1}
        };
        int[] expected = new int[N];

        // Crea il dataset in base alla logica: (tempo == 1 AND stanco == 0) OR (sole == 1 AND stanco == 0)
        for (int i = 0; i < N; i++)
        {
            int sole = input[i, 0];
            int tempo = input[i, 1];
            int stanco = input[i, 2];
            if ((tempo == 1 && stanco == 0) || (sole == 1 && stanco == 0))
            {
                expected[i] = 1;
            }
            else
            {
                expected[i] = 0;
            }
        }

        // Pesi casuali iniziali
        double[] weights = new double[3] { 0.0, 0.0, 0.0 };
        double bias = 0.0;

        // Addestramento
        for (int epoch = 0; epoch < EPOCHS; epoch++)
        {
            for (int i = 0; i < N; i++)
            {
                double sum = bias;
                for (int j = 0; j < 3; j++)
                {
                    sum += weights[j] * input[i, j];
                }

                // Predizione e calcolo dell'errore
                int output = Activation(sum);
                int error = expected[i] - output;

                // Aggiornamento dei pesi
                for (int j = 0; j < 3; j++)
                {
                    weights[j] += LEARNING_RATE * error * input[i, j];
                }
                bias += LEARNING_RATE * error;
            }
        }

        // Output dei pesi finali
        Console.WriteLine("Pesi allenati:");
        for (int i = 0; i < 3; i++)
        {
            // Usa CultureInfo.InvariantCulture per formattare con il punto decimale
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Peso {0}: {1:F6}", i, weights[i]));
        }
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Bias: {0:F6}", bias));

        // Test finale automatico
        Console.WriteLine("\nTest automatico del percettrone:");
        for (int i = 0; i < N; i++)
        {
            double sum = bias;
            for (int j = 0; j < 3; j++)
            {
                sum += weights[j] * input[i, j];
            }
            int output = Activation(sum);
            Console.WriteLine($"Input [{input[i, 0]}, {input[i, 1]}, {input[i, 2]}] => Correre: {output} (Atteso: {expected[i]})");
        }
        
        // Opzionale: Salviamo i pesi su file in modo simile a come è stato fatto in Attività 1
        SalvaPesi("pesi_corsa.txt", weights, bias);

        // Test manuale interattivo
        Console.WriteLine("\n--- TEST MANUALE ---");
        Console.WriteLine("Inserisci i dati per fare una previsione:");
        
        int[] userTest = new int[3];
        Console.Write("C'è il sole? (1=Si, 0=No): ");
        _ = int.TryParse(Console.ReadLine(), out userTest[0]);
        
        Console.Write("Hai tempo libero? (1=Si, 0=No): ");
        _ = int.TryParse(Console.ReadLine(), out userTest[1]);
        
        Console.Write("Sei stanco? (1=Si, 0=No): ");
        _ = int.TryParse(Console.ReadLine(), out userTest[2]);

        double testSum = bias;
        for(int j = 0; j < 3; j++)
        {
            testSum += weights[j] * userTest[j];
        }
        int testOutput = Activation(testSum);

        if (testOutput == 1)
        {
            Console.WriteLine("\n=> Vai a correre!");
        }
        else
        {
            Console.WriteLine("\n=> Resta a casa!");
        }
    }

    // Funzione extra per esportare i pesi come avviene nell'Attività 1 (utilizzando CultureInfo.InvariantCulture)
    static void SalvaPesi(string filename, double[] weights, double bias)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                for (int i = 0; i < weights.Length; i++)
                {
                    sw.WriteLine(string.Format(CultureInfo.InvariantCulture, "w{0}: {1}", i + 1, weights[i]));
                }
                sw.WriteLine(string.Format(CultureInfo.InvariantCulture, "bias: {0}", bias));
            }
            Console.WriteLine($"\n[Info] Pesi e bias salvati correttamente nel file: {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[Errore] Impossibile salvare i pesi nel file: {ex.Message}");
        }
    }
}
