import FreeCAD
import ScrewMaker
import TechDraw
import TechDrawGui

sizes = ["M3","M4","M5","M6","M8","M10"]
types = [
    "ISO4017",   # Hex bolt
    "ISO4762",   # Socket head
    "ISO7380",   # Button head
    "ISO10642",  # Countersunk
]

for t in types:
    for s in sizes:

        doc = FreeCAD.newDocument()

        sm = ScrewMaker.Instance()
        bolt = sm.createFastener(t, s, "20")

        obj = doc.addObject("Part::Feature","Bolt")
        obj.Shape = bolt

        page = doc.addObject('TechDraw::DrawPage','Page')
        view = doc.addObject('TechDraw::DrawViewPart','View')

        view.Source=[obj]
        page.addView(view)

        filename=f"/export/{t}_{s}.svg"

        TechDrawGui.exportPageAsSvg(page,filename)

        FreeCAD.closeDocument(doc.Name)