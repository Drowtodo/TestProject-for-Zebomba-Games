using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class AbstractUIState : MonoBehaviour
{
    /// <summary>
    /// ���� � ������������ UIState
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// ����� �� ������������� UIState
    /// </summary>
    public abstract void Exit();
}
