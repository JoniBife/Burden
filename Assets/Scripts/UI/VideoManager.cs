using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoManager : MonoBehaviour
{
    private GameObject _videoDisplay;
    private VideoPlayer _videoPlayer;
    private Action _onVideoEnd;
    private bool _isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoDisplay = GameObject.Find("VideoDisplay");
        if (_videoDisplay == null)
        {
            throw new ArgumentNullException("Could not find the VideoDisplay");
        }
        _videoDisplay.SetActive(false);
    }

    private void LateUpdate()
    {
        //TODO Frame count is not matching the last frame for some reason this should be checked later on
        if (_isPlaying && _videoPlayer.frame == (long)_videoPlayer.frameCount - 1)
        {
            _isPlaying = false;
            _videoDisplay.SetActive(false);
            _onVideoEnd();
        }
    }

    public void PlayVideo(Action onVideoEnd)
    {
        _videoDisplay.SetActive(true);
        _videoPlayer.Play();
        _isPlaying = true;
        _onVideoEnd = onVideoEnd;
    }

    public void PlayVideo()
    {
        _videoDisplay.SetActive(true);
        _videoPlayer.Play();
        _isPlaying = true;
        _onVideoEnd = () => { Application.Quit(); };
    }

}
