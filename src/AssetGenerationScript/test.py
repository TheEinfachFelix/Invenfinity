import os
import sys

# 1. PFAD ZUR WORKBENCH SETZEN
# Dies muss VOR dem Import von screw_maker passieren
fasteners_path = r"C:\Users\felix\AppData\Roaming\FreeCAD\Mod\fasteners"
if fasteners_path not in sys.path:
    sys.path.append(fasteners_path)

import FreeCAD as App
import Part
import importSVG
import Draft
import screw_maker # Jetzt findet er auch die Unterordner

# ==========================================
# PARAMETER-KONFIGURATION
# ==========================================
output_path = r"C:\Github\Invenfinity\src\AssetGenerationScript\outp"
screw_length = "25"
screw_size = "M6"
iso_list = ["ISO4017", "ISO4762", "ISO7046"]

# Hilfsklasse, um die Parameter für die createScrew-Methode zu bündeln
class FastenerAttribs:
    def __init__(self, iso, size, length):
        self.Type = iso
        # baseType muss oft kleingeschrieben sein, damit die CSV gefunden wird
        # (z.B. "iso4017" für "iso4017head.csv")
        self.baseType = iso.lower() 
        self.Diameter = size
        self.calc_len = length
        self.LeftHanded = False
        self.thread = False
iso_to_function = {
    "ISO4017": "makeHexHeadBolt",
    "ISO4762": "makeCylinderHeadScrew",
    "ISO7046": "makeCountersunkHeadScrew"
}
# ==========================================

if not os.path.exists(output_path):
    os.makedirs(output_path)

# Dokument erstellen
doc = App.newDocument("Fastener_Generation")

# Instanz der Screw-Klasse aus screw_maker
screw_gen = screw_maker.Screw()

# Projektionsrichtung (Draufsicht/Seitenansicht)
projection_dir = App.Vector(0, -1, 0)

for iso in iso_list:
    function_name = iso_to_function.get(iso)
    
    if not function_name:
        print(f"Keine Funktion für {iso} gefunden.")
        continue

    name = f"{iso}_{screw_size}x{screw_length}"
    print(f"Erstelle: {name} mit Funktion: {function_name}")
    
    try:
        attribs = FastenerAttribs(iso, screw_size, screw_length)
        
        # WICHTIG: Hier wird jetzt function_name ("makeHexHeadBolt") 
        # statt "ISO4017" übergeben.
        shape = screw_gen.createScrew(function_name, attribs)
        
        if shape:
            obj = doc.addObject("Part::Feature", name)
            obj.Shape = shape
            doc.recompute()
            
            # 2D-Ansicht für SVG Export erzeugen
            view = Draft.make_shape2dview(obj, projection_dir)
            doc.recompute()
            
            file_path = os.path.join(output_path, f"{name}.svg")
            importSVG.export([view], file_path)
            print(f"Erfolgreich exportiert: {file_path}")
            
            # Objekte löschen für den nächsten Durchgang
            doc.removeObject(obj.Name)
            doc.removeObject(view.Name)
        else:
            print(f"Konnte Shape für {iso} nicht generieren.")
            
    except Exception as e:
        print(f"Fehler bei {iso}: {str(e)}")

print("Generierung abgeschlossen.")