using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField, Tooltip("Список колонок в сетке")]
    private List<Column> Columns;

    public Cell this[int colmn, int row] => Columns[colmn][row];

    /// <summary>
    /// Возвращает true, если все ячейки в сетке заполнены
    /// </summary>
    public bool IsFull { get {return Columns.TrueForAll(x => x.IsFull);} }

    /// <summary>
    /// Кол-во строк в сетке
    /// </summary>
    public int RowCount => Columns?[0].RowCount ?? 0;

    /// <summary>
    /// Кол-во колонок в сетке
    /// </summary>
    public int ColumnCount => Columns?.Count ?? 0;

    private void Awake()
    {
        //Проверка на совпадение кол-ва строк во всех колонках
        foreach(Column c in Columns)
        {
            if(c.RowCount != RowCount)
            {
                throw new RowCountException();
            }
        }

        for (int colmn = 0; colmn < Columns.Count; colmn++)
        {
            Columns[colmn].SetPosition(colmn);
        }
    }

    /// <summary>
    /// Возвращает true, если передаваемые координаты находятся внутри сетки
    /// </summary>
    /// <param name="column">Координаты колонки</param>
    /// <param name="row">Координата строки</param>
    /// <returns></returns>
    public bool IsInBounds(int column, int row)
    {
        return column >= 0 && column < ColumnCount && row >= 0 && row < RowCount;
    }

    /// <summary>
    /// Освобождает ячейки, находящиеся по заданным координатам
    /// </summary>
    /// <param name="cellsPosotion">Лист координата ячеек</param>
    public void FreeeCells(List<Position> cellsPosotion)
    {
        foreach(var pos in cellsPosotion)
        {
            this[pos.Column, pos.Row].Free();
        }
    }

    /// <summary>
    /// Полное освобождение сетки
    /// </summary>
    public void Clear()
    {
        foreach (var col in Columns)
        {
            for(int i = 0; i<col.RowCount; i++)
            {
                col[i].Clear();
            }
        }
    }

    /// <summary>
    /// Исключение вызываемое, если кол-во строк в разных колонках не совпадает
    /// </summary>
    class RowCountException : Exception
    {
        public new string Message { get; } = "Число строк в разных колоннках не совпадает";
    }
}
