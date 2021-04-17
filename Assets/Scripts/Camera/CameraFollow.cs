using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;

namespace Assets.Scripts.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _transformToFollow;

        [SerializeField]
        private float _moveTowardsSpeed = 3f;

        [SerializeField]
        private Vector2 _cameraBounds;

        [SerializeField]
        private Vector3 _followOffset;

        [SerializeField]
        private bool _followingX = true, _followingY = true;


        // If the camera starting position is set, we skip the first late update to ensure
        // that the stop camera following scripts have already affected this values
        private bool _ignoreFirstLateUpdate = false;

        private void Start()
        {
            if (SharedInfo.CameraStartingPosition != Vector3.zero)
            {
                transform.position = SharedInfo.CameraStartingPosition;
                _ignoreFirstLateUpdate = true;
            } 
            transform.position += _followOffset;
        }

        private void LateUpdate()
        {
            // Calculating target Y position
            Vector3 targetPosition = new Vector3(
                transform.position.x, 
                _followingY ? _transformToFollow.position.y : transform.position.y,
                transform.position.z
                );

            targetPosition += _followOffset;

            // Calculating the new camera position on the Y
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, _moveTowardsSpeed * Time.deltaTime);

            // If following X then set the new camera position to the character X
            if (_followingX)
                newPosition.x = _transformToFollow.position.x;

            // Finally calculating the difference between the current camera position an the character 
            // This difference is then used to verify if the camera is within the limited bound
            /*float xDifference = Mathf.Abs(_transformToFollow.position.x - transform.position.x);
            float yDifference = Mathf.Abs(_transformToFollow.position.y - transform.position.y);
            if (xDifference < _cameraBounds.x)
            {
                newPosition.x = transform.position.x;
            }

            if (yDifference < _cameraBounds.y)
            {
                newPosition.y = transform.position.y;
            }*/

            transform.position = newPosition;
        }

        public void StartFollowY()
        {
            _followingY = true;
        }

        public void StopFollowY()
        {
            _followingY = false;
        }

        public void StartFollowX()
        {
            _followingX = true;
        }

        public void StopFollowX()
        {
            _followingX = false;
        }
    }
}
