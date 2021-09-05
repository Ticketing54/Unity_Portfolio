using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

// 메뉴를 구성하는 클래스는 모노비헤이비어상속이 필요 없습니다.
// 클래스 내부의 함수는 모두 static이어야 한다.
// using UnityEditor; 추가

public class SaveMapData
{
    [MenuItem("Custom/SaveMapToCSV")]
    static void SaveMapCSV()
    {
        GameObject[] bds = GameObject.FindGameObjectsWithTag("Map");
        string data = string.Empty;

        foreach (GameObject one in bds)
        {
            data += one.name;
            data += ",";
            data += one.transform.position.x;
            data += ",";
            data += one.transform.position.y;
            data += ",";
            data += one.transform.position.z;
            data += ",";
            data += one.transform.localEulerAngles.x;
            data += ",";
            data += one.transform.localEulerAngles.y;
            data += ",";
            data += one.transform.localEulerAngles.z;
            data += ",";
            data += one.transform.localScale.x;
            data += ",";
            data += one.transform.localScale.y;
            data += ",";
            data += one.transform.localScale.z;
            data += "\n";
        }

        WriteData(Application.dataPath + "/" + "Road.csv", data);
    }



    static void WriteData(string _filename, string _wData)
    {
        using (FileStream f = new FileStream(_filename, FileMode.Create, FileAccess.Write))
        {
            // pc전용 파일 쓰기
            using (StreamWriter writer = new StreamWriter(f, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(_wData);
                writer.Close();
            }
            f.Close();
        }
    }

    [MenuItem("Custom/LoadMapFromCSV")]
    static void LoadMapCSV()
    {
        ReadData(Application.dataPath + "/" + "Village_1.csv");
    }

    static public void ReadData(string _filename)
    {
        using (FileStream f = new FileStream(_filename, FileMode.Open, FileAccess.Read))
        {
            // pc전용 파일 읽어오기( 경로문제를 플랫폼 별로 다르게 구현한다면 사용가능 )
            using (StreamReader reader = new StreamReader(f, System.Text.Encoding.UTF8))
            {
                string line = string.Empty;

                //csv에서 첫줄(컬럼명)은 그냥 읽고 넘어간다.
                //reader.ReadLine();

                while ((line = reader.ReadLine()) != null && (!string.IsNullOrEmpty(line)))
                {
                    string[] data = line.Split(',');
                    int start = data[0].IndexOf('_');
                    string objname = data[0].Substring(0, start);
                    Debug.Log(objname);

                    GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(objname));
                    Vector3 pos = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
                    Vector3 rot = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
                    Vector3 scale = new Vector3(float.Parse(data[7]), float.Parse(data[8]), float.Parse(data[9]));

                    obj.name = data[0];
                    obj.transform.position = pos;
                    obj.transform.localEulerAngles = rot;
                    obj.transform.localScale = scale;
                }

                reader.Close();
            }

            f.Close();
        }
    }
}
