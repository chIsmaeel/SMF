namespace SMF.SourceGenerator.Abstractions.Diagnostics;

/// <summary>
/// The diagnostic descriptors.
/// </summary>

public partial record DiagnosticDescriptors : DiagnosticDescriptorsBase
{
    /// <summary>
    /// Gets the invalid s m class access modifier.
    /// </summary>
    public static DiagnosticDescriptor InvalidSMClassAccessModifier { get; } = CreateDiagnosticDescriptor(
        id: "SMF001",
        title: CreateLocalizableString(nameof(SR.InvalidSMClassAccessModifierTitle)),
        messageFormat: CreateLocalizableString(nameof(SR.InvalidSMClassAccessModifierTitle)));

}
