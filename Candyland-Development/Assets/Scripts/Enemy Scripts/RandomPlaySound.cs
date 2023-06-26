using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlaySound : MonoBehaviour
{
    AudioSource squirelAudio;
    [SerializeField] int eventProbability;
    int soundDice;

    void Start()
    {
        squirelAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        soundDice = Random.Range(1, eventProbability + 1);

        if (soundDice == 1)
        {
            squirelAudio.Play();
        }
    }
}
