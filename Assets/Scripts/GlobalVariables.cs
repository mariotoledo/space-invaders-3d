using System.Collections.Generic;

public static class GlobalVariables
{
    private static readonly object lockObject = new object();
    private static Dictionary<string, object> variablesDictionary = new Dictionary<string, object>();

    public static Dictionary<string, object> VariablesDictionary => variablesDictionary;

    public static Dictionary<string, object> GetAll()
    {
        return variablesDictionary;
    }

    public static T Get<T>(string key)
    {
        if (variablesDictionary == null || !variablesDictionary.ContainsKey(key))
        {
            return default(T);
        }

        return (T)variablesDictionary[key];
    }

    public static void Set(string key, object value)
    {
        lock (lockObject)
        {
            if (variablesDictionary == null)
            {
                variablesDictionary = new Dictionary<string, object>();
            }
            variablesDictionary[key] = value;
        }
    }

}