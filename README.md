# Drawing3D

[![Build status](https://ci.appveyor.com/api/projects/status/9qh0190px6w4iacx?svg=true)](https://ci.appveyor.com/project/mccj/drawing3d)
[![NuGet](https://img.shields.io/nuget/v/Drawing3d.svg)](https://www.nuget.org/packages/Drawing3d)

----

## About

### Drawing3D ein 3D-Entwicklertool
#### Schwerpunkt CAD-Entwicklung

    Drawing3d ist eine Bibliothek, die die Programmierung von 3D-Applikationen enorm erleichtert.  
    Die zentrale Komponente ist die Device. Sie setzt auf verschiedenen Schnittstellen Modulen auf.   
    Viele Unternehmen schätzen ein CADSystem, das genau ihren Anforderungen angepasst ist. Bekannte CADSysteme haben einen großen Überhang an Möglichkeiten, die nie verwendet werden. Ein eigenes CAD-System in Auftrag zu geben, scheitert oft an den hohen Entwicklerkosten.  
    Unser Team verwendet Drawing3D seit Jahren für CAD-Entwicklungen und sonstige kleineree und mittelgroße Visualisierungen áuf der Basis von OpenGL. Mit Drawing3D kann preisgünstig entwickelt werden. Wir möchten dieses Tool allen Programmierern zu Verfügung stellen.  
    Das gesamte Tool ist im Sourcecode (C# DotNet) erhältlich!!! Viele Beispiele im Source machen das Erlernen leicht.

    Drawing3D ist unser Hauptprojekt, aus dem sich einige weitere Projekte entwickelten, so z.B.:

    Ein Vektorisierungstool, das aus einem Bitmap eine vektorisierte Grafik erstellt.

    Weiter ein Androidtool, mit dem Apps für Android mit OpenGl erstell werden können

    Wir wünschen dir viel Spaß bei der Verwendung von **Drawing 3d** .
 

## Products

* **Unser neuestes Produkt ist Drawing3d for Android**  

    Drawing3d für Android ist eine Graphikengine, mit deren Hilfe sehr einfach OpenGL ein Androidprogrammen implementiert werden kann. Es ist der komplette c#-Code erhältlich" (Gratis)  
    [More...](https://drawing3d.de/OpenGL%20for%20Android.aspx)

* **Drawing3d for DotNet**  

    Drawing3d ist eine Graphikengine auf der Grundlage von OpenGL Es beinhaltet eine Menge von Komponenten zur Erstellung von Grafikprogrammen. (Gratis)  
    * Einfache Zuordnung eines Windowcontrols zur Graphikengine
    * Automatische Navigation
    * Baumartige Strukturierung der grafischen Elemente (Entities), wobei jeder Knoten eine Transformation enthält,die auf das entsprechende Element ausgeübt wird.
    * Selektieren von grafischen Elementen. Der Selektionsprozess arbeitet nach Prioritäten ( Z-Ordnung vor Punktelement vor Line Element usw. ), die auch vergeben werden können.
    * Sämtliche OpenGLspezifische Funktionen sind in Komponenten zusammengefasst.
    * Editorenkomponenten ermöglichen interaktive Bearbeitung von grafischen Elementen. So z.B. gibt es einen CADEditor2D mit Hilfe mit wenigen Programmzeilen ein GrundCADSystem erstellt werden kann
    * Die mathematische Komponente beinhaltet neben Matrixoperationen, Berechnungen von Kurven (Splines, Nurbs, Beziers ) und Freihandflächen und vieles mehr...
    * Über ein Server Clientsystem werden Clientobjekte z.B über Mausereignisse informiert ( So kann ein "Hovern" leicht implementiert werden.
    * Eine Vielzahl von primitiven Elementen wie Sphere, Surfaces, Splines usw. sind vordefiniert  
    [Download](https://drawing3d.de/Downloads.aspx)
 

* **Vektorisierung (GNU License)**

    * Input ist ein beliebiges Bitmap ( Alle Graphikformate wie *.bmp, *.png, *.tif usw. werden unterstützt
    * Ausgabe ist eine Liste von Bezierkurven, wodurch aus einer Pixelgraphik eine "weiche" Vektorgrafik wird, die skalierbar ist
    * Für die Verwendung dieses Tools ist Drawing3d nicht notwendig
    * Alles ist im Sourcecode erhältlich, nur Systembibliotheken werden verwendet
    [Download](https://drawing3d.de/Downloads.aspx)


# Tutorial

https://drawing3d.de/Tutorial/HelloWorld.aspx


***
**Copyright © Drawing3D inc**   
https://drawing3d.de/Default.aspx