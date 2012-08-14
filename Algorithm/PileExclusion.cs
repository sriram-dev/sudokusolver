using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Support;
using Arrangement;

namespace Algorithms
{
    public class PileExclusion
    {
        public List<int>[][] RunPileExclusion(List<int>[][] nmatrix)
        {
            //rowwise 
            List<int>[][] matrix = nmatrix;
            int i = 0;
            int len = matrix.Length;
            bool isCommon;
            int count = 0;
            while (i < len)
            {
                isCommon = false;
                Console.WriteLine("i val : " + i);
                List<int>[] vertices_list = Helpers.InitializePossibilityMatrix(len);
                List<int>[] common_vertices = Helpers.InitializePossibilityMatrix(len);
                //so if 1 and 4 and 5 have common vertices then 4 and 5 will be added to 1
                //1 and 4 and 5 will be added to checkednos
                List<int> checked_nos = new List<int>();
                //Step 1 -- Forming the Vertices List for the Numbers.
                for (int k = 1; k <= len; k++)
                {
                    for (int j = 0; j < len; j++)
                    {
                        if (matrix[i][j].Contains(k))
                        {
                            if (!vertices_list[k - 1].Contains(j))
                                vertices_list[k - 1].Add(j);
                        }
                    }
                }
                //Console.WriteLine("vertices list ");
                //Display1xList(vertices_list);
                //Step 2 -- Parse vertices list to check if any of those have equal vertices
                Parse(ref isCommon, len, vertices_list, ref common_vertices, ref checked_nos);
                if (isCommon)
                {
                    // Console.WriteLine("is common true ");
                    Exclude(ref common_vertices, ref matrix, ref vertices_list, ref i);
                    matrix = Arrange.RedoPossibilityMatrix(matrix);
                    // DisplayPossibilityMatrix(matrix);
                }
                else
                {
                    //  Console.WriteLine("iscommon false");
                    i++;
                    count = 0;
                }
                count++;
                if (count > 8)
                {
                    i++;
                    //Console.WriteLine("count is greater than 5 ");
                    count = 0;
                }

            }
            //column wise
            i = 0;
            count = 0;
            while (i < len)
            {
                isCommon = false;
                List<int>[] vertices_list = Helpers.InitializePossibilityMatrix(len);
                List<int>[] common_vertices = Helpers.InitializePossibilityMatrix(len);
                //so if 1 and 4 and 5 have common vertices then 4 and 5 will be added to 1
                //1 and 4 and 5 will be added to checkednos
                List<int> checked_nos = new List<int>();
                for (int k = 1; k <= len; k++)
                {
                    for (int j = 0; j < len; j++)
                    {
                        if (matrix[j][i].Contains(k))
                        {
                            vertices_list[k - 1].Add(j);
                        }
                    }
                }
                Parse(ref isCommon, len, vertices_list, ref common_vertices, ref checked_nos);


                if (isCommon)
                {
                    ExcludeColumnWise(ref common_vertices, ref matrix, ref vertices_list, ref i);
                    matrix = Arrange.RedoPossibilityMatrix(matrix);
                    //DisplayPossibilityMatrix(matrix);
                }
                else
                {
                    i++;
                }
                count++;
                if (count > 8)
                {
                    i++;
                    count = 0;
                }

            }

            //segment wise           
            i = 0;
            count = 0;
            while (i < len)
            {
                isCommon = false;
                int movearea = (int)Math.Sqrt(len);
                int start_from_row = (i / movearea) * movearea;
                int start_from_col = (i % movearea) * movearea;
                List<int>[] vertices_list = Helpers.InitializePossibilityMatrix(len);
                List<int>[] common_vertices = Helpers.InitializePossibilityMatrix(len);
                //so if 1 and 4 and 5 have common vertices then 4 and 5 will be added to 1
                //1 and 4 and 5 will be added to checkednos
                List<int> checked_nos = new List<int>();
                //segment i say 0
                // 0 to movearea and 0 to movearea
                //say 1 -->  3 to movearea 
                // Console.WriteLine("start from row and col " + start_from_row + start_from_col);


                for (int j = start_from_row; j < (start_from_row + movearea); j++)
                {
                    for (int m = start_from_col; m < (start_from_col + movearea); m++)
                    {

                        int pos = (j - start_from_row) * movearea + (m - start_from_col);
                        for (int k = 1; k <= len; k++)
                        {
                            if (matrix[j][m].Contains(k))
                            {
                                vertices_list[k - 1].Add(pos);
                            }
                        }
                    }
                }

                //  Console.WriteLine("vertices list for " + i + " segment");

                //   Display1xList(vertices_list);
                Parse(ref isCommon, len, vertices_list, ref common_vertices, ref checked_nos);

                if (isCommon)
                {
                    ExcludeSegmentWise(ref common_vertices, ref matrix, ref vertices_list, ref i);
                    matrix = Arrange.RedoPossibilityMatrix(matrix);
                    //  DisplayPossibilityMatrix(matrix);
                }
                else
                {
                    i++;
                }
                count++;
                if (count > 8)
                {
                    i++;
                    count = 0;
                }

            }

            return matrix;
        }

