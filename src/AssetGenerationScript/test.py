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

    def get_category(self, iso_type):
        if iso_type in ScrewMaker.screwTables:
            return ScrewMaker.screwTables[iso_type][1].replace("make","")
        else:
            return "Unbekannter Typ" 

    def get_geometry_data(self, iso, size_str, length_str, is_full_thread):
        d = float(size_str.replace("M", ""))
        l_total = float(length_str)
        d_kern = d * 0.85
        
        # Kopfhöhe ermitteln: Bei Senkkopf ist k Teil der Gesamtlänge
        is_countersunk = "10642" in iso or "7046" in iso
        k = (d * 0.5) if is_countersunk else 0 
        
        # Das Gewinde darf maximal bis zum Kopf-Schaft-Übergang gehen
        l_max_possible = l_total - k

        if is_full_thread:
            l_thread = l_max_possible
        else:
            # Teilgewinde nach Norm: b = 2*d + 6 (für l <= 125)
            l_thread = min(2 * d + 6, l_max_possible - d)

        return {
            'd': d,
            'd_kern': d_kern,
            'l_total': l_total,
            'l_thread': l_thread,
            'k_offset': k, # Startpunkt des Schafts
            'offset': (d - d_kern) / 2
        }

    def create_thread_lines(self, g, is_full_thread):
        # Startpunkt ist immer an der Schraubenspitze (l_total)
        # Endpunkt ist Spitze minus Gewindelänge
        x_start = g['l_total']
        x_end = g['l_total'] - g['l_thread']
        
        # Der Auslauf (Runout) darf nicht in den Kopf ragen
        runout_x = max(g['k_offset'], x_end - g['offset'])
        
        lines = []
        # Kernlinien (Oben/Unten)
        lines.append(Draft.make_line(App.Vector(x_start, g['d_kern']/2, 0), App.Vector(x_end, g['d_kern']/2, 0)))
        lines.append(Draft.make_line(App.Vector(x_start, -g['d_kern']/2, 0), App.Vector(x_end, -g['d_kern']/2, 0)))
        
        # Auslauf nur zeichnen, wenn es kein Vollgewinde ist 
        # (oder wenn bei Vollgewinde noch Platz zum Kopf ist)
        if not is_full_thread or x_end > g['k_offset']:
            lines.append(Draft.make_line(App.Vector(x_end, g['d_kern']/2, 0), App.Vector(runout_x, g['d']/2, 0)))
            lines.append(Draft.make_line(App.Vector(x_end, -g['d_kern']/2, 0), App.Vector(runout_x, -g['d']/2, 0)))

        return lines


    def generate_iso(self, iso, size, length):
        """Hauptmethode für einen einzelnen ISO-Typ."""
        name = f"{iso}_{size}x{length}"
        print(f"-> Verarbeite: {name}")
        
        try:
            # 1. Modell erstellen
            attribs = FastenerModel(iso, size, length, Thread=False)
            shape = self.sm.createFastener(attribs)
            if not shape:
                return False

            obj = self.doc.addObject("Part::Feature", name)
            obj.Shape = shape
            self.doc.recompute()

            # 2. Geometrie-Daten holen
            g = self.get_geometry_data(iso, size, length, True)

            # --- SEITENANSICHT ---
            view_side = Draft.make_shape2dview(obj, App.Vector(0, -1, 0))
            view_side.VisibleOnly = False
            view_side.HiddenLines = True
            
            thread_lines = self.create_thread_lines(g, True)
            
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
    SCREW_LEN = "25"
    ISOS = ["ISO4162", "ISO4017", "ISO4762", "ISO10642"]

    worker = FastenerAutomation(OUTPUT)

    for iso in ISOS:
        success = worker.generate_iso(iso, SCREW_SIZE, SCREW_LEN)
        if success:
            print(f"Erfolg: {iso} exportiert.")
            print(worker.get_category(iso))

    print("\nAlle Aufgaben erledigt.")