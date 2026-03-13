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
iso_list = ["ISO4162","ISO4017", "ISO4762", "ISO10642"]

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
        attribs = FastenerModel(iso, screw_size, screw_length, Thread=True)
        
        # 2. Geometrie direkt über die Methode der Klasse erstellen
        # createFastener schlägt in screwTables nach und ruft createScrew auf
        shape = sm.createFastener(attribs)
        
        # ... (nach shape = sm.createFastener(attribs))

        if shape:
            # 1. Haupt-Objekt für Seitenansicht
            obj = doc.addObject("Part::Feature", name)
            obj.Shape = shape
            doc.recompute()
            
            # --- SEITENANSICHT ---
            view_side = Draft.make_shape2dview(obj, App.Vector(0, -1, 0))
            
            # Fehlervermeidung: Sicherer Zugriff auf Properties
            if hasattr(view_side, "VisibleOnly"):
                view_side.VisibleOnly = False
            if hasattr(view_side, "HiddenLines"):
                view_side.HiddenLines = True  # Erzeugt die gestrichelten Linien (wie oben im Bild)
            
            doc.recompute()
            importSVG.export([view_side], os.path.join(output_path, f"{name}_side.svg"))

            # --- DRAUFSICHT (KOPF) ---
            # Wir projizieren das GANZE Objekt von oben. 
            # Das erzeugt automatisch den Kreis (Schaft) innerhalb des Sechskants.
            view_head = Draft.make_shape2dview(obj, App.Vector(0, 0, 1))
            
            if hasattr(view_head, "HiddenLines"):
                view_head.HiddenLines = False # Kopf meist ohne verdeckte Linien
            
            doc.recompute()
            importSVG.export([view_head], os.path.join(output_path, f"{name}_head.svg"))

        else:
            print(f"Fehler: createFastener lieferte kein Shape für {iso}")

    except Exception as e:
        print(f"Fehler bei {iso}: {str(e)}")

def get_function_name(iso_type):
    """
    Gibt den Namen der Erzeugungs-Funktion für einen gegebenen ISO/DIN Typ zurück.
    """
    if iso_type in ScrewMaker.screwTables:
        # Index 0 ist die Familie ("Screw", "Nut", etc.)
        # Index 1 ist der Funktionsname ("makeHexHeadBolt", etc.)
        return ScrewMaker.screwTables[iso_type][1].replace("make","")
    else:
        return "Unbekannter Typ"

# Beispielaufruf für deine Liste:
for iso in iso_list:
    func_name = get_function_name(iso)
    print(f"ISO: {iso} nutzt die Funktion: {func_name}")

print("Generierung abgeschlossen.")