        void Parse(ref bool isCommon, int len, List<int>[] vertices_list, ref List<int>[] common_vertices, ref List<int> checked_nos)
        {

            for (int l = 0; l < len; l++)
            {
                if (!checked_nos.Contains(l))
                {
                    for (int m = l + 1; m < len; m++)
                    {
                        if (vertices_list[l].Count == vertices_list[m].Count)
                        {
                            //check if they have same elements
                            int n = 0;
                            for (n = 0; n < vertices_list[l].Count; n++)
                            {
                                if (!vertices_list[m].Contains(vertices_list[l][n]))
                                {
                                    break;
                                }
                            }
                            //if it contains all the same vertices then
                            if (n == vertices_list[l].Count)
                            {
                                common_vertices[l].Add(m);
                                isCommon = true;
                                checked_nos.Add(m);
                            }
                        }
                    }
                }
            }
        }
        //common_vertices list will have list of nos that have common vertices with this number(starts from 0)
        //for each element in common_vertices , if it is non empty 
        //get the vertices_list of that element 
        //in each of those positions in matrix remove any number which is not one of these nos

        //Step 3- Remove nos From Matrix if Pile Exclusion is successful
        void Exclude(ref List<int>[] common_vertices, ref List<int>[][] matrix, ref List<int>[] vertices_list, ref int i)
        {
            Console.WriteLine("in exlude rowwise for row " + i);
            for (int o = 0; o < common_vertices.Length; o++)
            {
                if (common_vertices[o].Count != 0)
                {
                    // Console.WriteLine("common vertices count: " + common_vertices[o].Count + "vertices list count " + vertices_list[o].Count);
                    if ((common_vertices[o].Count + 1) == vertices_list[o].Count)
                    {
                        //for each element in verticeslist of 0
                        //o and all elements in common_vertices[0]
                        //Vertices List = For each number it contains the set of all vertices that the number is in (for this column)
                        //Common vertices = For each number it ll contain the set of all other numbers which has the same vertices 


                        for (int k = 0; k < matrix[i].Length; k++)
                        {
                            if (vertices_list[o].Contains(k)) // if the number o+1 has k as its vertex
                            {
                                List<int> toremove = new List<int>();
                                for (int j = 0; j < matrix[i][k].Count; j++) //for each element in that vertex
                                {
                                    //Console.WriteLine("checking for element :" + (matrix[i][k][j]) + "for the number" + (o + 1));
                                    int elem = 0;
                                    elem = matrix[i][k][j];
                                    //if the element is not itself or any element in common vertices list of it , remove the element
                                    if ((elem != (o + 1)) && (!common_vertices[o].Contains(elem - 1)))
                                    {
                                       // Console.WriteLine("going to remove element " + elem);
                                        toremove.Add(elem);
                                    }
                                }
                                for (int j = 0; j < toremove.Count; j++)
                                {
                                    matrix[i][k].Remove(toremove[j]);
                                }
                            }
                        }
                    }
                }
            }
        }

