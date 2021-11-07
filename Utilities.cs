using System;
using System.Collections.Generic;
using System.Drawing;

namespace Project
{
    public class Utilities
    {

        //A method to load an image from a path:
        public static Image LoadImage(string path)
        {
            Image img = Image.FromFile(path);
            return img;
        }

        //A method to save an array of colors(pixels) as a bitmap and then as an image to a path:
        public static void SavePhoto(Color[][] colors, string path)
        {
            var bmp = ConvertToBitmap(colors);
            bmp.Save(path);
        }

        //A method to convert an array of colors(pixels) to bitmap object:
        public static Bitmap ConvertToBitmap(Color[][] colors)
        {
            Bitmap bmp = new Bitmap(colors[0].Length, colors.Length, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < colors.Length; y++)
            {
                for (int x = 0; x < colors[y].Length; x++)
                    bmp.SetPixel(x, y, colors[y][x]);
            }
            return bmp;
        }

        //A method to convert image object to array of colors(pixels) using bitmap:
        public static Color[][] ConvertImageToColorArray(Image image)
        {
            Color[][] colors = new Color[image.Height][];
            using (Bitmap bmp = new Bitmap(image))
            {
                for (int y = 0; y < image.Height; y++)
                {
                    colors[y] = new Color[image.Width];
                    for (int x = 0; x < colors[y].Length; x++)
                        colors[y][x] = bmp.GetPixel(x, y);
                }
            }
            return colors;
        }

        // A method to calculate each pixels energy and store it to an array:
        public static double[][] CalculateEnergyArray(Color[][] colors)
        {
            double[][] Array = new double[colors.Length][];
            //interating inside pixels:
            for (int i = 0; i < colors.Length; i++)
            {
                Array[i] = new double[colors[i].Length];
                for (int j = 0; j < Array[i].Length; j++)
                {
                    double energy;
                    //implementing given energy function:
                    if (0 < i && i < colors.Length - 1 && 0 < j && j < colors[i].Length - 1)
                    {
                        double deltaXRed = colors[i + 1][j].R - colors[i - 1][j].R;
                        double deltaXGreen = colors[i + 1][j].G - colors[i - 1][j].G;
                        double deltaXBlue = colors[i + 1][j].B - colors[i - 1][j].B;
                        double deltaX = Math.Pow(deltaXRed, 2) + Math.Pow(deltaXGreen, 2) + Math.Pow(deltaXBlue, 2);

                        double deltaYRed = colors[i][j + 1].R - colors[i][j - 1].R;
                        double deltaYGreen = colors[i][j + 1].G - colors[i][j - 1].G;
                        double deltaYBlue = colors[i][j + 1].B - colors[i][j - 1].B;
                        double deltaY = Math.Pow(deltaYRed, 2) + Math.Pow(deltaYGreen, 2) + Math.Pow(deltaYBlue, 2);

                        energy = Math.Round(Math.Pow(deltaX + deltaY, 0.5), 3);
                    }
                    //setting the borders energy to 1.000(becasue theyre not calculatable using the function):
                    else
                        energy = 1_000;

                    Array[i][j] = energy;
                }
            }
            return Array;
        }
        public static Color[][] FandR_V_MinSeam_DP(Color[][] colors, Color[][] colors2, long SeamsCount, string redFilePath)
        {
            //loop with length of seams coount:
            for (int l = 1; l < SeamsCount + 1; l++)
            {
                var EnergyArray = CalculateEnergyArray(colors);
                int x = 0;
                int y = 0;
                Point[,] M = new Point[colors.Length, colors[0].Length];
                double[,] N = new double[colors.Length, colors[0].Length];

                for (int j = 0; j < colors[0].Length; j++)
                {
                    N[0, j] = 1_000;
                }

                //Edge pixels are only made of two coresponding available pixels comparison:
                for (int i = 1; i < colors.Length; i++)
                {
                    for (int j = 0; j < colors[i].Length; j++)
                    {
                        if (j == 0)
                        {
                            if (N[i - 1, j + 1] < N[i - 1, j])
                            {
                                x = i - 1;
                                y = j + 1;
                            }
                            else
                            {
                                x = i - 1;
                                y = j;
                            }


                        }
                        else if (j == colors[i].Length - 1)
                        {
                            if (N[i - 1, j] < N[i - 1, j - 1])
                            {
                                x = i - 1;
                                y = j;
                            }
                            else
                            {
                                x = i - 1;
                                y = j - 1;
                            }

                        }
                        //General edges made of three top pixels comparison:
                        else
                        {
                            x = i - 1;
                            y = j - 1;
                            if (N[i - 1, j - 1] < N[i - 1, j] && N[i - 1, j - 1] < N[i - 1, j + 1])
                            {
                                x = i - 1;
                                y = j - 1;
                            }

                            if (N[i - 1, j] < N[i - 1, j - 1] && N[i - 1, j] < N[i - 1, j + 1])
                            {
                                x = i - 1;
                                y = j;
                            }

                            if (N[i - 1, j + 1] < N[i - 1, j] && N[i - 1, j + 1] < N[i - 1, j - 1])
                            {
                                x = i - 1;
                                y = j + 1;
                            }
                        }
                        M[i, j] = new Point(x, y);

                        N[i, j] = EnergyArray[i][j] + N[x, y];
                    }

                }
                //find the minimum elemts index using the sorted dictionary:
                List<(double, int)> Dic = new List<(double, int)>();
                for (int z = 0; z < colors[0].Length; z++)
                {
                    Dic.Add((N[colors.Length - 1, z], z));
                }
                Dic.Sort((x, y) => y.Item1.CompareTo(x.Item1));
                Console.WriteLine(Dic[Dic.Count - 1]);
                int index = 0;
                index = Dic[(Dic.Count - l)].Item2;
                int myx = colors.Length - 1;
                int myy = index;
                Point myp = new Point();
                myp.X = myx;
                myp.Y = myy;
                colors[myx][myy] = Color.Red;
                colors[M[myx, myy].X][M[myx, myy].Y] = Color.Red;
                colors2[myx][myy] = Color.Red;
                colors2[M[myx, myy].X][M[myx, myy].Y] = Color.Red;

                //backtrack pixels and set to red:
                for (int z = 0; z < colors.Length - 2; z++)
                {
                    myx = M[myx, myy].X;
                    myy = M[myx, myy].Y;
                    colors[M[myx, myy].X][M[myx, myy].Y] = Color.Red;
                    colors2[M[myx, myy].X][M[myx, myy].Y] = Color.Red;
                }


                SavePhoto(colors2, redFilePath);
                //Produce new Colors(pixels) array:
                List<Color[]> list2 = new List<Color[]>();
                for (int i = 0; i < colors.Length; i++)
                {
                    List<Color> list1 = new List<Color>();
                    for (int j = 0; j < colors[i].Length; j++)
                    {
                        Point mytp = new Point();
                        mytp.X = i;
                        mytp.Y = j;
                        if (colors[i][j] != Color.Red)
                            list1.Add(colors[i][j]);
                    }
                    list2.Add(list1.ToArray());
                }
                colors = list2.ToArray();
            }

            return colors;
        }
    }
}
