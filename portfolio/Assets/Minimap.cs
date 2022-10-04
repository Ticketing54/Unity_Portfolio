using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniMap : MonoBehaviour
{

    [SerializeField]
    Minimap_Maximum miniMax;
    [SerializeField]
    CanvasGroup miniMax_CG;

    Coroutine miniMaxFade;

    [SerializeField]
    Minimap_Minimum miniMin;
    [SerializeField]
    Image miniMinButton;

    //미니맵들 상태관리    
    bool miniMax_Active = false;
    bool miniMin_Active = false;
    private void Awake()
    {
        UIManager.uimanager.ATryOpenMinimap_Max += TryOpenMinimap_Maximum;
        UIManager.uimanager.ATryOpenMinimap_Min += TryOpenMinimap_Minimum;
        miniMax.gameObject.SetActive(false);
        miniMin.gameObject.SetActive(false);
    }


    #region KeyboardShorcut    
    void TryOpenMinimap_Maximum()
    {
        if (miniMaxFade != null)
        {
            StopCoroutine(miniMaxFade);
            miniMaxFade = null;
        }

        miniMax_Active = !miniMax_Active;

        if (miniMax_Active)
        {
            miniMaxFade = StartCoroutine(CoMinimFadein());
        }
        else
        {
            miniMaxFade = StartCoroutine(CoMinimFadeout());
        }
    }

    IEnumerator CoMinimFadein()
    {
        miniMax.gameObject.SetActive(true);
        while (miniMax_CG.alpha < 1)
        {
            miniMax_CG.alpha += Time.deltaTime * 4f;
            yield return null;
        }

        miniMaxFade = null;
    }

    IEnumerator CoMinimFadeout()
    {
        while (miniMax_CG.alpha > 0)
        {
            miniMax_CG.alpha -= Time.deltaTime * 4f;
            if (miniMax_CG.alpha <= 0)
            {
                miniMax.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }

        miniMaxFade = null;
    }

    void TryOpenMinimap_Minimum()
    {
        miniMin_Active = !miniMin_Active;

        if (miniMin_Active)
        {
            miniMin.gameObject.SetActive(true);
            miniMinButton.gameObject.SetActive(true);
        }
        else
        {
            miniMin.gameObject.SetActive(false);
            miniMinButton.gameObject.SetActive(false);
        }


    }

    #endregion
}