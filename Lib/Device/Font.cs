using System;
using System.Collections.Generic;
using System.Drawing;
using Drawing3d.Windows;

namespace Drawing3d
{


    public partial class OpenGlDevice
    {

        /// <summary>
        /// draws a text, with a font and a n extrusion. It is transformed by Transormation.
        /// </summary>
        /// <param name="F">the font, which is used.</param>
        /// <param name="Transformation">the transformation</param>
        /// <param name="s">the text, which will be drawn</param>
        /// <param name="Extrusion">the extrusion.</param>
        public void drawText(Font F, Matrix Transformation, string s, double Extrusion)
        {

            PushMatrix();
           
            MulMatrix(Transformation);
            if (F.FontStyle == FontStyle.Italic)
            {
                Matrix Italic = Matrix.identity;
                Italic.a01 = 0.3f;
                MulMatrix(Italic);
            }
           
            drawText(F, s, Extrusion);
         
            PopMatrix();
        
          
        }
        /// <summary>
        /// draws a text, with a font and a n extrusion at a position.
        /// </summary>
        /// <param name="F">the font, which is used.</param>
        /// <param name=" Position">the position where the text will be drawn.</param>
        /// <param name="s">the text, which will be drawn</param>
        /// <param name="Extrusion">the extrusion.</param>
        public void drawText(Font F, xyz Position, string s, double Extrusion)
        {
            drawText(F, Matrix.Translation(Position), s, Extrusion);
        }
        /// <summary>
        /// gets the minimal rectangle, which includes the text s.
        /// </summary>
        /// <param name="F">font</param>
        /// <param name="s">text</param>
        /// <returns>the minimal rectangle.</returns>
        public xy getEnvText(Font F, string s)
        {
            return F.getEnvText(s);
        }
        /// <summary>
        /// draws a text, with a font and a n extrusion. It will be draw at the current position..
        /// </summary>
        /// <param name="F">the font, which is used.</param>
        /// <param name="s">the text, which will be drawn</param>
        /// <param name="Extrusion">the extrusion.</param>
        public void drawText(Font F, string s, double Extrusion)
        {
            PushMatrix();
            Material Save = Material;
                MulMatrix(Matrix.Scale(F.FontSize, F.FontSize, Extrusion));
            {
                xyz Start = ModelMatrix * new xyz(0, 0, 0);
             
                for (int i = 0; i < s.Length; i++)
                {
                    MeshContainer M = MeshCreator.MeshListCurrent;
                    byte b = (byte)s[i];
                    drawLetter(F, b, s,Start, i, this);
                    Material = Save;
                    M = MeshCreator.MeshListCurrent;
                    MulMatrix(Matrix.Translation(new xyz(F.GlyphInfo[b].Deltax, 0, 0)));
                }


            }
            PopMatrix();
        }

        private Byte CompileChar = 255;
        private Font CompileFont = null;
        private void Compiledraw(OpenGlDevice Device)
        {
          
            RenderKind Save = Device.RenderKind;
            Device.RenderKind = RenderKind.Render;
            PushMatrix();
            ModelMatrix = Matrix.identity;
            Loca L = CompileFont.GlyphInfo[CompileChar].Curves;
            if (L.Count == 0) // blank
            {
                PopMatrix();
                return;
            }
            PolygonMode SavePolygonMode = Device.PolygonMode;
            Device.PolygonMode = PolygonMode.Fill;
            bool Cw = L[0].ClockWise;
            if (Cw)
            {
                drawExtruded(L, 1);
                PushMatrix();
                MulMatrix(Matrix.Translation(new xyz(0, 0, 1)));
                drawPolyPolyCurve(L);
                PopMatrix();
                L.Invert();
                drawPolyPolyCurve(L);


            }
            else
            {
                drawPolyPolyCurve(L);
                L.Invert();
                drawExtruded(L, 1);

                PushMatrix();
                MulMatrix(Matrix.Translation(new xyz(0, 0, 1)));
                drawPolyPolyCurve(L);
                PopMatrix();
           }

            PopMatrix();
            Device.RenderKind = Save;
            Device.PolygonMode = SavePolygonMode;
        }



