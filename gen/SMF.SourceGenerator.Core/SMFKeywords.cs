namespace SMF.SourceGenerator.Core;
public static class SMFKeywords
{
    public static string[] Keywords { get; } = new string[]
    {
       "record",
       "int",
       "float",
       "double",
       "string",
       "accessor:set",
       "accessor:private_set",
       "accessor:protected_set",
       "accessor:init",



    };

    public static string[] DataTypes { get; } = new string[]
  {
       "int",
       "float",
       "double",
       "string",
  };

    public static string[] PropertyAccessors { get; } = new string[]
{
       "init",
       "set",
       "private_set",
       "protected_set",
};


    public static List<string> Records { get; } = new();
}
