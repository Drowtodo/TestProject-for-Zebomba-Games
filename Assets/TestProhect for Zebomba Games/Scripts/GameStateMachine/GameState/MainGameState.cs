using UnityEngine;

public class MainGameState : AbstractGameState
{
    public GameObject MainGameplayObjectsHandler;

    protected override void OnEnter()
    {
        MainGameplayObjectsHandler.SetActive(true);
    }

    protected override void OnExit()
    {
        MainGameplayObjectsHandler.SetActive(false);
        GameplayController.GameEnd();
    }
}
