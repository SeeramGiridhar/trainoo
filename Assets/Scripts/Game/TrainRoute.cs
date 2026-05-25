using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a train route connecting two stations
/// </summary>
public class TrainRoute
{
    public int Id { get; private set; }
    public Station StartStation { get; private set; }
    public Station EndStation { get; private set; }
    public float Distance { get; private set; }
    
    private List<Train> trainsOnRoute = new List<Train>();
    private float profitability = 0.5f; // 0-1 scale

    private static int routeCounter = 0;

    public TrainRoute(Station startStation, Station endStation, float distance)
    {
        Id = routeCounter++;
        StartStation = startStation;
        EndStation = endStation;
        Distance = distance;
    }

    /// <summary>
    /// Add a train to this route
    /// </summary>
    public void AddTrain(Train train)
    {
        if (!trainsOnRoute.Contains(train))
        {
            trainsOnRoute.Add(train);
            train.AssignRoute(this);
        }
    }

    /// <summary>
    /// Remove a train from this route
    /// </summary>
    public void RemoveTrain(Train train)
    {
        trainsOnRoute.Remove(train);
    }

    /// <summary>
    /// Get all trains on this route
    /// </summary>
    public List<Train> GetTrainsOnRoute()
    {
        return new List<Train>(trainsOnRoute);
    }

    /// <summary>
    /// Calculate profitability based on demand and passenger/cargo volume
    /// </summary>
    public void UpdateProfitability()
    {
        int totalPassengers = StartStation.GetPassengerCount() + EndStation.GetPassengerCount();
        int totalCargo = StartStation.GetCargoCount() + EndStation.GetCargoCount();
        
        // Profitability increases with demand, but decreases if trains are not picking up goods
        float demandFactor = (totalPassengers + totalCargo) / 1000f;
        demandFactor = Mathf.Clamp01(demandFactor);
        
        profitability = Mathf.Lerp(profitability, demandFactor, 0.1f);
    }

    /// <summary>
    /// Get route profitability
    /// </summary>
    public float GetProfitability()
    {
        return profitability;
    }

    /// <summary>
    /// Get route information
    /// </summary>
    public string GetStatus()
    {
        return $"Route: {StartStation.Name} → {EndStation.Name}\n" +
               $"Distance: {Distance:F1} units\n" +
               $"Trains: {trainsOnRoute.Count}\n" +
               $"Profitability: {profitability:P}";
    }

    public float GetDistance() => Distance;
    public Vector3 GetStartPosition() => StartStation.Position;
    public Vector3 GetEndPosition() => EndStation.Position;
}