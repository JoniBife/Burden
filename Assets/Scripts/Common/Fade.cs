using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Common
{
    public class Fade : MonoBehaviour
    {
        public void RestartGame()
        {
            SharedInfo.RestartInfo();
            ZoneManager.LoadZone(0);
        }
    }
}