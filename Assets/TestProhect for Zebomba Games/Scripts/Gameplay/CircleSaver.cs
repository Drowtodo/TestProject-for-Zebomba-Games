using UnityEngine;

public class CircleSaver : MonoBehaviour
{
    /// <summary>
    /// Автоматически возвращаем Transform попадаемый в зону тригера 
    /// </summary>
    /// <param name="collision"></param>
    public void OnCircleSave(Collider2D collision)
    {
        if (collision.TryGetComponent(out Circle circle))
        {
            CirclePool.Return(circle);
        }
    }
}
