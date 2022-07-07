using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;


public class timelinetest : MonoBehaviour
{
    PlayableDirector director;    
    Coroutine start;

    void Start()
    {
        start = StartCoroutine(CostartTimeline());
    }

    IEnumerator CostartTimeline()
    {
        GameObject obj = GameObject.Find("Timeline");
        director = obj.GetComponent<PlayableDirector>();
        director.playOnAwake = false;
        start = StartCoroutine(CoPlayTimeline(director));
        yield return null;
    }
    IEnumerator CoPlayTimeline(PlayableDirector _asset)
    {
        _asset.Play();
        Debug.Log("½ÃÀÛ");
        StartCoroutine(CheckTime(_asset));
        

        yield return null;
    }

  
    IEnumerator CheckTime(PlayableDirector _asset)
    {
        float timer = 0;
        while(true)
        {

            if(_asset.gameObject.activeSelf== false)
            {
                break;
            }
            Debug.Log(timer);
            timer++;
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("²ö³²");
    }
}
