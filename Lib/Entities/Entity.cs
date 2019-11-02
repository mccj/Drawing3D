using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
namespace Drawing3d
{
   
    /// <summary>
    /// is a definition of an event for drawing. See e.g <see cref="MeshCreator.CreateMeshes(OpenGlDevice, DrawAction)"/>.
    /// </summary>
    /// <param name="Device">Device in which it will be drawn.</param>
    public delegate void DrawAction(OpenGlDevice Device);
    /// <summary>
    /// the base class for a lot of drawing objects.
    /// </summary>
    [Serializable]
    public class Entity : CustomEntity, ITransform, IDisposable
    {
     
        Texture _Texture = null;
        /// <summary>
        /// gets and sets a <see cref="Texture"/>
        /// </summary>
        [Browsable(false)]
        public Texture Texture
        {
            get { return _Texture; }
            set {
                _HasTexture = true;
                _Texture = value;
                }
        }
        Entity _Pattern = null;
        /// <summary>
        /// is a <see cref="Entity"/> converted to a <see cref="Texture"/>.
        /// The field <see cref="Texture"/> must be null !!
        /// </summary>


        public Entity Pattern
        {
            get { return _Pattern; }
            set
            {
                _Pattern = value;
            }
        }
        /// <summary>
        /// gets and sets whether the entity is snappenable.
        /// </summary>
        public bool SnappEnable = true;
        int _PatternWidth = 1;
        /// <summary>
        /// sets and gets the width of the <see cref="Pattern"/> in world coordinates.
        /// </summary>
       public int PatternWidth
        {
            get { return _PatternWidth; }
            set
            {
                _PatternWidth = value;
            }
        }
        int _PatternHeight = 1;
        /// <summary>
        /// sets and gets the height of the <see cref="Pattern"/> in world coordinates.
        /// </summary>


        public int PatternHeight
        {
            get { return _PatternHeight; }
            set
            {
                _PatternHeight = value;
            }
        }
        int _PatternX = 0;
        /// <summary>
        /// sets and gets the x origin of the <see cref="Pattern"/> in world coordinates.
        /// </summary>


        public int PatternX
        {
            get { return _PatternX; }
            set
            {
                _PatternX = value;
            }
        }
        int _PatternY = 0;
        /// <summary>
        /// sets and gets the y origin of the <see cref="Pattern"/> in world coordinates.
        /// </summary>

        public int PatternY
        {
            get { return _PatternY; }
            set
            {
                _PatternY = value;
             }
        }
        static bool _Compiling = false;
        /// <summary>
        /// sets and gets, wether the <see cref="Entity"/> is in the compile state. See also <see cref="CompileEnable"/>, <see cref="Compile(OpenGlDevice)"/>
        /// </summary>
        public static bool Compiling
        {
            get { return _Compiling; }
            set
            {
                _Compiling = value;
            }
        }
        static Entity _CurrentEntity = null;
        /// <summary>
        /// is the entity, which is yet drawn.
        /// </summary>
        public static Entity CurrentEntity
        {
            get { return _CurrentEntity; }
            set { _CurrentEntity = value; }
        }
       
        /// <summary>
        /// produces a <b>copy</b> of the entity. Every ivolved part must have the dirctive <b>serialized</b>.
        /// </summary>
        /// <returns>the copied entity.</returns>
        public virtual Entity Clone()
        {
           try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                System.IO.MemoryStream stream = new System.IO.MemoryStream(10000);
                formatter.Serialize(stream, this);
                stream.Position = 0;
                Entity result = formatter.Deserialize(stream) as Entity;
                stream.Close();
                return result;
            }
            catch (Exception E)
            {

                System.Windows.Forms.MessageBox.Show(E.Message);
            }
            return null;
        }
       
        bool _Invalid = false;
        bool _Visible = true;
        /// <summary>
        /// gets and sets the visibility. Default is <b>true</b>.
        /// </summary>


