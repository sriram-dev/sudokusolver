using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;

namespace SudokuSolverTest
{
    [TestClass]
    public class SudokuTest
    {
        [TestMethod]
        public void TestSolve()
        {
           // int[][] input = new int[4][]{new int[4]{1,0,0,0}, new int[4]{0,0,0,3}, new int[4]{4,0,0,0}, new int[4]{0,2,0,0}};
            int[][] input = new int[9][] {new int[9]{ 9,0,0,1,3,0,4,0,0}, new int[9]{ 0,5,0,8,9,6,3,7,0}, new int[9]{ 0,2,6,0,0,4,0,0,0},
            new int[9]{ 0,0,1,6,0,7,0,0,0}, new int[9]{ 8,6,7,5,0,9,0,0,0}, new int[9]{0,0,0,2,0,3,0,0,0} ,
            new int[9]{6,0,0,3,0,0,9,4,0}, new int[9]{0,1,3,9,2,5,0,6,0}, new int[9]{0,0,5,4,0,0,1,0,3}};

            int[][] input2 = new int[9][]{ new int[9]{5,0,0,0,0,4,0,0,0}, new int[9]{0,0,7,0,0,1,0,0,0} , new int[9]{0,1,3,5,0,0,2,0,8},
                new int[9]{0,8,0,1,6,0,0,0,4},new int[9]{6,9,0,0,0,0,0,8,2} , new int[9]{3,0,0,0,5,8,0,1,0},
                new int[9]{2,0,6,7,0,5,8,4,0}, new int[9]{0,0,0,4,0,0,3,0,0}, new int[9]{1,0,0,8,0,0,0,0,5}};
            
            Solver s = new Solver();
          //  Display2x2(input);
            int[][] output = s.Solve(input2);
           // Console.WriteLine(output[0][2]);
            Display2x2(output);
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
    }
}
