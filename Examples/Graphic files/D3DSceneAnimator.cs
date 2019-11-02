using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drawing3d;

namespace Sample
{
    public class D3DSceneAnimator : OglAnimator
    {
        Scene Scene = null;
        public override void Start()
        {
            if (Scene.HasAnimations)
            {
                Scene.SceneAnimator.AnimationPlaybackSpeed = 1;
                Scene.SceneAnimator.ActiveAnimation = 0;
                Scene.SceneAnimator.AnimationCursor = 0;
                
                base.Start();
            }
        }
        public void Start(int AnimationId)
        {
            if (Scene.HasAnimations)
            {
                Scene.SceneAnimator.ActiveAnimation = -1;
                Scene.SceneAnimator.AnimationPlaybackSpeed = 1;
                Scene.SceneAnimator.ActiveAnimation = AnimationId;
                Scene.SceneAnimator.AnimationCursor = 0;
               


                base.Start();
            }
        }
        public override void End()
        {
            if (Scene.HasAnimations)
            {
                Scene.SceneAnimator.AnimationPlaybackSpeed = 0;

               
            }
            base.End();
        }
        public void Continue()
        {
            if (Scene.HasAnimations)
            {
                Scene.SceneAnimator.Loop = true;
                Scene.SceneAnimator.AnimationPlaybackSpeed = 1;
                int Save = Scene.SceneAnimator.ActiveAnimation;
                Scene.SceneAnimator.ActiveAnimation = -1;
                Scene.SceneAnimator.ActiveAnimation = Save;
               


                base.Start();
            }
        }

        public override void OnAnimate()
        {

            Scene.SceneAnimator.Update((float)(CurrentTime) / 1000f);
            Scene.SkinninEvaluator.Update();
            ResetTime();

            base.OnAnimate();

        }
        public D3DSceneAnimator(OpenGlDevice Device, Scene Scene)
            : base(Device)
        {
            this.Scene = Scene;
            Duration = -1;
        }
    }
}
