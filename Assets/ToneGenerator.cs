using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
//Anything inside the script will "require" this component
public class ToneGenerator : MonoBehaviour
{
    private AudioSource source;
    private AudioClip clip;
    //  Audio Source Component

    private float[] samples = new float[44100];
    //Establishing the max number of samples
    private int sampleFrequecy = 44100;

    private float[] notes = { 523.25f, 391.995f };
    private int currentNote;
    [SerializeField]
    private int[] sequence = { 0, 0, 1, 1, 0 };

    [SerializeField]
    private float sampleLengthInSeconds = 1;
    [SerializeField]
    private float beatsPerMinuteTempo = 120;
    // Creating the notes using their special frequencies


    void Awake()
    {
        source = GetComponent<AudioSource>();
        //refrences "source" above
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMelody();
    }

    void CreateTone(float note)
        //Create own method then reference it in void start
    {
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Mathf.Sin(Mathf.PI * 2 * i * note / sampleFrequecy);
            //Equation for generating a sine wave
        }

        clip = AudioClip.Create("Note", samples.Length, 1, sampleFrequecy, false);
        clip.SetData(samples, 0);
    }

    void PlayMelody()
    {
        if (currentNote < sequence.Length)
            currentNote += 1;
        else
            currentNote = 0;

        CreateTone(notes[sequence[currentNote]]);

        source.clip = clip;
        source.Play();
    }
    //Create an array for the notes, Create a tempo; create sounds at certain intervals. Coroutine/invokerepeating to have a beat. Synthesise tones
}
