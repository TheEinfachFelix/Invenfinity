using System;
using System.Collections.Generic;
using System.Text;
using Backend.Domain;

namespace Backend.Test
{
    internal static class TestData
    {
        public static DBinType binType1 { get { return new DBinType(1, 2, 1, 1); } }
        public static DBinType binType2 { get { return new DBinType(2, 1, 1, 2); } }
        public static DBin bin1(DBinType type)
        {
            return new DBin(1, type);
        }
        public static DBin bin2(DBinType type)
        {
            return new DBin(2, type);
        }
        public static DPart part1 { get { return new DPart(1, null); } }
        public static DPart part2 { get { return new DPart(2, 12); } }
        public static DLocation locRoot { get { return new DLocation(1, "root", null); } }
        public static DLocation loc1(DLocation parent)
        {
            return new DLocation(2, "loc1", parent);
        }
        public static DGrid grid(DLocation loc)
        {
            return new DGrid(1, "grid1", loc, 5, 6);
        }
    }
}
