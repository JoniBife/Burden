using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Dialog
{
    [System.Serializable]
    public class Dialog
    {
        public Sprite image;

        public string name;

        [TextArea(3,20)]
        public string sentence;
    }
}