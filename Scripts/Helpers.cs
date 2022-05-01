using Godot;
using Newtonsoft.Json;

public static class Helpers
{
  private static StateManager stateManager;
  private static GameManager gameManager;
  private static Inputcontroller inputController;

    public static void PrintObject(object obj)
    {
      string serializedObject = JsonConvert.SerializeObject(obj, Formatting.Indented);
      GD.Print(serializedObject);
    }

    public static void SetStateManager(StateManager _stateManager)
    {
      stateManager = _stateManager;
    }

    public static void SetGameManager(GameManager _gameManager)
    {
      gameManager = _gameManager;
    }

    public static void SetInputController(Inputcontroller _inputController)
    {
      inputController = _inputController;
    }

    // TODO Move this to a static resource provider class
    public static StateManager GetStateManager()
    {
      if (stateManager != null)
      {
        return stateManager;
      }
      else return null;
    }

    public static GameManager GetGameManager()
    {
      if (gameManager != null)
      {
        return gameManager;
      }
      else return null;
    }

    public static Inputcontroller GetInputController()
    {
      if (inputController != null)
      {
        return inputController;
      }
      else return null;
    }
}

