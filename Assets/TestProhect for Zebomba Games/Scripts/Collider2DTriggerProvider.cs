using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��������������� ����� ��� ��������� ������� OnTriggerEnter2D, OnTriggerStay2D � OnTriggerExit2D
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collider2DTriggerProvider : MonoBehaviour
{
    [Tooltip("����������, ����� Collider2D ������ � ������� (������ ���������� ������)")]
    public UnityEvent<Collider2D> OnTrigger2DEnter;
    [Tooltip("���������� � ������ ����� ��� ���� ���������� Collider2D, ������� �������� �������� (������ ���������� ������)")]
    public UnityEvent<Collider2D> OnTrigger2DStay;
    [Tooltip("����������, ����� Collider2D �������� �������� �������� (������ ���������� ������)")]
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
