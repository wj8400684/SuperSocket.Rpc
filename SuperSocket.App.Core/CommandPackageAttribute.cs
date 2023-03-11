namespace Core;

[AttributeUsage(AttributeTargets.All,
    AllowMultiple = false)]
public sealed class CommandPackageAttribute : Attribute
{
    public CommandPackageAttribute(Type packageType)
    {
        PackageType = packageType;
    }

    public Type PackageType { get; private set; }
}
