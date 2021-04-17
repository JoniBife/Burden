using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Common
{
    public class ZoneManager 
    {
        private static int _currentScene = 0;

        public static void LoadNextZone()
        {
            SceneManager.LoadScene(++_currentScene);
        }

        public static void LoadPreviousZone()
        {
            SceneManager.LoadScene(--_currentScene);
        }

        public static void LoadZone(int zoneId)
        {
            SceneManager.LoadScene(zoneId);
        }
        
    }
}