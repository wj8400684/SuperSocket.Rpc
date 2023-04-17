using System.Buffers;
using System.Diagnostics;

namespace ClientMemoryPack;

/// <summary>
/// 提供ArrayPool的扩展
/// </summary>
public static class ArrayPoolExtensions
{
    /// <summary>
    /// 申请可回收的IArrayOwner
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrayPool"></param>
    /// <param name="length">有效数据长度</param>
    /// <returns></returns>
    public static IArrayOwner<T> RentArrayOwner<T>(this ArrayPool<T> arrayPool, int length)
    {
        return new ArrayOwner<T>(arrayPool, length);
    }
}

/// <summary>
/// 定义数组持有者的接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IArrayOwner<T> : IDisposable
{
    /// <summary>
    /// 获取数据有效数据长度
    /// </summary>
    int Length { get; }

    /// <summary>
    /// 获取持有的数组
    /// </summary>
    T[] Array { get; }

    /// <summary>
    /// 转换为Span
    /// </summary>
    /// <returns></returns>
    Span<T> AsSpan();

    /// <summary>
    /// 转换为Memory
    /// </summary>
    /// <returns></returns>
    Memory<T> AsMemory();
}

/// <summary>
/// 表示数组持有者
/// </summary>
/// <typeparam name="T"></typeparam>
[DebuggerDisplay("Length = {Length}")]
sealed class ArrayOwner<T> : IArrayOwner<T>
{
    private bool disposed = false;
    private readonly ArrayPool<T> _arrayPool;

    /// <summary>
    /// 获取数据有效数据长度
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// 获取持有的数组
    /// </summary>
    public T[] Array { get; }

    public Span<T> AsSpan()
    {
        return Array.AsSpan(0, Length);
    }

    public Memory<T> AsMemory()
    {
        return Array.AsMemory(0, Length);
    }

    /// <summary>
    /// 数组持有者
    /// </summary>
    /// <param name="arrayPool"></param>
    /// <param name="length"></param> 
    public ArrayOwner(ArrayPool<T> arrayPool, int length)
    {
        _arrayPool = arrayPool;
        Length = length;
        Array = arrayPool.Rent(length);
    }

    /// <summary>
    /// 将对象进行回收
    /// </summary>
    public void Dispose()
    {
        if (disposed == false)
        {
            _arrayPool.Return(Array);
            GC.SuppressFinalize(this);
        }
        disposed = true;
    }

    ~ArrayOwner()
    {
        Dispose();
    }
}
