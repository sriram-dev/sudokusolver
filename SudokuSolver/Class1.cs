using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public class Solver
    {
        public  int[][] Solve(int[][] matrix )
        {
            int len = matrix.Length;
            int[][] output = { } ;
            //Get the initial possibility matrix
            List<int>[][] possibility_matrix = GetPossibilityMatrix(matrix);
            Console.WriteLine("got possibility matrix");
            DisplayPossibilityMatrix(possibility_matrix);
            //Call Pile Exclusion Algorithm
            List<int>[][] newpossibility_matrix = RunPileExclusion(possibility_matrix);
            Console.WriteLine("After pile Exclusion ");           
            newpossibility_matrix = RemoveRedundency(newpossibility_matrix);
            newpossibility_matrix = RedoPossibilityMatrix(newpossibility_matrix);
            newpossibility_matrix = ImplementPairElimination(newpossibility_matrix);  
            //Aw we need more elimination methods. One last and then go for brute force
            //one more.. Single chain method !
            

            DisplayPossibilityMatrix(newpossibility_matrix);
            return output;
        }

        List<int>[][] RemoveRedundency(List<int>[][] matrix)
        {
            int len = matrix.Length;
            //get the vertices list from the possibility matrix for every row
           // Console.WriteLine("inside Remove redundancy ");
            //DisplayPossibilityMatrix(matrix);
            for (int i = 0; i < len; i++)
            {
                List<int>[] vertices_list = InitializePossibilityMatrix(len);
                
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
               // Console.WriteLine("vertices list of " + i);
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
                            if (vertices_list[e-1].Count==1)
                            {
                           //     Console.WriteLine("count is 1 ");
                                for (int m = 0; m < matrix[i][j].Count; m++)
                                {
                                    if (matrix[i][j][m] != e)
                                    {
                                      //  Console.WriteLine("removing " + matrix[i][j][m]);
                                        toremove.Add(matrix[i][j][m]);                                        
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < toremove.Count; k++)
                        {
                            matrix[i][j].Remove(toremove[k]);
                        }
                    }
                }
                //Console.WriteLine("after removing redundancy ");
                //Display1xList(matrix[i]);
            }

            return matrix;
        }


        List<int>[][] GetPossibilityMatrix(int[][] current)
        {
            int len = current.Length;
            //initialise possibility matrix
            List<int>[][] possibility_matrix = new List<int>[len][];
            for (int i = 0; i < possibility_matrix.Length; i++)
            {
                possibility_matrix[i] = InitializePossibilityMatrix(len);
            }
            List<int>[] row_possibility = InitializePossibilityMatrix(len);
            List<int>[] column_possibility = InitializePossibilityMatrix(len);
            List<int>[] segment_possibility = InitializePossibilityMatrix(len);

            //Fill row and column and segment possibilities with just one [parse
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (current[i][j] != 0)
                    {
                        int elem = current[i][j];
                        if(!row_possibility[i].Contains(elem))
                            row_possibility[i].Add(elem);
                        if(!column_possibility[j].Contains(elem))
                            column_possibility[j].Add(elem);
                        int seg = FindSegment(i, j,len);
                        if(!segment_possibility[seg].Contains(elem))
                            segment_possibility[seg].Add(elem);
                    }
                }
            }
            //Console.WriteLine("row possibility in possibilty matrix init");
          //  Display1xList(row_possibility);
            //Fill in the Possibility Matrix acc to other values 
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (current[i][j] != 0)
                    {
                        possibility_matrix[i][j].Add(current[i][j]);
                    }
                    else
                    {
                        for (int k = 1; k <= len; k++)
                        {
                            int seg = FindSegment(i,j, len);
                            if((!row_possibility[i].Contains(k)) && (!column_possibility[j].Contains(k)) && (!segment_possibility[seg].Contains(k))){
                                possibility_matrix[i][j].Add(k);
                            }
                        }
                    }
                }
            }
            
            return possibility_matrix;
        }

        List<int>[][] RedoPossibilityMatrix(List<int>[][] matrix)
        {
            int len = matrix.Length;
           
            bool repeat = false;
            while (true)
            {
                List<int>[] row_possibility = InitializePossibilityMatrix(len);
                List<int>[] column_possibility = InitializePossibilityMatrix(len);
                List<int>[] segment_possibility = InitializePossibilityMatrix(len);
              
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
                            int seg = FindSegment(i, j, len);
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
                    List<int>[] vertices_list = InitializePossibilityMatrix(len);
                    for (int j = 0; j < len; j++)
                    {
                        for (int k = 1; k <= len; k++)
                        {
                            
                                if (matrix[i][j].Contains(k))
                                {                                    
                                    vertices_list[k-1].Add(j);
                                }
                            
                        }
                    }
                    //?
                  //  Console.WriteLine("inside Redo possibility matrix .. i value is :" + i);
                    //DisplayPossibilityMatrix(matrix);
                    for (int j = 0; j < len; j++)
                    {
                           if(matrix[i][j].Count>1){
                               List<int> toremove = new List<int>();
                               for (int k = 0; k < matrix[i][j].Count; k++)
                               {
                                   int elem = matrix[i][j][k];
                                   
                                   int seg = FindSegment(i, j, len);
                                  
                                   if (row_possibility[i].Contains(elem) || (column_possibility[j].Contains(elem)) || (segment_possibility[seg].Contains(elem)))
                                   {
                                     //  Console.WriteLine("removing element " + elem);
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

        List<int>[][] RunPileExclusion(List<int>[][] nmatrix)
        {
           //rowwise 
            List<int>[][] matrix = nmatrix;
            int i=0;
            int len = matrix.Length;
            bool isCommon ;
            int count = 0;
            while (i < len)
            {
                isCommon = false;
                Console.WriteLine("i val : " + i);
                List<int>[] vertices_list = InitializePossibilityMatrix(len);
                List<int>[] common_vertices = InitializePossibilityMatrix(len);
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
                            if(!vertices_list[k-1].Contains(j))
                            vertices_list[k-1].Add(j);
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
                    matrix = RedoPossibilityMatrix(matrix);
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
                  List<int>[] vertices_list = InitializePossibilityMatrix(len);
                  List<int>[] common_vertices = InitializePossibilityMatrix(len);
                  //so if 1 and 4 and 5 have common vertices then 4 and 5 will be added to 1
                  //1 and 4 and 5 will be added to checkednos
                  List<int> checked_nos = new List<int>();
                  for (int k = 1; k <= len; k++)
                  {
                      for (int j = 0; j < len; j++)
                      {
                          if (matrix[j][i].Contains(k))
                          {
                              vertices_list[k-1].Add(j);
                          }
                      }
                  }
                  Parse(ref isCommon, len, vertices_list, ref common_vertices, ref checked_nos);


                  if (isCommon)
                  {
                      ExcludeColumnWise(ref common_vertices, ref matrix, ref vertices_list, ref i);
                      matrix = RedoPossibilityMatrix(matrix);
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
                  int start_from_col = (i%movearea) * movearea; 
                  List<int>[] vertices_list = InitializePossibilityMatrix(len);
                  List<int>[] common_vertices = InitializePossibilityMatrix(len);
                  //so if 1 and 4 and 5 have common vertices then 4 and 5 will be added to 1
                  //1 and 4 and 5 will be added to checkednos
                  List<int> checked_nos = new List<int>();
                  //segment i say 0
                  // 0 to movearea and 0 to movearea
                   //say 1 -->  3 to movearea 
                 // Console.WriteLine("start from row and col " + start_from_row + start_from_col);

                
                      for (int j = start_from_row; j < (start_from_row+ movearea); j++)
                      {
                          for(int m=start_from_col;m< (start_from_col+movearea);m++){

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
                      matrix = RedoPossibilityMatrix(matrix);
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

        void Parse(ref bool isCommon , int len ,List<int>[] vertices_list, ref List<int>[] common_vertices, ref List<int> checked_nos)
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
        void Exclude(ref List<int>[] common_vertices ,ref List<int>[][] matrix,ref List<int>[] vertices_list, ref int i ){
            Console.WriteLine("in exlude rowwise for row " + i);
            for (int o = 0; o < common_vertices.Length; o++)
                {
                    if (common_vertices[o].Count != 0)
                    {
                       // Console.WriteLine("common vertices count: " + common_vertices[o].Count + "vertices list count " + vertices_list[o].Count);
                        if ((common_vertices[o].Count+1) == vertices_list[o].Count)
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
                                        if ((elem != (o + 1)) && (!common_vertices[o].Contains(elem-1)))
                                        {
                                          //  Console.WriteLine("going to remove element " + elem);
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
                        if ((common_vertices[o].Count+1) == vertices_list[o].Count)
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
                                       // Console.WriteLine("going to remove element " + elem);
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
            //Are common Vertices Right ? 
            // Are Vertices List right ?

            
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
                        int start_from_col = (i % movearea) * movearea;
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

                                 if ((elem != (o + 1)) && (!common_vertices[o].Contains(elem - 1)))
                                 {
                                   //  Console.WriteLine("going to remove element " + elem);
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

        List<int>[][] ImplementPairElimination(List<int>[][] matrix)
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
                            List<int>[] seg_positions = GetSegmentPositions(i, j, matrix.Length);
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
            matrix = RedoPossibilityMatrix(matrix);
            return matrix;
        }

        List<int>[] GetSegmentPositions(int i, int j, int len)
        {
            List<int>[] arr = new List<int>[len-1];
            // initialize array
            for(int k=0;k< (len-1);k++){
                arr[k] = new List<int>();
            }
            int z =0;
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

        List<int>[] InitializePossibilityMatrix(int len)
        {
            List<int>[] mat = new List<int>[len];
            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = new List<int>();
            }
            return mat;
        }

        int FindSegment(int i, int j, int len)
        {
            int seg = 0;
            int divisor =(int) Math.Sqrt(len);
            int ival=0, jval=0;
            ival = (i<divisor) ? 0 : i/divisor;
            jval = (j<divisor) ? 0 : j/divisor;
            int temp = ival;
            while (temp != 0)
            {
                ival = ival + divisor -1;
                temp--;
            }
            seg = ival + jval;
            return seg;
        }

        void Display2x2(int[][] mat)
        {
            for (int i = 0; i < mat.Length; i++)
            {
                for (int j = 0; j < mat[i].Length; j++)
                {
                    Console.WriteLine(mat[i][j]);
                }
            }
        }

        void DisplayPossibilityMatrix(List<int>[][] mat)
        {
            for (int i = 0; i < mat.Length; i++)
            {
                for (int j = 0; j < mat[i].Length; j++)
                {
                    for(int k = 0; k < mat[i][j].Count; k++)
                    {
                        Console.Write(mat[i][j][k]); 
                    }

                    Console.Write("        ");
                }
                Console.WriteLine();
            }
        }

        void Display1xList(List<int>[] mat)
        {
            for (int i = 0; i < mat.Length; i++)
            {
                for (int j = 0; j < mat[i].Count; j++)
                {
                    Console.Write(mat[i][j] + ",");
                }
                Console.WriteLine();
            }
        }

        void Display1xn(List<int> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                Console.Write(i + ":" + arr[i] + " ");
            }
        }

    }
     
}
