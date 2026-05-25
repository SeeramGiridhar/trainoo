using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Core game manager that handles game state, resources, and overall game flow
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float initialBalance = 50000f;
    
    private float currentBalance;
    private int currentDay;
    private bool isPaused;
    private float gameSpeed = 1f;

    private List<Station> stations = new List<Station>();
    private List<Train> trains = new List<Train>();
    private List<TrainRoute> routes = new List<TrainRoute>();

    // Events for UI updates
    public delegate void BalanceChangedHandler(float newBalance);
    public event BalanceChangedHandler OnBalanceChanged;

    public delegate void DayChangedHandler(int newDay);
    public event DayChangedHandler OnDayChanged;

    public delegate void GameStateChangedHandler(bool paused);
    public event GameStateChangedHandler OnGameStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeGame();
    }

    private void InitializeGame()
    {
        currentBalance = initialBalance;
        currentDay = 1;
        isPaused = false;
        gameSpeed = 1f;

        OnBalanceChanged?.Invoke(currentBalance);
        OnDayChanged?.Invoke(currentDay);
    }

    private void Update()
    {
        if (isPaused) return;

        // Update all trains
        foreach (Train train in trains)
        {
            train.UpdateTrain(Time.deltaTime * gameSpeed);
        }
    }

    #region Money Management

    /// <summary>
    /// Add money to player balance
    /// </summary>
    public bool AddMoney(float amount)
    {
        currentBalance += amount;
        OnBalanceChanged?.Invoke(currentBalance);
        return true;
    }

    /// <summary>
    /// Spend money from player balance
    /// </summary>
    public bool SpendMoney(float amount)
    {
        if (currentBalance >= amount)
        {
            currentBalance -= amount;
            OnBalanceChanged?.Invoke(currentBalance);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if player can afford an item
    /// </summary>
    public bool CanAfford(float cost)
    {
        return currentBalance >= cost;
    }

    public float GetBalance()
    {
        return currentBalance;
    }

    #endregion

    #region Day Management

    /// <summary>
    /// Advance to the next day and process daily income
    /// </summary>
    public void NextDay()
    {
        currentDay++;
        ProcessDailyIncome();
        OnDayChanged?.Invoke(currentDay);
    }

    private void ProcessDailyIncome()
    {
        float dailyIncome = 0f;

        // Calculate income from all trains and routes
        foreach (Train train in trains)
        {
            if (train.IsActive)
            {
                dailyIncome += train.GetDailyIncome();
            }
        }

        // Subtract maintenance costs
        float maintenanceCost = trains.Count * 500f; // Base maintenance per train
        maintenanceCost += stations.Count * 200f; // Base maintenance per station

        float netIncome = dailyIncome - maintenanceCost;
        AddMoney(netIncome);

        Debug.Log($"Day {currentDay}: Income: ${dailyIncome}, Maintenance: ${maintenanceCost}, Net: ${netIncome}");
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }

    #endregion

    #region Pause and Speed Control

    /// <summary>
    /// Toggle game pause state
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;
        OnGameStateChanged?.Invoke(isPaused);
    }

    /// <summary>
    /// Set game speed multiplier
    /// </summary>
    public void SetGameSpeed(float speed)
    {
        gameSpeed = Mathf.Max(0.1f, speed);
    }

    /// <summary>
    /// Increase game speed
    /// </summary>
    public void SpeedUp()
    {
        gameSpeed = Mathf.Min(gameSpeed * 1.5f, 3f);
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    #endregion

    #region Station Management

    /// <summary>
    /// Create a new station at the specified position
    /// </summary>
    public Station CreateStation(Vector3 position, string stationName)
    {
        if (!CanAfford(5000f))
        {
            Debug.Log("Insufficient funds to create station");
            return null;
        }

        SpendMoney(5000f);

        Station station = new Station(stationName, position, stations.Count + 1);
        stations.Add(station);

        Debug.Log($"Station created: {stationName}");
        return station;
    }

    /// <summary>
    /// Get all stations in the game
    /// </summary>
    public List<Station> GetAllStations()
    {
        return new List<Station>(stations);
    }

    public int GetStationCount()
    {
        return stations.Count;
    }

    #endregion

    #region Train Management

    /// <summary>
    /// Create a new train for the player
    /// </summary>
    public Train CreateTrain(string trainName, TrainType type)
    {
        if (!CanAfford(10000f))
        {
            Debug.Log("Insufficient funds to purchase train");
            return null;
        }

        SpendMoney(10000f);

        Train train = new Train(trainName, type, trains.Count + 1);
        trains.Add(train);

        Debug.Log($"Train created: {trainName}");
        return train;
    }

    /// <summary>
    /// Get all trains in the game
    /// </summary>
    public List<Train> GetAllTrains()
    {
        return new List<Train>(trains);
    }

    /// <summary>
    /// Get active trains count
    /// </summary>
    public int GetActiveTrainCount()
    {
        return trains.FindAll(t => t.IsActive).Count;
    }

    #endregion

    #region Route Management

    /// <summary>
    /// Create a route between two stations
    /// </summary>
    public TrainRoute CreateRoute(Station startStation, Station endStation, float distance)
    {
        if (!CanAfford(distance * 2000f))
        {
            Debug.Log("Insufficient funds to build route");
            return null;
        }

        SpendMoney(distance * 2000f);

        TrainRoute route = new TrainRoute(startStation, endStation, distance);
        routes.Add(route);

        Debug.Log($"Route created from {startStation.Name} to {endStation.Name}");
        return route;
    }

    /// <summary>
    /// Get all routes in the game
    /// </summary>
    public List<TrainRoute> GetAllRoutes()
    {
        return new List<TrainRoute>(routes);
    }

    #endregion
}