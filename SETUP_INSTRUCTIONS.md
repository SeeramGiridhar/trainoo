# Trainoo - Quick Setup Guide 🚂

## One-Click Setup Instructions

Follow these simple steps to get your game ready to play:

### Step 1: Create Initializer GameObject
- In Unity Hierarchy, right-click
- Select: **Create Empty**
- Name it: `Initializer`

### Step 2: Attach SceneSetup Script
- With `Initializer` selected
- In Inspector, click **Add Component**
- Search for and add: **SceneSetup**

### Step 3: Run Setup
- With `Initializer` still selected
- Find the **SceneSetup** component in Inspector
- Click the dropdown menu (⋮) on the component header
- Select: **Setup Game Scene**

### Step 4: Play!
- Click the **Play** button
- Your game is now ready!

---

## What Gets Created

The setup script automatically creates:

✅ **Canvas** with complete UI layout
✅ **GameManager** - Game logic and economics
✅ **UIManager** - Player interactions and display
✅ **GameMapRenderer** - Visual rendering
✅ All buttons, text displays, and list panels

---

## Game Controls

### Building (Left Panel)
- **Add Station** ($5,000) - Creates new passenger/cargo source
- **Add Train** ($10,000) - Purchases random train type
- **Build Route** ($2,000/unit) - Connects two stations

### Game Controls (Bottom)
- **Next Day** - Advance time and process income
- **Pause** - Freeze game
- **Speed Up** - Increase game speed (max 3x)

### Information Display
- **Balance** (top-left) - Your current money
- **Day** (top-center) - Current game day
- **Trains** (top-right) - Number of active trains
- **Stations** (top-right) - Number of stations
- **Stations Panel** (right) - Details of all stations
- **Trains Panel** (right) - Details of all trains

---

## Train Types

| Type | Passengers | Cargo | Speed |
|------|-----------|-------|-------|
| Passenger Train | 500 | 100 | 10 |
| Cargo Train | 50 | 1,000 | 8 |
| Fast Train | 300 | 50 | 15 |
| Hybrid Train | 300 | 300 | 10 |

---

## How to Play

1. **Start with stations**
   - Click "Add Station" twice ($10,000 total)

2. **Buy a train**
   - Click "Add Train" ($10,000)

3. **Build a route**
   - Click "Build Route" to connect stations

4. **Generate income**
   - Click "Next Day" repeatedly
   - Watch your money increase!

5. **Expand**
   - Add more stations and trains
   - Build more routes
   - Optimize your network

---

## Tips for Success

- **Start small** - Build 2 stations, buy 1 train, create 1 route
- **Wait for income** - Let trains run for several days before expanding
- **Manage costs** - Trains need fuel and maintenance
- **Plan routes** - Shorter routes = faster income cycles
- **Diversify trains** - Use different types for different routes

---

## Troubleshooting

### Game Won't Start
- Check Console for errors (Window → General → Console)
- Make sure TextMeshPro is imported
- Verify all scripts are in Assets/Scripts/

### No Income
- Create at least 2 stations
- Buy a train
- Build a route between stations
- Click "Next Day" to see income

### UI Not Showing
- Run the setup again
- Check that Canvas exists in Hierarchy
- Verify all buttons are wired correctly

---

## Next Steps

After playing the game, you can:
- Add sound effects
- Create save/load system
- Add more features
- Deploy to build

Enjoy building your Trainoo railway empire! 🚂💰

Visit the repository: https://github.com/SeeramGiridhar/trainoo
