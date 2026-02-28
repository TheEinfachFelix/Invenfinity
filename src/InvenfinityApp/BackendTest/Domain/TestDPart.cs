using Backend.Domain;
using BackendTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Test.Domain
{
    internal class TestDPart
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestPart1()
        {
            var data = new DPart(1, null);
            Assert.That(data.PartId, Is.EqualTo(1));
            Assert.That(data.InventreeId, Is.Null);
            Assert.That(data.Bins, Is.Not.Null);
        }

        [Test]
        public void TestPart2()
        {
            var data = new DPart(2, 12);
            Assert.That(data.PartId, Is.EqualTo(2));
            Assert.That(data.InventreeId, Is.EqualTo(12));
            Assert.That(data.Bins, Is.Not.Null);
        }
    }
}
