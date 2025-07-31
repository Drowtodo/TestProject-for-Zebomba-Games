using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class CirclePool : MonoBehaviour
{
    public static CirclePool Instance { get; private set; }

    [SerializeField, Tooltip("������ ������������ ��� �������� ������� Circle �� �����")]
    private GameObject CirclePrefab;

    [SerializeField, Tooltip("������ ��������� ������ ��� ��������� ���� ��������")]
    private List<Color> ColorsList;
    [SerializeField, Range(3, 100), Tooltip("���-�� ��������������� �������� ������ �����")]
    private int MaxItemInOneColor = 9;
    public bool IsGenerated { get; private set; } = false;

    private Dictionary<Color, PoolContainer> _containers;
    private void Start()
    {
        if(Instance != null)
        {
            Debug.LogError("��� ���������� ������ ��������� CirclePool!\n������� ������ ����� ���������.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _containers = new();
    }

    /// <summary>
    /// ������������ ��� �������, ���� �� �� ��� ������������ ������.
    /// </summary>
    /// <param name="progress">������ IProgress ������������ ��� �������� ��������� ���������.</param>
    /// <returns></returns>
    public static async UniTask Generate(IProgress<float> progress = null)
    {
        if(Instance.IsGenerated)
        {
            progress?.Report(1f);
            return;
        }

        int maxItem = Instance.ColorsList.Count * Instance.MaxItemInOneColor;
        for (int j = 0; j < Instance.ColorsList.Count; j++)
        {
            var curCont = new GameObject().AddComponent<PoolContainer>();
            curCont.transform.SetParent(Instance.transform);
            curCont.transform.localPosition = Vector3.zero;
            curCont.name = typeof(PoolContainer).Name;
            try
            {
                Instance._containers.Add(Instance.ColorsList[j], curCont);
            }
            catch(ArgumentException e)
            {
                Debug.LogError($"{e.GetType().Name}: CirclePool ��� �������� ��������� � ������: {Instance.ColorsList[j]}");
                continue;
            }
            for (int i = 0; i < Instance.MaxItemInOneColor; i++)
            {
                var circle = Instantiate(Instance.CirclePrefab).GetComponent<Circle>();
                circle.Init(Instance.ColorsList[j]);
                curCont.Add(circle);
                progress?.Report((j * Instance.MaxItemInOneColor + i + 1) / (float)maxItem);
                await UniTask.Yield();
            }
        }
        Instance.IsGenerated = true;
    }
    
    /// <summary>
    /// ����� ���������� ��������� ��������� Circle. ���� ����� ���, �� ���������� null.
    /// </summary>
    /// <returns></returns>
    public static Circle GetRandom()
    {
        var containers = Instance._containers.Values.Where(x => x.HasFree).ToList();
        if(containers.Count > 0)
        {
            var cont = containers[Random.Range(0, containers.Count)];
            if (cont.TryGet(out Circle circle))
            {
                return circle;
            }
        }
        return null;
    }

    /// <summary>
    /// ������������ ��� ����������� Circle ������� � ��� ���������.
    /// </summary>
    /// <param name="circle">Circle ��� �����������</param>
    public static void Return(Circle circle)
    {
        if(circle != null)
        {
            Instance._containers[circle.Color].Return(circle);
        }
    }
}
