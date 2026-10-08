# Open Points List

## 🔴 Bugs

Aktuell keine bekannten Bugs.

&nbsp;

## 🟢 Recipe Model

- [ ] Bierstile als Enum

- [ ] Filter für Bierstil auf Index-Seite

&nbsp;

## 🔵 BrewBatch

- [ ] Hefe bei abweichender BatchSize korrekt skalieren
    - Hefemenge passend zur BatchSize skalieren
    - Einzelne und mehrere Hefen testen
    - Vorausgewählte Hefe beim Anlegen eines BrewBatch berücksichtigen

- [ ] Index-Seite mit Filter-Pills für Status

- [ ] Übernahme RecipeVersion → BrewBatch abschließend definieren
    - Prüfen, welche Rezeptdaten im BrewBatch tatsächlich persistiert werden müssen
    - Historische Rezeptdaten möglichst über die exakte BrewRecipeVersion beziehen
    - TapWater im BrewBatch nur noch als "To be determined" behandeln

- [ ] Gärzugaben in den BrewBatch übernehmen
    - Gärzugaben analog zu den ausgewählten Rezeptdaten in den BrewBatch übernehmen
    - Mengen entsprechend der BatchSize skalieren
    - Im BrewBatch weiterhin zwischen Zutatenübersicht und Gärprozess unterscheiden

- [ ] Schnellaktion "Brew" im BrewBatch Details
    - Braudatum auf heute setzen
    - Status auf `Brewed` setzen
    - Kein Umweg über Edit

- [ ] Schnellaktion "Bottled" im BrewBatch Details
    - Abfülldatum auf heute setzen
    - Status auf `Bottled` setzen
    - Kein Umweg über Edit
    - Perspektivisch mit Abfüll-/Karbonisierungsworkflow verbinden

- [ ] Erwarteten Alkoholanstieg durch Flaschengärung anzeigen
    - aus der berechneten Haushaltszuckermenge ableiten
    - zusätzlichen Alkoholgehalt in % vol anzeigen
    - zunächst nur berechneter Anzeigewert, nicht persistieren
    - bei der Karbonisierungsberechnung im BrewBatch Edit und ggf. Details darstellen

- [ ] Workflow-Aktionen in der BrewBatch-Übersicht
    - Aktionen abhängig von BrewBatch.Status anzeigen
    - Planned → Ready
        - Status direkt auf Ready setzen
    - Planned / Ready → Brewed
        - Dialog mit den beim Brauen benötigten Ist-Daten
        - Braudatum mit aktuellem Datum vorbelegen
        - Menge
        - Stammwürze
        - ggf. Sudhausausbeute
        - nach Bestätigung Status auf Brewed setzen
    - Brewed → Bottled
        - Dialog mit den beim Abfüllen benötigten Ist-Daten
        - Abfülldatum mit aktuellem Datum vorbelegen
        - Abfüllmenge
        - Abfülltemperatur
        - Restextrakt
        - Karbonisierung und benötigten Haushaltszucker live anzeigen
        - Best-to-Drink aus Reifezeit darstellen
        - nach Bestätigung Status auf Bottled setzen
    - Edit weiterhin für Korrekturen und Sonderfälle verwenden

&nbsp;

## 🟣 Versionierung

- [ ] BrewBatches über alle Versionen eines Rezepts laden und anzeigen
    - Nicht nur Batches der aktuell ausgewählten BrewRecipeVersion laden
    - Alle Batches des BrewRecipe über sämtliche Versionen darstellen
    - Batches der aktuell ausgewählten RecipeVersion im UI hervorheben

&nbsp;

## 🟠 UI / UX
- [ ] Übersetzungen und Bezeichnungen vereinheitlichen
    - BrewRecipe Index
    - BrewRecipe Create
    - BrewRecipe Details
    - BrewRecipe Edit
    - BrewBatch Index
    - BrewBatch Create
    - BrewBatch Details
    - BrewBatch Edit
    - Enum-Werte und dynamisch erzeugte Texte berücksichtigen
    - Buttons, Tabellenüberschriften, Status- und Hinweistexte prüfen

- [ ] Details und Edit perspektivisch zusammenführen
    - Grundsätzlich immer das kompakte Recipe Sheet anzeigen
    - Werte werden erst beim Anklicken zu Inputs
    - Speichern-Button initial deaktivieren
    - Speichern erst bei tatsächlichen Änderungen aktivieren
    - Für komplexe Listen ggf. komplette Zeile oder Section in Bearbeitungsmodus versetzen
    - Dirty-State und Navigation bei ungespeicherten Änderungen berücksichtigen
    - Validierung, Hinzufügen, Löschen und Sortieren berücksichtigen

- [ ] Responsive Smartphone-Ansicht für Recipe/BrewBatch Sheet
    - Desktop und Tablet bleiben primäres Layout
    - Zwei Sheet-Spalten auf schmalen Displays untereinander darstellen
    - Keine festen Breiten verwenden, die den späteren Umbau verhindern
    - Tabellen bei Bedarf horizontal scrollbar halten

- [ ] Navigation für einspaltiges Smartphone-Layout
    - Anker für Zutaten und Brauprozess vorsehen
    - Kompakte Navigation zum direkten Sprung zwischen den Bereichen
    - Besonders für BrewBatch am Brautag optimieren

&nbsp;

## 🗑️ Soft Delete / Datenpflege

- [ ] Cleanup für soft-gelöschte Daten implementieren
    - Zuerst gelöschte BrewBatches physisch entfernen
    - Danach gelöschte BrewRecipeVersions entfernen, die von keinem BrewBatch mehr referenziert werden
    - Danach gelöschte BrewRecipes ohne verbliebene Version entfernen
    - Vor endgültigem Löschen Referenzen konsequent prüfen
    - Vorerst ausdrücklich Später™

&nbsp;

## 🔌 Externe Integrationen

- [ ] braumischung.de Integration prüfen
    - HOPS-Rezept bzw. BrewBatch an den "Brautomat" übergeben
    - aktuell erfolgt die Übergabe über URL-/Form-Parameter
    - Zuordnung von HOPS-Zutaten zu braumischung.de-Artikelnummern erforderlich
    - Problem: Artikelnummern können sich möglicherweise mit neuen Chargen ändern
    - Betreiber kontaktieren und nach stabiler Produkt-/Sorten-ID bzw. geeigneter Schnittstelle fragen
    - Rezeptverwaltung bewusst nicht mit braumischung.de zusammenführen
    - Keine Benutzerverwaltung nur für diese Integration einführen

&nbsp;

## 🧪 Praxistest

- [ ] Echte Rezepte in HOPS anlegen
    - unterschiedliche Schüttungen
    - mehrere Hopfengaben desselben Hopfens
    - unterschiedliche Alpha-Werte
    - mehrere / einzelne Hefen
    - Wasserzusätze
    - Gärzugaben
    - lange Zutatenbezeichnungen
    - Create → Details → BrewBatch Create → BrewBatch Details → Print durchtesten

- [ ] Druckansicht mit echten Rezepten/BrewBatches prüfen
    - Seitenbreite vollständig nutzen
    - große Tabellen kontrollieren
    - `print-hide` für nicht benötigte UI-Elemente verwenden
    - Kochzeit-Spalte der Hopfengaben bei Bedarf nur im Print ausblenden