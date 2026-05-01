namespace Sia;

public class Grid
{
    public int Width { get; }
    public int Height { get; }
    public int Length => Width * Height;
    
    /// <summary>
    /// Raw cell data, row-major order. Index as Data[y * Width + x].
    /// Modifying values directly bypasses InBounds checks - use the indexer for safe access.
    /// </summary>
    public byte[] Data { get; }
    
    public Grid(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        
        Width = width;
        Height = height;
        Data = new byte[Length];
    }

    public Grid(Grid source)
    {
        Width = source.Width;
        Height = source.Height;
        Data = new byte[Length];
        Array.Copy(source.Data, Data, Length);
    }

    public byte this[int x, int y]
    {
        get => InBounds(x, y) ? Data[y * Width + x] : (byte)0;
        set { if (InBounds(x, y)) Data[y * Width + x] = value; }
    }
    
    private bool InBounds(int x, int y) => (uint)x < (uint)Width && (uint)y < (uint)Height;
    
    public void Fill(byte value)
    {
        Array.Fill(Data, value);
    }
}