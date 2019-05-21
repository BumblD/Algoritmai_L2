using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2._4Uzd
{
    class UnboundedKnapsackParallel
    {
        static int max(int a, int b)
        {
            return (a > b) ? a : b;
        }
        //Returns the maximum value that can
        // be put in a knapsack of capacity W
        public static int KnapsackRecursive(int W, int[] wt, int[] val, int n)
        {
            if (n <= 0 || W <= 0)
                return 0;
            // If weight of the nth item is  
            // more than Knapsack capacity W, 
            // then this item cannot be  
            // included in the optimal solution 
            if (wt[n - 1] > W)
                return KnapsackRecursive(W, wt, val, n - 1);
            // Return the maximum of two cases:  
            // (1) nth item included  
            // (2) not included 
            else return max(val[n - 1] +
                KnapsackRecursive(W - wt[n - 1], wt, val, n),
                       KnapsackRecursive(W, wt, val, n - 1));
        }

        public static int KnapsackParallel(int W, int n, int[] val, int[] wt)
        {
            if (n == 0 || W == 0)
                return 0;
            if (wt[n - 1] > W)
            {
                int data = 0;
                int countCPU = 4;
                Task[] tasks = new Task[countCPU];
                for (var j = 0; j < countCPU; j++)
                    tasks[j] = Task.Factory.StartNew(
                    (Object h) =>
                    {
                        data = KnapsackRecursive(W, wt, val, n - 1);
                    }, j);
                Task.WaitAll(tasks);
                return data;
            }
            else
            {
                int data = 0;
                int countCPU = 4;
                Task[] tasks = new Task[countCPU];
                for (var j = 0; j < countCPU; j++)
                    tasks[j] = Task.Factory.StartNew(
                    (Object h) =>
                    {
                        data = max(val[n - 1] +
                  KnapsackRecursive(W - wt[n - 1], wt, val, n),
                         KnapsackRecursive(W, wt, val, n - 1));
                    }, j);
                Task.WaitAll(tasks);
                return data;
            }
        }
    }
}
