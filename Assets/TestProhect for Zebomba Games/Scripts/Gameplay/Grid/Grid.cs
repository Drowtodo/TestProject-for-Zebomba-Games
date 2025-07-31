using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField, Tooltip("������ ������� � �����")]
    private List<Column> Columns;

    public Cell this[int colmn, int row] => Columns[colmn][row];

    /// <summary>
    /// ���������� true, ���� ��� ������ � ����� ���������
    /// </summary>
    public bool IsFull { get {return Columns.TrueForAll(x => x.IsFull);} }

    /// <summary>
    /// ���-�� ����� � �����
    /// </summary>
    public int RowCount => Columns?[0].RowCount ?? 0;

    /// <summary>
    /// ���-�� ������� � �����
    /// </summary>
    public int ColumnCount => Columns?.Count ?? 0;

    private void Awake()
    {
        //�������� �� ���������� ���-�� ����� �� ���� ��������
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
    /// ���������� true, ���� ������������ ���������� ��������� ������ �����
    /// </summary>
    /// <param name="column">���������� �������</param>
    /// <param name="row">���������� ������</param>
    /// <returns></returns>
    public bool IsInBounds(int column, int row)
    {
        return column >= 0 && column < ColumnCount && row >= 0 && row < RowCount;
    }

    /// <summary>
    /// ����������� ������, ����������� �� �������� �����������
    /// </summary>
    /// <param name="cellsPosotion">���� ���������� �����</param>
    public void FreeeCells(List<Position> cellsPosotion)
    {
        foreach(var pos in cellsPosotion)
        {
            this[pos.Column, pos.Row].Free();
        }
    }

    /// <summary>
    /// ������ ������������ �����
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
    /// ���������� ����������, ���� ���-�� ����� � ������ �������� �� ���������
    /// </summary>
    class RowCountException : Exception
    {
        public new string Message { get; } = "����� ����� � ������ ��������� �� ���������";
    }
}
