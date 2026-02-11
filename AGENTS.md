# AGENTS.md - HorizonQuestDemo Development Guide

## Project Overview
HorizonQuestDemo is an Unreal Engine 5.7 demo project showcasing the HorizonQuest plugin - a general-purpose quest graph system for non-linear storytelling. The project demonstrates quest management, quest requirements, rewards, and UI integration.

**Engine Version:** UE 5.7 (supports 4.27-5.7)  
**Platform:** Win64, Mac, Linux  
**Module:** HorizonQuestDemo (Runtime)

---

## Build & Compilation

### Generate Project Files
```bash
# Windows
GenerateProjectFiles.bat

# macOS/Linux
./GenerateProjectFiles.sh
```

### Build Commands
```bash
# Development build (Win64)
Engine\Build\BatchFiles\Build.bat HorizonQuestDemo Win64 Development -Project=HorizonQuestDemo.uproject

# Shipping build
Engine\Build\BatchFiles\Build.bat HorizonQuestDemo Win64 Shipping -Project=HorizonQuestDemo.uproject

# Editor build
Engine\Build\BatchFiles\Build.bat HorizonQuestDemoEditor Win64 Development -Project=HorizonQuestDemo.uproject
```

### Open in Editor
```bash
# Double-click the .uproject file or use command line
HorizonQuestDemo.uproject
```

---

## Testing

### Running Tests
The project includes test plugins (Gauntlet, EditorTests, FunctionalTestingEditor, RuntimeTests). Tests are typically run through the Unreal Editor or via command line:

```bash
# Run functional tests
Engine\Build\BatchFiles\RunUAT.bat BuildPlugin -Plugin=HorizonQuestDemo -CreateChangelist
```

### Single Test Execution
Tests are managed through the Unreal Editor's Test Automation window or via Blueprint/C++ test classes.

---

## Code Style Guidelines

### C++ Conventions
- **Naming:** Follow Unreal Engine standards
  - Classes: `AClassName`, `UClassName`, `FStructName`
  - Functions: `PascalCase` (e.g., `GetQuestManager()`)
  - Variables: `PascalCase` with type prefix (e.g., `bIsActive`, `CameraBoom`)
  - Private members: Prefix with underscore or use private access specifier
  - Constants: `CONSTANT_NAME` or `ConstantName`

- **Includes:** Use forward declarations in headers; include in .cpp files
  ```cpp
  // Header
  class USpringArmComponent;
  
  // Implementation
  #include "GameFramework/SpringArmComponent.h"
  ```

- **UPROPERTY/UFUNCTION Macros:**
  ```cpp
  UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Quest")
  class UHorizonQuestManagerComponent* QuestManager;
  
  UFUNCTION(BlueprintCallable, Category = "Quest")
  void AcceptQuest(const FString& QuestID);
  ```

- **Comments:** Use `//` for single-line, `/** */` for documentation
  ```cpp
  /** Accepts a quest by ID and triggers callbacks */
  void AcceptQuest(const FString& QuestID);
  ```

### Module Dependencies
- **PublicDependencyModuleNames:** Core, CoreUObject, Engine, InputCore, HorizonQuest
- Add new dependencies in `HorizonQuestDemo.Build.cs`

### Formatting
- **Indentation:** 4 spaces (per .editorconfig)
- **Line Length:** Max 120 characters
- **Braces:** Allman style (opening brace on new line for classes/functions)
- **Spacing:** One space after keywords (`if`, `for`, `while`)

### Error Handling
- Use `check()` for critical assertions in development
- Use `ensure()` for recoverable errors
- Log errors with `UE_LOG(LogTemp, Error, TEXT("Message"))`
- Return false or nullptr for failed operations; use callbacks for async results

### Blueprint Integration
- Expose gameplay-critical functions with `UFUNCTION(BlueprintCallable)`
- Use `UPROPERTY(EditAnywhere, BlueprintReadWrite)` for designer-editable values
- Implement callbacks as `FSimpleDelegate` or `FSimpleMulticastDelegate`

### Network Replication
- Enable `bReplicates = true` on replicated actors
- Use `UPROPERTY(Replicated)` for replicated variables
- Implement `GetLifetimeReplicatedProps()` for custom replication
- Use `Server` and `Client` RPC specifiers for network calls

---

## Project Structure

