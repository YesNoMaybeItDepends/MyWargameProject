using Godot;
using Newtonsoft.Json;

public static class Helpers
{
  private static StateManager stateManager;

    public static void PrintObject(object obj)
    {
      string serializedObject = JsonConvert.SerializeObject(obj, Formatting.Indented);
      GD.Print(serializedObject);
    }

    public static void SetStateManager(StateManager _stateManager)
    {
      stateManager = _stateManager;
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
}

