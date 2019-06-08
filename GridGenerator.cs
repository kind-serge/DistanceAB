using System;

namespace DistanceAB
{
    internal class GridGenerator
    {
        public static byte[] Generate(int width, int height, int? seed = null)
        {
            var rnd = new Random(seed ?? Environment.TickCount);

            var grid = new byte[width * height];

            var i = 0;

            var minDist = rnd.Next((int)Math.Pow(width * height, 0.2) * 2);

            var aBuffer = new int[width];
            var bBuffer = new int[width];

            for (var p = 0; p < aBuffer.Length; p++)
                aBuffer[p] = minDist;
            for (var p = 0; p < bBuffer.Length; p++)
                bBuffer[p] = minDist;

            for (var y = 0; y < height; y++)
            {
                for (var p = 0; p < aBuffer.Length; p++)
                    aBuffer[p] = aBuffer[p] + 1;
                for (var p = 0; p < bBuffer.Length; p++)
                    bBuffer[p] = bBuffer[p] + 1;

                for (var x = 0; x < width; x++, i++)
                {
                    grid[i] = (byte)'O';

                    if (rnd.Next(1000) > 900)
                    {
                        if (rnd.Next(1000) >= 500)
                        {
                            if (bBuffer[x] >= minDist)
                            {
                                grid[i] = (byte)'A';
                                aBuffer[x] = 0;

                                var v = 1;
                                for (var f = x - 1; f >= 0 && v < aBuffer[f]; f--, v++)
                                    aBuffer[f] = v;
                                v = 1;
                                for (var f = x + 1; f < width && v < aBuffer[f]; f++, v++)
                                    aBuffer[f] = v;
                            }
                        }
                        else
                        {
                            if (aBuffer[x] >= minDist)
                            {
                                grid[i] = (byte)'B';
                                bBuffer[x] = 0;

                                var v = 1;
                                for (var f = x - 1; f >= 0 && v < bBuffer[f]; f--, v++)
                                    bBuffer[f] = v;
                                v = 1;
                                for (var f = x + 1; f < width && v < bBuffer[f]; f++, v++)
                                    bBuffer[f] = v;
                            }
                        }
                    }
                }

                continue;
            }

            return grid;
        }
    }
}
