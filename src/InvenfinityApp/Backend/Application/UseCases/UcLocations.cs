using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.DTOs;
using Backend.Application.DTOs.Location;
using Backend.Domain;
using Backend.Infrastructure.Datenbank;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Application.UseCases
{
    public class UcLocations
    {
        private readonly UcRoot _root;
        internal UcLocations(UcRoot root)
        {
            _root = root;
        }

        public DTOTreeLocation GetLocations()
        {
            return LocationFactory.CreateLocation(_root.Data.Root);
        }

        public IDotTreeEditItem GetEditItem(IDotTreeItem item)
        {
            return item switch
            {
                DTOTreeLocation => LocationFactory.CreateEditItem(
                                       _root.Data.Root.getLocationByID(item.Id)
                                       ?? throw new Exception()),
                DTOTreeGrid => LocationFactory.CreateEditItem(
                                           _root.Data.Root.getGridByID(item.Id)
                                           ?? throw new Exception()),
                _ => throw new InvalidOperationException()
            };
        }

        public void EditItem(IDotTreeEditItem item)
        {
            bool TreeOrderChanged = false;
            switch (item)
            {
                case DTOTreeEditLocation:
                    var loc = _root.Data.Root.getLocationByID(item.Id) ?? throw new Exception();
                    loc.Name = item.Name;
                    if (loc.ParentId != item.ParentId) TreeOrderChanged = true;
                    loc.ParentId = item.ParentId;
                    _root.RepoDatabase.UpdateSingleLocation(loc);
                    break;
                case DTOTreeEditGrid:
                    var grid = _root.Data.Root.getGridByID(item.Id) ?? throw new Exception();
                    grid.Name = item.Name;
                    if (grid.LocationId != item.ParentId) TreeOrderChanged = true;
                    grid.LocationId = item.ParentId;
                    if (grid.Xmax != item.Xsize || grid.Ymax != item.Ysize)
                        grid.ResizeGrid(item.Xsize, item.Ysize);
                    _root.RepoDatabase.UpdateSingleGrid(grid);
                    break;
                default:
                    throw new Exception("Invalid type");
            }
            
            if (TreeOrderChanged)
            {
                _root.RepoDatabase.ReloadLocationData(_root.Data);
            }
        }

        public void DeleteItem(IDotTreeEditItem item)
        {
            var id = item.Id;
            switch (item)
            {
                case DTOTreeEditLocation:
                    var loc = _root.Data.Root.getLocationByID(item.Id) ?? throw new Exception();
                    if (id == 1) throw new Exception("Cant delete the root");
                    if (!loc.isDeletable()) throw new Exception("Location is not deletable");
                    _root.RepoDatabase.DeleteLocation(id);
                    break;
                case DTOTreeEditGrid:
                    var grid = _root.Data.Root.getGridByID(item.Id) ?? throw new Exception();
                    if (!grid.isDeletable()) throw new Exception("Grid is not deletable");
                    _root.RepoDatabase.DeleteGrid(id);
                    break;
                default:
                    throw new Exception("Invalid type");
            }

            _root.RepoDatabase.ReloadLocationData(_root.Data);
        }
    }
}
