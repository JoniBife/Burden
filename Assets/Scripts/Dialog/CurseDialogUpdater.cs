using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;

namespace Assets.Scripts.Dialog
{
    public class CurseDialogUpdater : MonoBehaviour
    {

        public Interactable BeforeCurse;
        public Interactable AfterCurse;

        private void Start()
        {
            AfterCurse.enabled = false;
        }

        void Update()
        {
            if (SharedInfo.CurseStarted)
            {
                BeforeCurse.enabled = false;
                AfterCurse.enabled = true;
            } else
            {
                BeforeCurse.enabled = true;
                AfterCurse.enabled = false;
            }
        }
    }
}