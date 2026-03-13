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

# Hilfsklasse für die Attribute, die createFastener erwartet
class FastenerAttribs:
    def __init__(self, iso, size, length):
        self.Type = iso
        self.baseType = iso
        self.Diameter = size
        self.calc_len = length
        
        # Diese Attribute werden von der Geometrie-Engine (screw_maker.py) erwartet:
        self.calc_diam = size   # Wird intern oft überschrieben, muss aber da sein
        self.LeftHanded = False
        self.Thread = False     # False = einfache Darstellung
        self.SimpThread = True  # Oft als Fallback für 'thread' genutzt
        self.SymThread = False  # Symbolisches Gewinde
        self.CustomPitch = "0"  # Standardmäßig kein Custom-Pitch
        self.CustomDia = "0"    # Standardmäßig kein Custom-Diameter

# ==========================================
# PARAMETER
# ==========================================
output_path = r"C:\Github\Invenfinity\src\AssetGenerationScript\outp"
screw_length = "25"
screw_size = "M6"
iso_list = ["ISO4017", "ISO4762", "ISO7046"]

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
        attribs = FastenerAttribs(iso, screw_size, screw_length)
        
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