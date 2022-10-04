using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    
    public AudioMixer AudioMixer;
    [SerializeField]
    AudioListener mainListener;
    [SerializeField]
    AudioSource bgm;


    public static SoundManager soundmanager;
    private void Awake()
    {
        if (soundmanager == null)
        {
            soundmanager = this;
            DontDestroyOnLoad(gameObject);            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OffMainAudio()
    {
        mainListener.enabled = false;
    }
    public void soundsPlay(string _name , GameObject obj =null)
    {
        GameObject sounds = new GameObject(_name + "Sounds");        
        AudioSource audiosource = sounds.AddComponent<AudioSource>();
        AudioClip _clip = ResourceManager.resource.GetSound(_name);
        audiosource.clip = _clip;
        audiosource.Play();
        audiosource.outputAudioMixerGroup = AudioMixer.FindMatchingGroups("SFX")[0];
        if (obj != null)
        {
            audiosource.transform.position = obj.transform.position;
            audiosource.spatialBlend = 1;
        }
        
        Destroy(audiosource.gameObject, audiosource.clip.length);
        
    }

    public AudioSource GetSounds(string _name)
    {
        GameObject obj = new GameObject(_name);
        AudioSource audiosource = obj.AddComponent<AudioSource>();
        AudioClip _clip = ResourceManager.resource.GetSound(_name);

        if(_clip == null)
        {
            return null;
        }


        audiosource.clip = _clip;
        audiosource.outputAudioMixerGroup = AudioMixer.FindMatchingGroups("SFX")[0];
        audiosource.spatialBlend = 1;
        audiosource.Stop();
        return audiosource;
    }
    public void ControlBGMVolume(float val)
    {
        AudioMixer.SetFloat("Bgm", Mathf.Log10(val)*20);
    }
    public void ControlMasterVolume(float val)
    {
        AudioMixer.SetFloat("Master", Mathf.Log10(val)*20);
    }
    public void ControlSFXVolume(float val)
    {
        AudioMixer.SetFloat("SFX", Mathf.Log10(val)*20);
    }
    public void BgmPlay(string _mapName)
    {
        if(bgm != null)
        {
            Destroy(bgm);
        }

        GameObject obj = new GameObject(_mapName);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = ResourceManager.resource.GetSound(_mapName);
        bgm = source;
        bgm.outputAudioMixerGroup = AudioMixer.FindMatchingGroups("BGM")[0];        
        bgm.loop = true;
        bgm.volume = 0.1f;
        bgm.Play();
    }
}
