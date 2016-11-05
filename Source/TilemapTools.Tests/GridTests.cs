﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TilemapTools;

namespace TilemapTools.Tests
{
    [TestClass]
    public class GridTests
    {
        [TestMethod]
        public void GridBlock_GetBlockCellLocation_BlockLocations()
        {
            ShortPoint blockLocation;
            int cellX;
            int cellY;

            GetBlockCellLocation(1, 1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(1, 1));

            GetBlockCellLocation(1, -1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(1, -1));

            GetBlockCellLocation(-1, -1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-1, -1));

            GetBlockCellLocation(-1, 1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-1, 1));

            GetBlockCellLocation(16, 16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(1, 1));

            GetBlockCellLocation(16, -16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(1, -1));

            GetBlockCellLocation(-16, -16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-1, -1));

            GetBlockCellLocation(-16, 16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-1, 1));

            GetBlockCellLocation(17, 17, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(2, 2));

            GetBlockCellLocation(17, -17, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(2, -2));

            GetBlockCellLocation(-17, -17, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-2, -2));

            GetBlockCellLocation(-17, 17, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-2, 2));

            GetBlockCellLocation(20, 20, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(2, 2));

            GetBlockCellLocation(20, -20, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(2, -2));

            GetBlockCellLocation(-20, -20, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-2, -2));

            GetBlockCellLocation(-20, 20, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(blockLocation, new ShortPoint(-2, 2));
        }

        public void GridBlock_GetBlockCellLocation_CellLocations()
        {
            ShortPoint blockLocation;
            int cellX;
            int cellY;

            GetBlockCellLocation(1, 1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(1, -1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 0);

            GetBlockCellLocation(-1, -1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 0);

            GetBlockCellLocation(-1, 1, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(16, 16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(16, -16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 15);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(-16, -16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 15);

            GetBlockCellLocation(-16, 16, out blockLocation, out cellX, out cellY);
            Assert.AreEqual(cellX, 0);
            Assert.AreEqual(cellY, 0);

            //GetBlockCellLocation(17, 17, out blockLocation, out cellX, out cellY);
            //Assert.AreEqual(cellX, 0);
            //Assert.AreEqual(cellY, 0);

            //GetBlockCellLocation(17, -17, out blockLocation, out cellX, out cellY);
            //Assert.AreEqual(cellX, 0);
            //Assert.AreEqual(cellY, 0);

            //GetBlockCellLocation(-17, -17, out blockLocation, out cellX, out cellY);
            //Assert.AreEqual(cellX, 0);
            //Assert.AreEqual(cellY, 0);

            //GetBlockCellLocation(-17, 17, out blockLocation, out cellX, out cellY);
            //Assert.AreEqual(cellX, 0);
            //Assert.AreEqual(cellY, 0);

            //GetBlockCellLocation(20, 20, out blockLocation, out cellX, out cellY);
            //Assert.AreEqual(cellX, 0);
            //Assert.AreEqual(cellY, 0);

            //GetBlockCellLocation(20, -20, out blockLocation, out cellX, out cellY);
            //Assert.AreEqual(cellX, 0);
            //Assert.AreEqual(cellY, 0);

            //GetBlockCellLocation(-20, -20, out blockLocation, out cellX, out cellY);
            //Assert.AreEqual(cellX, 0);
            //Assert.AreEqual(cellY, 0);

            //GetBlockCellLocation(-20, 20, out blockLocation, out cellX, out cellY);
            //Assert.AreEqual(cellX, 0);
            //Assert.AreEqual(cellY, 0);
        }

        static void GetBlockCellLocation(int x, int y, out ShortPoint blockLocation, out int cellX, out int cellY, int blockSize = 16)
        {
            GridBlock.GetBlockCellLocation(ref x, ref y, ref blockSize,out blockLocation, out cellX, out cellY);
        }
    }
}
