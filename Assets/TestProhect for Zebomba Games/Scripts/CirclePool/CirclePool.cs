using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class CirclePool : MonoBehaviour
{
    public static CirclePool Instance { get; private set; }

    [SerializeField, Tooltip("Префаб используемый для создания объекта Circle на сцене")]
    private GameObject CirclePrefab;

    [SerializeField, Tooltip("Список возможных цветов для генерации пула объектов")]
    private List<Color> ColorsList;
    [SerializeField, Range(3, 100), Tooltip("Кол-во сгенерированных объектов одного цвета")]
    private int MaxItemInOneColor = 9;
    public bool IsGenerated { get; private set; } = false;

    private Dictionary<Color, PoolContainer> _containers;
    private void Start()
    {
        if(Instance != null)
        {
            Debug.LogError("Уже существует другой экземпляр CirclePool!\nТекущий объект будет уничтожен.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _containers = new();
    }

    /// <summary>
    /// Генерируется пул кружков, если он не был сгенерирован раньше.
    /// </summary>
    /// <param name="progress">Объект IProgress используется для передачи прогресса генерации.</param>
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
                Debug.LogError($"{e.GetType().Name}: CirclePool уже содержит контейнер с цветом: {Instance.ColorsList[j]}");
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
    /// Метод возвращает случайный свободный Circle. Если таких нет, то возвращает null.
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
    /// Используется для возвращения Circle обратно в пул свободных.
    /// </summary>
    /// <param name="circle">Circle для возвращения</param>
    public static void Return(Circle circle)
    {
        if(circle != null)
        {
            Instance._containers[circle.Color].Return(circle);
        }
    }
}
