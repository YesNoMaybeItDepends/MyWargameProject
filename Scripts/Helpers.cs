using Godot;
using Newtonsoft.Json;

public static class Helpers
{
    public static void PrintObject(object obj)
    {
      string serializedObject = JsonConvert.SerializeObject(obj, Formatting.Indented);
      GD.Print(serializedObject);
    }
}

