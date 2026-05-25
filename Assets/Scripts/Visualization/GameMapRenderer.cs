using UnityEngine;

/// <summary>
/// Handles visual rendering of the game map, stations, and trains
/// </summary>
public class GameMapRenderer : MonoBehaviour
{
    [SerializeField] private Material stationMaterial;
    [SerializeField] private Material trainMaterial;
    [SerializeField] private Material routeMaterial;
    [SerializeField] private float stationRadius = 2f;
    [SerializeField] private float trainRadius = 1f;

    private void Start()
    {
        if (stationMaterial == null)
            stationMaterial = new Material(Shader.Find("Standard"));
        if (trainMaterial == null)
            trainMaterial = new Material(Shader.Find("Standard"));
        if (routeMaterial == null)
            routeMaterial = new Material(Shader.Find("Standard"));
    }

    private void Update()
    {
        DrawGameMap();
    }

    private void DrawGameMap()
    {
        // Draw all stations
        foreach (Station station in GameManager.Instance.GetAllStations())
        {
            DrawStation(station);
        }

        // Draw all routes
        foreach (TrainRoute route in GameManager.Instance.GetAllRoutes())
        {
            DrawRoute(route);
        }

        // Draw all trains
        foreach (Train train in GameManager.Instance.GetAllTrains())
        {
            DrawTrain(train);
        }
    }

    private void DrawStation(Station station)
    {
        // Create a visual representation for each station
        Vector3 position = station.Position;
        
        // Use Gizmos for visualization (only visible in editor)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(position, stationRadius);
        
        // Draw label
        Debug.Log($"Station {station.Name} at {position}");
    }

    private void DrawRoute(TrainRoute route)
    {
        Vector3 start = route.GetStartPosition();
        Vector3 end = route.GetEndPosition();
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(start, end);
    }

    private void DrawTrain(Train train)
    {
        Vector3 position = train.GetCurrentPosition();
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, trainRadius);
    }

    /// <summary>
    /// Create visual GameObjects for persistent rendering
    /// </summary>
    public void CreateStationVisuals()
    {
        foreach (Station station in GameManager.Instance.GetAllStations())
        {
            GameObject stationGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            stationGO.name = $"Station_{station.Id}";
            stationGO.transform.position = station.Position;
            stationGO.transform.localScale = Vector3.one * stationRadius * 2;
            stationGO.GetComponent<Renderer>().material = stationMaterial;
            
            // Remove collider as we don't need physics for visualization
            Collider collider = stationGO.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);
        }
    }

    /// <summary>
    /// Create visual GameObjects for trains
    /// </summary>
    public void CreateTrainVisuals()
    {
        foreach (Train train in GameManager.Instance.GetAllTrains())
        {
            GameObject trainGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trainGO.name = $"Train_{train.Id}";
            trainGO.transform.localScale = Vector3.one * trainRadius * 2;
            trainGO.GetComponent<Renderer>().material = trainMaterial;
            
            // Remove collider
            Collider collider = trainGO.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);
            
            // Add component to update position
            TrainVisual trainVisual = trainGO.AddComponent<TrainVisual>();
            trainVisual.SetTrain(train);
        }
    }
}

/// <summary>
/// Component to update train visual position in real-time
/// </summary>
public class TrainVisual : MonoBehaviour
{
    private Train train;

    public void SetTrain(Train trainRef)
    {
        train = trainRef;
    }

    private void Update()
    {
        if (train != null)
        {
            transform.position = train.GetCurrentPosition();
        }
    }
}