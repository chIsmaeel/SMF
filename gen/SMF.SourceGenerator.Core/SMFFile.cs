namespace SMF.SourceGenerator.Core;
using SMF.SourceGenerator.Abstractions.Helpers;

public record SMFFile(string FileCode, GlobalOptions GlobalOptions)
{
    private List<SMFRecord>? _sMFRecords;
    private string[]? _lines;

    public string[] Lines
    {
        get
        {
            if (_lines is not null) return _lines;
            return _lines = FileCode.Split('\n');
        }
    }

    public List<SMFRecord> SMFRecords
    {
        get
        {
            if (_sMFRecords is not null) return _sMFRecords;
            _sMFRecords = new();

            int i = 0;
            while (i < Lines.Length)
            {
                var record = Lines[i].Split(' ');
                if (record.Length == 0)
                {
                    i++;
                    continue;
                }

                if (!(record[0].EndsWith(":") && record[1].Trim() == "record"))
                {
                    i++;
                    continue;
                }

                var list = new List<string>();
                list.Add(Lines[i].Trim());
                int j = i + 1;
                while (j < Lines.Length)
                {
                    var propertyLineArray = Lines[j].Trim().Split(' ');
                    if (Lines[j].StartsWith("  ") && propertyLineArray[0].EndsWith(":"))
                        list.Add(Lines[j].Trim());
                    else
                        break;
                    j++;
                }
                i = j + 1;
                _sMFRecords.Add(new(list, this));
            }

            return _sMFRecords;
        }
    }
}
