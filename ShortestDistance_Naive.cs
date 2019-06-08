namespace DistanceAB
{
    /// <summary>
    /// Scan the grid, and for any encountered 'A' or 'B', searches the entire grid for the nearest counterpart.
    /// Does not use extra memory, worst complexity is W*H*W*H.
    /// </summary>
    internal class ShortestDistance_Naive
    {
        public static int? Find(byte[] grid, int w, int h)
        {
            int? minDist = null;

            unsafe
            {
                fixed (byte* pGrid = grid)
                {
                    var ptr = pGrid;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++, ptr++)
                        {
                            int? nearest = null;
                            switch (*ptr)
                            {
                                case (byte)'A':
                                    nearest = FindNearest(pGrid, w, h, x, y, (byte)'B');
                                    break;

                                case (byte)'B':
                                    nearest = FindNearest(pGrid, w, h, x, y, (byte)'A');
                                    break;
                            }
                            if (nearest.HasValue && (!minDist.HasValue || nearest < minDist))
                                minDist = nearest;
                        }
                    }
                }
            }

            return minDist;
        }

        private static unsafe int? FindNearest(byte* pGrid, int w, int h, int posX, int posY, byte symbol)
        {
            int? minDist = null;

            var maxY = h;
            var minX = 0;
            var maxX = w;

            for (var y = 0; y < maxY; y++)
            {
                for (var x = minX; x < maxX; x++)
                {
                    if (pGrid[x + y * w] == symbol)
                    {
                        var hDist = posX - x;
                        if (hDist < 0)
                            hDist = -hDist;

                        var vDist = posY - y;
                        if (vDist < 0)
                            vDist = -vDist;

                        var dist = hDist + vDist;

                        if (!minDist.HasValue || dist < minDist.Value)
                        {
                            minDist = dist;

                            maxY = posY + dist;
                            if (maxY > h)
                                maxY = h;

                            minX = posX - dist;
                            if (minX < 0)
                                minX = 0;

                            maxX = posX + dist;
                            if (maxX > w)
                                maxX = w;
                        }
                    }
                }
            }

            return minDist;
        }
    }
}
