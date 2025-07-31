using Cysharp.Threading.Tasks;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [Range(0f, 180f),Tooltip("���� ���������� ��������")]
    public float angle = 45;
    [Range(0f, 360f), Tooltip("�������������� ����������� �����, ������������� ������� ������������� ����������")]
    public float offsetAngle = 0f;
    [Tooltip("�������� �������� �������")]
    public float speed = 1.0f;
    [Tooltip("Transform ����� ��� ��������� Circle")]
    public Transform CircleAnchor;
    /// <summary>
    /// ������� ������������ Circle
    /// </summary>
    public Circle CurrentCircle { get; private set; }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Cos(Time.realtimeSinceStartup * speed) + offsetAngle);//������������ ��������
    }

    /// <summary>
    /// ����������� �������
    /// </summary>
    public void Free()
    {
        if(CurrentCircle != null)
        {
            CurrentCircle.Free();
            CurrentCircle = null;
        }
    }

    /// <summary>
    /// ����������� Circle � ��������
    /// </summary>
    /// <param name="circle">������������ Circle</param>
    /// <returns></returns>
    public bool Add(Circle circle)
    {
        if(CurrentCircle == null)
        {
            circle.Grab(this);
            CurrentCircle = circle;
            return true;
        }
        return false;
    }

}
