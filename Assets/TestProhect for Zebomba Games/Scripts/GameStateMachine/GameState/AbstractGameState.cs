using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractGameState : MonoBehaviour
{
    [SerializeField,Tooltip("������������, ���� ��� �������� ��������� ��������� ����������� UI")]
    protected AbstractUIState UIState;

    [Tooltip("����� ���������� �� ��������� ���� �������� ��� ����� � ���������")]
    public UnityEvent OnStateEnter;
    [Tooltip("����� ���������� �� ��������� ���� �������� ��� ������ �� ���������")]
    public UnityEvent OnStateExit;

    /// <summary>
    /// ���� � GameState. �� ��������� ���������� ������������ ������� OnStateEnter
    /// </summary>
    public void Enter()
    {
        if(UIState != null) UIState.Enter();
        OnEnter();
        OnStateEnter?.Invoke();
    }

    protected abstract void OnEnter();

    /// <summary>
    /// ���� �� GameState. �� ��������� ���������� ������������ ������� OnStateExit
    /// </summary>
    public void Exit()
    {
        if (UIState != null) UIState.Exit();
        OnExit();
        OnStateExit?.Invoke();
    }

    protected abstract void OnExit();
}
