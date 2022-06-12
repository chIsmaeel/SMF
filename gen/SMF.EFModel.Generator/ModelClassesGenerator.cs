namespace SMF.EFModel.Generator;

using Microsoft.CodeAnalysis;
using SMF.SourceGenerator.Abstractions.Helpers;
using SMF.SourceGenerator.Abstractions.Templates;
using SMF.SourceGenerator.Abstractions.Templates.TypeTemplates;
using SMF.SourceGenerator.Abstractions.Templates.TypeTemplates.MemberTemplates.PropertyTemplates;
using SMF.SourceGenerator.Core;

using System;
[Generator]
internal class ModelClassesGenerator : IncrementalGenerator
{
    protected override Action<IncrementalGeneratorInitializationContext> Execute => c =>
    {
        c.RegisterSourceOutput(SMFRecords, (c, r) =>
        {
            string nameSpace = string.IsNullOrEmpty(r.ModelNamespace) ? r.GlobalOptions.RootNamespace + ".Models" : r.ModelNamespace;
            var context = new SMFProductionContext(c);

            var fileScopedNamespace = new FileScopedNamespaceTemplate(nameSpace);
            var modelClassBuilder = new ClassTypeTemplate(r.RecordName);
            //#if DEBUG
            //            if (!System.Diagnostics.Debugger.IsAttached)
            //                System.Diagnostics.Debugger.Launch();
            //#endif
            foreach (var property in r.SMFProperties)
            {
                property.TryGetSecondAccessor(out var sAccessor);
                modelClassBuilder.Members.Add(new AutoPropertyTemplate(property.DataType, property.PropertyName)
                {
                    SecondAccessor = sAccessor
                });
            }

            fileScopedNamespace.TypeTemplates.Add(modelClassBuilder);

            context.AddSource(fileScopedNamespace);
            //c.AddSource(r.SMFRecords.FirstOrDefault().RecordName, "/*" + r.SMFRecords.FirstOrDefault().RecordDeclarationSyntaxString + "*/");
        });
    };
}

