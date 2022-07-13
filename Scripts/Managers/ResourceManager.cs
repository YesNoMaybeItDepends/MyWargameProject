using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ResourceManager : Node2D, IService
{
    private Dictionary<string, object> resources = new Dictionary<string, object>();

    public T GetResource<T>(string name)
    {
        var resource = (T)resources[name];
        if (resource == null)
        {
            GD.PushError($"Error loading resource {name} of type {typeof(T).FullName}");
        }

        // TODO if resource is null, it should look for resource in files?

        return resource;
    }

    // res://Assets/PH2_DeadPlainsGray_04.png
    public bool SetResource<T>(string path) where T : class
    {
        string name = string.Empty;

        // https://stackoverflow.com/questions/9363145/regex-for-extracting-filename-from-path
        string REGEX_FILENAME_WITH_EXTENSION = @"[ \w-]+\.[\w-]*$";
        string REGEX_FILENAME_WITHOUT_EXTENSION = @"[\w-]*$";

        Regex regex = new Regex(REGEX_FILENAME_WITH_EXTENSION);
        Match match = regex.Match(path);

        if (match.Success)
        {
            name = match.Value;
        }
        else
        {
            GD.PushError($"Error loading resource of type {typeof(T).FullName} with path {path}");
            return false;
        }

        if (path == null)
        {
            return false;
        }

        GD.Print(path);
        T resource = GD.Load<T>(path);
        
        if (resource == null)
        {
            GD.PushError("uh oh hot dog");
        }

        GD.Print(resource);
        resources.Add(name, resource);

        return true;
    }
}


