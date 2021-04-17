using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Common
{
    public class AnimatorParameter
    {
        // MainCharacter
        public static readonly string RUNNING = "Running";
        public static readonly string RUNNING_SPEED = "RunningSpeed";
        public static readonly string SLASH = "Slash";
        public static readonly string SLASH_SPEED = "SlashSpeed";
        public static readonly string PLAY_INSTRUMENT = "Instrument";
        public static readonly string DIG = "Dig";
        public static readonly string JUMP = "Jump";
        public static readonly string FALLING = "Falling";
        public static readonly string TORCH = "Torch";
        public static readonly string ON_GROUND = "OnGround";
        public static readonly string ON_WALL = "OnWall";

        // Transitions
        public static readonly string FADE_IN = "FadeIn";

        // Kujenga Boss
        public static readonly string LADDER_SMASH = "LadderSmash";
        public static readonly string SLAM = "Slam";

        // Mchawi Boss
        public static readonly string PREPARE_EXPLOSION = "PrepareExplosion";

        // Environment
        public static readonly string PORTAL_OPEN = "PortalOpen";
    }
}