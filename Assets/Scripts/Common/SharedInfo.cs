using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Common
{
    public static class SharedInfo
    {
        public static float Sanity = 100;
        public static bool KujengaBossDefeated = false;
        public static bool MchawiBossDefeated = false;
        public static bool CurseStarted = false;
        public static Vector3 PlayerStartingPosition = Vector3.zero;
        public static Vector3 CameraStartingPosition = Vector3.zero;
        public static int CurrentScene = 0;
        public static int PreviousScene = 0;
        public static bool InitialScenePlayed = false;
        public static bool LostInstrument = false;
        public static bool LostTorch = false;

        public static void RestartInfo()
        {
            InitialScenePlayed = false;
            CurrentScene = 0;
            PreviousScene = 0;
            Sanity = 0;
            KujengaBossDefeated = false;
            MchawiBossDefeated = false;
            LostInstrument = false;
            LostTorch = false;
            CurseStarted = false;
            PlayerStartingPosition = Vector3.zero;
            CameraStartingPosition = Vector3.zero;
        }
    }
}