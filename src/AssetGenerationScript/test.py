import os
import sys

# Pfad zur Workbench setzen
fasteners_path = r"C:\Users\felix\AppData\Roaming\FreeCAD\Mod\fasteners"
if fasteners_path not in sys.path:
    sys.path.append(fasteners_path)

import FreeCAD as App
import Part
import importSVG
import Draft
import ScrewMaker 
sys.path.insert(1, "C:\Github\Invenfinity\src\AssetGenerationScript")
from FastnerModel import FastenerModel

# ==========================================
# PARAMETER
# ==========================================
output_path = r"C:\Github\Invenfinity\src\AssetGenerationScript\outp"
screw_length = "25"
screw_size = "M6"
iso_list = ["ISO4162","ISO4017", "ISO4762", "ISO7046"]

if not os.path.exists(output_path):
    os.makedirs(output_path)

doc = App.newDocument("Fastener_Generation")

# Wir instanziieren die Klasse aus deinem Code-Snippet
sm = ScrewMaker.FSScrewMaker()
projection_dir = App.Vector(0, -1, 0)

for iso in iso_list:
    name = f"{iso}_{screw_size}x{screw_length}"
    print(f"Generiere {name} via createFastener...")
    
    try:
        # 1. Attribute vorbereiten
        attribs = FastenerModel(iso, screw_size, screw_length)
        
        # 2. Geometrie direkt über die Methode der Klasse erstellen
        # createFastener schlägt in screwTables nach und ruft createScrew auf
        shape = sm.createFastener(attribs)
        
        if shape:
            # Objekt in FreeCAD erstellen, um es zu exportieren
            obj = doc.addObject("Part::Feature", name)
            obj.Shape = shape
            doc.recompute()
            
            # 2D-Ansicht für SVG
            view = Draft.make_shape2dview(obj, projection_dir)
            doc.recompute()
            
            file_path = os.path.join(output_path, f"{name}.svg")
            importSVG.export([view], file_path)
            print(f"Erfolgreich exportiert: {file_path}")
            
            # Aufräumen
            doc.removeObject(obj.Name)
            doc.removeObject(view.Name)
        else:
            print(f"Fehler: createFastener lieferte kein Shape für {iso}")

    except Exception as e:
        print(f"Fehler bei {iso}: {str(e)}")

print("Generierung abgeschlossen.")