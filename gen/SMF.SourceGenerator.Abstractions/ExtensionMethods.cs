namespace SMF.SourceGenerator.Abstractions;

/// <summary>
/// The extension methods.
/// </summary>

public static class ExtensionMethods
{
    /// <summary>
    /// Gets the qualified name.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <returns>A string.</returns>
    public static string GetQualifiedName(this ClassDeclarationSyntax cds)
    {
        var nameSpace = cds?.GetContainingNamespace();
        if (nameSpace is null || nameSpace == cds?.Identifier.Text)
        {
            return cds?.Identifier.Text!;
        }

        return nameSpace + "." + cds!.Identifier.Text;
    }

    /// <summary>
    /// Gets the containing namespace.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <returns>A string.</returns>
    public static string? GetContainingNamespace(this ClassDeclarationSyntax cds)
    {
        return cds?.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>() is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDS
            ? fileScopedNamespaceDS.Name.ToString()
            : cds?.FirstAncestorOrSelf<NamespaceDeclarationSyntax>() is NamespaceDeclarationSyntax namespaceDeclarationSyntax
            ? namespaceDeclarationSyntax.Name.ToString() : null;
    }

    /// <summary>
    /// Gets the using qualified names.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <returns>An IEnumerable&lt;string?&gt;? .</returns>
    public static IEnumerable<string?>? GetUsingDeclarationSyntaxNames(this ClassDeclarationSyntax? cds)
    {
        var usingQualifiedNames = new List<string?>();
        if (cds!.Parent!.Parent is CompilationUnitSyntax compilationUnitSyntax && compilationUnitSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(compilationUnitSyntax.Usings.Select(_ => _.Name.ToString()));
        if (cds.Parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDS && fileScopedNamespaceDS.Usings.Count > 0)
            usingQualifiedNames.AddRange(fileScopedNamespaceDS.Usings.Select(_ => _.Name.ToString()));
        if (cds.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax && namespaceDeclarationSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(namespaceDeclarationSyntax.Usings.Select(_ => _.Name.ToString()));
        return usingQualifiedNames;
    }

    /// <summary>
    /// Gets the all possible qualified names.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <param name="identifierName">The identifier name.</param>
    /// <returns>An IEnumerable&lt;string?&gt;? .</returns>
    public static IEnumerable<string?>? GetAllPossibleQualifiedNames(this ClassDeclarationSyntax? cds, string identifierName)
    {
        var usingQualifiedNames = new List<string?>
        {
            identifierName,
            cds!.GetContainingNamespace() + "." + identifierName
        };
        if (cds!.Parent!.Parent is CompilationUnitSyntax compilationUnitSyntax && compilationUnitSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(compilationUnitSyntax.Usings.Select(_ => _.Name.ToString() + "." + identifierName));
        if (cds.Parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDS && fileScopedNamespaceDS.Usings.Count > 0)
            usingQualifiedNames.AddRange(fileScopedNamespaceDS.Usings.Select(_ => _.Name.ToString() + "." + identifierName));
        if (cds.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax && namespaceDeclarationSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(namespaceDeclarationSyntax.Usings.Select(_ => _.Name.ToString() + "." + identifierName));
        return usingQualifiedNames;
    }

    /// <summary>
    /// Gets the all possible qualified names.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <returns>An IEnumerable&lt;string?&gt;? .</returns>
    public static IEnumerable<string?>? GetAllPossibleQualifiedNames(this ClassDeclarationSyntax? cds)
    {
        var usingQualifiedNames = new List<string?>
        {

            cds!.GetContainingNamespace()
        };
        if (cds!.Parent!.Parent is CompilationUnitSyntax compilationUnitSyntax && compilationUnitSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(compilationUnitSyntax.Usings.Select(_ => _.Name.ToString()));
        if (cds.Parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDS && fileScopedNamespaceDS.Usings.Count > 0)
            usingQualifiedNames.AddRange(fileScopedNamespaceDS.Usings.Select(_ => _.Name.ToString()));
        if (cds.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax && namespaceDeclarationSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(namespaceDeclarationSyntax.Usings.Select(_ => _.Name.ToString()));
        return usingQualifiedNames;
    }

    /// <summary>
    /// Fors the each.
    /// </summary>
    /// <param name="values">The values.</param>
    /// <param name="action">The action.</param>
    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var value in values)
        {
            action(value);
        }
    }

    /// <summary>
    /// Have the more than one c d s.
    /// </summary>
    /// <param name="cdsArray">The cds array.</param>
    /// <param name="cds">The cds.</param>
    /// <returns>A list of ClassDeclarationSyntax?.</returns>
    public static IEnumerable<ClassDeclarationSyntax?>? GetAllPartialClasses(this IEnumerable<ClassDeclarationSyntax?>? cdss, ClassDeclarationSyntax? cds)
    {
        return cdss.Where(_ => _!.GetQualifiedName() == cds!.GetQualifiedName());
    }

    /// <summary>
    /// Have the more than one c d s.
    /// </summary>
    /// <param name="cdsFirst">The cds first.</param>
    /// <param name="cdsSecond">The cds second.</param>
    /// <returns>A bool.</returns>
    public static bool GetAllPartialClasses(this ClassDeclarationSyntax? cdsFirst, ClassDeclarationSyntax? cdsSecond)
    {
        var hasSameIdentifier = cdsFirst?.Identifier.ValueText == cdsSecond?.Identifier.ValueText;
        if (!hasSameIdentifier)
        {
            return false;
        }

        string? firstNamespace = null;
        string? secondNamespace = null;

        var cdsFirstHasFileScopedNamespace = cdsFirst?.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();
        var cdsFirstHasNamespaceScopedNamespace = cdsFirst?.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString();
        firstNamespace = cdsFirstHasFileScopedNamespace ?? cdsFirstHasNamespaceScopedNamespace;

        var cdsSecondHasFileScopedNamespace = cdsSecond?.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();
        var cdsSecondHasNamespaceScopedNamespace = cdsSecond?.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString();

        secondNamespace = cdsSecondHasFileScopedNamespace ?? cdsSecondHasNamespaceScopedNamespace;

        var r = firstNamespace == secondNamespace;

        return r;
    }


}
