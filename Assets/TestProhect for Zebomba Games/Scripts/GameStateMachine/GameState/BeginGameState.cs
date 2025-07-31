using UnityEngine;

public class BeginGameState : AbstractGameState
{
    public Animator BeginAnimation;

    protected override void OnEnter()
    {
        BeginAnimation.gameObject.SetActive(true);
        BeginAnimation.SetTrigger("Start");
    }

    protected override void OnExit()
    {
        BeginAnimation.SetTrigger("Stop");
        BeginAnimation.gameObject.SetActive(false);
    }
}
