using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiEffectControl : MonoBehaviour
{
    
    PoolingManager<TextMeshProUGUI> textPool;           // UI 텍스트 풀링
    PoolingManager<Image> imagePool;                    // UI 이미지 풀링                          
    

    // Samples
    [SerializeField]
    bufimage        sampleBufimage;                     // 버프 
    [SerializeField]
    TextMeshProUGUI sampleDamageEffect;                 // 데미지 이펙트
    [SerializeField]
    TextMeshProUGUI sampleNickName;                     // 닉네임
    [SerializeField]
    Image           sampleHpBar;                        // 체력바
    [SerializeField]
    Image           sampleDot;                          // 미니맵 표시


    // Parent

    GameObject bufLine;


    private void Awake()
    {        
        textPool = new PoolingManager<TextMeshProUGUI>();        
        textPool.Add("DamageEffect", sampleDamageEffect);
        textPool.Add("NickName", sampleNickName);


        imagePool = new PoolingManager<Image>();
        imagePool.Add("HpBar", sampleHpBar);
        imagePool.Add("BufImage", sampleBufimage);
        imagePool.Add("Dot", sampleDot);
        
        
    }

   
    Image


}
