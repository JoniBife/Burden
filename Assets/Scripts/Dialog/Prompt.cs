using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Assets.Scripts.Dialog
{
    public class Prompt : MonoBehaviour
    {
        [SerializeField]
        private float _triggerPromptRadius;

        [SerializeField]
        private string _text;

        private GameObject _player;
        public Text _UIText;

        // Use this for initialization
        void Start() 
        {
            _player = GameObject.Find("Player");
            if (_player == null)
            {
                throw new ArgumentNullException("Player could not be found in the scene");
            }

            /*_UIText = GameObject.Find("PromptText").GetComponent<Text>();
            if (_UIText)
            {
                throw new ArgumentNullException("PromptText could not be found in the scene");
            }*/

            _UIText.gameObject.SetActive(false);
            
        }

        // Update is called once per frame
        void Update()
        {
            float distToPlayer = (transform.position - _player.transform.position).magnitude;

            if (distToPlayer < _triggerPromptRadius)
            {
                _UIText.gameObject.SetActive(true);
                _UIText.text = _text;
            } else
            {
                _UIText.gameObject.SetActive(false);
            }
        }
    }
}