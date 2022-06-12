namespace SMF.SourceGenerator.Core;

/// <summary>
/// The extension methods.
/// </summary>

public static class ExtensionMethods
{
   
    #region SyntaxNode

    /// <summary>
    /// Are the module or model or model property or model property.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsSMFClass(this SyntaxNode node)
    {
        return node.IsModuleClass() || node.IsModelClass() || node.IsControllerClass();
    }

    /// <summary>
    /// Are the module class.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsModuleClass(this SyntaxNode node)
    {
        return node is ClassDeclarationSyntax cds && cds!.Identifier.ValueText.EndsWith("Module");
    }

    /// <summary>
    /// Are the model class.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsModelClass(this SyntaxNode node)
    {
        return node is ClassDeclarationSyntax cds && cds!.Identifier.ValueText.EndsWith("Model");
    }


    /// <summary>
    /// Are the controller class.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsControllerClass(this SyntaxNode node)
    {
        return node is ClassDeclarationSyntax cds && cds!.Identifier.ValueText.EndsWith("Controller");
    }

    /// <summary>
    /// Are the model property.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsModelProperty(this SyntaxNode node)
    {
        return node is PropertyDeclarationSyntax pds && pds.Type.ToString().StartsWith("SM.");
    }

    /// <summary>
    /// Are the model field.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsModelField(this SyntaxNode node)
    {
        return node is FieldDeclarationSyntax fds && (fds.Parent as ClassDeclarationSyntax)!.Identifier.ValueText.EndsWith("Model");
    }
    #endregion SyntaxNode

}
