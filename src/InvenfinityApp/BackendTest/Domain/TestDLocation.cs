using Backend.Domain;
using BackendTest;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Test.Domain
{
    internal class TestDLocation
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLocationCtor()
        {
            var loc = new DLocation(1, "loc1", null);
            Assert.That(loc.LocationId, Is.EqualTo(1));
            Assert.That(loc.Name, Is.EqualTo("loc1"));
            Assert.That(loc.Parent, Is.Null);
            Assert.That(loc.ParentId, Is.Null);
            Assert.That(loc.Grids, Is.Empty);
            Assert.That(loc.Childeren, Is.Empty);
            Assert.That(loc.isDeletable(), Is.False);
        }

        [Test]
        public void TestLocationCtorWithParent()
        {
            var parent = new DLocation(1, "parent", null);
            var loc = new DLocation(2, "loc1", parent);
            Assert.That(loc.LocationId, Is.EqualTo(2));
            Assert.That(loc.Name, Is.EqualTo("loc1"));
            Assert.That(loc.Parent, Is.EqualTo(parent));
            Assert.That(loc.ParentId, Is.EqualTo(1));
            Assert.That(loc.Grids, Is.Empty);
            Assert.That(loc.Childeren, Is.Empty);
            Assert.That(loc.isDeletable(), Is.True);
            Assert.That(parent.Childeren, Has.Count.EqualTo(1));
            Assert.That(parent.Childeren, Contains.Item(loc));
            Assert.That(parent.isDeletable(), Is.False);
            Assert.That(parent.getLocationByID(2), Is.EqualTo(loc));
        }
        [Test]
        public void TestLocationAddChild()
        {
            var parent = new DLocation(1, "parent", null);
            var child = new DLocation(2, "child", null);
            parent.AddChild(child);
            Assert.That(parent.Childeren, Contains.Item(child));
        }
        [Test]
        public void TestGridAdd()
        {
            var test = TestData.locRoot;
            var parent = TestData.locRoot;
            var grid = TestData.grid(test);
            Assert.That(parent.Grids, Is.Not.Null);
            Assert.That(parent.Grids, Has.Count.EqualTo(0));
            parent.AddGrid(grid);
            Assert.That(parent.Grids, Has.Count.EqualTo(1));
        }
        [Test]
        public void TestGridGetByID()
        {
            var parent = TestData.locRoot;
            var grid = TestData.grid(parent);
            Assert.That(parent.getGridByID(grid.GridId), Is.EqualTo(grid));
        }
    }
}
