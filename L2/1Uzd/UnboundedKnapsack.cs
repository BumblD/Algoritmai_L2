using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2._1Uzd
{
    class UnboundedKnapsack
    {
        private static int max(int i, int j)
        {
            return (i > j) ? i : j;
        }

        // Returns the maximum value  
        // with knapsack of W capacity 
        public static int UnboundedKnapsackDynamic(int n, int W, int[] val, int[] wt)
        {
            // dp[i] is going to store maximum value 
            // with knapsack capacity i. 
            int[] dp = new int[W + 1];

            for (int i = 0; i <= W; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (wt[j] <= i)
                    {
                        dp[i] = Math.Max(dp[i], dp[i - wt[j]] + val[j]);
                    }
                }
            }
            return dp[W];
        }

        // Returns the maximum value that can  
        // be put in a knapsack of capacity W 
        public static int UnboundedKnapsackRecursive(int W, int[] wt, int[] val, int n)
        {
            // Base Case 
            if (n == 0 || W == 0)
                return 0;

            // If weight of the nth item is  
            // more than Knapsack capacity W, 
            // then this item cannot be  
            // included in the optimal solution 
            if (wt[n - 1] > W)
                return UnboundedKnapsackRecursive(W, wt, val, n - 1);

            // Return the maximum of two cases:  
            // (1) nth item included  
            // (2) not included 
            else return max(val[n - 1] + 
                UnboundedKnapsackRecursive(W - wt[n - 1], wt, val, n), 
                UnboundedKnapsackRecursive(W, wt, val, n - 1));
        }
    }
}
