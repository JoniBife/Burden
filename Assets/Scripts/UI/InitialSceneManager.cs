using UnityEngine;
using System.Collections;
using Assets.Scripts.MainCharacter;
using Assets.Scripts.Common;

namespace Assets.Scripts.UI
{
    public class InitialSceneManager : MonoBehaviour
    {
        private GameObject _fadeOut;
        private GameObject _promptText;
        private PlayerController _playerController;
        private ItemsController _itemsController;
        private VideoManager _videoManager;

        // Use this for initialization
        void Start()
        {
            _itemsController = GameObject.Find("Player").GetComponent<ItemsController>();
            _videoManager = GameObject.Find("VideoPlayer").GetComponent<VideoManager>();
            _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            _promptText = GameObject.Find("Interact text");
            _fadeOut = GameObject.Find("FadeOut");

            if (SharedInfo.InitialScenePlayed)
            {
                _promptText.SetActive(false);
            } else
            {
                _fadeOut.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!SharedInfo.InitialScenePlayed)
            {
                _playerController.PausePlayer();
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    _promptText.SetActive(false);
                    _videoManager.PlayVideo(()=> {
                        _fadeOut.SetActive(true);
                        _playerController.ResumePlayer();
                        _itemsController.ActivateTorch();
                        SharedInfo.InitialScenePlayed = true;
                    });
                }
            }
        }
    }
}