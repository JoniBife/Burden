using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMovement : MonoBehaviour
{
    public bool going_down = true;
    public float duration = 5.0f;
    public float timeElapsed = 0.0f;
    public Vector3 initial_pos;
    public Vector3 final_pos;
    public float timeStopped= 3.0f;

    public float startRadius;

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        initial_pos = transform.position;
        StartCoroutine(UpDown());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Player)
        {
            other.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject == Player)
        {
            other.collider.transform.SetParent(null);
        }
    }

    IEnumerator UpDown()
    {
        while (true) { 
            if (timeElapsed >= duration)
            {
                going_down = !going_down;
                timeElapsed = 0;
                yield return new WaitForSeconds(timeStopped);
            }

            if (timeElapsed < duration)
            {
                if (going_down)
                {
                    transform.position = Vector3.Lerp(initial_pos, final_pos, timeElapsed / duration);

                }
                else
                {
                    transform.position = Vector3.Lerp(final_pos, initial_pos, timeElapsed / duration);
                }
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}

