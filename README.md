# System Identity & Onboarding Blueprint

## Executive Summary
This system is an interactive WPF-based game application built on a reactive architecture. It provides a real-time gaming environment featuring player-controlled entities, obstacle management, and a persistent leaderboard system. The core value proposition lies in its decoupled architecture, utilizing `ReactiveUI` for state management and `DynamicData` for high-performance collection handling, allowing for a scalable and maintainable game loop.

## Primary Entrypoints
*   [[WpfApp1/MainWindow.xaml.cs]]: The primary UI controller. Initializes the game context, handles user input events, and bridges the XAML view with the game logic.
*   [[WpfApp1/Models/GameViewModel.cs]]: The central state container. Orchestrates the game loop, manages entity collections (cars, enemies, coins, trees), and handles game state transitions (pause/resume/running).
*   [[WpfApp1/App.xaml]]: Defines the application startup configuration and global resource dictionaries, including project-wide styles.

## Knowledge Holders & Ownership Risks
*   **Core Logic Ownership**: Unknown.
*   **Risk Analysis**: The architecture relies on multiple loosely coupled models (`Car`, `Enemy`, `Bullet`) that are managed through the `GameViewModel`. A bus-factor risk exists due to the lack of explicit documentation on the spatial grid collision algorithms implemented in [[WpfApp1/Models/Coin.cs]] and [[WpfApp1/Models/Obstacle.cs]]. Modification of these algorithms requires deep understanding of the coordinate systems used across the game canvas.

## Quick Start, Setup & Verification
### Prerequisites
*   .NET Framework 4.8 SDK.
*   Visual Studio 2017 or later with the "Desktop development with C++" and ".NET desktop development" workloads.

### Configuration
The application relies on project-level configurations defined in:
*   [[WpfApp1/WpfApp1.csproj]]: Contains project references and NuGet package dependencies.
*   [[WpfApp1/App.config]]: Configures runtime binding redirects for required dependencies like `System.Reactive` and `System.Text.Json`.

### Setup Steps
1.  Open [[WpfApp1.sln]] in Visual Studio.
2.  Restore NuGet packages using the built-in package manager or via the command line.
3.  Ensure all references in [[WpfApp1/packages.config]] are resolved.
4.  Build the solution using the `Debug` configuration.

> [!TIP]
> If build errors occur regarding missing assemblies, ensure the `packages` directory is properly restored relative to the solution root.

### Smoke Test / Verification
To verify the local build:
1.  Launch the application from Visual Studio.
2.  Observe the main menu video playback.
3.  Click "Start Game" to ensure the `GameCanvas` renders and the `GameViewModel` successfully initializes the entities.

## Operating Model & Next Steps
### Core Runtime Behavior
The system operates as a single-threaded WPF application. The game loop is driven by `DispatcherTimer` instances within the [[WpfApp1/MainWindow.xaml.cs]] and [[WpfApp1/Models/GameViewModel.cs]]. Telemetry and state changes are propagated via standard `INotifyPropertyChanged` patterns.

### Next Steps for Engineers
*   Review [[WpfApp1/Models/GameViewModel.cs]] to understand the interaction between `ReactiveUI` observables and entity movement.
*   Analyze the collision logic in [[WpfApp1/Models/Bullet.cs]] and [[WpfApp1/Models/Enemy.cs]] before implementing new game mechanics.
*   Examine [[WpfApp1/Styles.xaml]] to understand the custom UI template overrides used for the game interface.