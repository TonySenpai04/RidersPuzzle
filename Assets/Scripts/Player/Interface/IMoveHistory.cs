using System;

public interface IMoveHistory
{
    public void AddMove(int row, int col);

    public Tuple<int, int> UndoMove();

    public Tuple<int, int> GetLastMove(); // Lấy nhưng không xóa
 
    public bool HasHistory();

    public void ClearHistory();
    public void GetAllHistory();
}