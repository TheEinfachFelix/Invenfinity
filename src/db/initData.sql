INSERT INTO "Location" ("LocationID", "name", "MasterLocationID")
VALUES
  (1, 'Root', NULL),
  (2, 'Testlager', 1),
  (3, 'Testsschubladen', 2)
ON CONFLICT ("LocationID") DO NOTHING;

INSERT INTO "Grid" ("GridID", "LocationID", "Name")
VALUES
  (1, 2, 'Oberste Schublade')
ON CONFLICT ("GridID") DO NOTHING;

INSERT INTO "BinType" ("BinTypeID", "SlotCount", "X", "Y")
VALUES
  (1, 1, 1, 1),
  (2, 3, 2, 1)
ON CONFLICT ("BinTypeID") DO NOTHING;

INSERT INTO "Bin" ("BinID", "BinTypeID")
VALUES
  (1, 1),
  (2, 2)
ON CONFLICT ("BinID") DO NOTHING;

INSERT INTO "Part" ("PartID", "InventreeID")
VALUES
  (1, NULL),
  (2, NULL)
ON CONFLICT ("PartID") DO NOTHING;

INSERT INTO "BinPart" ("BinID", "PartID", "SlotNr")
VALUES
  (1, 1, 1),
  (2, 2, 2),
  (2, 1, 1)
ON CONFLICT ("BinID", "PartID") DO NOTHING;

INSERT INTO "GridPos" ("GridPosID", "X", "Y", "BinID", "GridID")
VALUES
  (1, 0, 0, 1, 1),
  (2, 1, 0, 1, 1),
  (3, 0, 1, 2, 1),
  (4, 1, 1, NULL, 1)
ON CONFLICT ("GridPosID") DO NOTHING;