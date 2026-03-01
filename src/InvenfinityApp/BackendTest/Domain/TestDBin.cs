using Backend.Domain;
using DBconnector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Test.Domain
{
    internal class TestDBin
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBin1()
        {
            var data = TestData.binType1;
            var bin1 = new DBin(1, data);
            Assert.That(bin1.BinType, Is.EqualTo(data));
            Assert.That(bin1.Slots, Is.Not.Null);
            Assert.That(bin1.Grid, Is.Null);
            Assert.Throws<Exception>(() => bin1.GetPosition());
            Assert.That(bin1.BinId, Is.EqualTo(1));
            Assert.That(bin1.Slots.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestBin2()
        {
            var data = TestData.binType2;
            var bin1 = new DBin(2, data);
            Assert.That(bin1.BinType, Is.EqualTo(data));
            Assert.That(bin1.Slots, Is.Not.Null);
            Assert.That(bin1.Grid, Is.Null);
            Assert.Throws<Exception>(() => bin1.GetPosition());
            Assert.That(bin1.BinId, Is.EqualTo(2));
            Assert.That(bin1.Slots.Count, Is.EqualTo(1));
        }
        [Test]
        public void TestParts()
        {
            var type1 = TestData.binType1;
            var bin1 = new DBin(1, type1);
            var part1 = TestData.part1;
            var part2 = TestData.part2;
            Assert.That(bin1.Slots[0], Is.Null);
            Assert.That(bin1.Slots[1], Is.Null);
            bin1.AddPart(part1, 0);
            Assert.That(bin1.Slots[0], Is.EqualTo(part1));
            Assert.That(bin1.Slots[1], Is.Null);
            bin1.AddPart(part2, 1);
            Assert.That(bin1.Slots[0], Is.EqualTo(part1));
            Assert.That(bin1.Slots[1], Is.EqualTo(part2));
            bin1.RemovePart(part1);
            Assert.That(bin1.Slots[0], Is.Null);
            Assert.That(bin1.Slots[1], Is.EqualTo(part2));
            bin1.RemovePart(part2);
            Assert.That(bin1.Slots[0], Is.Null);
            Assert.That(bin1.Slots[1], Is.Null);
        }
    }
}
