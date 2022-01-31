using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Cat Sounds")]
    public AudioSource catFootSteps;
    public AudioSource catAngry;
    public AudioSource catPur;

    [Header("Pushable Combined Sounds")]
    public AudioSource pushableCombinedSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPushableCombiningSound()
    {
        pushableCombinedSound.Play();
    }

    public void PlayCatFootSteps()
    {
        catFootSteps.Play();
    }

    public void PlayCatPur()
    {
        catPur.gameObject.SetActive(true);
        //catPur.Play();
    }

    public void PlayCatAngry()
    {
        catAngry.Play();
    }
}
