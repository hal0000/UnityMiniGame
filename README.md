# UnityMiniGame
Created for a technical demonstration.

---

## 1. OVERVIEW

This project demonstrates a simple in-game currency system, daily bonus mechanic, a short “rewarded-ad” style wait system, and a rigged “Wheel of Fortune” minigame in Unity.

It includes:
- Spending/earning coins
- Saving/loading player data
- Daily bonus availability based on time checks
- A short wait mechanic for extra coin
- A rigged wheel game with fixed win spins and a 5% chance after the 10th spin

---

## 2. FEATURES

### SPEND ONE COIN
- Deducts 1 coin from the player’s balance (if sufficient coins are available).

### GET EXTRA COIN
- Simulates a 5-second wait using a slider (like a rewarded ad).
- After the wait completes, the user can claim 1 coin.

### WHEEL OF FORTUNE
- The user selects a number (1-8) and pays 1 coin to spin.
- Certain spins are guaranteed to win:
  - 2nd, 7th, and 8th spins are always wins.
  - After the 10th spin, there is a fixed 5% chance of winning.
- Winning yields 8 coins; losing yields 0.
- The wheel spins with a coroutine for smooth animation.

### DAILY BONUS
- Once per day at **13:00 UTC**, the user can claim 1 free coin.
- If the player is brand new (timestamps = 0), they can claim immediately.
- The daily bonus button automatically updates to “Ready” or “Not Ready.”

### DELETE USER DATA
- Resets all saved data (coins, spin count, timestamps) to default values.
- Creates a fresh player profile and re-registers the `CurrencyManager`.

### RESPONSIVE UI
- `CanvasController` adapts the UI layout and scale to different screen resolutions.

---

## 3. PROJECT STRUCTURE
Scripts/
├─ BaseScene.cs
├─ BlockingWait.cs
├─ CanvasController.cs
├─ CurrencyManager.cs
├─ GameManager.cs
├─ MainMenu.cs
├─ SaveSystem.cs
├─ ServiceLocator.cs
├─ ShowCurrentTime.cs
├─ TimerUtility.cs
└─ WheelOfFortune.cs

---

## 4. HOW TO RUN

1. **Open in Unity**  
   - Use Unity **2020.3.7f1** or a newer LTS version.

2. **Scene Setup**  
   - The **MainMenu** scene (or a scene containing **MainMenu**) is your main entry point.  
   - Ensure **GameManager** is present so that the `ServiceLocator` registers the required services on startup.

3. **Play Mode**  
   - Press **Play** in the Unity Editor.  
   - The UI will display:
     - Spend One Coin  
     - Get Extra Coin  
     - Wheel of Fortune  
     - Daily Bonus (ready/not ready)  
     - Delete User Data  
   - The bottom label shows the current date/time in UTC.

4. **Testing Daily Bonus**  
   - Adjust your system clock or simulate future time in code to test daily bonus availability.  
   - A new user (with no saved data) can claim immediately.

5. **Wheel of Fortune**  
   - Select a number (1-8) and pay 1 coin to spin.  
   - Spin #2, #7, #8 = guaranteed wins.  
   - After spin #10, there is a 5% chance to win.  
   - A win awards 8 coins; a loss awards 0.

6. **Delete User Data**  
   - Completely resets the save file.  
   - The daily bonus availability resets, spin count resets, and coins go back to default (5).

---

## 5. KEY SCRIPTS

- **GameManager.cs**  
  Singleton that persists across scenes. Registers itself and a `CurrencyManager` into the `ServiceLocator`.

- **BaseScene.cs**  
  Loads player data in `Awake()`. **MainMenu** extends this class.

- **MainMenu.cs**  
  Main UI with buttons to spend coins, claim daily bonus, launch the wheel, etc. Also checks bonus availability via a coroutine.

- **WheelOfFortune.cs**  
  A rigged minigame. Certain spins are forced to win, and spins after #10 have a 5% chance of winning. Uses a coroutine for spinning animation.

- **BlockingWait.cs**  
  Simulates a 5-second “rewarded-ad” wait using a slider. On completion, triggers an event to grant 1 coin.

- **CurrencyManager.cs**  
  Handles all coin operations (spend/earn). Manages daily bonus claims via `TimerUtility`.

- **TimerUtility.cs**  
  Checks daily bonus availability at **13:00 UTC**. Also prevents clock manipulation by comparing the last verified time.

- **SaveSystem.cs**  
  Loads, saves, and deletes player data via `BinaryFormatter`. Holds a `PlayerData` class with gold, timestamps, and spin count.

- **ServiceLocator.cs**  
  Simple DI container to register and retrieve services by type.

- **CanvasController.cs**  
  Dynamically adjusts the UI scale for different screen resolutions.

- **ShowCurrentTime.cs**  
  Displays the current UTC time, updated once per second.

---

## 6. USAGE NOTES & CUSTOMIZATION

- **WAIT TIME**  
  In `BlockingWait.cs`, change `waitTime` to adjust the waiting period for **Get Extra Coin**.

- **DAILY BONUS TIME**  
  In `TimerUtility.cs`, modify the hour (13:00) if you want a different daily bonus schedule.

- **WHEEL SPIN DURATION**  
  In `WheelOfFortune.cs`, the `SpinToSegment` coroutine uses a `duration` variable (default 3f). Adjust it for faster or slower spins.

- **SAVE FORMAT**  
  The `SaveSystem` uses `BinaryFormatter`. If needed, switch to JSON or another method for better security or compatibility.

---
- Scripts by **Halil Mentes**, created for a technical demonstration.
