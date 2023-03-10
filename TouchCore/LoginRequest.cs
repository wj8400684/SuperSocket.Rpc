namespace TouchCore;

[MemoryPack.MemoryPackable]
public sealed partial class LoginRequest
{
    public string? Username { get; set; }

    public string? Password { get; set; }
}

[MemoryPack.MemoryPackable]
public sealed partial class LoginResponse
{
    public bool Successful { get; set; }
}