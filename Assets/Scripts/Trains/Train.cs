using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Different types of trains available in the game
/// </summary>
public enum TrainType
{
    PassengerTrain,
    CargoTrain,
    FastTrain,
    HybridTrain
}

/// <summary>
/// Represents a train that travels on routes and transports goods
/// </summary>
public class Train
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public TrainType Type { get; private set; }
    public bool IsActive { get; set; }

    // Capacity based on train type
    public int PassengerCapacity { get; private set; }
    public int CargoCapacity { get; private set; }

    // Current load
    private int currentPassengers;
    private int currentCargo;

    // Movement and routes
    private TrainRoute currentRoute;
    private float positionOnRoute; // 0-1, where 0 is start and 1 is end
    private float speed; // Units per second

    // Economics
    private float fuelLevel;
    private float maintenanceHealth = 100f;
    private float dailyIncomeGenerated;

    public Train(string name, TrainType type, int id)
    {
        Id = id;
        Name = name;
        Type = type;
        IsActive = true;
        currentPassengers = 0;
        currentCargo = 0;
        positionOnRoute = 0f;
        fuelLevel = 100f;

        // Set train properties based on type
        switch (type)
        {
            case TrainType.PassengerTrain:
                PassengerCapacity = 500;
                CargoCapacity = 100;
                speed = 10f;
                break;
            case TrainType.CargoTrain:
                PassengerCapacity = 50;
                CargoCapacity = 1000;
                speed = 8f;
                break;
            case TrainType.FastTrain:
                PassengerCapacity = 300;
                CargoCapacity = 50;
                speed = 15f;
                break;
            case TrainType.HybridTrain:
                PassengerCapacity = 300;
                CargoCapacity = 300;
                speed = 10f;
                break;
        }
    }

    /// <summary>
    /// Assign a route to this train
    /// </summary>
    public void AssignRoute(TrainRoute route)
    {
        currentRoute = route;
        positionOnRoute = 0f;
    }

    /// <summary>
    /// Update train movement and generate income
    /// </summary>
    public void UpdateTrain(float deltaTime)
    {
        if (!IsActive || currentRoute == null) return;

        // Move along route
        float distanceThisFrame = speed * deltaTime;
        float routeDistance = currentRoute.GetDistance();
        positionOnRoute += (distanceThisFrame / routeDistance);

        // Handle route completion
        if (positionOnRoute >= 1f)
        {
            CompleteRoute();
            positionOnRoute = 0f;
        }

        // Consume fuel and update maintenance
        ConsumeFuel(deltaTime);
        UpdateMaintenance(deltaTime);
    }

    private void ConsumeFuel(float deltaTime)
    {
        float fuelConsumption = speed * 0.1f * deltaTime;
        fuelLevel -= fuelConsumption;

        if (fuelLevel <= 0f)
        {
            fuelLevel = 0f;
            IsActive = false;
            Debug.Log($"Train {Name} is out of fuel!");
        }
    }

    private void UpdateMaintenance(float deltaTime)
    {
        float maintenanceLoss = 0.5f * deltaTime; // Lose maintenance over time
        maintenanceHealth -= maintenanceLoss;

        if (maintenanceHealth <= 0f)
        {
            maintenanceHealth = 0f;
            IsActive = false;
            Debug.Log($"Train {Name} needs maintenance!");
        }
    }

    private void CompleteRoute()
    {
        // Generate income from passengers and cargo transported
        float passengerIncome = currentPassengers * 50f;
        float cargoIncome = currentCargo * 25f;
        
        dailyIncomeGenerated += passengerIncome + cargoIncome;

        GameManager.Instance.AddMoney(passengerIncome + cargoIncome);

        // Unload and pick up cargo at destination
        currentPassengers = 0;
        currentCargo = 0;

        Debug.Log($"Train {Name} completed route. Income: ${passengerIncome + cargoIncome}");
    }

    /// <summary>
    /// Pick up passengers at a station
    /// </summary>
    public int LoadPassengers(Station station, int maxCapacity)
    {
        int canLoad = PassengerCapacity - currentPassengers;
        int toLoad = Mathf.Min(maxCapacity, canLoad);
        int loaded = station.RemovePassengers(toLoad);
        currentPassengers += loaded;
        return loaded;
    }

    /// <summary>
    /// Pick up cargo at a station
    /// </summary>
    public int LoadCargo(Station station, int maxCapacity)
    {
        int canLoad = CargoCapacity - currentCargo;
        int toLoad = Mathf.Min(maxCapacity, canLoad);
        int loaded = station.RemoveCargo(toLoad);
        currentCargo += loaded;
        return loaded;
    }

    /// <summary>
    /// Refuel the train
    /// </summary>
    public void Refuel()
    {
        fuelLevel = 100f;
    }

    /// <summary>
    /// Maintain the train
    /// </summary>
    public void Maintain()
    {
        maintenanceHealth = 100f;
        IsActive = true;
    }

    /// <summary>
    /// Get daily income for this train
    /// </summary>
    public float GetDailyIncome()
    {
        float income = dailyIncomeGenerated;
        dailyIncomeGenerated = 0f;
        return income;
    }

    /// <summary>
    /// Get train status
    /// </summary>
    public string GetStatus()
    {
        return $"Train: {Name}\n" +
               $"Type: {Type}\n" +
               $"Passengers: {currentPassengers}/{PassengerCapacity}\n" +
               $"Cargo: {currentCargo}/{CargoCapacity}\n" +
               $"Fuel: {fuelLevel:F1}%\n" +
               $"Maintenance: {maintenanceHealth:F1}%\n" +
               $"Status: {(IsActive ? "Active" : "Inactive")}";
    }

    public Vector3 GetCurrentPosition()
    {
        if (currentRoute == null) return Vector3.zero;
        return Vector3.Lerp(currentRoute.GetStartPosition(), currentRoute.GetEndPosition(), positionOnRoute);
    }

    public float GetFuelLevel() => fuelLevel;
    public float GetMaintenanceHealth() => maintenanceHealth;
    public int GetPassengerCount() => currentPassengers;
    public int GetCargoCount() => currentCargo;
}