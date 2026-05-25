# Trainoo - Train Management Strategy Game

A Unity-based management and strategy game where you build and manage train routes, purchase trains, and optimize your railway empire.

## Game Overview

Trainoo is a sophisticated train management simulation designed for adults who enjoy strategy games. Build an efficient railway network, manage your resources, and make strategic decisions to maximize profits.

## Features

- **Route Building**: Design and construct train routes across a dynamic map
- **Train Management**: Purchase and manage multiple trains with different capabilities
- **Station Network**: Build and upgrade stations to increase efficiency
- **Resource Management**: Monitor finances, fuel, and maintenance costs
- **Real-time Simulation**: Watch your trains move along routes in real-time
- **Strategy Elements**: Optimize routes, manage schedules, and plan expansions
- **Economic Simulation**: Dynamic pricing, demand fluctuation, and market competition

## Game Mechanics

### Resources
- **Money**: Your primary resource for purchasing trains and building infrastructure
- **Passengers/Cargo**: Generated at stations, transported by trains for income
- **Reputation**: Affects demand and prices in your network

### Building System
- Add stations at strategic locations
- Connect stations with train tracks
- Place trains on routes
- Manage train schedules and assignments

### Progression
- Start with limited capital
- Expand your network gradually
- Unlock new train types and technologies
- Increase profitability through optimization

## Project Structure

```
trainoo/
├── Assets/
│   ├── Scripts/
│   │   ├── Game/
│   │   │   ├── GameManager.cs
│   │   │   ├── Station.cs
│   │   │   └── TrainRoute.cs
│   │   ├── Trains/
│   │   │   └── Train.cs
│   │   ├── UI/
│   │   │   └── UIManager.cs
│   │   └── Visualization/
│   │       └── GameMapRenderer.cs
│   ├── Scenes/
│   ├── Prefabs/
│   └── Resources/
├── ProjectSettings/
└── README.md
```

## Requirements

- Unity 2022 LTS or later
- C# 9.0 or later
- TextMesh Pro (included with Unity)

## Getting Started

1. Clone the repository
2. Open the project in Unity
3. Create a new scene or load the main scene
4. Set up the UI Canvas with the required text and button elements
5. Attach GameManager, UIManager, and GameMapRenderer to appropriate GameObjects
6. Press Play to start the game

## Initial Setup Instructions

### Step 1: Create UI Canvas
- Right-click in Hierarchy → UI → Canvas
- Add Text elements for: Balance, Day, Train Count, Station Count
- Add Buttons for: Next Day, Pause, Speed Up, Add Station, Add Train, Build Route

### Step 2: Attach Scripts
- Create empty GameObject "GameManager" → Attach GameManager.cs
- Create empty GameObject "UIManager" → Attach UIManager.cs
- Create empty GameObject "GameMapRenderer" → Attach GameMapRenderer.cs

### Step 3: Configure References
- Drag UI elements to UIManager script fields in Inspector
- Assign materials to GameMapRenderer if desired

## Game Features

### Train Types
- **Passenger Train**: High passenger capacity, medium speed
- **Cargo Train**: High cargo capacity, slower speed
- **Fast Train**: Lower capacity, high speed
- **Hybrid Train**: Balanced passenger and cargo capacity

### Economics
- Generate income from transporting passengers and cargo
- Pay maintenance costs for trains and stations
- Manage fuel consumption
- Track daily profits and losses

## Development Status

Early Development - Core mechanics being implemented

## Future Features
- Advanced route optimization
- Train scheduling system
- Economic events and disasters
- Multiplayer support
- Save/Load system
- Advanced graphics and animations

## License

MIT License - See LICENSE file for details