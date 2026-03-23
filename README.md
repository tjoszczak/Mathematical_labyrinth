# 🧠 Math Maze

An educational 2D puzzle-arcade game created with **C# WPF (.NET 6.0)**. 
The player must find their way through a dark maze, racing against the clock, and solve a dynamically generated math puzzle at the finish line to advance to the next level.

## 🎮 About the Project

The project combines arcade game elements with math education. It uses a custom engine based on a `DispatcherTimer` loop (60 FPS) to handle smooth movement and collisions, while maintaining a clean architecture separated into models, views, and services.

### ✨ Main Features:
* **Fog of War:** An advanced algorithm that limits the player's field of view. Walls and the finish line are only visible when the player is within a specific radius.
* **File-based Map Loading (Level Manager):** Levels are not hardcoded. The manager parses `.txt` files to generate objects (Walls, Start, Finish) on a coordinate grid.
* **Dynamic Difficulty (Quiz Manager):** Math puzzles at the end of each level are generated randomly. The higher the level, the harder the operations (ranging from simple addition, to multiplication tables, to order of operations and division without remainders).
* **Timer System:** A strict time limit (60 seconds) to complete each of the 5 levels adds a sense of urgency.
* **Custom Collision System:** A physics/collision system built from scratch that checks for rectangle intersections (Bounding Boxes) between the player and environmental elements.

## 🕹️ Controls

* <kbd>W</kbd> - Move Up
* <kbd>S</kbd> - Move Down
* <kbd>A</kbd> - Move Left
* <kbd>D</kbd> - Move Right
* <kbd>ESC</kbd> - Main Menu / Pause

## 🛠️ Architecture and Technologies

The game was written using Object-Oriented Programming (OOP) principles, divided into clear layers:
* **UI / Game Loop:** `MainWindow.xaml` (XAML) + `MainWindow.xaml.cs` (C#)
* **Services (Business Logic):**
  * `LevelManager.cs` - loads and parses map files (`#`, `S`, `E`).
  * `QuizManager.cs` - generates math problems.
* **Models (Data Structures):**
  * `Player.cs` - player data and physics.
  * `Wall.cs` - obstacle properties.
  * `MathQuestion.cs` - question structure.

## 🚀 How to run the project?

1. Clone the repository to your local machine:
   ```bash
   git clone [https://github.com/YourUsername/MathMaze.git](https://github.com/YourUsername/MathMaze.git)
