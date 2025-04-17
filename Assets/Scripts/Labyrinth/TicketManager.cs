using UnityEngine;

public class TicketManager : MonoBehaviour
{
    public static TicketManager instance;

   [SerializeField] private int ticketCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTicket();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadTicket()
    {
        
    }

    private void SaveTicket()
    {
      
    }

    public int GetTicketCount()
    {
        return ticketCount;
    }

    public void AddTicket(int amount)
    {
        ticketCount += amount;
        SaveTicket();
    }

    public bool UseTicket()
    {
        if (ticketCount > 0)
        {
            ticketCount--;
            SaveTicket();
            return true;
        }
        return false;
    }

    public bool HasTicket()
    {
        return ticketCount > 0;
    }
}
