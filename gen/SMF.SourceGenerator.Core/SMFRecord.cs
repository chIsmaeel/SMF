namespace SMF.SourceGenerator.Core;
using SMF.SourceGenerator.Abstractions.Helpers;

public record SMFRecord(List<string> RecordLines, SMFFile SMFFile)
{
    private string[]? _declationSyntaxes;
    private string? _recordDeclarationSyntax;
    private List<SMFProperties>? _sMFProperties;
    private List<string>? _members;
    private string? _recordName;
    private string? _recordModelNamespace;

    public GlobalOptions GlobalOptions => SMFFile.GlobalOptions;

    public string ModelNamespace
    {
        get
        {
            if (_recordModelNamespace is not null) return _recordModelNamespace;
            _recordModelNamespace = GetValue("model_namespace");
            if (_recordModelNamespace is null)
                return _recordModelNamespace = GlobalOptions.RootNamespace + ".Models";
            _recordModelNamespace = _recordModelNamespace.Replace("PROJECT_NAME", SMFFile.GlobalOptions.RootNamespace);
            return _recordModelNamespace;
        }
    }

    public string RecordDeclarationSyntaxString => _recordDeclarationSyntax ??= RecordLines[0];
    public string[] DeclationSyntaxes => _declationSyntaxes ??= RecordDeclarationSyntaxString.Split(' ');

    public string RecordName => _recordName ??= DeclationSyntaxes[0].Substring(0, DeclationSyntaxes[0].Length - 1).Pascalize();
    public List<string> Members
    {
        get
        {
            if (_members is not null) return _members;
            _members = new();
            _members.AddRange(RecordLines.Skip(1));
            return _members;
        }
    }


    public List<SMFProperties> SMFProperties
    {
        get
        {
            if (_sMFProperties is not null) return _sMFProperties;
            _sMFProperties = new();
            foreach (var property in Members)
            {
                var i = SMFKeywords.Records;
                var propertySyntax = property.Trim().Split(' ');
                if (SMFKeywords.DataTypes.Any(_ => _ == propertySyntax[1]))
                    _sMFProperties!.Add(new(propertySyntax, this));
                else if (SMFKeywords.Records.Any(_ => _ == propertySyntax[1].Pascalize()))
                {
                    propertySyntax[1] = propertySyntax[1].Pascalize();
                    _sMFProperties!.Add(new(propertySyntax, this));
                }
            }

            return _sMFProperties;
        }
    }

    public string? GetValue(string ofType)
    {
        var value = SMFFile.Lines.Where(_ => _.StartsWith($"{ofType}=")).FirstOrDefault();
        if (value is null) return null;

        value = value.Split('=')[1];
        return value.Trim();
    }
}
