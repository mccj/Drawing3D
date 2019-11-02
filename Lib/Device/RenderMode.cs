namespace Drawing3d
{
    public partial class OpenGlDevice

    {
        /// <summary>
        /// the mode for the refreshing the scene by the <see cref="Timer"/>. See also <see cref="Intervall"/>.
        /// </summary>
        public enum Mode
        {
        /// <summary>
        /// the timer refrehed the scene in <see cref="Intervall"/> milli seconds.
        /// </summary>
        Allways,
        /// <summary>
        /// the scene will be refrehed, when a mouse event happens.
        /// </summary>
        WhenMouseEvent,
        /// <summary>
        /// the timer does'nt refresh.
        /// Remark: the <see cref="Navigating"/> is on. See also <see cref="NavigationKind"/>.
        /// </summary>
        WhenNeeded

        }
        Mode _RenderMode = Mode.Allways;
        private void SetRenderMode(Mode Value)
        { 
            if (Value == RefreshMode) return;
        
            UnSetTimer();
            if (Value == Mode.Allways) SetTimer();
            _RenderMode = Value;
        }
        /// <summary>
        /// gets and sets the <see cref="Mode"/> for the refrehing.
        /// </summary>
        public Mode RefreshMode
        { get { return _RenderMode; }
        set { SetRenderMode(value); }
        }
    }
}
