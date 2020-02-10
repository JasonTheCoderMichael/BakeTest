using UnityEngine;

//[ExecuteInEditMode]
public class SimpleMove : MonoBehaviour
{
    public float duration;
    public float left;
    public float right;

    private float m_timeElapsed;
    private Vector3 curPos;
    private bool m_fromLeftToRight;
    void OnEnable()
    {
        m_timeElapsed = 0;
        curPos = transform.position;
        curPos.x = left;
        m_fromLeftToRight = true;
    }

    void Update()
    {
        if (duration <= 0)
        {
            return;
        }

        m_timeElapsed += Time.deltaTime;
        if (m_timeElapsed > duration)
        {
            m_timeElapsed = 0;
            m_fromLeftToRight = !m_fromLeftToRight;
        }

        float percent = Mathf.Clamp01(m_timeElapsed / duration);
        if (m_fromLeftToRight)
        {
            curPos.x = Mathf.Lerp(left, right, percent);
        }
        else
        {
            curPos.x = Mathf.Lerp(right, left, percent);
        }

        transform.position = curPos;
    }
}
