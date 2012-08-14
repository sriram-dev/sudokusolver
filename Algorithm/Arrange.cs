using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Support;


namespace Algorithms
{
    public class Arrange
    {
         public static List<int>[][] RedoPossibilityMatrix(List<int>[][] matrix)
        {
            int len = matrix.Length;

            bool repeat = false;
            while (true)
            {
                List<int>[] row_possibility = Helpers.InitializePossibilityMatrix(len);
                List<int>[] column_possibility = Helpers.InitializePossibilityMatrix(len);
                List<int>[] segment_possibility = Helpers.InitializePossibilityMatrix(len);

                repeat = false;

                //Fill row and column and segment possibilities with just one [parse
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < len; j++)
                    {
                        if (matrix[i][j].Count == 1)
                        {

                            int elem = matrix[i][j][0];
                            if (!row_possibility[i].Contains(elem))
                                row_possibility[i].Add(elem);
                            if (!column_possibility[j].Contains(elem))
                                column_possibility[j].Add(elem);
                            int seg = Helpers.FindSegment(i, j, len);
                            if (!segment_possibility[seg].Contains(elem))
                                segment_possibility[seg].Add(elem);
                        }

                    }
                }
                //  Console.WriteLine("row possibility in redo is ");
                // Display1xList(row_possibility);

                //for positions  which have more than 1 element, remove elements if they are in any of row,column or segment possibilities
                //?
                for (int i = 0; i < len; i++)
                {
                    List<int>[] vertices_list = Helpers.InitializePossibilityMatrix(len);
                    for (int j = 0; j < len; j++)
                    {
                        for (int k = 1; k <= len; k++)
                        {

                            if (matrix[i][j].Contains(k))
                            {
                                vertices_list[k - 1].Add(j);
                            }

                        }
                    }
                    //?
                    //  Console.WriteLine("inside Redo possibility matrix .. i value is :" + i);
                    //DisplayPossibilityMatrix(matrix);
                    for (int j = 0; j < len; j++)
                    {
                        if (matrix[i][j].Count > 1)
                        {
                            List<int> toremove = new List<int>();
                            for (int k = 0; k < matrix[i][j].Count; k++)
                            {
                                int elem = matrix[i][j][k];

                                int seg = Helpers.FindSegment(i, j, len);

                                if (row_possibility[i].Contains(elem) || (column_possibility[j].Contains(elem)) || (segment_possibility[seg].Contains(elem)))
                                {
                                   //   Console.WriteLine("removing element " + elem);
                                    //matrix[i][j].Remove(elem);
                                    toremove.Add(elem);
                                    repeat = true;
                                }

                            }
                            for (int k = 0; k < toremove.Count; k++)
                            {
                                matrix[i][j].Remove(toremove[k]);
                            }


                        }
                    }
                    matrix = RemoveRedundency(matrix);
                }
                if (!repeat)
                {
                    break;
                }

            }
            return matrix;
        }


        public static List<int>[][] RemoveRedundency(List<int>[][] matrix)
        {
            int len = matrix.Length;
            //get the vertices list from the possibility matrix for every row
            // Console.WriteLine("inside Remove redundancy ");
            //DisplayPossibilityMatrix(matrix);
            for (int i = 0; i < len; i++)
            {
                List<int>[] vertices_list = Helpers.InitializePossibilityMatrix(len);

                for (int j = 0; j < len; j++)
                {
                    for (int k = 1; k <= len; k++)
                    {
                        if (matrix[i][j].Contains(k))
                        {
                            vertices_list[k - 1].Add(j);
                        }
                    }
                }
                
                // Display1xList(vertices_list);

                // Console.WriteLine("corres row is ");
                // Display1xList(matrix[i]);
                for (int j = 0; j < len; j++)
                {
                    List<int> toremove = new List<int>();
                    int cnt = matrix[i][j].Count;
                    if (cnt > 1)
                    {
                        //       Console.WriteLine("count greater than 1 for pos " + i + "," + j);
                        for (int k = 0; k < cnt; k++)
                        {
                            int e = matrix[i][j][k];
                            //  Console.WriteLine("parsing element " + e);
                            // Console.WriteLine("count of " + e + "is " + vertices_list[e-1].Count);
                            if (vertices_list[e-1].Count == 1)
                            {
                                //     Console.WriteLine("count is 1 ");
                                
                                    for (int m = 0; m < matrix[i][j].Count; m++)
                                    {
                                        if (matrix[i][j][m] != e)
                                        {                                          
                                            toremove.Add(matrix[i][j][m]);
                                        }
                                    }
                                

                            }
                        }                       
                    }
                    for (int k = 0; k < toremove.Count; k++)
                    {
                        matrix[i][j].Remove(toremove[k]);
                    }
                }
                //Console.WriteLine("after removing redundancy ");
                //Display1xList(matrix[i]);
            }

            return matrix;
        }

    
    }
}