        private void drawLetter(Font F, byte b, String s,xyz Start, int iD, OpenGlDevice Device)
        {
            
            if (F.Compiled[b] == null)
            {
                CompileChar = b;
                CompileFont = F;
                MeshCreator.Push();
                bool SaveRegSnap = Selector.RegisterSnap;
                Selector.RegisterSnap = false;
                F.Compiled[b] = MeshCreator.Compile(this, Compiledraw);
                Selector.RegisterSnap = SaveRegSnap;
                MeshCreator.Pop();

            }

            if (F.Compiled[b] == null) throw new Exception("Compile error of " + (char)(b));
            
              if (RenderKind == RenderKind.SnapBuffer)
            {
                Object Handle = null;
                if ((RenderKind == RenderKind.SnapBuffer))
                {
                    Handle = Selector.RegisterSnapItem(new TextSnappItem(F, s, iD, Start, ModelMatrix * new xyz(0, 0, 0)));
                    if (F.GlyphInfo[b].BlackBoxY<0.1) // blank
                        drawBox(new xyz(0, 0, 0), new xyz(F.GlyphInfo[b].Deltax, 1, 1));
                    else
                    drawBox(new xyz(0, 0, 0), new xyz(F.GlyphInfo[b].Deltax, F.GlyphInfo[b].BlackBoxY, 1));
                    if ((RenderKind == RenderKind.SnapBuffer))
                        Selector.UnRegisterSnapItem(Handle);
                }
              
                return;
            }
           if (Entity.Compiling)
            {
                MeshContainer M = F.Compiled[b] as MeshContainer;
                for (int i = 0; i < M.Progs.Count; i++)
                {
                    Mesh E = (M.Progs[i] as Mesh).Clone() as Mesh;
                    E.Transform(ModelMatrix);
                    MeshCreator.MeshListCurrent.Progs.Add(E);
                }
               
                return;
            }
       
          F.Compiled[b].Paint(this);
        }
    }

        #region GLYPHMETRICSFLOAT Struct
        /// <summary>
        /// The <b>GLYPHMETRICSFLOAT</b> structure contains information about the placement and orientation of a glyph in a
        /// character cell.
        /// </summary>
        /// <remarks>The values of <b>GLYPHMETRICSFLOAT</b> are specified as notional units.</remarks>
        /// <seealso cref="PointF" />

        public struct GLYPHMETRICSFLOAT
        {
            /// <summary>
            /// Specifies the width of the smallest rectangle (the glyph's black box) that completely encloses the glyph.
            /// </summary>
            public float gmfBlackBoxX;

            /// <summary>
            /// Specifies the height of the smallest rectangle (the glyph's black box) that completely encloses the glyph.
            /// </summary>
            public float gmfBlackBoxY;

            /// <summary>
            /// Specifies the x and y coordinates of the upper-left corner of the smallest rectangle that completely encloses the glyph.
            /// </summary>
            public PointF gmfptGlyphOrigin;

            /// <summary>
            /// Specifies the horizontal distance from the origin of the current character cell to the origin of the next character cell.
            /// </summary>
            public float gmfCellIncX;

            /// <summary>
            /// Specifies the vertical distance from the origin of the current character cell to the origin of the next character cell.
            /// </summary>
            public float gmfCellIncY;
        };
        #endregion GLYPHMETRICSFLOAT Struct

        /// <summary>
        /// contains infos of a letter.
        /// </summary>
        [Serializable]
        public class GlyphInfo
        {
            /// <summary>
            /// describes the outline of the lette. See <see cref="Loca"/>
            /// </summary>
            public Loca Curves;
            /// <summary>
            /// the width of the letter
            /// </summary>
            public double Deltax;
            /// <summary>
            /// the height of the letter
            /// </summary>
            public double BlackBoxY;
            /// <summary>
            /// an empty constructor.
            /// </summary>
            public GlyphInfo()
            { }
        }

        /// <summary>
        /// Describes a font. See also <seealso cref="Font.getInstalledFonts"/>
        /// </summary>
        [Serializable]
        public class Font
        {
            /// <summary>
            /// list of installed fontnames.
            /// </summary>
            /// <returns></returns>
            public static List<string> getInstalledFonts()
            {
                System.Drawing.Text.InstalledFontCollection F = new System.Drawing.Text.InstalledFontCollection();
                List<string> Result = new List<string>();
                for (int i = 0; i < F.Families.Length; i++)
                {
                    Result.Add(F.Families[i].Name);
                }
                return Result;
            }
            /// <summary>
            /// gets and sets the size of the font.
            /// </summary>
            public double FontSize = 1;
            /// <summary>
            /// Contains geometric infos about chars.
            /// </summary>
            public GlyphInfo[] GlyphInfo = new GlyphInfo[256];
            /// <summary>
            /// contains the compiled chars.
            /// </summary>
            public Entity[] Compiled = new Entity[256];
            /// <summary>
            /// Contains metric infos about the font.
            /// </summary>
            public float Descent = 0;
            /// <summary>
            /// an array, whose is idexed by the char id.See also <see cref="GLYPHMETRICSFLOAT"/>
            /// </summary>
            private GLYPHMETRICSFLOAT[] agmf = new GLYPHMETRICSFLOAT[256];
            /// <summary>
            /// The FontStyle
            /// </summary>
            public FontStyle FontStyle = FontStyle.Regular;
            String _FontName = "";
            /// <summary>
            /// gets and sets the name of the font.
            /// </summary>
            public String FontName
            {
                get { return _FontName; }
                set
                {
                    if (value == FontName) return;
                    _FontName = value;
                    Load(FontName, this.FontStyle);


                }
            }
            /// <summary>
            /// If the font type is modern this field has value true.
            /// </summary>
            bool IsModernType = false;
            /// <summary>
            /// a constructor with the font name.
            /// </summary>
            /// <param name="FontName">name of the font.</param>
            public Font(string FontName)
            {
                this.FontStyle = System.Drawing.FontStyle.Regular;
                this.FontName = FontName;

            }

