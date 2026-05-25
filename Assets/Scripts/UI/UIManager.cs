using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages all UI elements and player interactions
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI trainCountText;
    [SerializeField] private TextMeshProUGUI stationCountText;
    [SerializeField] private Transform stationsListContainer;
    [SerializeField] private Transform trainsListContainer;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button nextDayButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button speedUpButton;
    [SerializeField] private Button addStationButton;
    [SerializeField] private Button addTrainButton;
    [SerializeField] private Button buildRouteButton;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Subscribe to game events
        GameManager.Instance.OnBalanceChanged += UpdateBalance;
        GameManager.Instance.OnDayChanged += UpdateDay;
        GameManager.Instance.OnGameStateChanged += UpdateGameState;

        // Button listeners
        nextDayButton.onClick.AddListener(OnNextDayClick);
        pauseButton.onClick.AddListener(OnPauseClick);
        speedUpButton.onClick.AddListener(OnSpeedUpClick);
        addStationButton.onClick.AddListener(OnAddStationClick);
        addTrainButton.onClick.AddListener(OnAddTrainClick);
        buildRouteButton.onClick.AddListener(OnBuildRouteClick);

        // Initial update
        UpdateBalance(GameManager.Instance.GetBalance());
        UpdateDay(GameManager.Instance.GetCurrentDay());
    }

    private void Update()
    {
        // Continuous UI updates
        UpdateTrainCount();
        UpdateStationCount();
    }

    #region Update Methods

    private void UpdateBalance(float balance)
    {
        if (balanceText != null)
        {
            balanceText.text = $"${balance:F0}";
        }
    }

    private void UpdateDay(int day)
    {
        if (dayText != null)
        {
            dayText.text = $"Day {day}";
        }
    }

    private void UpdateTrainCount()
    {
        int activeTrains = GameManager.Instance.GetActiveTrainCount();
        if (trainCountText != null)
        {
            trainCountText.text = activeTrains.ToString();
        }
    }

    private void UpdateStationCount()
    {
        int stations = GameManager.Instance.GetStationCount();
        if (stationCountText != null)
        {
            stationCountText.text = stations.ToString();
        }
    }

    private void UpdateGameState(bool paused)
    {
        isPaused = paused;
        pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = isPaused ? "Resume" : "Pause";
    }

    #endregion

    #region Button Callbacks

    private void OnNextDayClick()
    {
        GameManager.Instance.NextDay();
        UpdateStationsList();
        UpdateTrainsList();
    }

    private void OnPauseClick()
    {
        GameManager.Instance.TogglePause();
    }

    private void OnSpeedUpClick()
    {
        GameManager.Instance.SpeedUp();
    }

    private void OnAddStationClick()
    {
        if (GameManager.Instance.CanAfford(5000f))
        {
            Vector3 randomPosition = new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f));
            string stationName = $"Station {GameManager.Instance.GetStationCount() + 1}";
            GameManager.Instance.CreateStation(randomPosition, stationName);
            UpdateStationsList();
        }
        else
        {
            ShowNotification("Insufficient funds for station!");
        }
    }

    private void OnAddTrainClick()
    {
        if (GameManager.Instance.CanAfford(10000f))
        {
            TrainType type = (TrainType)(Random.Range(0, 4));
            string trainName = $"Train {GameManager.Instance.GetActiveTrainCount() + 1}";
            GameManager.Instance.CreateTrain(trainName, type);
            UpdateTrainsList();
        }
        else
        {
            ShowNotification("Insufficient funds for train!");
        }
    }

    private void OnBuildRouteClick()
    {
        var stations = GameManager.Instance.GetAllStations();
        if (stations.Count >= 2)
        {
            Station start = stations[Random.Range(0, stations.Count)];
            Station end = stations[Random.Range(0, stations.Count)];
            
            if (start != end)
            {
                float distance = Vector3.Distance(start.Position, end.Position);
                if (GameManager.Instance.CanAfford(distance * 2000f))
                {
                    GameManager.Instance.CreateRoute(start, end, distance);
                }
                else
                {
                    ShowNotification("Insufficient funds for route!");
                }
            }
        }
        else
        {
            ShowNotification("Need at least 2 stations to create a route!");
        }
    }

    #endregion

    #region List Updates

    private void UpdateStationsList()
    {
        // Clear existing list
        foreach (Transform child in stationsListContainer)
        {
            Destroy(child.gameObject);
        }

        // Add each station
        foreach (Station station in GameManager.Instance.GetAllStations())
        {
            CreateStationListItem(station);
        }
    }

    private void CreateStationListItem(Station station)
    {
        GameObject item = new GameObject($"Station_{station.Id}");
        item.transform.SetParent(stationsListContainer);
        
        TextMeshProUGUI text = item.AddComponent<TextMeshProUGUI>();
        text.text = $"{station.Name}\nPass: {station.GetPassengerCount()}, Cargo: {station.GetCargoCount()}";
    }

    private void UpdateTrainsList()
    {
        // Clear existing list
        foreach (Transform child in trainsListContainer)
        {
            Destroy(child.gameObject);
        }

        // Add each train
        foreach (Train train in GameManager.Instance.GetAllTrains())
        {
            CreateTrainListItem(train);
        }
    }

    private void CreateTrainListItem(Train train)
    {
        GameObject item = new GameObject($"Train_{train.Id}");
        item.transform.SetParent(trainsListContainer);
        
        TextMeshProUGUI text = item.AddComponent<TextMeshProUGUI>();
        text.text = $"{train.Name} ({train.Type})\nFuel: {train.GetFuelLevel():F0}%";
    }

    #endregion

    #region Notifications

    private void ShowNotification(string message)
    {
        Debug.Log(message);
        if (infoText != null)
        {
            infoText.text = message;
        }
    }

    #endregion

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBalanceChanged -= UpdateBalance;
            GameManager.Instance.OnDayChanged -= UpdateDay;
            GameManager.Instance.OnGameStateChanged -= UpdateGameState;
        }
    }
}