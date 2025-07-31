using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolContainer : MonoBehaviour
{
    private List<Circle> _circles = new();

    /// <summary>
    /// ������� ��������� Circle.
    /// </summary>
    public int FreeCount { get; private set; } = 0;

    /// <summary>
    /// ��������� true, ���� � ���������� ���� ��������� Circle.
    /// </summary>
    public bool HasFree { get { return FreeCount > 0; } }

    /// <summary>
    /// ���������� ������ Circle � ���������.
    /// </summary>
    /// <param name="circle">����������� Circle</param>
    public void Add(Circle circle)
    {
        _circles.Add(circle);
        Return(circle);
    }

    /// <summary>
    /// ������������ ��� ����������� ��� ������������� Circle � ���������.
    /// </summary>
    /// <param name="circle">������������ Circle</param>
    public void Return(Circle circle)
    {
        if(_circles.Contains(circle))
        {
            circle.ReturnToContainer(transform);
            FreeCount++;
        }
    }

    /// <summary>
    /// ���������� true, ���� ������� �������� �� ���������� ��������� Circle.
    /// </summary>
    /// <param name="circle">Circle, � ������� ����� ������� ���� �� ��������� Circle � ����������</param>
    /// <returns></returns>
    public bool TryGet(out Circle circle)
    {
        if(HasFree)
        {
            circle = _circles.Where((x) => !x.gameObject.activeSelf).FirstOrDefault();
            circle.gameObject.SetActive(true);
            FreeCount--;
            return true;
        }
        circle = null;
        return false;
    }
}
