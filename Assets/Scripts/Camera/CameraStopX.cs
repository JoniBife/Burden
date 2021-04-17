using Assets.Scripts.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStopX : MonoBehaviour
{

    private CameraFollow _cameraMovement;
    [SerializeField]
    private BoxCollider2D _boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        _cameraMovement = FindObjectOfType<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _cameraMovement.StopFollowX();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _cameraMovement.StartFollowX();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            transform.position + new Vector3(_boxCollider2D.offset.x, _boxCollider2D.offset.y),
            new Vector3(_boxCollider2D.bounds.extents.x * 2,
            _boxCollider2D.bounds.extents.y * 2, 1));
    }
}