```
HorizonQuestDemo/
├── Source/
│   ├── HorizonQuestDemo/
│   │   ├── Public/
│   │   │   ├── HorizonQuestDemo.h
│   │   │   ├── HorizonQuestDemoCharacter.h
│   │   │   └── HorizonQuestDemoGameMode.h
│   │   ├── Private/
│   │   │   ├── HorizonQuestDemo.cpp
│   │   │   ├── HorizonQuestDemoCharacter.cpp
│   │   │   ├── HorizonQuestDemoGameMode.cpp
│   │   │   └── HorizonQuestDemoPrivate.h
│   │   └── HorizonQuestDemo.Build.cs
│   ├── HorizonQuestDemo.Target.cs
│   └── HorizonQuestDemoEditor.Target.cs
├── Content/
│   ├── _HorizonQuestDemo/
│   ├── _HorizonQuestUseCase/
│   └── Maps/
├── Config/
├── Plugins/
│   └── HorizonQuest/
├── HorizonQuestDemo.uproject
└── HorizonQuestDemo.sln
```

---

## Key Classes & Patterns

### Character Setup
- `AHorizonQuestDemoCharacter`: Third-person character with camera boom
- Inherits from `ACharacter`; implements input handling
- Uses `USpringArmComponent` for camera positioning

### Game Mode
- `AHorizonQuestDemoGameMode`: Manages game rules and player spawning
- Integrates HorizonQuest plugin systems

### Quest Integration
- Add `UHorizonQuestManagerComponent` to PlayerState for quest tracking
- Add `UHorizonQuestFlagManagerComponent` for quest flags
- Use callbacks: `OnAcceptQuestEvent`, `OnCompleteQuestEvent`, `OnQuestStateChangedEvent`

---

## Common Tasks

### Adding a New Quest
1. Create DataTable with `FHorizonQuestTableRow`
2. Create QuestGraph asset and define quest nodes
3. Assign requirements/rewards via Blueprint
4. Call `QuestManager->AcceptQuest()` or `CompleteQuest()`

### Extending Quest Nodes
```cpp
UCLASS()
class HORIZONQUESTDEMO_API UCustomQuestGraphNode : public UHorizonQuestGraphNode
{
    GENERATED_BODY()
public:
    UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = "Custom")
    TArray<FSoftClassPath> DialogueScenes;
};
```

### Network Replication
- Enable replication on manager components
- Send Accept/Complete RPCs from PlayerController
- Use client prediction for UI responsiveness

---

## Editor Configuration

### .editorconfig
- UTF-8 encoding
- 4-space indentation (tabs converted to spaces)
- Max line length: 120 characters
- MSBuild files: 2-space indentation
- YAML files: 2-space indentation

### Visual Studio Configuration
- Solution: `HorizonQuestDemo.sln`
- Use `.vsconfig` for recommended extensions
- IntelliSense enabled for Unreal API

---

## Git Workflow

### Branches
- `main`: Stable release branch
- Feature branches: `feature/quest-system`, `feature/ui-improvements`
- Hotfix branches: `hotfix/bug-name`

### Commit Convention
Follow Kano commit format:
- `feat: Add new quest type system`
- `fix: Resolve quest completion callback issue`
- `refactor: Simplify quest manager logic`
- `docs: Update quest integration guide`

### Submodules
Project uses git submodules for plugin dependencies. Update with:
```bash
git submodule update --init --recursive
```

---

## Debugging Tips

### Logging
```cpp
UE_LOG(LogTemp, Warning, TEXT("Quest accepted: %s"), *QuestID);
UE_LOG(LogTemp, Error, TEXT("Failed to complete quest"));
```

### Debug UI
- Use `HorizonQuestDebugTileView` to list all quests
- Enable `ShowDebug Quest` console command
- Check quest state in Output Log

### Breakpoints
- Set breakpoints in Visual Studio
- Attach debugger: `Debug > Attach to Process > UE4Editor.exe`

---

## Performance Considerations

- Use `TObjectPtr` instead of raw pointers (UE5+)
- Cache frequently accessed components
- Use `UPROPERTY(Transient)` for temporary data
- Enable Fast TArray Replication for large quest arrays (see README)
- Profile with `stat unit` console command

---

## Resources

- **Documentation:** https://github.com/dorgonman/HorizonQuestDemo
- **Marketplace:** https://www.unrealengine.com/marketplace/en-US/product/horizon-quest-general-purpose-quest-graph-system
- **Contact:** dorgonman@hotmail.com
- **Demo Videos:** https://youtu.be/ju3tL1lv7lU (full), https://youtu.be/mANMYY476mM (quick start)

---

## Version Info

Current: **5.7.0**  
Supported: **4.27 - 5.7**  
Last Updated: February 2025
