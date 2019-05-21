using L2._4Uzd;
using L2._1Uzd;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2._2Uzd;

namespace L2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program started...\n");
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;

            //SearchTest(seed, 1000000);
            //UnboundedKnapsackTest(seed, 150, 150);
            //UnboundedKnapsackPrallelTest(seed, 1000000, 150);
            //MinimumSumPartition(seed, 10000);

            int[] sizes = new int[6] { 200000, 400000, 800000, 1600000, 3200000, 6400000 }; //Hashmap paieska
            //Int[] sizes = new Int[7] { 250000, 500000, 1000000, 2000000, 4000000, 8000000, 16000000}; //Knapsack dinaminis
            //int[] sizes = new int[6] { 1250, 2500, 5000, 10000, 15000, 20000 };  //MinSumPartition

            foreach (int n in sizes)
            {
                //SearchTest(seed, n);
                //MinimumSumPartition(seed, n);
                //UnboundedKnapsackTest(seed, n, 600);
            }

            //int[] minSizes = new int[7] { 10, 15, 20, 25, 30, 35, 40 }; // kai w=300
            int[] minSizes = new int[7] { 90, 100, 110, 120, 130, 140, 150 }; //kai w=150

            foreach (int n in minSizes)
            {
                //UnboundedKnapsackTest(seed, n, 300);
                UnboundedKnapsackPrallelTest(seed, n, 150);
            }

            Console.WriteLine("\nDone!");
            Console.ReadKey();
        }

        private static void UnboundedKnapsackTest(int seed, int size, int weight)
        {
            Stopwatch stopwatch = new Stopwatch();
            double ts;
            int W = weight;
            int[] val = RndIntArray(seed, size);
            int[] wt = RndIntArray(seed, size);
            int n = size;

            //W = 100;
            //val = new int[] { 10, 30, 20, 20, 15 };
            //wt = new int[] { 5, 10, 15, 20, 2 };
            //n = val.Length;

            stopwatch.Reset();
            stopwatch.Start();
            Console.Write("Rezultatas: " + UnboundedKnapsack.UnboundedKnapsackDynamic(n, W, val, wt) + " ");
            stopwatch.Stop();
            ts = stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine("Unbounded Knapsack Dynamic - kiekis: {0}   laikas: {1}",n ,ts.ToString());
            Console.Beep();

            /*stopwatch.Reset();
            stopwatch.Start();
            Console.Write("Rezultatas: " + UnboundedKnapsack.UnboundedKnapsackRecursive(W, wt, val, n) + " ");
            stopwatch.Stop();
            ts = stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine("Unbounded Knapsack Recursive - kiekis: {0}   laikas: {1}", n, ts.ToString());
            Console.Beep();*/
        }

        private static void UnboundedKnapsackPrallelTest(int seed, int size, int weight)
        {
            Stopwatch stopwatch = new Stopwatch();
            double ts;
            int W = weight;
            int[] val = RndIntArray(seed, size);
            int[] wt = RndIntArray(seed, size);
            int n = val.Length;

            stopwatch.Start();
            int rec = UnboundedKnapsackParallel.KnapsackRecursive(W, wt, val, n);
            stopwatch.Stop();
            ts = stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine("Unbounded Knapsack Recursive - kiekis: {0}   laikas: {1}", n, ts.ToString());
            Console.Beep();
            
            stopwatch.Reset();
            stopwatch.Start();
            Console.WriteLine(UnboundedKnapsackParallel.KnapsackParallel(W, n, val, wt));
            stopwatch.Stop();
            ts = stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine("Unbounded Knapsack Recursive Parallel - kiekis: {0}   laikas: {1}", n, ts.ToString());
            Console.Beep();
        }

        private static void MinimumSumPartition(int seed, int n)
        {
            int[] arr = RndIntArrayLimited(seed, n, 10);
            int diff;
            Stopwatch stopwatch = new Stopwatch();
            double ts;

            stopwatch.Start();
            diff = MinSumPartition.findMin(arr, n);
            stopwatch.Stop();
            ts = stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine("Mažiausias skirtumas tarp 2 masyvų yra: {0}\nIšnaudotas laikas: {1}", diff, ts);
            Console.Beep();
        }

        private static void SearchTest(int seed, int n)
        {
            Stopwatch stopwatch = new Stopwatch();
            double ts;
            HashMap<int, string> mymap = new HashMap<int, string>();
            string[] values = new string[n];
            Random rnd = new Random(seed);
            string value;
            int key;
            for (int i = 0; i < n; i++)
            {
                value = Path.GetRandomFileName().Substring(0, 8);
                key = Math.Abs(value.GetHashCode());
                mymap.InsertNode(key, value);
                values[i] = value;
            }
            //Console.WriteLine("HASHMAP");
            //mymap.Display();
            //Console.WriteLine("\n\n GET");

            // Sequential loop
            int counter = 0;
            stopwatch.Start();
            for (int i = 0; i < values.Length; i++)
            {
                if(mymap.Get(values[i])) counter++;
            }
            stopwatch.Stop();
            ts = stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine("Sequential search laikas : {0} -> kiekis: {1}", ts, counter);
            Console.Beep();

            // Parallel loop
            int countCPU = 4;
            Task<int>[] tasks = new Task<int>[countCPU];
            stopwatch.Reset();
            stopwatch.Start();
            for (var j = 0; j < countCPU; j++)
                tasks[j] = Task<int>.Factory.StartNew(
                (object p) =>
                {
                    int count = 0;
                    for (int i = (int)p; i < values.Length; i += countCPU)
                        if (mymap.Get(values[i])) count++;
                    return count;
                }, j);
            stopwatch.Stop();
            ts = stopwatch.Elapsed.TotalSeconds;
            int total = 0;
            for (var i = 0; i <  countCPU; i++) total += tasks[i].Result;
            Console.WriteLine("Parallel search laikas   : {0} -> kiekis: {1}", ts, total);
            Console.Beep();
        }

        private static int[] RndIntArray(int seed, int n)
        {
            int[] arr = new int[n];
            Random rnd = new Random(seed);
            for (int i = 0; i < n; i++)
            {
                arr[i] = rnd.Next(25, 50);
            }
            return arr;
        }

        private static int[] RndIntArrayLimited(int seed, int n, int limit)
        {
            int[] arr = new int[n];
            Random rnd = new Random(seed);
            for (int i = 0; i < n; i++)
            {
                arr[i] = rnd.Next(limit);
            }
            return arr;
        }
    }
}
