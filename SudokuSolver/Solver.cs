using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Support;
using Algorithms;


namespace SudokuSolver
{
    public class Solver
    {
        public  int[][] Solve(int[][] matrix )
        {
            int len = matrix.Length;
            int[][] output = { } ;            
            List<int>[][] possibility_matrix = GetPossibilityMatrix(matrix);            
            //DisplayPossibilityMatrix(possibility_matrix);

            //Call Pile Exclusion Algorithm
            PileExclusion exclude = new PileExclusion();
            possibility_matrix = exclude.RunPileExclusion(possibility_matrix);
            possibility_matrix = Arrange.RemoveRedundency(possibility_matrix);
            possibility_matrix = Arrange.RedoPossibilityMatrix(possibility_matrix);

            // Pair Elimination algorithm
            PairElimination pairElimination = new PairElimination();
            possibility_matrix = pairElimination.ImplementPairElimination(possibility_matrix);

            //Single Chain Algorithm

            Helpers.DisplayPossibilityMatrix(possibility_matrix);
            return output;
        }

       

        List<int>[][] GetPossibilityMatrix(int[][] current)
        {
            int len = current.Length;
            //initialise possibility matrix
            List<int>[][] possibility_matrix = new List<int>[len][];
            for (int i = 0; i < possibility_matrix.Length; i++)
            {
                possibility_matrix[i] = Helpers.InitializePossibilityMatrix(len);
            }
            List<int>[] row_possibility = Helpers.InitializePossibilityMatrix(len);
            List<int>[] column_possibility = Helpers.InitializePossibilityMatrix(len);
            List<int>[] segment_possibility = Helpers.InitializePossibilityMatrix(len);

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
                        int seg = Helpers.FindSegment(i, j,len);
                        if(!segment_possibility[seg].Contains(elem))
                            segment_possibility[seg].Add(elem);
                    }
                }
            }
           
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
                            int seg = Helpers.FindSegment(i,j, len);
                            if((!row_possibility[i].Contains(k)) && (!column_possibility[j].Contains(k)) && (!segment_possibility[seg].Contains(k))){
                                possibility_matrix[i][j].Add(k);
                            }
                        }
                    }
                }
            }
            
            return possibility_matrix;
        }        

    }
     
}
