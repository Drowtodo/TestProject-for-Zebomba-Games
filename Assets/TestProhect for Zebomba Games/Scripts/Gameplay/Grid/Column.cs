using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField, Tooltip("������ ����� � �������")]
    private List<Cell> Cells;

    public Cell this[int index] => Cells[index];

    /// <summary>
    /// ���������� true, ���� ��� ������ � ������� ���������
    /// </summary>
    public bool IsFull { get { return Cells.TrueForAll((x) => !x.IsFree); } }

    /// <summary>
    /// ���-�� ����� � �������
    /// </summary>
    public int RowCount => Cells?.Count ?? 0;

    /// <summary>
    /// ������� ������� � �����
    /// </summary>
    public int? Position { get; private set; }

    /// <summary>
    /// ���� Circle ����� � ����� ������ ��������� ������, �� ��� �����������.
    /// </summary>
    /// <param name="cell">����������� ������</param>
    /// <param name="circle">�������� Circle</param>
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
    /// ��������� ���������� �������, ���� ����� ��� �� ���� �����������
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
