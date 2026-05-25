using UnityEngine;

/// <summary>
/// Represents a train station in the game
/// </summary>
public class Station
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public Vector3 Position { get; private set; }
    
    public int PassengerCapacity { get; private set; } = 500;
    public int CargoCapacity { get; private set; } = 1000;
    
    private int currentPassengers;
    private int currentCargo;
    private float reputation = 0.5f; // 0-1 scale
    
    private float lastPassengerGeneration;
    private float lastCargoGeneration;

    public Station(string name, Vector3 position, int id)
    {
        Id = id;
        Name = name;
        Position = position;
        currentPassengers = 0;
        currentCargo = 0;
    }

    /// <summary>
    /// Generate passengers at this station
    /// </summary>
    public void GeneratePassengers(float deltaTime)
    {
        lastPassengerGeneration += deltaTime;

        if (lastPassengerGeneration >= 1f) // Generate every second
        {
            int newPassengers = Random.Range(5, 20);
            AddPassengers(newPassengers);
            lastPassengerGeneration = 0f;
        }
    }

    /// <summary>
    /// Generate cargo at this station
    /// </summary>
    public void GenerateCargo(float deltaTime)
    {
        lastCargoGeneration += deltaTime;

        if (lastCargoGeneration >= 2f) // Generate every 2 seconds
        {
            int newCargo = Random.Range(10, 40);
            AddCargo(newCargo);
            lastCargoGeneration = 0f;
        }
    }

    /// <summary>
    /// Add passengers to the station
    /// </summary>
    public int AddPassengers(int count)
    {
        int canAdd = PassengerCapacity - currentPassengers;
        int toAdd = Mathf.Min(count, canAdd);
        currentPassengers += toAdd;
        return toAdd;
    }

    /// <summary>
    /// Remove passengers from the station (train picks them up)
    /// </summary>
    public int RemovePassengers(int count)
    {
        int toRemove = Mathf.Min(count, currentPassengers);
        currentPassengers -= toRemove;
        return toRemove;
    }

    /// <summary>
    /// Add cargo to the station
    /// </summary>
    public int AddCargo(int count)
    {
        int canAdd = CargoCapacity - currentCargo;
        int toAdd = Mathf.Min(count, canAdd);
        currentCargo += toAdd;
        return toAdd;
    }

    /// <summary>
    /// Remove cargo from the station (train picks it up)
    /// </summary>
    public int RemoveCargo(int count)
    {
        int toRemove = Mathf.Min(count, currentCargo);
        currentCargo -= toRemove;
        return toRemove;
    }

    /// <summary>
    /// Get station status
    /// </summary>
    public string GetStatus()
    {
        return $"Station: {Name}\n" +
               $"Passengers: {currentPassengers}/{PassengerCapacity}\n" +
               $"Cargo: {currentCargo}/{CargoCapacity}\n" +
               $"Reputation: {reputation:P}";
    }

    public int GetPassengerCount() => currentPassengers;
    public int GetCargoCount() => currentCargo;
    public float GetReputation() => reputation;
    
    /// <summary>
    /// Upgrade station capacity
    /// </summary>
    public void UpgradeCapacity()
    {
        PassengerCapacity = (int)(PassengerCapacity * 1.5f);
        CargoCapacity = (int)(CargoCapacity * 1.5f);
    }
}