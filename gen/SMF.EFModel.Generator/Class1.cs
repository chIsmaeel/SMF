namespace SMF.EFModel.Generator;

using Humanizer;

public class Class1
{
    public string? MyProperty { get; set; }
    public void MyMethod()
    {
        MyProperty.Pluralize();
    }
}
