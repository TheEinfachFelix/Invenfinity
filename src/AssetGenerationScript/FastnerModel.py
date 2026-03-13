class FastenerModel:
    def __init__(self, fastener_type, diameter="Auto", length="20", **kwargs):
        # 1. Alle Standard-Attribute aus FastenersCmd.txt (FastenerAttribs)
        self.Type = fastener_type
        self.Diameter = diameter
        self.Thread = kwargs.get('Thread', False)
        self.LeftHanded = kwargs.get('LeftHanded', False)
        self.MatchOuter = kwargs.get('MatchOuter', False)
        self.Length = length
        self.LengthCustom = kwargs.get('LengthCustom', 0.0)
        self.Width = kwargs.get('Width', None)
        self.DiameterCustom = kwargs.get('DiameterCustom', 6.0)
        self.PitchCustom = kwargs.get('PitchCustom', 1.0)
        self.Tcode = kwargs.get('Tcode', None)
        self.Blind = kwargs.get('Blind', False)
        self.ScrewLength = kwargs.get('ScrewLength', None)
        self.SlotWidth = kwargs.get('SlotWidth', None)
        self.ExternalDiam = kwargs.get('ExternalDiam', None)
        self.KeySize = kwargs.get('KeySize', None)

        # Interne Verwaltungs-Attribute
        self.familyType = kwargs.get('familyType', "")
        self.dimTable = None
        self.baseType = fastener_type

        # 2. Sofortige Berechnung der 'calc_'-Attribute
        self.calc_diam = None
        self.calc_pitch = None
        self.calc_len = None
        
        # Trigger der automatischen Berechnung
        self._generate_calculated_values()
        
        # 3. Das finale Label generieren
        self.Label = self._generate_label()

    def _clean_decimals(self, val):
        """Hilfsfunktion zum Formatieren von Zahlen (entfernt .0)."""
        if val is None: return ""
        val_str = str(val)
        return val_str.rstrip('0').rstrip('.') if "." in val_str else val_str

    def _generate_calculated_values(self):
        """Berechnet die Werte analog zur 'execute'-Logik."""
        # Durchmesser
        if self.Diameter == 'Auto':
            self.calc_diam = "6.0"  # Default-Logik falls kein ScrewMaker vorhanden
        elif self.Diameter == 'Custom':
            self.calc_diam = str(self.DiameterCustom)
        else:
            self.calc_diam = str(self.Diameter)

        # Länge
        if self.Length == 'Custom':
            self.calc_len = self._clean_decimals(self.LengthCustom)
        else:
            self.calc_len = str(self.Length)

        # Pitch
        if self.Diameter == 'Custom':
            self.calc_pitch = self.PitchCustom
        else:
            self.calc_pitch = None

    def _generate_label(self):
        """Erzeugt den Anzeigenamen."""
        disp_diam = self._clean_decimals(self.calc_diam)
        label_parts = [disp_diam]
        
        if self.calc_len:
            label_parts.append(f"x{self._clean_decimals(self.calc_len)}")
        if self.Width:
            label_parts.append(f"x{self.Width}")
        if self.LeftHanded:
            label_parts.append("LH")
        if self.SlotWidth:
            label_parts.append(f" x {self.SlotWidth}")
            
        return f"{''.join(label_parts)}-{self.Type}"

# --- Test ---
if __name__ == "__main__":
    # Beispiel 1: Standard
    bolt1 = FastenerModel("ISO4017", diameter="8.0", length="45")
    print(f"Bolt 1: {bolt1.Label} (Diam: {bolt1.calc_diam}, Len: {bolt1.calc_len})")

    # Beispiel 2: Custom mit Keyword-Arguments
    bolt2 = FastenerModel("ISO4017", diameter="Custom", length="Custom", 
                          DiameterCustom=7.5, LengthCustom=32.0, LeftHanded=True)
    print(f"Bolt 2: {bolt2.Label}")