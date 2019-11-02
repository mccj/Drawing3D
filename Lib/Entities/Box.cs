using System;
namespace Drawing3d
{   
    /// <summary>
    /// is an <see cref="Entity"/>, who draws a box.
    /// </summary>
    [Serializable]
    public class BoxEntity : Entity
    {
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public BoxEntity() : base()
        {
        }
        /// <summary>
        /// is a constructor with <b>origin</b> and <b>size</b>.
        /// </summary>
        /// <param name="Origin">the origin of the box.</param>
        /// <param name="Size">the size of the box.</param>
        public BoxEntity(xyz Origin,xyz Size):base()
        {
            this.Origin = Origin;
            this.Size = Size;
        }
        xyz _Origin = new xyz(0, 0, 0);
        /// <summary>
        /// is the origin of the box. The default is 0,0,0.
        /// </summary>
        public xyz Origin
        {
            get { return _Origin; }
            set
            {
                _Origin = value;
            }
        }
        xyz _Size = new xyz(1, 1, 1);
        /// <summary>
        /// is the size of the box. The default is 1,1,1.
        /// </summary>
        public xyz Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
            }
        }
        /// <summary>
        /// overrides <see cref="CustomEntity.OnDraw(OpenGlDevice)"/>.
        /// </summary>
        /// <param name="Device"><see cref="OpenGlDevice"/> in which the box will be drawn.</param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            Device.drawBox(Origin,Size);
            base.OnDraw(Device);
        }
    }
}
