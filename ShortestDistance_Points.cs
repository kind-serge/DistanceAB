using System.Collections.Generic;

namespace DistanceAB
{
    /// <summary>
    /// Scan the grid, memorizes all locations of 'A' or 'B', then tries to find the closest pair.
    /// Uses up to W*H of extra memory, worst complexity is W*H.
    /// </summary>
    internal class ShortestDistance_Points
    {
        public static int? Find(byte[] grid, int w, int h)
        {
            var aSet = new List<(int x, int y)>();
            var bSet = new List<(int x, int y)>();
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
                                    aSet.Add((x, y));
                                    break;

                                case (byte)'B':
                                    bSet.Add((x, y));
                                    break;
                            }
                        }
                    }
                }
            }

            if (aSet.Count == 0 || bSet.Count == 0)
                return null;

            int minDist = int.MaxValue;

            // NOTE: can be optimized with a BSP-tree for large data sets.
            foreach (var aPoint in aSet)
            {
                foreach (var bPoint in bSet)
                {
                    var hDist = aPoint.x - bPoint.x;
                    if (hDist < 0)
                        hDist = -hDist;

                    var vDist = aPoint.y - bPoint.y;
                    if (vDist < 0)
                        vDist = -vDist;

                    var dist = hDist + vDist;

                    if (dist < minDist)
                        minDist = dist;
                }
            }

            return minDist;
        }
    }
}
