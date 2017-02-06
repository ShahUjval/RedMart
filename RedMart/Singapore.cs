using System;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace RedMart
{
    public class Singapore
    {
        /// <summary>
        /// 
        /// </summary>
        static int[,] mapMatrix = new int[,] { };
        /// <summary>
        /// 
        /// </summary>
        static int[,] pathLengthMatrix = new int[,] { };
        /// <summary>
        /// 
        /// </summary>
        static int[,] finalElevationMatrix = new int[,] { };
        /// <summary>
        /// 
        /// </summary>
        static int row = 0;
        /// <summary>
        /// 
        /// </summary>
        static int column = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            PopulateMatrix();

            PopulatePathLengthAndFinalElevationMatrix();

            #region Printing the Matrix
            //loop through the mapMatrix 
            /*
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    Console.Write(mapMatrix[i,j] + " ");
                }
                Console.WriteLine();
            }
             * */
            #endregion

            //BruteForce
            
            int length = -1;
            int drop = -1;
            // Pass as Refrence to get the Values.
            int pathLength;
            int finalElevation;

            int _tempDrop;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    int initialElevation = mapMatrix[i, j];
                    findLongestPathWithSteepestAscent(i, j, out pathLength, out finalElevation);  // for i,jth element
                    _tempDrop = initialElevation - finalElevation;
                    if (pathLength > length || (pathLength == length && _tempDrop > drop))
                    {
                        length = pathLength;
                        drop = _tempDrop;
                    }
                }
            }

            //after the brute force run Length and drop should give you the correct answer.

            Console.WriteLine("Length = {0} , drop = {1}", length, drop);
            Console.ReadLine();
        }

        /// <summary>
        /// Intilizes the pathLengthMatrix and finalElevationMatrix with the default values
        /// </summary>
        private static void PopulatePathLengthAndFinalElevationMatrix()
        {
            pathLengthMatrix = new int[row, column];
            finalElevationMatrix = new int[row, column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    pathLengthMatrix[i, j] = finalElevationMatrix[i, j] = -1;
                }
            }
        }

        /// <summary>
        /// Method to Populate the Matrix from the Given map.txt file
        /// </summary>
        private static void PopulateMatrix()
        {
            //Bool to read the first line
            bool isFirst = true;


            //static counter to count the number of Rows 
            int rowCount = 0;

            // Read every line in the file.
            using (StreamReader reader = new StreamReader(@"C:\map.txt")) //Matrix.txt
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirst)
                    {
                        //fetch the n*n size and intilize the array
                        string[] size = line.Split(' ');

                        // retrieve the row and column size
                        row = int.Parse(size[0]);
                        column = int.Parse(size[1]);

                        // create the mapMatrix of size row*column
                        mapMatrix = new int[row, column];

                        // our task is done. 
                        isFirst = false;
                    }
                    else
                    {
                        //start filling mapMatrix - retrieve the first row in to array
                        string[] rowElements = line.Split(' ');

                        //store one row at a time.
                        for (int j = 0; j < column; j++)
                        {
                            mapMatrix[rowCount, j] = int.Parse(rowElements[j]);
                        }

                        // Advance to next Row
                        rowCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Method will find the Longest Path and Steepest Ascent
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="pathLength"></param>
        /// <param name="finalElevation"></param>
        private static void findLongestPathWithSteepestAscent(int i, int j, out int pathLength, out int finalElevation)
        {

            #region commented code
            /*
            // making sure we are within the boundries
            if((j+1) <= column && (j-1) >= 0)
            {
                if (mapMatrix[i, (j + 1)] < mapMatrix[i, j])
                {

                }
                if (mapMatrix[i, (j - 1)] < mapMatrix[i, j])
                {

                }

            }

            if ((i + 1) <= row && (i - 1) >= 0)
            {
                if (mapMatrix[(i + 1), j] < mapMatrix[i, j])
                {

                }
                if (mapMatrix[(i - 1), j] < mapMatrix[i, j])
                {

                }
            }
            */
            #endregion

            pathLength = pathLengthMatrix[i, j];
            if (pathLength != -1)
            {
                finalElevation = finalElevationMatrix[i, j];
            }

            int currentElevation = mapMatrix[i, j];
            int longestPath = 0;
            int lowestElevation = currentElevation;

            Point[] neighbours =
                {
                    new Point(i, j-1),
                    new Point(i, j+1),
                    new Point(i-1, j),
                    new Point(i+1, j),
                };

            foreach (var point in neighbours)
            {
                int neighbour = 0;
                try
                {
                    neighbour = mapMatrix[point.Y, point.X];
                }
                catch (Exception)
                {
                    continue;
                }

                if (neighbour < currentElevation)
                {
                    findLongestPathWithSteepestAscent(point.Y, point.X, out pathLength, out finalElevation);
                    if (pathLength > longestPath || (pathLength == longestPath && finalElevation < lowestElevation))
                    {
                        longestPath = pathLength;
                        lowestElevation = finalElevation;
                    }
                }
            }

            pathLength = longestPath + 1;
            pathLengthMatrix[i, j] = pathLength;
            finalElevationMatrix[i, j] = lowestElevation;
            finalElevation = lowestElevation;

        }

    }
}
