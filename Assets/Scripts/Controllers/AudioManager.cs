using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("# AudioSources")]
    AudioSource effectSource;
    public AudioSource ambienSource;
    public AudioClip[] backgroundSounds;
    public AudioClip[] gameplayBackgroundSound;

    [Header("# Effects Clips")]
    public AudioClip[] clipEffect;
    void Awake()
    {
        instance = this;
        effectSource = gameObject.AddComponent<AudioSource>() as AudioSource;
    }
    void Start()
    {
        //RandomBackground();
    }
    public void RandomBackground(){
        int randomIndex = Random.Range(0,backgroundSounds.Length);
        ambienSource.clip = backgroundSounds[randomIndex];
        ambienSource.Play();
    }
    public void PlayGameplayBackground(int _index){
        int randomIndex = Random.Range(0,gameplayBackgroundSound.Length);
        ambienSource.clip = gameplayBackgroundSound[_index];
        ambienSource.Play();

    }
    public void PlayBackgroundSound(int _index){
        ambienSource.clip = backgroundSounds[_index];
        ambienSource.Play();
    }
    public void OnVolumeChange(Slider _slider)
    {
        ambienSource.volume = _slider.value;
    }
    public void OnVolumeChangeSFX(Slider _slider){
        effectSource.volume = _slider.value;
    }
    public void PlayErrorSound(){
        effectSource.clip=clipEffect[0];
        effectSource.Play();
    }
    public void PlayerSendSound()
    {
        int random = Random.Range(1,4);
        effectSource.clip=clipEffect[random];
        effectSource.Play();
    }
    public void PlayClipEffect(){
        effectSource.clip=clipEffect[2];
        effectSource.Play();
    }
    public void PlayClipEff(int _indexSound)
    {
        effectSource.clip=clipEffect[_indexSound];
        effectSource.Play();
    }
}