        public bool Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                SetInvalid(true);
            }
        }
        /// <summary>
        /// initiate a repainting if you set <b>true</b>.
        /// </summary>
        [Browsable(false)]
        public bool Invalid
        {
            set
            {
                SetInvalid(value);
            }
            get { return _Invalid; }
        }
        /// <summary>
        /// the protected setter of <see cref="Invalid"/>.
        /// </summary>
        /// <param name="value"></param>
        virtual protected void SetInvalid(bool value)
        {
                _Invalid = value; }
        Entity _Parent = null;
        /// <summary>
        /// gets and sets the parent of the object. See also <see cref="Children"/>
        /// </summary>
        [Browsable(false)]
        public Entity Parent
        {
            get { return _Parent; }
            set
            {
                if (value == _Parent) return;
                if (_Parent != null)
                {
                    _Parent.Children.Remove(this);
                }
                if (value != null)
                {
                    value.Children.Add(this);
                }
                _Parent = value;
                SetInvalid(true);
            }
        }
        Texture GetPatternTexture(OpenGlDevice Device)
        {
            Texture Save = Device.texture;
            if (Pattern == null) return null;
            Rectangled R = new Rectangled(Pattern.PatternX, Pattern.PatternY, Pattern.PatternWidth, Pattern.PatternHeight);
            FBO FrameBuffer = new FBO();
            FrameBuffer.BackGround = System.Drawing.Color.Transparent;
            Device.PushMatrix();
            xyz RU = new xyz(R.Width, R.Height, 0);
            xyz RD = new xyz(R.X, R.Y, 0);
            Point _RU = Device.ToScr(RU);
            Point _RD = Device.ToScr(RD);
            FrameBuffer.Init((int)(_RU.X - _RD.X), (int)(_RD.Y - _RU.Y));
            FrameBuffer.EnableWriting();
            Pattern.Paint(Device);
            Device.PopMatrix();
            FrameBuffer.DisableWriting();
            Texture result = FrameBuffer.Texture;
            Device.texture = Save;
            result.WorldWidth = R.Width;
            result.WorldHeight = R.Height;
            result.Translation = new xy(R.X, R.Y);
            FrameBuffer.Dispose(true);
            return result;
        }
        /// <summary>
        /// defines a class for <see cref="Children"/>.
        /// </summary>
        [Serializable]
        public class ChildrenList : ArrayList
        {
            /// <summary>
            /// a constructor.
            /// </summary>
            /// <param name="E">the top of the list.</param>
            public ChildrenList(Entity E)
                : base(5)
            {
                This = E;
            }
            Entity This = null;
            /// <summary>
            /// overrides <b>AddRange</b>.
            /// </summary>
            /// <param name="c"></param>
            public override void AddRange(ICollection c)
            {
                foreach (object E in c)
                {
                    if (!(E is Entity))
                    {
                        throw new Exception("Member must be an entity " + E.GetType().ToString());

                    }
                    (E as Entity)._Parent = This;
                }
                base.AddRange(c);


            }
            /// <summary>
            /// you can free use this tag.
            /// </summary>
            public object Tag;
            /// <summary>
            /// redefines the indexer
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            new public Entity this[int i]
            {
                get
                {
                    return base[i] as Entity;
                }
            }
            /// <summary>
            /// overrides <b>Remove</b>.
            /// </summary>
            /// <param name="obj"></param>
            public new   void Remove(Entity obj)
            {
                (obj as Entity)._Parent = null;
                base.Remove(obj);
            }
            /// <summary>
            /// overrides <b>RemoveAt</b>.
            /// </summary>
            /// <param name="index"></param>
            public override void RemoveAt(int index)
            {
                (this[index] as Entity)._Parent = null;
                base.RemoveAt(index);
            }
            /// <summary>
            /// overrides <b>Add</b>.
            /// </summary>
            /// <param name="value"></param>
            public new int Add(Entity value)
            {

                if (!(value is Entity))
                {
                    throw new Exception("Member must be an entity " + value.GetType().ToString());

                }

                    (value as Entity)._Parent = This;
                return base.Add(value);
            }
            /// <summary>
            /// overrides <b>Insert</b>.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            public new void Insert(int index, Entity value)
            {
                if (!(value is Entity))
                {
                    throw new Exception("Member must be an entity " + value.GetType().ToString());

                }
                base.Insert(index, value);
                (this[index] as Entity)._Parent = This;
            }
            /// <summary>
            /// overrides <b>InsertRange</b>.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="c"></param>
            public override void InsertRange(int index, ICollection c)
            {
                foreach (object E in c)
                {
                    if (!(E is Entity))
                    {
                        throw new Exception("Member must be an entity " + E.GetType().ToString());

                    }
                    (E as Entity)._Parent = This;
                }
                base.InsertRange(index, c);
            }

        }
        /// <summary>
        /// holds all entities, which has <b>this</b> as <see cref="Parent"/>.
        /// </summary>
        public ChildrenList Children = null;
        bool _HasTransformation = false;
        /// <summary>
        /// indicates, that the entity is transformed.(Only a get method.)
        /// </summary>
        [Browsable(false)]
        bool HasTransformation
        {
            get { return _HasTransformation; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="T"></param>
        protected void SetTransFormation(Matrix T)
        { if (!_Transformation.Equals(T)) SetInvalid(true);
            _Transformation = T;
        }
        Matrix _Transformation = Matrix.identity;
        /// <summary>
        /// gets and sets the transformation, who will applied to the object. See also <see cref="Matrix"/>
        /// </summary>
        [Browsable(false)]
        public Matrix Transformation
        {
            set
            {
                if (value.Equals(_Transformation)) return;
                Matrix M = _Transformation.invert() * value;
             
                SetTransFormation(value);
                _HasTransformation = true;
            }
            get { return _Transformation; }
        }
       
       
        bool _HasMaterial = false;

        [Browsable(false)]
        bool HasMaterial
        {
            get { return _HasMaterial; }
        }
        bool _HasTexture = false;

        [Browsable(false)]
        bool HasTexture
        {
            get { return _HasTexture; }
        }
        Material _Material = Materials.Chrome;
        /// <summary>
        /// gets and sets the <see cref="Material"/>. See also <see cref="Materials"/>.
        /// </summary>
        [Browsable(false)]
        public Material Material
        {
            get { return _Material; }
            set
            {
                _Material = value;
                _HasMaterial = true;

            }
        }
        void MaterialAssign(Material value)
        {
            if (!Material.Equals(value))
            {
                _Material.Ambient = value.Ambient;
                _Material.Diffuse = value.Diffuse;
                _Material.Specular = value.Specular;
                _Material.Translucent = value.Translucent;
                _Material.Emission = value.Emission;
                _Material.Shininess = value.Shininess;
                _HasMaterial = true;
            }
        }
        bool _HasOpacity = false;
        [Browsable(false)]
        bool HasOpacity
        {
            get { return _HasOpacity; }
        }
        Double _Opacity = 1;
        /// <summary>
        /// gets and set the opacity (transparency). Values must been between 0 (absolutly transparent) and 1 (no transparency).
        /// Default is 1.
        /// </summary>
        [Browsable(false)]
        public Double Opacity
        {
            get { return _Opacity; }
            set
            {
                _Opacity = value;
                if (value != 1)
                    _HasOpacity = true;
                else
                    _HasOpacity = false;

            }
        }

        //[NonSerialized]
        MeshContainer _Mesh = null;
        /// <summary>
        /// sets the <see cref="Mesh"/> if the entity is compiled.
        /// </summary>
        /// <param name="value">a <see cref="MeshContainer"/>, which holds the compiled informatios about the entity.</param>
        protected virtual void setMesh(MeshContainer value)   // test von Internal gwechselt auf protected virtual
        {
            if (_Mesh != value)
            { if (_Mesh != null) _Mesh.Dispose(); _Mesh = value;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public MeshContainer Mesh
        {
            get { return _Mesh; }

        }
        /// <summary>
        /// virtual method to set <see cref="CompileEnable"/>
        /// </summary>
        /// <param name="value"></param>
        protected virtual void setCompileEnable(bool value)
        {
           _CompileEnable = value;
        }
        bool _CompileEnable = true;
        /// <summary>
        /// to avert <b>compiling</b> you can set this property false. Default is true.
        /// </summary>
        [Browsable(false)]
        public bool CompileEnable
        {
            get { return _CompileEnable; }
            set
            {
                setCompileEnable(value);
               
                setMesh(null);
               // SetInvalid(true);
            }
        }
        /// <summary>
        /// empty constructor.
        /// </summary>
        public Entity():base()
        {
            Children = new ChildrenList(this);
        }
        /// <summary>
        /// is a constructor with the <see cref="Parent"/>.
        /// </summary>
        /// <param name="Parent">the parrent, which holds in the <see cref="Children"/>list <b>this</b> object.</param>
        public Entity(Entity Parent) : this()
        {
            if (Parent != null)
                Parent.Children.Add(this);
        }
        /// <summary>
        /// searches the entity by name. Use the format with delimiters \\. E. g:"Object1\\Object2\\Object3"
        /// </summary>
        /// <param name="N">the searchpath</param>
        /// <returns>gives the object. If not found the result is null.</returns>
        public Entity Find(string N)
        {

            string[] s = N.Split(new char[] { '\\' });
            if ((s.Length > 0) && (s[0] == ""))
            {
                N = N.Substring(1);
                s = N.Split(new char[] { '\\' });

            }
            if ((s.Length == 1) && (s[0] == Name))
            {
                return this;
            }
            else
            {
                if (s[0] == Name)
                {
                    int p = N.IndexOf(Name) + Name.Length + 1;
                    string K = N.Substring(p);
                    for (int i = 0; i < Children.Count; i++)
                    {   Entity Result= Children[i].Find(K);
                        if (Result != null) return Result;
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Internal.
        /// </summary>
        internal static void CheckCompiling()
        {

            if (Entity.Compiling)
                MeshCreator.CheckMeshCreator();


        }
        

    
     
      
       
   
        void internDraw(OpenGlDevice Device)
        {
            Entity Save = CurrentEntity;
            CurrentEntity = this;
            OnDraw(Device);
            CurrentEntity = Save;

        }
        /// <summary>
        /// returns a list of <see cref="Mesh"/>es. You can paint this list to obtain the hole entity.
        /// </summary>
        /// <param name="Device">the <see cref="OpenGlDevice"/> in which the entity will be played.</param>
        /// <returns>a list of <see cref="Mesh"/></returns>
        public List<Mesh> getMeshes(OpenGlDevice Device)
        {   
            MeshContainer M = DoCompile(Device);
            List<Mesh> Result = new List<Drawing3d.Mesh>();
            for (int i = 0; i < M.Progs.Count; i++)
            {
                if (M.Progs[i] is Mesh) Result.Add(M.Progs[i] as Mesh);
            }
            return Result;
            //Device.BeginMeshMode();
            //bool SaveCompileEnable = CompileEnable;
            //CompileEnable = false;
            //setMesh(null);
            //Paint(Device);
            //CompileEnable = SaveCompileEnable;
            //return Device.EndMeshMode();
        }
        /// <summary>
        /// gets or sets the name. See also <see cref="Find(string)"/>
        /// </summary>
        [Browsable(false)]
        public string Name { get; set; }
       
        /// <summary>
        /// calls recursively the <see cref="CustomEntity.OnDraw(OpenGlDevice)"/>method of the members in the <see cref="Children"/>list.
        /// </summary>
        /// <param name="Device"></param>
        public void Paint(OpenGlDevice Device)

        {
             if ((!Visible) && (Device.RenderKind != RenderKind.SnapBuffer)) return;
            double SaveOpacity = Device.Translucent;
            OpenGlDevice.CheckError();
            if (Device.RenderKind == RenderKind.Render)
                if ((Pattern != null) && (Texture == null))
                    Texture = GetPatternTexture(Device);// ActivatePattern(Device);
            OpenGlDevice.CheckError();


            Entity SaveCurrent = CurrentEntity;
 
           
            if (HasTexture)
            {
             
                Device.texture = Texture;
                
            }
            if (HasMaterial)
            {
          
                Device.Material = Material;
             
            }
            if (HasOpacity)
                Device.Translucent = System.Math.Min(Opacity, Device.Translucent);

            CurrentEntity = this;
            if (Device.OutFitChanged) setMesh(null);
            if (Device.RenderKind == RenderKind.Render)
            if (Invalid) Device.invalid = true;
            if (((Mesh == null) || (Invalid)) && (CompileEnable) && (!Compiling) && (Device.RenderKind == RenderKind.Render))
                Compile(Device); // sollte nicht bei refreshSnapBuffer auf gerufen werden

            Invalid = false;
            

            if (Device.Selector.CreateSnap) // kommt vonRefreshMeshBuffer
            {
                if (SnappEnable)
                MeshCreator.CreateMeshes(Device, CompileMeshDraw);  // Entity wird gedrawt im snap items mode 

                return; // eventuell Children interndraw braucht nicht aufgerufen werden!!!
            }
       
            if (HasTransformation)
            {
                Device.PushMatrix();
                Device.MulMatrix(Transformation);

            }
            if ((Mesh != null))// && (Device.RenderKind == RenderKind.Render))
            { Mesh.internDraw(Device); base.OnDraw(Device); }
            else
            {
 
                if (Mesh != null) Mesh.Paint(Device);
                else
                    internDraw(Device);   // bei snap wird im createsnap registriert
            }

            if (Mesh == null)
            {
                for (int i = 0; i < Children.Count; i++)
                    if (Children[i].Visible) Children[i].Paint(Device);
            }
            CurrentEntity = SaveCurrent;
            if (HasTransformation)
                Device.PopMatrix();
            if (HasOpacity)
                Device.Translucent = SaveOpacity;

            if (Entity.Compiling) MeshCreator.CheckMeshCreator();
            OpenGlDevice.CheckError();
           if ( Device.RenderKind == RenderKind.Render)
            Invalid = false;
            
        }

        void CompileDraw(OpenGlDevice Device)
        {

            OnDraw(Device);//  Paint(Device);
            ////if (Draw != null) Draw(this, Device);
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Paint(Device);
            }
        }
        void CompileMeshDraw(OpenGlDevice Device)
        {
            if (Device.RenderKind== RenderKind.SnapBuffer)
            {
                Device.PushMatrix();
                Device.MulMatrix(Transformation);
            }
            OnDraw(Device);
            //if (Draw != null) Draw(this, Device);
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Paint(Device);
            }
            if (Device.RenderKind == RenderKind.SnapBuffer)
            {
                Device.PopMatrix();
                
            }
        }
        /// <summary>
        /// is a <see cref="CustomEntity.DrawEvent"/>, which is called from <see cref="OnForegroundDraw(OpenGlDevice)"/>.
        /// </summary>
        public event DrawEvent ForegroundDraw;
        /// <summary>
        /// is a very useful method. If you have a complicated background and a simple object, before the background you can use
        /// this method by drawing the simple object in the foreground. See also <see cref="OpenGlDevice.ForegroundDrawEnable"/>.
        /// </summary>
        /// <param name="Device">is the used device.</param>
        public virtual void OnForegroundDraw(OpenGlDevice Device)
        {
            if (ForegroundDraw != null) ForegroundDraw(this, Device);
        }

        /// <summary>
        /// gives the <see cref="Box"/>, which is the smallest contining <b>this</b>.
        /// </summary>
         /// <returns>the enclosing box.</returns>
        public virtual Box GetMaxBox()
        {
           Box MaxBox = Box.ResetBox();
            for (int i = 0; i < Children.Count; i++)
            {
                Box B = Children[i].GetMaxBox();
                MaxBox = MaxBox.GetMaxBox(B);
            }

            return MaxBox;
      }
        MeshContainer DoCompile(OpenGlDevice Device)
        {

            Device.PushMatrix();
            Device.ModelMatrix = Matrix.identity;
            if (Mesh != null) Mesh.Dispose();
            MeshContainer M = MeshCreator.Compile(Device, CompileDraw);
            Device.PopMatrix();
            return M;

        }

        /// <summary>
        /// compiliert <b>this</b>. See also <see cref="CompileEnable"/>, <see cref="MeshCreator.CreateMeshes(OpenGlDevice, DrawAction)"/>.
        /// </summary>
        /// <param name="Device"></param>
        virtual public void Compile(OpenGlDevice Device)
        {  // Sollte nur von Entity im paint aufgerufen werden
            if (!CompileEnable) return;
            MeshContainer M = DoCompile(Device);
            setMesh(M);
            //Device.PushMatrix();
            //Device.ModelMatrix = Matrix.identity;
            //if (Mesh != null) Mesh.Dispose();
            //MeshContainer M=   MeshCreator.Compile(Device,CompileDraw);
            //setMesh(M);
            //Device.PopMatrix();
        }
        /// <summary>
        /// transforms with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="T">the transformationmatrix</param>
        public virtual void Transform(Matrix T)
        {
            Transformation = Transformation * T;
        }
        /// <summary>
        /// gives the <see cref="Children"/> free by calling their <b>Dispose</b>method.
        /// </summary>
        public virtual void Dispose()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Dispose();
            }
        }
    }
}