using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgm;
    public List<AudioClip> bgmlist = new List<AudioClip>();
    public AudioMixer AudioMixer;



    public static SoundManager soundmanager;
    private void Awake()
    {
        if (soundmanager == null)
        {
            soundmanager = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        bgmListUpdate();
        






    }

    public void bgmListUpdate()
    {
        AudioClip[] one = Resources.LoadAll<AudioClip>("Sounds/Bgm");
        for(int i = 0; i < one.Length; i++)
        {
            
            bgmlist.Add(one[i]);
        }
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for(int i = 0; i < bgmlist.Count; i++)
        {
            if (arg0.name == bgmlist[i].name)
                BgmPlay(bgmlist[i]);
        }
    }

    public void soundsPlay(string _Name , GameObject obj =null)
    {
        GameObject sounds = new GameObject(_Name + "Sounds");        
        AudioSource audiosource = sounds.AddComponent<AudioSource>();        
        AudioClip _clip = Resources.Load<AudioClip>("Sounds/" + _Name);        
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
    public void BgmPlay(AudioClip clip)
    {
        bgm.outputAudioMixerGroup = AudioMixer.FindMatchingGroups("BGM")[0];
        bgm.clip = clip;
        bgm.loop = true;
        bgm.volume = 0.1f;
        bgm.Play();

    }
}
