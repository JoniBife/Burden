using Assets.Scripts.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour, IPausable
{
    [SerializeField]
    private float _sanityDecreaseFrequency = 1; // In seconds
    [SerializeField]
    private float _sanityDecreaseRate = 5;
    [SerializeField]
    private float _maxSanity= 100;

    private float _currSanity = 100;

    public GameObject SanityBar;
    public Slider Slider;

    private bool _started = false;

    private bool _paused = false;

    public Canvas canvas;
    public GameObject image;

    // Start is called before the first frame update
    void Start()
    {
        _started = SharedInfo.CurseStarted;
        SanityBar.SetActive(_started);

        if (_started)
        {
            _currSanity = SharedInfo.Sanity;
            StartCoroutine(SanityDecreaseRoutine()); // We have to restart the sanity decrease coroutine
        }

        Slider.minValue = 0;
        Slider.maxValue = _maxSanity;   
    }

    // Update is called once per frame
    void Update()
    {
        if (_started)
        {
            Slider.value = _maxSanity - _currSanity;
            SharedInfo.Sanity = _currSanity;
        }

        if (_currSanity == 0)
        {
            GameObject image2 = Instantiate(image, canvas.transform);
            image2.GetComponentInChildren<Text>().text = "Game over";
        }

    }

    public void StartSanityManager()
    {
        if (!_started)
        {
            _started = true;
            SanityBar.SetActive(true);
            StartCoroutine(SanityDecreaseRoutine());
            SharedInfo.CurseStarted = true; // Should not be done here!
        }
    }

    public IEnumerator SanityDecreaseRoutine()
    {
        while(true)
        {
            DecreaseSanity(_sanityDecreaseRate);
            if (_currSanity == 0)
                break;
            yield return new WaitForSeconds(_sanityDecreaseFrequency);
        }
    }

    public float DecreaseSanity(float decreaseValue)
    {
        if (_paused)
            return _currSanity;

        if ((_currSanity - decreaseValue) < 0)
        {
            _currSanity = 0;
            // No more sanity event
        } else
            _currSanity -= decreaseValue;

        return _currSanity;
    }

    public float IncreaseSanity(float increaseValue)
    {
        if (_paused)
            return _currSanity;

        _currSanity += increaseValue;

        if (_currSanity > _maxSanity)
            _currSanity = _maxSanity;
            
        return _currSanity;
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Resume()
    {
        _paused = false;
    }
}
