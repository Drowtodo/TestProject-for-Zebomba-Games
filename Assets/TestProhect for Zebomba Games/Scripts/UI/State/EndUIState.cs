using TMPro;
using UnityEditor;
using UnityEngine;

public class EndUIState : AbstractUIState
{
    [Tooltip("Используется для отображения кол-ва заработанных очков во время игры")]
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
    /// Закрытие игры
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
    /// Перезапуск игрового процесса
    /// </summary>
    public void RestartGame()
    {
        GameStateMachine.ChangeState(GameStateMachine.Current.BeginState);
    }
}
