using TMPro;
using UnityEditor;
using UnityEngine;

public class EndUIState : AbstractUIState
{
    [Tooltip("������������ ��� ����������� ���-�� ������������ ����� �� ����� ����")]
    public TMP_Text EarnedPoints;

    public override void Enter()
    {
        gameObject.SetActive(true);
        EarnedPoints.text = GameplayController.Instance.Points.ToString();
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �������� ����
    /// </summary>
    public void CloseGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// ���������� �������� ��������
    /// </summary>
    public void RestartGame()
    {
        GameStateMachine.ChangeState(GameStateMachine.Current.BeginState);
    }
}
