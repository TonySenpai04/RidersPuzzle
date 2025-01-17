using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveHistory:IMoveHistory
{
    private Stack<Tuple<int, int>> moveHistory;

    public MoveHistory()
    {
        moveHistory = new Stack<Tuple<int, int>>();
    }

    // Thêm vị trí mới vào lịch sử
    public void AddMove(int row, int col)
    {
        moveHistory.Push(Tuple.Create(row, col));
    }
    public void GetAllHistory()
    {
        foreach (var item in moveHistory)
        {
            Debug.Log(item);
        }
    }

    // Truy xuất vị trí cuối cùng và xóa khỏi lịch sử
    public Tuple<int, int> UndoMove()
    {
        if (moveHistory.Count > 0)
        {
            return moveHistory.Pop();
        }
        return null;
    }

    // Kiểm tra xem có lịch sử để undo hay không
    public bool HasHistory()
    {
        return moveHistory.Count > 0;
    }
    public Tuple<int, int> GetLastMove() // Lấy nhưng không xóa
    {
        if (moveHistory.Count > 0)
        {
            return moveHistory.Peek(); // Lấy vị trí cuối cùng nhưng không xóa nó khỏi stack
        }
        return null;
    }

    // Xóa toàn bộ lịch sử di chuyển
    public void ClearHistory()
    {
        moveHistory.Clear();
    }
}
