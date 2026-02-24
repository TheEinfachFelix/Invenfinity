using Backend.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases
{
    public class UcGrid
    {
        private readonly UcRoot _root;
        internal UcGrid(UcRoot root)
        {
            _root = root;
        }
        public DTOGrid getGridByID(int id) 
        {
            var Dgrid = _root.Data.findGridbyID(id);

            return GridFactory.CreateGrid(Dgrid);
        }
    }
}
