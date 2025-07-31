/// <summary>
/// Класс хранящий информацию о координтах
/// </summary>
public record Position
{
    public int Row { get; }
    public int Column { get; }

    public Position (int column, int row)
    {
        Row = row;
        Column = column;
    }
}
