import FreeCAD as App
import Part
import Draft
import importSVG
import math
import os

# 1. Neues Dokument erstellen
doc = App.newDocument("ISO_Schrauben_Generator")

def create_iso4017(name, d, l, k, s):
    """
    Erstellt eine Sechskantschraube nach ISO 4017 (früher DIN 933)
    d = Gewindedurchmesser, l = Länge, k = Kopfhöhe, s = Schlüsselweite
    """
    # Schaft (glatter Zylinder für das Gewinde)
    shaft = Part.makeCylinder(d / 2.0, l)
    
    # Sechskantkopf (Berechnung des Umkreisradius aus der Schlüsselweite)
    r = (s / 2.0) / math.cos(math.pi / 6.0)
    head_points = [App.Vector(r * math.cos(i * math.pi / 3), r * math.sin(i * math.pi / 3), 0) for i in range(7)]
    head_wire = Part.makePolygon(head_points)
    head_face = Part.Face(head_wire)
    head_solid = head_face.extrude(App.Vector(0, 0, k))
    
    # Kopf oben auf den Schaft setzen
    head_solid.translate(App.Vector(0, 0, l))
    
    # Bauteile verschmelzen
    screw = shaft.fuse(head_solid)
    obj = doc.addObject("Part::Feature", name)
    obj.Shape = screw
    return obj

def create_iso4762(name, d, l, dk, k, s, t):
    """
    Erstellt eine Zylinderschraube mit Innensechskant nach ISO 4762 (früher DIN 912)
    d = Durchmesser, l = Länge, dk = Kopfdurchmesser, k = Kopfhöhe
    s = Schlüsselweite Innensechskant, t = Tiefe Innensechskant
    """
    # Schaft
    shaft = Part.makeCylinder(d / 2.0, l)
    
    # Zylinderkopf
    head = Part.makeCylinder(dk / 2.0, k)
    head.translate(App.Vector(0, 0, l))
    
    # Innensechskant ausstanzen
    r = (s / 2.0) / math.cos(math.pi / 6.0)
    socket_points = [App.Vector(r * math.cos(i * math.pi / 3), r * math.sin(i * math.pi / 3), 0) for i in range(7)]
    socket_wire = Part.makePolygon(socket_points)
    socket_face = Part.Face(socket_wire)
    socket_solid = socket_face.extrude(App.Vector(0, 0, t))
    
    # Innensechskant positionieren (von oben in den Kopf absenken)
    socket_solid.translate(App.Vector(0, 0, l + k - t))
    
    # Innensechskant vom Kopf abziehen
    head_with_socket = head.cut(socket_solid)
    
    # Alles verschmelzen
    screw = shaft.fuse(head_with_socket)
    obj = doc.addObject("Part::Feature", name)
    obj.Shape = screw
    return obj

# 2. Schrauben generieren (Beispielmaße für M8x20)
# ISO 4017 (Sechskant) M8x20: d=8, l=20, k=5.3, s=13
screw1 = create_iso4017("ISO4017_M8x20", d=8, l=20, k=5.3, s=13)

# ISO 4762 (Zylinderkopf) M8x20: d=8, l=20, dk=13, k=8, s=6, t=4
screw2 = create_iso4762("ISO4762_M8x20", d=8, l=20, dk=13, k=8, s=6, t=4)

doc.recompute()

# 3. Für den SVG-Export in 2D projizieren
# Blickrichtung exakt von der Seite (Y-Achse) für eine technische Seitenansicht
projection_dir = App.Vector(0, -1, 0)

view1 = Draft.make_shape2dview(screw1, projection_dir)
view2 = Draft.make_shape2dview(screw2, projection_dir)
doc.recompute()

# 4. SVG-Dateien exportieren
output_dir = "C:\Github\Invenfinity\src\AssetGenerationScript\outp" # Speichert im Basis-Benutzerordner
file1 = os.path.join(output_dir, "ISO4017_M8x20.svg")
file2 = os.path.join(output_dir, "ISO4762_M8x20.svg")

importSVG.export([view1], file1)
importSVG.export([view2], file2)

App.Console.PrintMessage(f"Erfolg! Die SVGs wurden gespeichert unter: {output_dir}\n")