using Backend.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Test.Domain
{
    internal class TestDBinType
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestTestData1()
        {
            var data = new DBinType(1, 2, 1, 1);
            Assert.That(data.BinTypeId, Is.EqualTo(1));
            Assert.That(data.SlotCount, Is.EqualTo(2));
            Assert.That(data.X, Is.EqualTo(1));
            Assert.That(data.Y, Is.EqualTo(1));
        }

        [Test]
        public void TestTestData2()
        {
            var data = new DBinType(2, 1, 1, 2);
            Assert.That(data.BinTypeId, Is.EqualTo(2));
            Assert.That(data.SlotCount, Is.EqualTo(1));
            Assert.That(data.X, Is.EqualTo(1));
            Assert.That(data.Y, Is.EqualTo(2));
        }

        [Test]
        public void TestBinlist()
        {
            var data = TestData.binType1;
            Assert.That(data.Bins, Is.Not.Null);
            Assert.That(data.Bins, Is.Empty);
            Assert.That(data.IsDeletable(), Is.True);
            var bin1 = TestData.bin1(data);
            Assert.That(data.Bins, Has.Count.EqualTo(1));
            Assert.That(data.Bins, Contains.Item(bin1));
            Assert.That(bin1.BinType, Is.EqualTo(data));
            var bin2 = TestData.bin2(data);
            Assert.That(data.Bins, Has.Count.EqualTo(2));
            Assert.That(data.Bins, Contains.Item(bin2));
            Assert.That(bin1.BinType, Is.EqualTo(data));
            data.Bins.Remove(bin1);
            Assert.That(data.Bins, Has.Count.EqualTo(1));
            Assert.That(data.Bins, Does.Not.Contain(bin1));
        }
    }
}
