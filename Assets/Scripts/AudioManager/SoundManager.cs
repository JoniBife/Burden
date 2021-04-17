using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] musics;
    private List<AudioSource> aud_arr = new List<AudioSource>();
    public int chosen = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in musics)
        {
            AudioSource aud = gameObject.AddComponent<AudioSource>();
            aud.clip = item;
            aud.volume = 0.05f;
            aud.playOnAwake = false;
            aud.loop = true;
            aud_arr.Add(aud);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < aud_arr.Count; i++)
        {
            if(i == chosen & !aud_arr[i].isPlaying)
            {
                aud_arr[i].Play();
            }
            else if(i != chosen)
            {
                aud_arr[i].Stop();
            }
        }
    }

    public void setChosen(int c)
    {
        chosen = c;
    }
}
