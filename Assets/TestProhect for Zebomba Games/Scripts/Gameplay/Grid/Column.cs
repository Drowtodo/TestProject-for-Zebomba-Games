using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField, Tooltip("Список ячеек в колонке")]
    private List<Cell> Cells;

    public Cell this[int index] => Cells[index];

    /// <summary>
    /// Возвращает true, если все ячейки в колонке заполнены
    /// </summary>
    public bool IsFull { get { return Cells.TrueForAll((x) => !x.IsFree); } }

    /// <summary>
    /// Кол-во строк в колонке
    /// </summary>
    public int RowCount => Cells?.Count ?? 0;

    /// <summary>
    /// Позиция колонки в сетке
    /// </summary>
    public int? Position { get; private set; }

    /// <summary>
    /// Если Circle попал в самую нижнию свободную ячейку, то она закрывается.
    /// </summary>
    /// <param name="cell">Проверяемая ячейка</param>
    /// <param name="circle">Попавший Circle</param>
    public void OnCircleEnterInCell(Cell cell, Circle circle)
    {
        int previousIndex = Cells.IndexOf(cell) - 1;
        if (previousIndex == -1 || (previousIndex >= 0 && !Cells[previousIndex].IsFree))
        {
            Cells[previousIndex + 1].Lock(circle);
            GameplayController.CheckGrid(cell);
        }
    }

    /// <summary>
    /// Установка координаты колонки, если ранее она не была установлена
    /// </summary>
    /// <param name="column"></param>
    public void SetPosition(int column)
    {
        if(Position == null)
        {
            Position = column;
            for (int row = 0; row < Cells.Count; row++)
            {
                Cells[row].SetPosition(column, row);
            }
        }
    }
}
