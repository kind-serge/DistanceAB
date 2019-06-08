using System;
using System.Diagnostics;

namespace DistanceAB
{
    class Program
    {
        static void Main(string[] args)
        {
            var width = 500;
            var height = 500;
            var grid = GridGenerator.Generate(width, height);

            WarmpUp(grid, width, height);

            if (width < Console.WindowWidth && height < Console.WindowWidth)
            {
                RenderGrid(grid, width, height);
                Console.WriteLine();
            }

            TimeSpan baseTime;

            {
                var sw = Stopwatch.StartNew();
                var d = ShortestDistance_Naive.Find(grid, width, height);
                baseTime = sw.Elapsed;

                Console.WriteLine();
                Console.WriteLine("Naive");
                Console.WriteLine($"  time: {baseTime}");
                Console.WriteLine($"  result: {d}");
            }

            {
                var sw = Stopwatch.StartNew();
                var d = ShortestDistance_Points.Find(grid, width, height);
                var time = sw.Elapsed;

                Console.WriteLine();
                Console.WriteLine("All points");
                Console.WriteLine($"  time: {time}");
                Console.WriteLine($"  result: {d}");
                Console.WriteLine($"  speedup: {baseTime.TotalMilliseconds / time.TotalMilliseconds:N1}X");
            }

            {
                var sw = Stopwatch.StartNew();
                var d = ShortestDistance_Optimized.Find(grid, width, height);
                var time = sw.Elapsed;

                Console.WriteLine();
                Console.WriteLine("Optimized");
                Console.WriteLine($"  time: {time}");
                Console.WriteLine($"  result: {d}");
                Console.WriteLine($"  speedup: {baseTime.TotalMilliseconds / time.TotalMilliseconds:N1}X");
            }
            Console.WriteLine();
        }

        static unsafe void WarmpUp(byte[] grid, int w, int h)
        {
            unsafe
            {
                fixed (byte* pGrid = grid)
                {
                    var ptr = pGrid;
                    var end = pGrid + w * h;
                    for (; ptr != end; ptr++)
                        if (*ptr == 255)
                            *ptr = 255;
                }
            }
        }

        private static void RenderGrid(byte[] grid, int w, int h)
        {
            var fg = Console.ForegroundColor;
            var bg = Console.BackgroundColor;
            try
            {
                unsafe
                {
                    fixed (byte* pGrid = grid)
                    {
                        var ptr = pGrid;
                        for (var y = 0; y < h; y++)
                        {
                            for (var x = 0; x < w; x++, ptr++)
                            {
                                switch (*ptr)
                                {
                                    case (byte)'A':
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                                        break;
                                    case (byte)'B':
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.BackgroundColor = ConsoleColor.DarkRed;
                                        break;
                                    case (byte)'O':
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.BackgroundColor = ConsoleColor.Black;
                                        break;
                                }
                                Console.Write((char)*ptr);
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            finally
            {
                Console.ForegroundColor = fg;
                Console.BackgroundColor = bg;
            }
        }
    }
}
