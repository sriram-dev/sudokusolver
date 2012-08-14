using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Support
{
    public static class Helpers
    {
        public static List<int>[] InitializePossibilityMatrix(int len)
        {
            List<int>[] mat = new List<int>[len];
            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = new List<int>();
            }
            return mat;
        }

       public static void Display2x2(int[][] mat)
        {
            for (int i = 0; i < mat.Length; i++)
            {
                for (int j = 0; j < mat[i].Length; j++)
                {
                    Console.WriteLine(mat[i][j]);
                }
            }
        }

        public static void DisplayPossibilityMatrix(List<int>[][] mat)
        {
            for (int i = 0; i < mat.Length; i++)
            {
                for (int j = 0; j < mat[i].Length; j++)
                {
                    for (int k = 0; k < mat[i][j].Count; k++)
                    {
                        Console.Write(mat[i][j][k]);
                    }

                    Console.Write("        ");
                }
                Console.WriteLine();
            }
        }

        public static void Display1xList(List<int>[] mat)
        {
            for (int i = 0; i < mat.Length; i++)
            {
                Console.Write("i  :");
                for (int j = 0; j < mat[i].Count; j++)
                {
                    Console.Write(mat[i][j] + ",");
                }
                Console.WriteLine();
            }
        }

        public static void Display1xn(List<int> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                Console.Write(i + ":" + arr[i] + " ");
            }
        }

      public static int FindSegment(int i, int j, int len)
       {
           int seg = 0;
           int divisor = (int)Math.Sqrt(len);
           int ival = 0, jval = 0;
           ival = (i < divisor) ? 0 : i / divisor;
           jval = (j < divisor) ? 0 : j / divisor;
           int temp = ival;
           while (temp != 0)
           {
               ival = ival + divisor - 1;
               temp--;
           }
           seg = ival + jval;
           return seg;
       }

     public static  List<int>[] GetSegmentPositions(int i, int j, int len)
      {
          List<int>[] arr = new List<int>[len - 1];
          // initialize array
          for (int k = 0; k < (len - 1); k++)
          {
              arr[k] = new List<int>();
          }
          int z = 0;
          int movearea = (int)Math.Sqrt(len);
          int start_from_row = (i / movearea) * movearea;
          int start_from_col = (j / movearea) * movearea;
          Console.WriteLine("inside getsegment... movearea, startrow, startcolm " + movearea + start_from_row + start_from_col);
          for (int k = start_from_row; k < (start_from_row + movearea); k++)
          {
              for (int l = start_from_col; l < (start_from_col + movearea); l++)
              {
                  if (!((k == i) && (l == j)))
                  {
                      arr[z].Add(k);
                      arr[z].Add(l);
                      z++;
                  }
              }
          }
          return arr;
      }

    }
}
