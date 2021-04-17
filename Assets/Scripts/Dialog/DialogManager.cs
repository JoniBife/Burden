using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine.EventSystems;
using Assets.Scripts.MainCharacter;
using System.Collections;

namespace Assets.Scripts.Dialog
{
    public class DialogManager : MonoBehaviour
    {
        public Text _name; // Using _ because otherwise it hides the member from super
        public Text sentence;
        public Image image;
        public GameObject dialogBox;
        public bool PlayingDialog { get; private set; }

        private bool _singleInteraction;

        private Queue<Dialog> _dialogQueue = new Queue<Dialog>();

        private GameEvent _dialogEndEvent;

        [SerializeField]
        private KeyCode _nextKey;

        [SerializeField]
        private KeyCode _closeKey;

        // Player Controllers
        private List<IPausable> _pausables = new List<IPausable>();
        private IPausable _interactable;

        // Since the start dialog and advance dialog keys are the same we have to skip the first frame because otherwise it detects the key press and skips a dialog
        private bool _skipFrame = false;

        private void Start()
        {
            _pausables.Add(FindObjectOfType<MovementController2D>());
            _pausables.Add(FindObjectOfType<AttackController>());
            _pausables.Add(FindObjectOfType<SanityManager>());
            _pausables.Add(FindObjectOfType<ItemsController>());
        }

        private void Update()
        {
            if (!PauseMenuController.gameIsPaused)
            {
                if (dialogBox.activeSelf && !_skipFrame)
                {
                    if (Input.GetKeyDown(_nextKey))
                    {
                        DisplayNextSentence();
                    }
                    else if (Input.GetKeyDown(_closeKey) && !_singleInteraction)
                    {
                        CloseDialogBox();
                    }
                }

                if (_skipFrame)
                    _skipFrame = false;
            }
        }

        public void PlayDialog(Interactable interactable, GameEvent onDialogEnd, bool singleInteraction)
        {
            PlayingDialog = true;

            _singleInteraction = singleInteraction;

            _dialogEndEvent = onDialogEnd;

            _interactable = interactable;

            _skipFrame = true;

            DisablePlayerControlls();

            foreach (Dialog d in interactable.dialog)
            {
                _dialogQueue.Enqueue(d);
            }

            DisplayNextSentence(); // First update dialog name and text
            DisplayDialogBox(); // Then show dialog box
        }

        private void DisplayDialogBox()
        {
            dialogBox.SetActive(true);
        }

        public void CloseDialogBox()
        {
            _dialogQueue.Clear();
            dialogBox.SetActive(false);
            EnablePlayerControlls();
            StartCoroutine(EnableDialogAfterDelay());
        }

        public void DisplayNextSentence()
        {

            if (_dialogQueue.Count == 0)
            {
                // Once the dialogue is over we reset single interaction
                _singleInteraction = false;

                CloseDialogBox();

                if (_dialogEndEvent != null)
                    _dialogEndEvent.Raise();
                
                return;
            }

            Dialog dialog = _dialogQueue.Dequeue();

            _name.text = dialog.name;
            sentence.text = dialog.sentence;
            image.sprite = dialog.image;
        }

        private void DisablePlayerControlls()
        {
            foreach(IPausable pausable in _pausables)
            {
                pausable.Pause();
            }
            _interactable.Pause();
        }

        private void EnablePlayerControlls()
        {
            foreach (IPausable pausable in _pausables)
            {
                pausable.Resume();
            }
            _interactable.Resume();
        }

        private IEnumerator EnableDialogAfterDelay()
        {
            yield return new WaitForSeconds(1.0f);
            PlayingDialog = false;
        }

    }
}