using System;
using System.Drawing;

namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter inputFilePath:");
            string my_in1 = Console.ReadLine();
            string inputFilePath = @my_in1;

            Console.WriteLine("Enter outputFilePath:");
            string my_in2 = Console.ReadLine();
            string outputFilePath = @my_in2;


            Console.WriteLine("Enter redoutputFilePath:");
            string my_in3 = Console.ReadLine();
            string redFilePath = @my_in3;

            Console.WriteLine("Enter vertical seams count:");
            long verticalSeamsCount = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter horizental seams count:");
            long horizontalSeamsCount = Convert.ToInt32(Console.ReadLine());

            var image = Utilities.LoadImage(inputFilePath);

            var colors = Utilities.ConvertImageToColorArray(image);
            var colors2 = Utilities.ConvertImageToColorArray(image);

            colors = Utilities.FandR_V_MinSeam_DP(colors, colors2, verticalSeamsCount, redFilePath);
            Utilities.SavePhoto(colors, outputFilePath);


            var theimage = Utilities.LoadImage(redFilePath);
            colors2 = Utilities.ConvertImageToColorArray(theimage);

            var bit = Utilities.ConvertToBitmap(colors);
            bit.RotateFlip(RotateFlipType.Rotate90FlipNone);
            bit.Save(outputFilePath);

            var bit2 = Utilities.ConvertToBitmap(colors2);
            bit2.RotateFlip(RotateFlipType.Rotate90FlipNone);
            bit2.Save(redFilePath);

            image = Utilities.LoadImage(outputFilePath);
            var image2 = Utilities.LoadImage(redFilePath);

            var mycolors = Utilities.ConvertImageToColorArray(image);
            var mycolors2 = Utilities.ConvertImageToColorArray(image2);

            mycolors = Utilities.FandR_V_MinSeam_DP(mycolors, mycolors2, horizontalSeamsCount, redFilePath);
            Utilities.SavePhoto(colors, outputFilePath);

            theimage = Utilities.LoadImage(redFilePath);
            colors2 = Utilities.ConvertImageToColorArray(theimage);

            bit2 = Utilities.ConvertToBitmap(colors2);
            bit2.RotateFlip(RotateFlipType.Rotate270FlipNone);
            bit2.Save(redFilePath);

            bit = Utilities.ConvertToBitmap(mycolors);
            bit.RotateFlip(RotateFlipType.Rotate270FlipNone);
            bit.Save(outputFilePath);


        }
    }
}
