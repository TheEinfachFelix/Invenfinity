using Backend.Domain;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Test.Domain
{
    internal class TestDset
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDsetsetup()
        {
            var dset = new Dset();
            Assert.That(dset.Parts, Is.Empty);
            Assert.That(dset.Types, Is.Empty);
            Assert.That(dset.Root, Is.Null);
            var loc = TestData.locRoot;
            dset.Root = loc;
            Assert.That(dset.Root, Is.EqualTo(loc));

            var types = new List<DBinType> { TestData.binType1, TestData.binType2 };
            dset.Types = types;
            Assert.That(dset.Types, Is.EqualTo(types));

            var parts = new List<DPart> { TestData.part1, TestData.part2 };
            dset.Parts = parts;
            Assert.That(dset.Parts, Is.EqualTo(parts));
        }
        [Test]
        public void TestDsetFindPart()
        {
            var dset = new Dset();
            var parts = new List<DPart> { TestData.part1, TestData.part2 };
            dset.Parts = parts;
            Assert.That(dset.findPartbyID(1), Is.EqualTo(parts[0]));
            Assert.That(dset.findPartbyID(2), Is.EqualTo(parts[1]));
            Assert.Throws<Exception>(() => dset.findPartbyID(3));
        }
        [Test]
        public void TestDsetFindType()
        {
            var dset = new Dset();
            var types = new List<DBinType> { TestData.binType1, TestData.binType2 };
            dset.Types = types;
            Assert.That(dset.findBinTypebyID(1), Is.EqualTo(types[0]));
            Assert.That(dset.findBinTypebyID(2), Is.EqualTo(types[1]));
            Assert.Throws<Exception>(() => dset.findBinTypebyID(3));
        }
    }
}
