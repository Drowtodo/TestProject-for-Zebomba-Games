using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Cell : MonoBehaviour
{
    /// <summary>
    /// ������� Circle � ������
    /// </summary>
    public Circle Circle { get; private set; }
    private Animator _anim;

    /// <summary>
    /// ���������� true, ���� ������ ��������
    /// </summary>
    public bool IsFree { get { return Circle == null; } }
    [Tooltip("���������� ����� Circle �������� � ������")]
    public UnityEvent<Cell, Circle> OnCircleEnterInCell;

    /// <summary>
    /// ���������� ������ ������ �����
    /// </summary>
    public Position Position { get; private set; }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    /// <summary>
    /// �������� ������ � ����������� Circle
    /// </summary>
    /// <param name="circle">Circle, ������� �������� � ������</param>
    public void Lock(Circle circle)
    {
        Circle = circle;
        _anim.SetBool("Locked", true);
    }

    /// <summary>
    /// �������� ������ � ������� ����������� Circle
    /// </summary>
    public void Unlock()
    {
        Circle = null;
        _anim.SetBool("Locked", false);
    }

    /// <summary>
    /// ���� � ������ ����� Circle � ��� ��������, �� ���������� ������������ ������� OnCircleEnterInCell
    /// </summary>
    /// <param name="other"></param>
    public void OnCircleEnter(Collider2D other)
    {
        if(other.TryGetComponent(out Circle circle))
        {
            if (IsFree)
            {
                OnCircleEnterInCell?.Invoke(this, circle);
            }
        }
    }

    /// <summary>
    /// ��������� ������, ���� �� �� ������ Circle
    /// </summary>
    public void OnCircleExit()
    { 
        if(!IsFree)
        {
            Unlock();
        }
    }

    /// <summary>
    /// ������������ ������ � ��������� Circle ������� � ��� � �������� ������� ������������ Circle
    /// </summary>
    public async void Free()
    {
        if(!IsFree)
        {
            await Circle.ReleaseParticles();
            CirclePool.Return(Circle);
            Unlock();
        }
    }

    /// <summary>
    /// ����� ������������ ���������� ������ � �����, ���� ��� �� ���� �������� �����
    /// </summary>
    /// <param name="column">����� �������</param>
    /// <param name="row">����� �������</param>
    public void SetPosition(int column, int row)
    {
        if (Position == null)
        {
            Position = new Position(column, row);
        }
    }

    /// <summary>
    /// ������� ������ ��� ������� ������������ Circle
    /// </summary>
    public void Clear()
    {
        if (!IsFree)
        {
            CirclePool.Return(Circle);
            Circle = null;
        }

        Unlock();
    }

}
