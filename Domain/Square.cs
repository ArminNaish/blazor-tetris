namespace BlazorTetris.Domain;

public class Square
{
    public int Index { get; set; }
    public bool IsTetromino { get; set; }
    public bool IsTaken { get; set; }
}