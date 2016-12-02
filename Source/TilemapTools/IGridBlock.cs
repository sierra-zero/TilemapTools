﻿using System;
using System.Collections.Generic;

namespace TilemapTools
{
    public interface IGridBlock
    {
        int BlockSize { get; }

        int CellCount { get; }

        bool IsEmpty { get; }

        ShortPoint Location { get; }
    }

    public interface IGridBlock<TCell> : IGridBlock, IEnumerable<CellLocationPair<TCell>>
    {
        TCell GetCell(int x, int y);
        bool SetCell(int x, int y, TCell value);
    }
}