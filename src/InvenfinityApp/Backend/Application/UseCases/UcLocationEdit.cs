using Backend.Application.DTOs;
using Backend.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases
{
    public class UcLocationEdit
    {
        private readonly UcRoot _root;
        internal UcLocationEdit(UcRoot root)
        {
            _root = root;
        }
        public IDotTreeEditItem GetEditItem(IDotTreeItem item)
        {
            return item switch
            {
                DTOs.DTOTreeLocation => LocationFactory.CreateLocationSingle(
                                       _root.Data.Root.FindLocationByID(item.Id)
                                       ?? throw new Exception()),
                DTOs.DTOTreeGrid => LocationFactory.CreateGrid(
                                           _root.Data.Root.FindGridByID(item.Id)
                                           ?? throw new Exception()),
                _ => throw new InvalidOperationException()
            };
        }
        public void CreateLocation(string name, int parentID)
        {
            _root.RepoDatabase.CreateLocation(name, parentID);
            _root.RepoDatabase.ReloadLocationData(_root.Data);
        }
        public void CreateGrid(string name, int parentID, int xsize, int ysize)
        {
            _root.RepoDatabase.CreateGrid(name, parentID, xsize, ysize);
            _root.RepoDatabase.ReloadLocationData(_root.Data);
        }
        public void EditItem(IDotTreeEditItem item)
        {
            bool TreeOrderChanged = false;
            switch (item)
            {
                case DTOTreeLocation:
                    var loc = _root.Data.Root.FindLocationByID(item.Id) ?? throw new Exception();
                    loc.Name = item.Name;
                    if (loc.ParentId != item.ParentId) TreeOrderChanged = true;
                    loc.ParentId = item.ParentId;
                    _root.RepoDatabase.UpdateSingleLocation(loc);
                    break;
                case DTOTreeGrid:
                    var grid = _root.Data.Root.FindGridByID(item.Id) ?? throw new Exception();
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
                case DTOTreeLocation:
                    var loc = _root.Data.Root.FindLocationByID(item.Id) ?? throw new Exception();
                    if (id == 1) throw new Exception("Cant delete the root");
                    if (!loc.isDeletable()) throw new Exception("Location is not deletable");
                    _root.RepoDatabase.DeleteLocation(id);
                    break;
                case DTOTreeGrid:
                    var grid = _root.Data.Root.FindGridByID(item.Id) ?? throw new Exception();
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
