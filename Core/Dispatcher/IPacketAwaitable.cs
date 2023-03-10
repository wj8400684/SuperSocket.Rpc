using Core;

namespace Core;

public interface IPacketAwaitable : IDisposable
{
    void Complete(RpcPackageInfo packet);

    void Fail(Exception exception);

    void Cancel();
}