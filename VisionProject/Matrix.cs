/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisionProject
{
    public static class Matrix
    {

        public static double[,] Sobel3x3Horizontal
        {
            get
            {
                return new double[,] 
                { { -1,  0,  1, }, 
                  { -2,  0,  2, }, 
                  { -1,  0,  1, }, };
            }
        }

        public static double[,] Sobel3x3Vertical
        {
            get
            {
                return new double[,] 
                { {  1,  2,  1, }, 
                  {  0,  0,  0, }, 
                  { -1, -2, -1, }, };
            }
        }

        public static double[,] Prewitt3x3Horizontal
        {
            get
            {
                return new double[,] 
                { { -1,  0,  1, }, 
                  { -1,  0,  1, }, 
                  { -1,  0,  1, }, };
            }
        }

        public static double[,] Prewitt3x3Vertical
        {
            get
            {
                return new double[,] 
                { {  1,  1,  1, }, 
                  {  0,  0,  0, }, 
                  { -1, -1, -1, }, };
            }
        }

    }
}
