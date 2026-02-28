using Backend.Domain;
using BackendTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Test.Domain
{
    internal class TestDGrid
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGridCtor()
        {
            var root = TestData.locRoot;
            var grid = new DGrid (1, "grid1", root, 5, 6);
            Assert.That(grid.GridId, Is.EqualTo(1));
            Assert.That(grid.Name, Is.EqualTo("grid1"));
            Assert.That(grid.Location, Is.EqualTo(root));
            Assert.That(grid.Xmax, Is.EqualTo(5));
            Assert.That(grid.Ymax, Is.EqualTo(6));
        }
        [Test]
        public void TestGridCreateGrid()
        {
            var root = TestData.locRoot;
            var grid = TestData.grid(root);
            Assert.That(grid.GridId, Is.EqualTo(1));
            Assert.That(grid.Name, Is.EqualTo("grid1"));
            Assert.That(grid.Location, Is.EqualTo(root));
            Assert.That(grid.Xmax, Is.EqualTo(5));
            Assert.That(grid.Ymax, Is.EqualTo(6));
            Assert.That(grid.LocationId, Is.EqualTo(1));
            Assert.That(grid.Grid.Count, Is.EqualTo(5));
            for (int x = 0; x < grid.Xmax; x++)
            {
                Assert.That(grid.Grid[x].Count, Is.EqualTo(6));
                for (int y = 0; y < grid.Ymax; y++)
                {
                    Assert.That(grid.Grid[x][y], Is.Null);
                }
            }
            var createdGrid = grid.createGrid();
            Assert.That(createdGrid.Count, Is.EqualTo(5));
            for (int x = 0; x < grid.Xmax; x++)
            {
                Assert.That(createdGrid[x].Count, Is.EqualTo(6));
                for (int y = 0; y < grid.Ymax; y++)
                {
                    var cell = createdGrid[x][y];
                    Assert.That(cell, Is.Null);
                }
            }
        }
        [Test]
        public void TestGridBinPos()
        {
            var root = TestData.locRoot;
            var grid = TestData.grid(root);
            var binType = TestData.binType2;
            var bin = TestData.bin1(binType);
            grid.AddBin(bin, 2, 3);
            Assert.That(grid.GetBinPosInGrid(bin), Is.EqualTo(new BinPos(2, 3)));
            Assert.That(bin.Grid, Is.EqualTo(grid));
            Assert.That(bin.GetPos(), Is.EqualTo(new BinPos(2, 3)));
            Assert.That(grid.Grid[2][3], Is.EqualTo(bin));
            Assert.That(grid.Grid[2][4], Is.EqualTo(bin));
            Assert.That(grid.GetAllBinPosInGrid(bin), Is.EqualTo([new BinPos(2,3), new BinPos(2,4)]));
            Assert.That(grid.GetAllBinsInGrid(), Is.EqualTo([bin]));
            grid.MoveBin(bin, 0, 0);
            Assert.That(grid.GetBinPosInGrid(bin), Is.EqualTo(new BinPos(0, 0)));
            Assert.That(grid.Grid[2][3], Is.Null);
            Assert.That(grid.Grid[2][4], Is.Null);
            Assert.That(grid.Grid[0][0], Is.EqualTo(bin));
            Assert.That(grid.Grid[0][1], Is.EqualTo(bin));
            Assert.That(grid.GetAllBinPosInGrid(bin), Is.EqualTo([new BinPos(0, 0), new BinPos(0, 1)]));
            Assert.That(grid.GetAllBinsInGrid(), Is.EqualTo([bin]));
            grid.RemoveBin(bin);
            Assert.Throws<Exception>(() => grid.GetBinPosInGrid(bin));
            Assert.That(bin.Grid, Is.Null);
            Assert.That(bin.GetPos(), Is.Null);


        }
        [Test]
        public void TestGridBinPosNotInGrid()
        {
            var root = TestData.locRoot;
            var grid = TestData.grid(root);
            var binType = TestData.binType2;
            var bin = TestData.bin1(binType);
            Assert.Throws<InvalidOperationException>(() => grid.MoveBin(bin, 5, 6));
            grid.AddBin(bin, 2, 3);
            Assert.That(grid.GetBinPosInGrid(bin), Is.EqualTo(new BinPos(2, 3)));
            Assert.That(bin.Grid, Is.EqualTo(grid));
            Assert.That(bin.GetPos(), Is.EqualTo(new BinPos(2, 3)));
            Assert.That(grid.Grid[2][3], Is.EqualTo(bin));
            Assert.That(grid.Grid[2][4], Is.EqualTo(bin));
            Assert.That(grid.GetAllBinPosInGrid(bin), Is.EqualTo([new BinPos(2, 3), new BinPos(2, 4)]));
            Assert.That(grid.GetAllBinsInGrid(), Is.EqualTo([bin]));
            Assert.Throws<InvalidOperationException>(() => grid.MoveBin(bin, 5, 6));
        }
        [Test]
        public void TestGridisfree()
        {
            var root = TestData.locRoot;
            var grid = TestData.grid(root);
            var binType = TestData.binType2;
            var bin = TestData.bin1(binType);
            grid.AddBin(bin, 2, 3);
            Assert.That(grid.IsAreaFree(0, 0, binType, 9), Is.True);
            Assert.That(grid.IsAreaFree(2, 3, binType, 9), Is.False);
            Assert.That(grid.IsAreaFree(9, 9, binType, 9), Is.False);
        }
        [Test]
        public void TestGridisDeletable()
        {
            var root = TestData.locRoot;
            var grid = TestData.grid(root);
            var binType = TestData.binType2;
            var bin = TestData.bin1(binType);
            Assert.That(grid.isDeletable(), Is.True);
            grid.AddBin(bin, 2, 3);
            Assert.That(grid.isDeletable(), Is.False);
        }
        [Test]
        public void TestGridResize()
        {
            var root = TestData.locRoot;
            var grid = TestData.grid(root);
            var binType = TestData.binType2;
            var bin = TestData.bin1(binType);
            grid.AddBin(bin, 2, 3);
            Assert.Throws<InvalidOperationException>(() => grid.ResizeGrid(2, 2));
            Assert.That(grid.GetMinRequiredGridSize().Ypos, Is.EqualTo(5));
            Assert.That(grid.GetMinRequiredGridSize().Xpos, Is.EqualTo(3));
            grid.MoveBin(bin, 0, 0);
            Assert.That(grid.GetMinRequiredGridSize().Ypos, Is.EqualTo(2));
            Assert.That(grid.GetMinRequiredGridSize().Xpos, Is.EqualTo(1));
            grid.ResizeGrid(2, 2);
            Assert.That(grid.Xmax, Is.EqualTo(2));
            Assert.That(grid.Ymax, Is.EqualTo(2));
        }
    }
}
