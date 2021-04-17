using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InvisibleWallBehaviour : MonoBehaviour
    {
        void Start()
        {
            if (SharedInfo.CurseStarted)
                GetComponent<BoxCollider2D>().isTrigger = false;
            else
                GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}