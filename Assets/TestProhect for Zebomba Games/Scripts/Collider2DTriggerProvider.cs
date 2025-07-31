using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Вспомогательный класс для обработки событий OnTriggerEnter2D, OnTriggerStay2D и OnTriggerExit2D
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collider2DTriggerProvider : MonoBehaviour
{
    [Tooltip("Вызывается, когда Collider2D входит в триггер (только двухмерная физика)")]
    public UnityEvent<Collider2D> OnTrigger2DEnter;
    [Tooltip("Вызывается в каждом кадре для всех эллементов Collider2D, которые касаются триггера (только двухмерная физика)")]
    public UnityEvent<Collider2D> OnTrigger2DStay;
    [Tooltip("Вызывается, когда Collider2D перестаёт касаться триггера (только двухмерная физика)")]
    public UnityEvent<Collider2D> OnTrigger2DExit;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger2DEnter?.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTrigger2DStay?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTrigger2DExit?.Invoke(collision);
    }
}