        void ExcludeColumnWise(ref List<int>[] common_vertices, ref List<int>[][] matrix, ref List<int>[] vertices_list, ref int i)
        {
            //if 2 nos have same no of vertices and tht number shld be <= 2(IMPORTANT) 
            Console.WriteLine("inside Exclude column wise " + i);
            for (int o = 0; o < common_vertices.Length; o++)
            {
                if (common_vertices[o].Count != 0)
                {
                    //for each element in verticeslist of 0
                    //o and all elements in common_vertices[0]
                    for (int k = 0; k < matrix[i].Length; k++)
                    {
                        if ((common_vertices[o].Count + 1) == vertices_list[o].Count)
                        {

                            if (vertices_list[o].Contains(k))
                            {
                                List<int> toremove = new List<int>();
                                for (int j = 0; j < matrix[k][i].Count; j++)
                                {
                                    //Console.WriteLine("checking for element :" + (matrix[i][k][j]) + "for the number" + (o + 1));
                                    int elem = 0;
                                    elem = matrix[k][i][j];

                                    if ((elem != (o + 1)) && (!common_vertices[o].Contains(elem - 1)))
                                    {
                                         Console.WriteLine("going to remove element " + elem);
                                        //matrix[k][i].Remove(elem);
                                        toremove.Add(elem);
                                    }
                                }
                                for (int j = 0; j < toremove.Count; j++)
                                {
                                    matrix[k][i].Remove(toremove[j]);
                                }
                            }
                        }
                    }
                }
            }
        }

        void ExcludeSegmentWise(ref List<int>[] common_vertices, ref List<int>[][] matrix, ref List<int>[] vertices_list, ref int i)
        {
           
            //common vertices list : for each number -- list that contains other numbers which have same vertices list as this number

            for (int o = 0; o < common_vertices.Length; o++)
            {
                if (common_vertices[o].Count != 0)
                {
                    if ((common_vertices[o].Count + 1) == vertices_list[o].Count)
                    {

                        //for each element in verticeslist of 0
                        //o and all elements in common_vertices[0]
                        int pos_i = 0;
                        int pos_j = 0;
                        int current = o, k = 0, j = 0;
                        int movearea = (int)Math.Sqrt(matrix.Length);
                        int start_from_row = (i / movearea) * movearea;
                        int start_from_col = (j / movearea) * movearea;
                        bool breakout = false;
                        for (k = start_from_row; k < (start_from_row + movearea); k++)
                        {
                            for (j = start_from_col; j < (start_from_col + movearea); j++)
                            {
                                if (current == 0)
                                {
                                    breakout = true;
                                    break;
                                }
                                current--;
                            }
                            if (breakout)
                                break;
                        }
                        pos_i = k;
                        pos_j = j;
                        //  Console.WriteLine("Inside Segment number : " + i + "and position " + o);
                        // Console.WriteLine("i, j value is " + pos_i + "," + pos_j);
                        List<int> toremove = new List<int>();
                        if (matrix[pos_i][pos_j].Count > 1)
                        {
                            for (j = 0; j < matrix[pos_i][pos_j].Count; j++)
                            {

                                //Console.WriteLine("checking for element :" + (matrix[i][k][j]) + "for the number" + (o + 1));
                                int elem = 0;
                                elem = matrix[pos_i][pos_j][j];

                                if ((elem != (o+1)) && (!common_vertices[o].Contains(elem-1)))
                                {
                                  /*  Console.WriteLine("current o value is " + o);
                                    Helpers.DisplayPossibilityMatrix(matrix);
                                    Helpers.Display1xList(common_vertices);
                                    Console.WriteLine("vertices list ");
                                    Helpers.Display1xList(vertices_list);
                                    Console.WriteLine("going to remove element " + elem + " from position " + pos_i + pos_j);*/
                                    toremove.Add(elem);
                                }
                            }
                            for (j = 0; j < toremove.Count; j++)
                            {
                                matrix[pos_i][pos_j].Remove(toremove[j]);
                            }
                        }
                    }
                }
            }
        }    
    }
}
