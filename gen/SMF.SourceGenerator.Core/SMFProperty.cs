namespace SMF.SourceGenerator.Core;
public record SMFProperties(string[] PropertySyntax, SMFRecord SMFRecord)
{
    private bool _hasSecondAccessor = false;
    bool _noSecondAccessor = true;
    private string? _propertyName;
    private string? _dataType;

    public bool TryGetSecondAccessor(out string? secondAccessor)
    {
        secondAccessor = null;
        if (_hasSecondAccessor && _noSecondAccessor is false) return true;
        if (GetColonValue("accessor") is string accessor && SMFKeywords.PropertyAccessors.Any(_ => _ == accessor))
        {
            secondAccessor = accessor.Replace("_", " ");
            _hasSecondAccessor = true;
        }
        else if (SMFRecord.GetValue("default_accessor") is string deafultAccessor && SMFKeywords.PropertyAccessors.Any(_ => _ == deafultAccessor))
        {
            secondAccessor = deafultAccessor.Replace("_", " ");
            _hasSecondAccessor = true;
        }
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
        var v = SMFRecord.GetValue("default_accessor");
        _noSecondAccessor = false;
        return _hasSecondAccessor;
    }

    public string? GetColonValue(string ofType)
    {
        var value = PropertySyntax.Where(_ => _.StartsWith($"{ofType}:")).FirstOrDefault();
        if (value is null) return null;

        value = value.Split(':')[1];
        return value;
    }

    public string DataType => _dataType ??= PropertySyntax[1];
    public string PropertyName => _propertyName ??= PropertySyntax[0].Substring(0, PropertySyntax[0].Length - 1);
}