            /// <summary>
            /// gets the minimal rectangle, which includes the text s.
            /// </summary>
            /// <param name="s">text</param>
            /// <returns>the right position as x value and the height as y value .</returns>
            public xy getEnvText(string s)
            {
                xy Result = new xy(0, 1);
                for (int i = 0; i < s.Length; i++)
                {
                    byte b = (byte)s[i];
                    Result.x += GlyphInfo[b].Deltax * FontSize;

                }
                Result.y = FontSize;
                return Result;
            }
            /// <summary>
            /// a dictionary for the <see cref="GlyphInfo"/>
            /// </summary>
            private static Dictionary<string, GlyphInfo[]> Glyphs = new Dictionary<string, Drawing3d.GlyphInfo[]>();
            private void Load(string FontName, FontStyle FS)
            {
                try
                {
                    GlyphInfo[] GI = Glyphs[FontName];
                    if (GI != null)
                    {
                        GlyphInfo = GI;
                        return;
                    }
                }
                catch (Exception)
                {


                }

                System.Drawing.Font FF = null;
                try
                {
                    FF = new System.Drawing.Font(FontName, 400, FS);

                }
                catch (Exception)
                {
                    IsModernType = true;

                }
                if (FF != null)
                    FF.Dispose();
                Bitmap B = new Bitmap(500, 500);
                Graphics G = Graphics.FromImage(B);
                IntPtr F = IntPtr.Zero;
                this.FontName = FontName;
                this.FontStyle = FS;
                //this.IsModernType = ModernFont;
                if (IsModernType)
                    F = FeaturesW32.CreateFont(80, 0, 0, 0, 0, false, false, false, 255, 0, 0, 0, 16, FontName);
                else
                    F = FeaturesW32.CreateFont(400, 0, 0, 0, 600, false, false, false, 0, 0, 0, 0, 2, FontName);

                IntPtr hdc = G.GetHdc();
                FeaturesW32.SelectObject(hdc, F);
            

                FeaturesW32.GLYPHMETRICS Metrics;
                FeaturesW32.TEXTMETRIC lpMetrics = new FeaturesW32.TEXTMETRIC();
                FeaturesW32.GetTextMetrics(hdc, ref lpMetrics);

                float Scale = (float)lpMetrics.tmHeight;
                Descent = (float)lpMetrics.tmDescent / Scale;
                double ModernyTrans = (float)lpMetrics.tmDescent / Scale;

                for (int i = 0; i < 256; i++)
                {
                    try
                    {

                        GlyphInfo[i] = new GlyphInfo();
                        if (!IsModernType)
                            GlyphInfo[i].Curves = FeaturesW32.GetOutLine(hdc, Scale, (Char)i, out Metrics);
                        else
                            GlyphInfo[i].Curves = FeaturesW32.GetModernOutLine(hdc, Scale, ModernyTrans, (Char)i, out Metrics);
                        if ((GlyphInfo[i].Curves.Count > 0) && (GlyphInfo[i].Curves[0].CrossProduct() >= 0))
                            GlyphInfo[i].Curves.Invert();


                        GlyphInfo[i].Deltax = (float)(Metrics.gmCellIncX) / (float)(lpMetrics.tmHeight);

                        agmf[i].gmfBlackBoxX = (float)Metrics.gmBlackBoxX / (float)(lpMetrics.tmHeight);
                        agmf[i].gmfBlackBoxY = (float)Metrics.gmBlackBoxY / (float)(lpMetrics.tmHeight);
                        GlyphInfo[i].BlackBoxY = agmf[i].gmfBlackBoxY;
                        agmf[i].gmfCellIncX = (float)Metrics.gmCellIncX / (float)(lpMetrics.tmHeight);
                        agmf[i].gmfCellIncY = (float)Metrics.gmCellIncY / (float)(lpMetrics.tmHeight);
                        agmf[i].gmfptGlyphOrigin.X = (float)Metrics.gmptGlyphOrigin.x.Fract / (float)(lpMetrics.tmHeight);
                        agmf[i].gmfptGlyphOrigin.Y = (float)Metrics.gmptGlyphOrigin.y.Fract / (float)(lpMetrics.tmHeight);

                    }
                    catch
                    {


                    }
                }
                G.ReleaseHdc(hdc);
                Glyphs.Add(FontName, GlyphInfo);
            }


        }
    }

