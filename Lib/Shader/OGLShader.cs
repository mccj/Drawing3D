
using System;
using System.Collections.Generic;

using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Collections;
namespace Drawing3d
{
    /// <summary>
    /// defines an eventhandler. See also <see cref="OGLShader.UpDateAllVars"/>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="UpdateObject"></param>
    public delegate void UpdateEventHandler(Field sender, Object UpdateObject);
    /// <summary>
    /// defines a class, which is associated to each field defined in the <see cref="OGLShader"/>.
    /// </summary>
    [Serializable]
    public class Field
    {
        /// <summary>
        /// the shader wich holds the var.
        /// </summary>
         internal OGLShader Owner;
        /// <summary>
        /// constructor which needs the name of the field.
        /// </summary>
        /// <param name="Name">name of the field.</param>
         public Field(string Name)
        {
            this._Name = Name;
        }
        /// <summary>
        /// is the type if the field is a uniform.
        /// </summary>
           public ActiveUniformType UType = ActiveUniformType.Float;
        /// <summary>
        /// is the type if the field is an attribute.
        /// </summary>
        public ActiveAttribType AType = ActiveAttribType.None;
        /// <summary>
        /// indicates or the field is a uniform or a attribute.
        /// </summary>
        public DefKind Kind = DefKind.None;
        /// <summary>
        /// data, wich are a value for the <see cref="DoUpdate"/> event.
        /// </summary>
        public object UpDateData = null;
        bool _used = true;
        /// <summary>
        /// internal.
        /// </summary>
        internal bool used { get { return _used; } set { _used = value; } }
        int _handle = -1;
        /// <summary>
        /// gets the handle of the field.
        /// </summary>
        public int handle
        {
            get { return _handle; }
            set { _handle = value; }
        }
        string _Name = "";
        /// <summary>
        /// gets the name of the field.
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }
        /// <summary>
        /// this event is called, when <see cref="Update"/> of the var is called or when <see cref="OGLShader.UpDateAllVars"/> is called.
        /// Here you have the possibility to change the value of the field. See also <see cref="SetValue(double)"/>.
        /// F.e: the Name is ModelMatrix you can add
        /// DoUpdate += ModelMatrixField_DoUpdate; where
        /// private void ModelMatrixField_DoUpdate(Field sender, object UpdateObject)<br/>
        ///{<br/>
        ///Matrix M = (Matrix)UpdateObject;<br/>
        ///sender.SetValue(M);<br/>
        ///}<br/>
        /// is.<br/>
        /// The UpdateObject mus be setted to the property ModelMatrix,where<br/>
        ///  Matrix ModelMatrix <br/>
        ///{ <br/>
        ///   get { return _ModelMatrix; } <br/>
        ///   set { _ModelMatrix = value; <br/>
        ///   ModelMatrixField.Update(); <br/>
        /// } <br/>
        /// } <br/>
        /// where ModelMatrixField is <b>this</b> Field.
        /// </summary>
        public event UpdateEventHandler DoUpdate;
        /// <summary>
        /// in this method <see cref="DoUpdate"/> . <see cref="OGLShader.UpDateAllVars"/> calls this method.
        /// </summary>
        public  void Update()
        {   
            if (DoUpdate != null) DoUpdate(this, UpDateData);
        }
       /// <summary>
       /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
       /// </summary>
       /// <param name="value">the new value</param>
       /// <returns>true when the shader is using and the field has a handle >=0.</returns>
        public bool SetValue(System.Drawing.Color value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            float[] C = new float[] { (float)value.R / 255f, (float)value.G / 255f, (float)value.B / 255f, (float)value.A / 255f };
           
            GL.Uniform4(handle, C[0], C[1], C[2], C[3]);
           
            return true;
        }
        /// <summary>
        /// sets a boolean value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(bool value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            if (value)
            GL.Uniform1(handle, 1);
            else
                GL.Uniform1(handle, 0);
            return true;
        }
        /// <summary>
        /// sets the <see cref="xyz"/> value to the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">is  <see cref="xyz"/> value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>
        public bool SetValue(xyz value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            float[] C = new float[] { (float)value.x, (float)value.y, (float)value.z };
            float[] N = new float[2];
            GL.Uniform3(handle, C[0], C[1], C[2]);
            GL.GetUniform(Owner.Handle, handle, N);
            return true;
        }
        /// <summary>
        /// sets the <see cref="xyzwf"/> value to the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">is  <see cref="xyzwf"/> value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>
        public bool SetValue(xyzwf value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            float[] C = new float[] { value.x, value.y, value.z, value.w };
            float[] N = new float[2];
            GL.Uniform4(handle, C[0], C[1], C[2],C[3]);
            GL.GetUniform(Owner.Handle, handle, N);
            return true;
        }
        /// <summary>
        /// sets a float[] value, which must have the length 4 to the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">is value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>
        public bool SetValue(float[] value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            if (value.Length == 4)
            {
                GL.Uniform4(handle, value[0], value[1], value[2], value[3]);
                return true;
            }
            return false;
        }
        /// <summary>
        /// sets a <see cref="Matrix"/> to the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">is matrix value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>
        public bool SetMatrix(Matrix value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            float[] M = value.ToFloat();
           
            GL.UniformMatrix4(handle, 1, false, ref M[0]);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(Drawing3d.Matrix value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            float[] M = value.ToFloat();
        
            GL.UniformMatrix4(handle, 1, false, ref M[0]);
            OpenGlDevice.CheckError();
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(int value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle, value);
           
         
         
           
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(float value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle, value);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(double value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle, value);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <param name="count">the number of values</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>
        public bool SetValue(int count, ref double value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle, count, ref value);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <param name="count">the number of values</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValuent(int count, double[] value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
                GL.Uniform1(handle,count, value);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <param name="count">the number of values</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValuent(int count, ref float value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle, count,ref value);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <param name="count">the number of values</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(int count,float[] value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            
            GL.UseProgram(Owner.Handle);
            float[] N = new float[count];
        int g=    GL.GetUniformLocation(Owner.Handle, Name);
            GL.Uniform1(handle,count, value);
            GL.GetUniform(Owner.Handle, handle, N);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <param name="count">the number of values</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(int count, ref int value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle,count, ref value);
            int[] h = new int[count];
            GL.GetUniform(Owner.Handle, handle, h);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <param name="count">the number of values</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(int count, int[] value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle,count, value);
            int[] h = new int[count];
            GL.GetUniform(Owner.Handle, handle, h);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(ref uint value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle, value);
            return true;
        }
        /// <summary>
        /// sets the value of the field. It works only when the shader is <see cref="OGLShader.Using"/> true is.
        /// </summary>
        /// <param name="value">the new value</param>
        /// <param name="count">the number of values</param>
        /// <returns>true when the shader is using and the field has a handle >=0.</returns>

        public bool SetValue(int count,uint[] value)
        {
            if (!(Owner.Using) || (handle < 0)) return false;
            GL.Uniform1(handle,count, value);
            return true;
        }
       
    }
    /// <summary>
    /// an enum, wich indicates Attribute or Uniform.
    /// </summary>
    public enum DefKind
    {
     /// <summary>
     /// no defined.
     /// </summary>
     None,
     /// <summary>
     /// field is an attribute.
     /// </summary>
     Attribute,
     /// <summary>
     /// field is uniform.
     /// </summary>
     Uniform
    }
   
    /// <summary>
    /// is a shader, wich encapsulate all shader funtions of OpenGl.
    /// </summary>
    public  class OGLShader:IDisposable
    {
        ///// <summary>
        ///// is a constructor, with the strings fagmentshader and vertexshader.
        ///// </summary>
        /////<param name ="FragmentShader" > the fragmentsader</param>
        /////<param name="VertexShader">the vertexsader</param>
        //public OGLShader(string FragmentShader, string VertexShader):this(FragmentShader, VertexShader,null)
        //{  
        //}
        /// <summary>
        /// is aconstructor with with the strings fagmentshader, the vertexshader and UpdateData. This data will be sended
        /// to the vars by a call of <see cref="UpDateAllVars"/>, except a var has setted his <see cref="Field.Update"/> to an other value.
        /// </summary>
        /// <param name="FragmentShader">the fragmentsader</param>
        /// <param name="VertexShader">the vertexsader</param>
        /// <param name="UpdateData">Data. See also <see cref="UpdateData"/></param>
        public OGLShader(string FragmentShader, string VertexShader, object UpdateData):this(FragmentShader, VertexShader, "", "", "", UpdateData)
        {
           
        }
   
        /// <summary>
        /// is the constructor
        /// </summary>
        /// <param name="FragmentShader">the fragmentshader.</param>
        /// <param name="VertexShader">the vertexshader</param>
        /// <param name="GeometricShader">the geometricshader</param>
        /// <param name="TessControlShader">the tessellationcontorlshader</param>
        /// <param name="TessEvaluationShader">the tesselationevaluationshader</param>
        /// <param name="UpdateData">the update data</param>
        public OGLShader(string FragmentShader, string VertexShader, string GeometricShader,string TessControlShader,string TessEvaluationShader, object UpdateData)
        {
            this.FragmentShader = FragmentShader;
            this.VertexShader = VertexShader;
            this.GeometricShader = GeometricShader;
            this.TessControlShader = TessControlShader;
            this.TessEvaluationShader = TessEvaluationShader;
            this.UpdateData = UpdateData;
            CompileAndLink();
        }

        string TessControlShader = "";
        string TessEvaluationShader = "";
        string GeometricShader = "";
      /// <summary>
      /// are the used fields of the shader.
      /// </summary>
      public  Hashtable Vars = new Hashtable();
        bool _using = false; 
        int FSHandle = -1;
        int VSHandle = -1;
        int GSHandle = -1;
        int TCHandle = -1;
        int TEHandle = -1;
        /// <summary>
        /// gets the handle of the shader.
        /// </summary>
        public  int Handle = -1;
        object _UpdateData = null;
        /// <summary>
        /// gets and sets the UpdateData. This value will be sent to  <see cref="Field.UpDateData"/> of all vars.
        /// </summary>
        public object UpdateData
        {
            get { return _UpdateData; }
            set { _UpdateData = value;
                foreach (var item in Vars.Values)
                {
                    (item as Field).UpDateData = value;
                }
               }
        }
        /// <summary>
        /// gets and sets the fragment sahder. if you change this you must call <see cref="CompileAndLink"/>.
        /// </summary>
        public string FragmentShader = "";
        /// <summary>
        /// gets and sets the vertex sahder. if you change this you must call <see cref="CompileAndLink"/>.
        /// </summary>
        public string VertexShader = "";
        void SetAllVars()
        {
            int Ct = 0;
            GL.GetProgram(Handle, GetProgramParameterName.ActiveAttributes, out Ct);
            int size = -1;
            String Name = "";
            int Len = -1;
            ActiveAttribType T = ActiveAttribType.None;
            for (int i = 0; i < Ct; i++)
            {
                
                GL.GetActiveAttrib(Handle, i, 100, out Len, out size, out T,out Name);
                int Pos = Name.IndexOf("[");
                if (Pos >0)
                {
                    Name = Name.Remove(Pos);


                }
                Field v = new Field(Name);
                v.Kind = DefKind.Attribute;
                v.AType=T;
                addVar(v);
            }
            ActiveUniformType U = ActiveUniformType.Float;
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out Ct);
            for (int i = 0; i < Ct; i++)
            { 
                GL.GetActiveUniform(Handle, i, 100, out Len, out size, out U,out  Name);
              
                Field v = new Field(Name);
                v.Kind = DefKind.Uniform;
                v.UType = U;
                   addVar(v);
            }
        }
        /// <summary>
        /// gets the languageversion of the shader.
        /// </summary>
        public string ShadingLanguageVersion
        {
            get { return GL.GetString(StringName.ShadingLanguageVersion); }
        }
        void addVar(Field P)
        {
            P.Owner = this;
            P.handle= GL.GetAttribLocation(Handle, P.Name);
            if (P.handle <0)
            P.handle = GL.GetUniformLocation(Handle, P.Name);
            Vars.Add(P.Name, P);
            if (P.UpDateData==null)
            { P.UpDateData = UpdateData; }
        }
        /// <summary>
        /// a call invoke the <see cref="Field.Update"/> of every var.
        /// </summary>
        public virtual void UpDateAllVars()
        {
            foreach (var item in Vars.Values)
            {   
                (item as Field).Update();
                OpenGlDevice.CheckError();
            }
            
        }  
        /// <summary>
        /// a call provides the associated <see cref="Field"/> of the <b>varName</b>.
        /// if it doesnt exists a null will be returned.
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public Field getvar(string varName)
        {
             var Result = Vars[varName] as Field;
            if (Result == null) return Result;
            return Result;
        }
        private void CompileFragment()
        {
            FSHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FSHandle, FragmentShader);
            GL.CompileShader(FSHandle);
             int[] compiled = new int[1];
            GL.GetShader(FSHandle, ShaderParameter.CompileStatus, compiled);
             
            if (compiled[0] == 0)
            {    
                MessageBox.Show("***" + "FragmentShader" + "***" + GL.GetShaderInfoLog(FSHandle));
                throw new Exception(GL.GetShaderInfoLog(FSHandle)); 
           }   
        }
        private void CompileVertex()
        {  
            
            VSHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VSHandle, VertexShader);
            GL.CompileShader(VSHandle);
            int[] compiled = new int[1];
            GL.GetShader(VSHandle, ShaderParameter.CompileStatus, compiled);

            if (compiled[0] == 0)
            {   
                MessageBox.Show("***" + "VertexShader" + "***" + GL.GetShaderInfoLog(VSHandle));
                throw new Exception(GL.GetShaderInfoLog(VSHandle));
            }  
        }
        private void CompileTessEvaluationShader()
        {
            if (TessEvaluationShader == "") return;
            TEHandle = GL.CreateShader(ShaderType.TessEvaluationShader);
            GL.ShaderSource(TEHandle, TessEvaluationShader);
            GL.CompileShader(TEHandle);
            int[] compiled = new int[1];
            GL.GetShader(TEHandle, ShaderParameter.CompileStatus, compiled);

            if (compiled[0] == 0)
            {
                MessageBox.Show("***" + "TessEvaluationShader" + "***" + GL.GetShaderInfoLog(TEHandle));
                throw new Exception(GL.GetShaderInfoLog(TEHandle));
            }
       }
        private void CompileTessControlShader()
        {
            if (TessControlShader == "") return;
            TCHandle = GL.CreateShader(ShaderType.TessControlShader);
            GL.ShaderSource(TCHandle, TessControlShader);
            GL.CompileShader(TCHandle);
            int[] compiled = new int[1];
            GL.GetShader(TCHandle, ShaderParameter.CompileStatus, compiled);

            if (compiled[0] == 0)
            {
                MessageBox.Show("***" + "TessControlShader" + "***" + GL.GetShaderInfoLog(TCHandle));
                throw new Exception(GL.GetShaderInfoLog(TCHandle));
            }
        }
        private void CompileGeometricShader()
        {
            if (GeometricShader == "") return;
            GSHandle = GL.CreateShader(ShaderType.GeometryShader);
            GL.ShaderSource(GSHandle, GeometricShader);
            GL.CompileShader(GSHandle);
            int[] compiled = new int[1];
            GL.GetShader(GSHandle, ShaderParameter.CompileStatus, compiled);

            if (compiled[0] == 0)
            {
                MessageBox.Show("***" + "GeometricShader" + "***" + GL.GetShaderInfoLog(GSHandle));
                throw new Exception(GL.GetShaderInfoLog(GSHandle));
            }
        }
        /// <summary>
        /// compiles and link the shader.
        /// </summary>
        void CompileAndLink()
        {   if (Handle >= 0) Dispose();
            Handle = GL.CreateProgram();
            Compile();
            Link();
        }
        /// <summary>
        /// must be <b>true</b> when the shader shoud work. Only in this case you can set a value with <see cref="Field.SetValue(double)"/>
        /// </summary>
        public bool Using
        {
            get { return _using; }
            set
            {
                _using = value;
                if (Using)
                {
                    GL.UseProgram(Handle);
                    OpenGlDevice.CheckError();
                    UpDateAllVars();
                    OpenGlDevice.CheckError();
                }
                else
                    GL.UseProgram(0);
            }
               
           
        }
        void Compile()
        {
            CompileFragment();
            CompileVertex();
            CompileGeometricShader();
            CompileTessControlShader();
            CompileTessEvaluationShader();
         }
        void Link()
        {
            try
            {
                GL.AttachShader(Handle, FSHandle);
                GL.AttachShader(Handle, VSHandle);
                if (GSHandle >=0)
                    GL.AttachShader(Handle, GSHandle);
                if (TCHandle >= 0)
                    GL.AttachShader(Handle, TCHandle);
                if (TEHandle >= 0)
                    GL.AttachShader(Handle, TEHandle);
                GL.LinkProgram(Handle);
                GL.DetachShader(Handle, FSHandle);
                if (GSHandle >= 0)
                    GL.DetachShader(Handle, GSHandle);
                GL.DetachShader(Handle, VSHandle);
                if (TCHandle >= 0)
                    GL.DetachShader(Handle, TCHandle);
                if (TEHandle >= 0)
                    GL.DetachShader(Handle, TEHandle);
            }
            catch (Exception)
            {
                Dispose();
                throw new Exception("Size to long");
            }
            int[] linked = new int[1];
            linked[0] = -1;
             GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, linked);
            if (linked[0] != 1)
            {
                Dispose();
                throw new Exception("***LinkError***" + GL.GetProgramInfoLog(Handle));
           }
            Vars.Clear();
            SetAllVars();
        }
        /// <summary>
        /// destructor
        /// </summary>
        ~OGLShader()
        {
           
              
            
           
        }
        /// <summary>
        /// gives free the resources of the fragmentshader, the vertexshader and the prorogam.
        /// </summary>
        public void Dispose()
        {
            try
            {


              
                if (FSHandle > 0)
                    GL.DeleteShader(FSHandle);
                FSHandle = -1;
                if (VSHandle > 0)
                    GL.DeleteShader(VSHandle);
                if (GSHandle > 0)
                    GL.DeleteShader(GSHandle);
                if (TCHandle>0)
                    GL.DeleteShader(TCHandle);
                if (TEHandle>0)
                    GL.DeleteShader(TEHandle);
                VSHandle = -1;
                if (Handle > 0)
                    GL.DeleteProgram(Handle);
                Handle = -1;
            }
            catch (Exception)
            {


            }
        }
   }

}
