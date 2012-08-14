using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Support;

namespace Algorithms
{
    public class PairElimination
    {
       public List<int>[][] ImplementPairElimination(List<int>[][] matrix)
        {
            //checking row wise 
            Console.WriteLine("------- In Pair Elimination ---------------");
            bool repeat = false;
            while (true)
            {
                repeat = false;
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[i].Length; j++)
                    {

                        if (matrix[i][j].Count == 2)
                        {
                            for (int l = 0; l < matrix[i].Length; l++)
                            {
                                if (l != j)
                                {
                                    if (matrix[i][l].Count == 2)
                                    {
                                        if (matrix[i][j].Contains(matrix[i][l][0]) && (matrix[i][j].Contains(matrix[i][l][1])))
                                        {
                                            //found a pair ! 
                                            //remove all instances of matrix[i][j][0] and [i][j][1] from any other column except l and j
                                            for (int m = 0; m < matrix[i].Length; m++) //m represents any column number which is not in pair
                                            {
                                                if ((m != j) && (m != l))
                                                {
                                                    List<int> toRemove = new List<int>();
                                                    for (int n = 0; n < matrix[i][m].Count; n++)
                                                    {
                                                        if (matrix[i][j].Contains(matrix[i][m][n]))
                                                        {
                                                            toRemove.Add(matrix[i][m][n]);
                                                            Console.WriteLine("remving " + matrix[i][m][n]);
                                                            repeat = true;
                                                        }
                                                    }
                                                    for (int n = 0; n < toRemove.Count; n++)
                                                    {
                                                        matrix[i][m].Remove(toRemove[n]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Wow 5 iterators !! 

                // now Column wise 
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[i].Length; j++)
                    {

                        if (matrix[j][i].Count == 2)
                        {
                            for (int l = 0; l < matrix[i].Length; l++)
                            {
                                if (l != j)
                                {
                                    if (matrix[l][i].Count == 2)
                                    {
                                        if (matrix[j][i].Contains(matrix[l][i][0]) && (matrix[j][i].Contains(matrix[l][i][1])))
                                        {
                                            //found a pair ! 
                                            //remove all instances of matrix[i][j][0] and [i][j][1] from any other column except l and j
                                            for (int m = 0; m < matrix[i].Length; m++) //m represents any column number which is not in pair
                                            {
                                                if ((m != j) && (m != l))
                                                {
                                                    List<int> toRemove = new List<int>();
                                                    for (int n = 0; n < matrix[m][i].Count; n++)
                                                    {
                                                        if (matrix[j][i].Contains(matrix[m][i][n]))
                                                        {
                                                            toRemove.Add(matrix[m][i][n]);
                                                            Console.WriteLine("removing " + matrix[m][i][n]);
                                                            repeat = true;
                                                        }
                                                    }
                                                    for (int n = 0; n < toRemove.Count; n++)
                                                    {
                                                        matrix[m][i].Remove(toRemove[n]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // List<int>[] seg_positions = GetSegmentPositions(3, 1, matrix.Length);
                // Console.WriteLine("segment positions");
                // Display1xList(seg_positions);
                // segment wise parse 
                // Algo : 
                // parse every element --- get every other positions of that segment
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        if (matrix[i][j].Count == 2)
                        {
                            List<int>[] seg_positions = Helpers.GetSegmentPositions(i, j, matrix.Length);
                            for (int k = 0; k < seg_positions.Length; k++)
                            {
                                //each position is in seg_pos[k]
                                int xpos = seg_positions[k][0];
                                int ypos = seg_positions[k][1];
                                if (matrix[xpos][ypos].Count == 2)
                                {
                                    if ((matrix[i][j].Contains(matrix[xpos][ypos][0])) && (matrix[i][j].Contains(matrix[xpos][ypos][1])))
                                    {
                                        //now we found 2 entries in a segment which has same elements 
                                        //again parse each element of seg_positions array 
                                        //remove all elements of matrix[i][j]
                                        for (int l = 0; l < seg_positions.Length; l++)
                                        {
                                            if (l != k)
                                            {
                                                List<int> toRemove = new List<int>();
                                                //parse nos in matrix[segmentpos[l][0] and [l][1]
                                                int xp = seg_positions[l][0];
                                                int yp = seg_positions[l][1];
                                                for (int m = 0; m < matrix[xp][yp].Count; m++)
                                                {
                                                    if (matrix[i][j].Contains(matrix[xp][yp][m]))
                                                    {
                                                        //mat contains tht element 
                                                        toRemove.Add(matrix[xp][yp][m]);
                                                        repeat = true;
                                                    }
                                                }
                                                for (int m = 0; m < toRemove.Count; m++)
                                                {
                                                    matrix[xp][yp].Remove(toRemove[m]);
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }


                if (!repeat)
                    break;
            }
            matrix = Arrange.RedoPossibilityMatrix(matrix);
            return matrix;
        }
    }
}
