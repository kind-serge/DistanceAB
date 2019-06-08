namespace DistanceAB
{
    /// <summary>
    /// Scans the grid only once, calculates distance to closest 'A' and 'B' on-the-fly per every row, finds the shortest distance on every row.
    /// Uses exactly W*2 of extra memory. The complexity is O(1) when supplementary operations are not counted.
    /// </summary>
    internal class ShortestDistance_Optimized
    {
        public static int? Find(byte[] grid, int w, int h)
        {
            int? minDist = null;

            var aBuffer = new int[w];
            var bBuffer = new int[w];

            var max = w * h;
            for (var i = 0; i < aBuffer.Length; i++)
                aBuffer[i] = max;
            for (var i = 0; i < bBuffer.Length; i++)
                bBuffer[i] = max;

            unsafe
            {
                fixed (byte* pGrid = grid)
                {
                    fixed (int* pABuffer = aBuffer)
                    {
                        fixed (int* pBBuffer = bBuffer)
                        {
                            var ptr = pGrid;

                            var seenAnyA = false;
                            var seenAnyB = false;

                            for (var y = 0; y < h; y++)
                            {
                                var lastA = -1;
                                var lastB = -1;

                                for (var x = 0; x < w; x++, ptr++)
                                {
                                    var hasA = false;
                                    var hasB = false;

                                    switch (*ptr)
                                    {
                                        case (byte)'A':
                                            pABuffer[x] = 0;

                                            if (lastA < 0)
                                            {
                                                if (!seenAnyA)
                                                {
                                                    var v = x;
                                                    for (var i = 0; i < x; i++, v--)
                                                        pABuffer[i] = v;
                                                }
                                                else
                                                {
                                                    var v = x;
                                                    for (var i = 0; i < x; i++, v--)
                                                    {
                                                        if (v < pABuffer[i] + 1)
                                                            pABuffer[i] = v;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var start = lastA + (x - lastA) / 2;
                                                if (!seenAnyA)
                                                {
                                                    var v = x - start;
                                                    for (var i = start; i < x; i++, v--)
                                                        pABuffer[i] = v;
                                                }
                                                else
                                                {
                                                    var v = x - start;
                                                    for (var i = start; i < x; i++, v--)
                                                    {
                                                        if (v < pABuffer[i])
                                                            pABuffer[i] = v;
                                                    }
                                                }
                                            }

                                            hasA = true;
                                            seenAnyA = true;
                                            lastA = x;
                                            break;

                                        case (byte)'B':
                                            pBBuffer[x] = 0;

                                            if (lastB < 0)
                                            {
                                                if (!seenAnyB)
                                                {
                                                    var v = x;
                                                    for (var i = 0; i < x; i++, v--)
                                                        pBBuffer[i] = v;
                                                }
                                                else
                                                {
                                                    var v = x;
                                                    for (var i = 0; i < x; i++, v--)
                                                    {
                                                        if (v < pBBuffer[i] + 1)
                                                            pBBuffer[i] = v;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var start = lastB + (x - lastB) / 2;
                                                if (!seenAnyB)
                                                {
                                                    var v = x - start;
                                                    for (var i = start; i < x; i++, v--)
                                                        pBBuffer[i] = v;
                                                }
                                                else
                                                {
                                                    var v = x - start;
                                                    for (var i = start; i < x; i++, v--)
                                                    {
                                                        if (v < pBBuffer[i])
                                                            pBBuffer[i] = v;
                                                    }
                                                }
                                            }

                                            hasB = true;
                                            seenAnyB = true;
                                            lastB = x;
                                            break;
                                    }

                                    if (!hasA)
                                    {
                                        if (x == 0)
                                        {
                                            pABuffer[0] = pABuffer[0] + 1;
                                        }
                                        else
                                        {
                                            var d1 = pABuffer[x];
                                            var d2 = pABuffer[x - 1];
                                            if (d1 < d2)
                                            {
                                                pABuffer[x] = d1 + 1;
                                            }
                                            else
                                            {
                                                pABuffer[x] = d2 + 1;
                                            }
                                        }
                                    }

                                    if (!hasB)
                                    {
                                        if (x == 0)
                                        {
                                            pBBuffer[0] = pBBuffer[0] + 1;
                                        }
                                        else
                                        {
                                            var d1 = pBBuffer[x];
                                            var d2 = pBBuffer[x - 1];
                                            if (d1 < d2)
                                            {
                                                pBBuffer[x] = d1 + 1;
                                            }
                                            else
                                            {
                                                pBBuffer[x] = d2 + 1;
                                            }
                                        }
                                    }
                                }

                                if ((lastA >= 0 && seenAnyB) || (lastB >= 0 && seenAnyA))
                                {
                                    for (var i = 0; i < w; i++)
                                    {
                                        var d = pABuffer[i] + pBBuffer[i];
                                        if (!minDist.HasValue || d < minDist.Value)
                                            minDist = d;
                                    }
                                }

                                continue;
                            }
                        }
                    }
                }
            }

            return minDist;
        }
    }
}
