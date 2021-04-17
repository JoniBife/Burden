using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Common
{
    public class ZoneAdvancer : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider2D _boxCollider2D;

        [SerializeField]
        private int _zoneId;

        [SerializeField]
        private Vector3 _playerStartingPosition;

        [SerializeField]
        private Vector3 _cameraStartingPosition;

        private bool _canAdvance = true;

        [SerializeField]
        private bool _ignoreFirst = false;
        [SerializeField]
        private int _ignoreFirstFromScene = -1;

        [SerializeField]
        private bool _turnToColliderOnCurseEvent;
        [SerializeField]
        private bool _removeOnCurseEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_ignoreFirst && (_ignoreFirstFromScene == -1 || _ignoreFirstFromScene == SharedInfo.PreviousScene))
            {
                _ignoreFirst = false;
            }
            else
            {
                if (_canAdvance && collision.CompareTag("Player"))
                {
                    SharedInfo.PreviousScene = SharedInfo.CurrentScene;
                    SharedInfo.CurrentScene = _zoneId;
                    SharedInfo.PlayerStartingPosition = _playerStartingPosition;
                    SharedInfo.CameraStartingPosition = _cameraStartingPosition;
                    ZoneManager.LoadZone(_zoneId);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                transform.position + new Vector3(_boxCollider2D.offset.x, _boxCollider2D.offset.y),
                new Vector3(_boxCollider2D.bounds.extents.x * 2,
                _boxCollider2D.bounds.extents.y * 2, 1));
        }

        private void Start()
        {
            if (SharedInfo.CurseStarted)
            {
                if (_turnToColliderOnCurseEvent)
                    GetComponent<BoxCollider2D>().isTrigger = false;
                if (_removeOnCurseEvent)
                    gameObject.SetActive(false);
            }

        }
    }
}