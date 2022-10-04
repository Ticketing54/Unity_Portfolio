using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PedtClearCaching
{
    [MenuItem("Util/CleanCache")]
    public static void CleanCache()
    {
        if (Caching.ClearCache())
        {
            EditorUtility.DisplayDialog("�˸�", "ĳ�ð� �����Ǿ����ϴ�.", "Ȯ��");
        }
        else
        {
            EditorUtility.DisplayDialog("����", "ĳ�� ������ �����߽��ϴ�.", "Ȯ��");
        }
    }
    
}
