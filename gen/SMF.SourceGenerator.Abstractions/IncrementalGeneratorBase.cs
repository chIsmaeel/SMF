namespace SMF.SourceGenerator.Abstractions;

using Microsoft.CodeAnalysis;

public abstract class IncrementalGeneratorBase : IIncrementalGenerator
{
    public abstract void Initialize(IncrementalGeneratorInitializationContext context);
}
