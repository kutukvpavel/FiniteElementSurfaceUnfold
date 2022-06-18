using System;
using g3;
using System.IO;
using SixLabors.ImageSharp.Formats.Png;

namespace FiniteElementSurfaceUnfold
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Processing case data...");
            Tiler t = new Tiler(new FemData(600, 500, new PipeToDuct()));
            t.ProgressChanged += T_ProgressChanged;
            Console.WriteLine("Calculating...");
            t.Calculate(new PipeToDuct.MySurfaceParams(500, 900, 370, 630), new Vector2d(1, 0), t.Configuration.nZ - 1);
            Console.WriteLine("Finished. Generating image...");
            //GraphicsProvider.DrawOutline(t.Outline).Save(File.Create(GetSavePath("outline")), new PngEncoder());
            GraphicsProvider.DrawAll(t.Rows).Save(File.Create(GetSavePath("isolines")), new PngEncoder());
            Console.WriteLine("Done.");
        }

        private static void T_ProgressChanged(object sender, float e)
        {
            Console.WriteLine($"\tProcessed {e:F0}% of rows...");
        }

        private static string GetSavePath(string name)
        {
            return Path.Combine(Environment.CurrentDirectory, $"{name}.png");
        }
    }
}
