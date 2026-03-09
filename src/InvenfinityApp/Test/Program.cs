// See https://aka.ms/new-console-template for more information
using LabelMaker;
using LabelMaker.Models.Bin;
using LabelMaker.Models.Part;

var Bin = new BinDataModel()
{
    UnitLength = 1,
    SlotCount = 1,
};
var Part = new PartDataModel()
{
    Thread = ScrewThreadType.M6,
    Length = 25,
    Head = ScrewHeadType.Senkkopf,
    Drive = ScrewDriveType.Philips
};
var path = "C:\\Github\\Invenfinity\\template.json";
var assetPath = "";
LabelMakerControll var = new(assetPath);
var.test(path,Bin,Part);