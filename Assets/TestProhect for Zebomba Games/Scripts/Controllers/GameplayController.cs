using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance { get; private set; }

    [Tooltip("Основной маятник")]
    public Pendulum Pendulum;
    [Tooltip("Основная сетка, внутри которой проверяются совпадения")]
    public Grid CirclesGrid;

    [Range(0.1f, 5f), Tooltip("Задержка перед появлением нового круга")]
    public float CircleDelay = 1f;
    [Range(2, 10), Tooltip("Кол-во кругов подряд для удаления линии")]
    public int MatchLength = 3;

    private int _points = 0;
    /// <summary>
    /// Кол-во очков заработанных входе одной игры
    /// </summary>
    public int Points
    {
        get { return _points; }
        private set
        {
            _points = value;
            OnPointsChange?.Invoke(value);
        }
    }
    [Tooltip("Событие вызывается при изменении кол-ва заработанных пользователем очков")]
    public UnityEvent<int> OnPointsChange;
    [Tooltip("Событие вызывается, когда изменяется следующий круг")]
    public UnityEvent<Circle> OnNextCircleChange;

    private Circle _nextCircle;

    private readonly Position[] _searchDirections = 
    {  
        new(0, 1), //горизонталь
        new(1, 0), //вертикаль
        new(1, 1), //диагональ ↘
        new(1, -1) //диагональ ↙
    };

    private void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("Уже существует другой экземпляр GameplayController!\nТекущий объект будет уничтожен.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Используется для начала игры
    /// </summary>
    public static void GameStart()
    {
        Instance.Points = 0;
        Instance.SetCircle();
        Instance.Pendulum.gameObject.SetActive(true);
    }

    /// <summary>
    /// Используется для окончания игры
    /// </summary>
    public static void GameEnd()
    {
        Instance.Pendulum.Free();
        Instance.CirclesGrid.Clear();
        CirclePool.Return(Instance._nextCircle);
        Instance._nextCircle = null;
    }

    /// <summary>
    /// Установка нового Circle на маятник
    /// </summary>
    private void SetCircle()
    {
        if(_nextCircle == null)
        {
            Pendulum.Add(CirclePool.GetRandom());
        }
        else
        {
            Pendulum.Add(_nextCircle);
        }
        _nextCircle = CirclePool.GetRandom();
        OnNextCircleChange?.Invoke(_nextCircle);
    }

    /// <summary>
    /// Сначала маятник освобождается от Circle, после задержки на него устанавливается новый Circle
    /// </summary>
    public static async void FreePendulum()
    {
        Instance.Pendulum.Free();
        await UniTask.WaitForSeconds(Instance.CircleDelay);
        Instance.SetCircle();
    }

    /// <summary>
    /// Проверка сетки на совпадения и заполненность, с автоматическим запуском очистки необходимых ячеек и подсчётом очков.
    /// </summary>
    /// <param name="startCell">Ячекйка, с которой начинается проверка</param>
    public static void CheckGrid(Cell startCell)
    {
        var list = Instance.GetPostionsForClear(startCell.Position.Column, startCell.Position.Row, startCell.Circle.Color);
        if(list.Count >= Instance.MatchLength)
        {
            Instance.Points += list.Count * (list.Count / Instance.MatchLength);
            Instance.CirclesGrid.FreeeCells(list);
        }
        else if(Instance.CirclesGrid.IsFull)
        {
            GameStateMachine.ChangeState(GameStateMachine.Current.EndState);
        }
    }

    #region Вспомогательные методы для проверки сетки
    private List<Position> GetPostionsForClear(int startColumn, int startRow, Color targetColor)
    {
        var list = new List<Position>() { new(startColumn, startRow) };

        for(int i = 0; i<_searchDirections.Length; i++)
        {
            var temp = new List<Position>();
            if(CheckLine(startColumn, startRow, _searchDirections[i].Column, _searchDirections[i].Row, targetColor, ref temp) >= MatchLength)
            {
                list.AddRange(temp);
            }
        }

        return list;
    }

    private int CheckLine(int startColumn, int startRow, int deltaColumn, int deltaRow, Color targetColor, ref List<Position> positionList)
    {
        int count = 1;
        count += CountMatches(startColumn, startRow, deltaColumn, deltaRow, targetColor, ref positionList);
        count += CountMatches(startColumn, startRow, -deltaColumn, -deltaRow, targetColor, ref positionList);
        return count;
    }

    private int CountMatches(int startColumn, int startRow, int deltaColumn, int deltaRow, Color targetColor, ref List<Position> positionList)
    {
        int count = 0;
        int column = startColumn + deltaColumn;
        int row = startRow + deltaRow;

        while(CirclesGrid.IsInBounds(column, row) && !CirclesGrid[column, row].IsFree && CirclesGrid[column, row].Circle.Color == targetColor)
        {
            count++;
            positionList.Add(new Position(column, row));
            column += deltaColumn;
            row += deltaRow;
        }
        return count;
    }
    #endregion
}
