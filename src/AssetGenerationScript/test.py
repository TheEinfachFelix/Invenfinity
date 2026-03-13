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
        attribs = FastenerModel(iso, screw_size, screw_length)
        
        # 2. Geometrie direkt über die Methode der Klasse erstellen
        # createFastener schlägt in screwTables nach und ruft createScrew auf
        shape = sm.createFastener(attribs)
        
        # ... (nach shape = sm.createFastener(attribs))

        if shape:
            # 1. Haupt-Objekt für Seitenansicht
            obj = doc.addObject("Part::Feature", name)
            obj.Shape = shape
            doc.recompute()
            
            # Export Seitenansicht
            view_side = Draft.make_shape2dview(obj, App.Vector(0, -1, 0))
            doc.recompute()
            importSVG.export([view_side], os.path.join(output_path, f"{name}_side.svg"))

            # --- KOPF-EXTRAKTION (NEUE LOGIK) ---
            bbox = shape.BoundBox
            
            # Wir erstellen zwei Test-Boxen: eine für das obere Ende, eine für das untere.
            # Da ein Kopf immer dicker ist als der Schaft, vergleichen wir die Flächen.
            
            # Box Oben (Z > 0)
            box_top = Part.makeBox(100, 100, bbox.ZMax + 1, App.Vector(-50, -50, 0.01))
            shape_top = shape.common(box_top)
            
            # Box Unten (Z < 0)
            box_bottom = Part.makeBox(100, 100, abs(bbox.ZMin) + 1, App.Vector(-50, -50, bbox.ZMin - 0.5))
            shape_bottom = shape.common(box_bottom)

            # Entscheidung: Welcher Teil ist der Kopf?
            # Wir nehmen den Teil, der in der Draufsicht (X-Y Ebene) die größere Ausdehnung hat.
            def get_width(s):
                if not s or not s.Vertexes: return 0
                return s.BoundBox.XLength

            if get_width(shape_top) >= get_width(shape_bottom):
                head_shape = shape_top
            else:
                head_shape = shape_bottom

            # Export Kopf-SVG
            if head_shape and head_shape.Vertexes:
                head_obj = doc.addObject("Part::Feature", f"{name}_HeadTemp")
                head_obj.Shape = head_shape
                doc.recompute()

                # Blick von oben
                view_head = Draft.make_shape2dview(head_obj, App.Vector(0, 0, 1))
                doc.recompute()
                
                importSVG.export([view_head], os.path.join(output_path, f"{name}_head.svg"))
                
                doc.removeObject(head_obj.Name)
                doc.removeObject(view_head.Name)

            # Aufräumen
            doc.removeObject(obj.Name)
            doc.removeObject(view_side.Name)

        else:
            print(f"Fehler: createFastener lieferte kein Shape für {iso}")

    except Exception as e:
        print(f"Fehler bei {iso}: {str(e)}")

print("Generierung abgeschlossen.")