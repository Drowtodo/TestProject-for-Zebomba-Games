using TMPro;
using UnityEngine;

/// <summary>
/// ��������������� ����� ��� ������ ��������� ���-�� �����
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class PointsUpdater : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void OnPointsChange(int points)
    {
        _text.text = points.ToString();
    }
}
