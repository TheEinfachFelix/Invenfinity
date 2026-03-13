import os
import sys
import FreeCAD as App
import Part
import importSVG
import Draft
import ScrewMaker

# 1. Pfade zentral verwalten
PATHS = [
    r"C:\Users\felix\AppData\Roaming\FreeCAD\Mod\fasteners",
    r"C:\Github\Invenfinity\src\AssetGenerationScript"
]
for p in PATHS:
    if p not in sys.path:
        sys.path.insert(0, p)

from FastnerModel import FastenerModel

class FastenerAutomation:
    def __init__(self, output_path):
        self.output_path = output_path
        self.sm = ScrewMaker.FSScrewMaker()
        self.doc = App.newDocument("Fastener_Generation")
        
        if not os.path.exists(self.output_path):
            os.makedirs(self.output_path)

    def get_category(self, iso_type) -> str:
        if iso_type in ScrewMaker.screwTables:
            return ScrewMaker.screwTables[iso_type][1].replace("make","")
        else:
            return "Unbekannter Typ" 

    def get_geometry_data(self, iso, size_str, length_str, thread_length_override=0):
        d_val = float(size_str.replace("M", ""))
        l_total = float(length_str)

        # ... (k_table bleibt identisch) ...
        k_table = {1.6: 1.0, 2.0: 1.2, 2.5: 1.5, 3.0: 1.65, 4.0: 2.7, 5.0: 2.7, 6.0: 3.3, 8.0: 4.65, 10.0: 5.0, 12.0: 6.0, 16.0: 8.0, 20.0: 10.0}
        countersunk_isos = ["10642", "7046", "2009", "14581", "7047"]
        is_countersunk = any(num in iso for num in countersunk_isos)
        k = k_table.get(d_val, d_val * 0.5) if is_countersunk else 0

        safety_margin = 0.8 if is_countersunk else 0
        l_max_possible = l_total - k - safety_margin

        # LOGIK-ÄNDERUNG:
        if thread_length_override > 0:
            # Nutze manuellen Wert, aber deckle ihn bei l_max_possible
            l_thread = min(float(thread_length_override), l_max_possible)
        else:
            l_thread = l_max_possible


        return {
            'd': d_val,
            'd_kern': d_val * 0.85,
            'l_total': l_total,
            'l_thread': l_thread,
            'k_offset': k + safety_margin,
            'offset': (d_val - (d_val * 0.85)) / 2
        }

    def create_thread_lines(self, g):
        # x_start ist die Schraubenspitze
        x_start = g['l_total']
        # x_end ist das Ende des Gewindes (Richtung Kopf)
        x_end = g['l_total'] - g['l_thread']
        
        # Sicherstellen, dass der Auslauf nicht in den Kopf ragt
        runout_x = max(g['k_offset'], x_end - g['offset'])
        
        lines = []
        # Gewindegrund-Linien (von Spitze bis x_end)
        lines.append(Draft.make_line(App.Vector(x_start, g['d_kern']/2, 0), App.Vector(x_end, g['d_kern']/2, 0)))
        lines.append(Draft.make_line(App.Vector(x_start, -g['d_kern']/2, 0), App.Vector(x_end, -g['d_kern']/2, 0)))
        
        # Schräger Auslauf (nur zeichnen, wenn nicht direkt am Kopf gestoppt wurde)
        if x_end > g['k_offset']:
            lines.append(Draft.make_line(App.Vector(x_end, g['d_kern']/2, 0), App.Vector(runout_x, g['d']/2, 0)))
            lines.append(Draft.make_line(App.Vector(x_end, -g['d_kern']/2, 0), App.Vector(runout_x, -g['d']/2, 0)))

        return lines


    def generate_iso(self, attribs : FastenerModel, ThreadLength):
        """Hauptmethode für einen einzelnen ISO-Typ."""
        iso = attribs.Type
        name = f"{worker.get_category(iso)}_{iso}"
        print(f"-> Verarbeite: {name}")
        
        try:
            # 1. Modell erstellen
            shape = self.sm.createFastener(attribs)
            if not shape:
                return False

            obj = self.doc.addObject("Part::Feature", name)
            obj.Shape = shape
            obj.Shape = shape.removeSplitter()
            self.doc.recompute()

            # 2. Geometrie-Daten holen
            if self.get_category(iso) == "SetScrew":
                ThreadLength = 0

            thread_lines = []
            if "Bolt" in worker.get_category(iso) or "Screw" in worker.get_category(iso) or "HexHeadWithFlange" in worker.get_category(iso):
                g = self.get_geometry_data(iso, attribs.Diameter, attribs.Length, ThreadLength)
                thread_lines = self.create_thread_lines(g)


            # --- SEITENANSICHT ---
            view_side = Draft.make_shape2dview(obj, App.Vector(0, -1, 0))
            view_side.VisibleOnly = True
            view_side.HiddenLines = True
            
            self.doc.recompute()
            importSVG.export([view_side] + thread_lines, os.path.join(self.output_path, f"{name}_side.svg"))

            # --- DRAUFSICHT ---
            view_head = Draft.make_shape2dview(obj, App.Vector(0, 0, 1))
            view_head.VisibleOnly = True
            view_side.HiddenLines = False

            self.doc.recompute()
            importSVG.export([view_head ], os.path.join(self.output_path, f"{name}_head.svg"))
            
            return True
        except Exception as e:
            print(f"Fehler bei {iso}: {e}")
            return False

# --- AUSFÜHRUNG ---
if __name__ == "__main__":
    # Konfiguration (Hier kannst du später leicht eine UI oder CSV-Import anbinden)
    OUTPUT = r"C:\Github\Invenfinity\src\AssetGenerationScript\outp"
    SCREW_SIZE = "M6"
    SCREW_LEN = "30"
    ISOS = ["ISO4162", "ISO4014", "ISO4762", "ISO10642", "ISO4026", "ISO4032", "ISO7040", "DIN1587", "ISO7089", "DIN603", "ISO7380-1", "ISO14580", "ISO4035", "ISO4161", "DIN6334", "DIN315" ]

    worker = FastenerAutomation(OUTPUT)

    for iso in ISOS:
        data = FastenerModel(iso, SCREW_SIZE, SCREW_LEN, Thread=False)
        success = worker.generate_iso(data, 20)
        if success:
            print(f"Erfolg: {iso} exportiert.")
            print(worker.get_category(iso))

    data = FastenerModel("DIN1151-A", "16x30",0)
    worker.generate_iso(data , 20)

    data = FastenerModel("ISO2936", "6",0, KeySize = "Standard")
    worker.generate_iso(data , 20)

    data = FastenerModel("DIN508", SCREW_SIZE, SlotWidth = "8 mm")
    worker.generate_iso(data , 20)

    data = FastenerModel("GN505", SCREW_SIZE, SlotWidth = "30/40 series")
    worker.generate_iso(data , 20)

    print("\nAlle Aufgaben erledigt.")