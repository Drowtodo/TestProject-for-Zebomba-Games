using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUIState : AbstractUIState
{
    [Tooltip("�������� ������������ ��� ��������� ��������")]
    public Image GeneratingBar;
    [Tooltip("�������� ��� �������� ����� ����� ������������")]
    public GameObject ClickWaitingPanel;
    [Tooltip("������������ ������ ��� ���� ��������� � ���������")]
    public GameObject LoadingHandler;
    [Tooltip("������������ ������ ��� ��� ��������� � �������� ���������")]
    public GameObject GameHandler;

    private bool _clickRelease = false;

    public override async void Enter()
    {
        gameObject.SetActive(true);
        LoadingHandler.SetActive(true);
        var progress = new Progress<float>(x => GeneratingBar.fillAmount = x);
        await CirclePool.Generate(progress); //���������� ��� �������� � ��������� ��������� � ������� ��������
        ClickWaitingPanel.SetActive(true);
        await UniTask.WaitWhile(() => !_clickRelease);//��� ������ ������������ ��������� �������� �������� � ��������� ����� �� ��� ��� �����������
        GameHandler.SetActive(true);
        ClickWaitingPanel.SetActive(false);
        LoadingHandler.SetActive(false);
        GameplayController.GameStart();
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
        _clickRelease = false;
        GameHandler.SetActive(false);
    }

    /// <summary>
    /// ��������� ����� � true ����� ����� �� ��������
    /// </summary>
    public void ClickRelease()
    {
        _clickRelease = true;
    }
}
