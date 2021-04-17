using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Common
{

    /**
     * Should be implemented by any script that
     * takes in user input, so that while there is
     * an interaction the user cannot for example
     * move the character.
     * 
     * In most cases Pause() just disables the script
     * and Resume() enables it back.
     */
    public interface IPausable 
    {
        void Pause();

        void Resume();
    }
}