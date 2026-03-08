using Backend.Application.DTOs;
using Backend.Application.DTOs.Location;
using Backend.Domain;
using Backend.Exceptions;
using Backend.Infrastructure.Datenbank;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.UseCases
{
    public class UcLocations
    {
        private readonly RepoDatabase _repo;
        private readonly Dset _data;
        internal UcLocations(RepoDatabase repo, Dset data)
        {
            _repo = repo;
            _data = data;
        }

        public DTOTreeLocation GetLocations()
        {
            return _data.Root.ToTreeDto();
        }

        public IDtoTreeEditItem GetEditItem(IDtoTreeItem item)
        {
            return item switch
            {
                DTOTreeLocation => (_data.Root.FindLocationByID(item.Id)
                                       ?? throw new NotFoundException("Location", item.Id)).ToSingleTreeDto(),
                DTOTreeGrid => (_data.Root.FindGridByID(item.Id)
                                       ?? throw new NotFoundException("Grid", item.Id)).ToTreeDto(),
                _ => throw new InvalidOperationException()
            };
        }
        public void CreateLocation(string name, int parentID)
        {
            _repo.CreateLocation(name, parentID);
            _repo.ReloadLocationData(_data);
        }
        public void CreateGrid(string name, int parentID, int xsize, int ysize)
        {
            _repo.CreateGrid(name, parentID, xsize, ysize);
            _repo.ReloadLocationData(_data);
        }
        public void EditItem(IDtoTreeEditItem item)
        {
            bool TreeOrderChanged = false;
            switch (item)
            {
                case DTOTreeLocation:
                    var loc = _data.Root.FindLocationByID(item.Id) ?? throw new NotFoundException("Location", item.Id);
                    loc.Name = item.Name;
                    if (loc.ParentId != item.ParentId) TreeOrderChanged = true;
                    loc.ParentId = item.ParentId;
                    _repo.UpdateSingleLocation(loc);
                    break;
                case DTOTreeGrid:
                    var grid = _data.Root.FindGridByID(item.Id) ?? throw new NotFoundException("Grid", item.Id);
                    grid.Name = item.Name;
                    if (grid.LocationId != item.ParentId) TreeOrderChanged = true;
                    grid.LocationId = item.ParentId;
                    if (grid.Xmax != item.Xsize || grid.Ymax != item.Ysize)
                        grid.ResizeGrid(item.Xsize, item.Ysize);
                    _repo.UpdateSingleGrid(grid);
                    break;
                default:
                    throw new Exception("Invalid type");
            }

            if (TreeOrderChanged)
            {
                _repo.ReloadLocationData(_data);
            }
        }

        public void DeleteItem(IDtoTreeEditItem item)
        {
            var id = item.Id;
            switch (item)
            {
                case DTOTreeLocation:
                    var loc = _data.Root.FindLocationByID(item.Id) ?? throw new NotFoundException("Location", item.Id);
                    if (id == 1) throw new Exception("Cant delete the root");
                    if (!loc.isDeletable()) throw new Exception("Location is not deletable");
                    _repo.DeleteLocation(id);
                    break;
                case DTOTreeGrid:
                    var grid = _data.Root.FindGridByID(item.Id) ?? throw new NotFoundException("Grid", item.Id);
                    if (!grid.isDeletable()) throw new Exception("Grid is not deletable");
                    _repo.DeleteGrid(id);
                    break;
                default:
                    throw new Exception("Invalid type");
            }

            _repo.ReloadLocationData(_data);
        }

    }
}
