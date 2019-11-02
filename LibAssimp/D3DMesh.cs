using System.Collections.Generic;
namespace Drawing3d
{
    /// <summary>
    /// inherits from <see cref="Mesh"/> and is used from animatable figures. See also <see cref="Scene"/> and <see cref="SceneAnimator"/>
    /// </summary>
    public class D3DMesh:Mesh
    {   
       /// <summary>
       /// contains a list if <see cref="Bone"/>.
       /// </summary>
       public List<Bone> Bones = new List<Bone>();
       /// <summary>
       /// returns <b>true</b> when <see cref="Bones"/> ´has an entry.
       /// </summary>
       public bool HasBones { get { return Bones.Count > 0; } }
    }

}


    
