using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PedtClearCaching
{
    [MenuItem("Util/CleanCache")]
    public static void CleanCache()
    {
        if (Caching.CleanCache())
        {
            EditorUtility.DisplayDialog("알림", "캐시가 삭제되었습니다.", "확인");
        }
        else
        {
            EditorUtility.DisplayDialog("오류", "캐시 삭제에 실패했습니다.", "확인");
        }
    }
    
}
