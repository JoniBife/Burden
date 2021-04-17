using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Common
{
    public class ZoneReturner : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider2D _boxCollider2D;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                // TODO Add fade in
                ZoneManager.LoadPreviousZone();
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
    }
}