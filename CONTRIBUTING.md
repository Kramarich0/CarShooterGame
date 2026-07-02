# Development Guide & Quality Standards

## Local Setup & Testing
To initialize the development environment, ensure you have the .NET Framework 4.8 SDK installed.

1.  **Solution Loading**: Open `[[WpfApp1.sln]]` in your preferred IDE (Visual Studio 2017+ recommended).
2.  **Restore Dependencies**: The project uses NuGet packages defined in `[[WpfApp1/packages.config]]`. Ensure your IDE is configured to restore packages automatically, or run `nuget restore [[WpfApp1.sln]]` via CLI.
3.  **Compilation**: Build the solution using the `Debug|Any CPU` configuration defined in `[[WpfApp1/WpfApp1.csproj]]`.
4.  **Verification**: Execute the application binary located in `[[WpfApp1/bin/Debug/]]` to verify the runtime environment matches the configuration in `[[WpfApp1/App.config]]`.

## Pre-Commit Verification Checklist (Fragile Zones)
Before committing, ensure that changes to these high-churn or tightly coupled modules are validated:

- [ ] [[WpfApp1/Models/Car.cs]]: Warning: Core entity. Action: Validate interaction logic with `[[WpfApp1/Models/GameViewModel.cs]]`.
- [ ] [[WpfApp1/Models/GameViewModel.cs]]: Warning: Central state management. Action: Verify binding integrity.
- [ ] [[WpfApp1/Models/Bullet.cs]]: Warning: High complexity. Action: Ensure collision detection logic remains accurate.
- [ ] [[WpfApp1/Models/Tree.cs]]: Warning: High churn. Action: Run full suite of local visual regression tests.

## Pre-Commit Security Checks
- **Secrets Management**: Scan the repository for hardcoded connection strings or API keys. Check `[[WpfApp1/App.config]]` specifically for sensitive environment-specific data.
- **Dependency Integrity**: Verify that any new package added to `[[WpfApp1/packages.config]]` is vetted for known vulnerabilities and aligns with existing framework constraints (e.g., ReactiveUI dependencies).
- **Injection Risks**: Ensure all data-driven game object properties (e.g., in `[[WpfApp1/Models/Enemy.cs]]` or `[[WpfApp1/Models/Coin.cs]]`) are sanitized if loaded from external configuration files.

## PR Quality Standards & Review Gates
- **Documentation**: All new classes must include XML documentation comments describing the public interface.
- **Test Coverage**: Logic introduced in `[[WpfApp1/Models/]]` must be accompanied by unit tests.
- **Review Gates**:
    - Any changes to `[[WpfApp1/WpfApp1.csproj]]` require review by a maintainer to ensure build reproducibility.
    - Architecture changes involving `[[WpfApp1/Models/GameViewModel.cs]]` require a summary of impact on the existing ReactiveUI event stream.

## Change Playbooks

### Adding a New Game Object
1.  Inherit from `[[WpfApp1/Models/GameObject.cs]]` (if applicable).
2.  Implement the required properties following the pattern observed in `[[WpfApp1/Models/Coin.cs]]` or `[[WpfApp1/Models/Obstacle.cs]]`.
3.  Register the object in `[[WpfApp1/Models/GameViewModel.cs]]` to ensure it is included in the game loop.

### Modifying the Leaderboard
1.  Update the data structure in `[[WpfApp1/Models/Leaderboard.cs]]`.
2.  Ensure `[[WpfApp1/Models/LeaderboardManager.cs]]` correctly handles the serialization of the updated structure.
3.  Verify that no breaking changes are introduced to the UI binding layer in `[[WpfApp1/MainWindow.xaml.cs]]`.

### Updating Dependencies
1.  Modify `[[WpfApp1/packages.config]]` to reflect the new version.
2.  Update the corresponding `bindingRedirect` sections in `[[WpfApp1/App.config]]` to ensure the runtime resolves the correct assembly versions.
3.  Perform a clean build of `[[WpfApp1.sln]]` to verify the assembly graph is consistent.