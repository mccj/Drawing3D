namespace Drawing3d
{
    public partial class OpenGlDevice
    {
        Matrix _ModelMatrix = Matrix.identity;
        /// <summary>
        /// GetMethod of the <see cref="ModelMatrix"/>.
        /// </summary>
        /// <returns></returns>
        private Matrix getModelMatrix()
        {
            return _ModelMatrix;
        }
        /// <summary>
        /// SetMethod of the <see cref="ProjectionMatrix"/>.
        /// </summary>
       private void setModelMatrix(Matrix value)
        {
           _ModelMatrix = value;
            if ((Shader != null) && (Shader.Using))
            {
                Field A = Shader.getvar("ModelMatrix");
                if (A != null) A.Update();
          
            }
       }
        /// <summary>
        /// gets and sets the model matrix. She  controls the behavior of the objects, drawn in the scene. See also <see cref="PushMatrix"/> and <see cref="PopMatrix"/>.
        /// </summary>
        public Matrix ModelMatrix
        {
            get
            {
                return getModelMatrix();
            }
            set
            { setModelMatrix(value); }
        }
        private System.Collections.Stack S = new System.Collections.Stack();
        /// <summary>
        /// Pushes the <see cref="ModelMatrix"/> in a stack. See also <see cref="PopMatrix"/>.
        /// A push must allways used with a popMatrix.
        /// </summary>
        public void PushMatrix()
        {
            S.Push(ModelMatrix);
        }
        /// <summary>
        /// Popes the <see cref="ModelMatrix"/> in a stack. See also <see cref="PushMatrix"/>.
       /// </summary>
        public void PopMatrix()
        {
            ModelMatrix = (Matrix)S.Pop();
        }
        /// <summary>
        /// Multiply the <see cref="ModelMatrix"/> with a Matrix.
        /// </summary>
        /// <param name="M">matrix which multiplys the modelmatrix.</param>
        public void MulMatrix(Matrix M)
        {
            ModelMatrix = ModelMatrix * M;
           
        }
     }
}
