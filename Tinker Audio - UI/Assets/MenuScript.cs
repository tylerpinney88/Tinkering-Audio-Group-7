using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuScript : MonoBehaviour
{
    public Canvas OptionsMenu;
    public Canvas MainMenu;


    [Range(1, 20000)]  //Creates a slider in the inspector
    public float frequency1;

    [Range(1, 20000)]  //Creates a slider in the inspector
    public float frequency2;

    public float sampleRate = 44100;
    public float waveLengthInSeconds = 2.0f;

    AudioSource audioSource;
    int timeIndex = 0;

    public float Countdown;
    bool CountdownOn;

    public float TestButtonCountdown;
    public bool IsButtonPressed;

    public Slider VolumeSlider;
    public bool SlideNoisePlaying;
    public float SlideCountdown;

    public Slider Frequency1Slider;
    public Slider Frequency2Slider;
    // Start is called before the first frame update
    void Start()
    {
        OptionsMenu.enabled = false;
        SlideNoisePlaying = false;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        Frequency1Slider.maxValue = 20000;
        Frequency2Slider.maxValue = 20000;
        Frequency1Slider.value = 300;
        Frequency2Slider.value = 300;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
            {
                timeIndex = 0;  //resets timer before playing sound
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }


        if (Countdown <= 0)
        {
            audioSource.Stop();
        }

        if (CountdownOn)
        {
            Countdown = Countdown -= Time.deltaTime;

        }

        if (SlideCountdown <= 0)
        {
            SlideNoisePlaying = false;
            
        }

        if (IsButtonPressed == true) 
        {
            audioSource.Play();
            TestButtonCountdown = TestButtonCountdown - Time.deltaTime;
        }

        if (TestButtonCountdown <= 0) 
        {
            IsButtonPressed = false;
        }


        audioSource.volume = VolumeSlider.value;
        SlideCountdown = SlideCountdown -= Time.deltaTime;
    }


    public void OptionButton()
    {
        OptionsMenu.enabled = true;
        MainMenu.enabled = false;
        audioSource.Play();
        Countdown = 0.2f;
        CountdownOn = true;

    }


    public void BackButton()
    {
        OptionsMenu.enabled = false;
        MainMenu.enabled = true;
        audioSource.Play();
        Countdown = 0.2f;
        CountdownOn = true;

    }

    public void MainMenuButton()
    {
        OptionsMenu.enabled = false;
        MainMenu.enabled = true;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void PlayButton()
    {

    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, frequency2, sampleRate);

            timeIndex++;

            //if timeIndex gets too big, reset it to 0
            if (timeIndex >= (sampleRate * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }


    public void OnSliderChange()
    {
        SlideCountdown = 0.1f;
        Countdown = SlideCountdown;
        if (!SlideNoisePlaying)
        {
            audioSource.Play();
            SlideNoisePlaying = true;
        }
    }

    public void OnFrequency1SliderChange() 
    {
        frequency1 = Frequency1Slider.value;
    }

    public void OnFrequency2SliderChange()
    {
        frequency2 = Frequency2Slider.value;
    }

    public void PlayAudio() 
    {
        if (IsButtonPressed == false) 
        {
            IsButtonPressed = true;
            TestButtonCountdown = 0.4f;
        }
    }
}

