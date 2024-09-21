
public interface INumberOfMoves 
{
    float GetMove();
    float GetCurrentMove();
    void SetMove(int value);
    void ReduceeMove(int damage);


    void IncreaseMove(int amount);
}
