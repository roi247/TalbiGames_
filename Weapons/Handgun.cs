using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace MultiplayerFps
{
    public class Handgun : RaycastFirearm
    {
        [SerializeField]
        Animator handgunAnimator;
        public override void PlayShootingAudio()
        {
            base.PlayShootingAudio();
        }

        public override void MakeLocalShootEffects()
        {
            base.MakeShootEffects();
            handgunAnimator.Play("Fire");
        }
    }
}


