namespace SMF.SourceGenerator.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using SMF.SourceGenerator.Abstractions.Helpers;
using System.Collections.Immutable;

/// <summary>
/// The incremetnal generator.
/// </summary>
public abstract class IncrementalGenerator : IncrementalGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        CreateSyntaxNode(context.SyntaxProvider);
        AdditionalTextsProvider(context.AdditionalTextsProvider);
        AnalyzerConfigOptionsProvider(context.AnalyzerConfigOptionsProvider);
        PostInitialization(context);
        Execute(context);
    }

    protected virtual Action<SyntaxValueProvider> CreateSyntaxNode => _ => { };
    protected virtual Action<IncrementalValuesProvider<AdditionalText>> AdditionalTextsProvider =>
        at =>
            SMFFilesAdditionalTexts = at.Where(static f => f.Path.EndsWith(".smf"));
    protected virtual Action<IncrementalGeneratorInitializationContext> PostInitialization => _ => { };
    protected virtual Action<IncrementalValueProvider<AnalyzerConfigOptionsProvider>> AnalyzerConfigOptionsProvider => _ =>
    {
        GlobalOptions = _.Select(static (r, _) => new GlobalOptions(r));
    };
    protected abstract Action<IncrementalGeneratorInitializationContext> Execute { get; }


    protected IncrementalValuesProvider<AdditionalText> SMFFilesAdditionalTexts { get; private set; }
    protected IncrementalValueProvider<GlobalOptions> GlobalOptions { get; private set; }

    protected IncrementalValuesProvider<string> SMFFileStrings =>
         SMFFilesAdditionalTexts.Select(static (r, _) =>
         {
             var text = r.GetText();
             if (text != null)
                 return text.ToString();
             return "";
         })!;

    protected IncrementalValueProvider<ImmutableArray<string>> SMFFileStringsCollection =>
        SMFFileStrings.Collect();

    protected IncrementalValuesProvider<SMFFile> SMFFiles =>
        SMFFileStrings.Combine(GlobalOptions).Select(static (r, _) =>
        {
            var obj = new SMFFile(r.Left, r.Right);
            foreach (var record in obj.SMFRecords)
                SMFKeywords.Records.Add(record.RecordName);
            return obj;
        });

    protected IncrementalValuesProvider<SMFRecord> SMFRecords =>
    SMFFiles.Collect().SelectMany(static (r, _) =>
    {
        var tempRecordList = new List<SMFRecord>();
        foreach (var record in r.SelectMany(_ => _.SMFRecords))
            tempRecordList.Add(record);
        return tempRecordList;
    });

    //protected IncrementalValuesProvider<SMFRecord> SMFRecords =>
    //  SMFFileStrings.Select(static (r, _) => new SMFRecord(r));
}

//#if DEBUG
//         if (!System.Diagnostics.Debugger.IsAttached)
//                    System.Diagnostics.Debugger.Launch();
//#endif
