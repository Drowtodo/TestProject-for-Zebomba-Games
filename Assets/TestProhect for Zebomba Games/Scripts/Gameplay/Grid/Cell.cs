using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Cell : MonoBehaviour
{
    /// <summary>
    /// “екущий Circle в €чейке
    /// </summary>
    public Circle Circle { get; private set; }
    private Animator _anim;

    /// <summary>
    /// ¬озвращает true, если €чейка свободна
    /// </summary>
    public bool IsFree { get { return Circle == null; } }
    [Tooltip("¬ызываетс€ когда Circle попадает в €чейку")]
    public UnityEvent<Cell, Circle> OnCircleEnterInCell;

    /// <summary>
    ///  оординаты €чейки внутри сетки
    /// </summary>
    public Position Position { get; private set; }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    /// <summary>
    /// «акрытие €чейки и запоминани€ Circle
    /// </summary>
    /// <param name="circle">Circle, который хранитс€ в €чейке</param>
    public void Lock(Circle circle)
    {
        Circle = circle;
        _anim.SetBool("Locked", true);
    }

    /// <summary>
    /// ќткрытие €чейки и очситка записанного Circle
    /// </summary>
    public void Unlock()
    {
        Circle = null;
        _anim.SetBool("Locked", false);
    }

    /// <summary>
    /// ≈сли в €чейку попал Circle и она свободна, то вызываетс€ срабатывани€ событи€ OnCircleEnterInCell
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
    /// ќткрывает €чейку, если из неЄ убрали Circle
    /// </summary>
    public void OnCircleExit()
    { 
        if(!IsFree)
        {
            Unlock();
        }
    }

    /// <summary>
    /// ќсвобождение €чейки с вовзратом Circle обратно в пул и запуском эффекта исчезновани€ Circle
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
    /// ћетод устаналивает координаты €чейки в сетке, если они не были записаны ранее
    /// </summary>
    /// <param name="column">Ќомер колонки</param>
    /// <param name="row">Ќомер стороки</param>
    public void SetPosition(int column, int row)
    {
        if (Position == null)
        {
            Position = new Position(column, row);
        }
    }

    /// <summary>
    /// ќчистка €чейки без эффекта исчезновени€ Circle
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
