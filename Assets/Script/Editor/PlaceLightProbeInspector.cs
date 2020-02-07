using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// TODO: float 累加误差问题 //

[CustomEditor(typeof(PlaceLightProbe))]
public class PlaceLightProbeInspector : Editor
{
    private SerializedProperty m_sceneRangeMin;
    private SerializedProperty m_sceneRangeMax;

    public void OnEnable()
    {
        m_sceneRangeMin = serializedObject.FindProperty("SceneRangeMin");
        m_sceneRangeMax = serializedObject.FindProperty("SceneRangeMax");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(m_sceneRangeMin);
        EditorGUILayout.PropertyField(m_sceneRangeMax);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Place Light Probe"))
        {
            PlaceLightProbe(m_sceneRangeMin.vector3Value, m_sceneRangeMax.vector3Value);

            //// 测试, 累加出现误差？？？ //
            //float curValue = 0f;
            //float offset = 1.30f;
            //float maxValue = 13.0f;
            //while (curValue <= maxValue)
            //{
            //    curValue += offset;
            //    float temp = maxValue - curValue;
            //    Debug.Log(maxValue - curValue);
            //}

            //int offsetInt = (int)(offset * 1000);
            //int curValueInt = 0;
            //int maxValueInt = (int)(maxValue * 1000);
            //while (curValueInt <= maxValueInt)
            //{
            //    curValueInt += offsetInt;
            //    curValue = curValueInt * 0.001f;
            //    Debug.Log(maxValueInt - curValueInt);
            //    Debug.Log(maxValue - curValue);
            //}
        }   
    }

    private void PlaceLightProbe(Vector3 sceneRangeMin, Vector3 sceneRangeMax)
    {
        PlaceLightProbe plp = target as PlaceLightProbe;
        if (plp == null)
        {
            return;
        }

        LightProbeGroup lightProbeGroup = plp.GetComponent<LightProbeGroup>();
        if (lightProbeGroup == null)
        {
            return;
        }

        List<Vector3> posList = new List<Vector3>();
        Vector3 range = sceneRangeMax - sceneRangeMin;
        if (range.x <= 0 || range.y <= 0 || range.z <= 0)
        {
            return;
        }

        Vector3 num = new Vector3(10, 10, 10);               // xyz方向上分别有多少个probe //
        Vector3 offset = new Vector3(range.x / num.x, range.y / num.y, range.z / num.z);

        Vector3 curPos = sceneRangeMin;

        // 累加方式，最后会有非常小的一个差值，导致curPos.y 和 sceneRangeMax.y 不一致 //
        while (curPos.y <= sceneRangeMax.y)
        {
            while (curPos.z <= sceneRangeMax.z)
            {
                while (curPos.x <= sceneRangeMax.x)
                {
                    posList.Add(curPos);
                    curPos.x += offset.x;
                }

                curPos.x = sceneRangeMin.x;
                curPos.z += offset.z;
            }

            curPos.x = sceneRangeMin.x;
            curPos.z = sceneRangeMin.z;
            curPos.y += offset.y;
        }

        lightProbeGroup.probePositions = posList.ToArray();
    }
}