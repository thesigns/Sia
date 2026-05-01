namespace Sia;

public class Grid
{
    public enum Neighbourhood
    {
        VonNeumann, // 4
        Moore, // 8
    }
    
    public int Width { get; }
    public int Height { get; }
    public int Length => Width * Height;
    
    /// <summary>
    /// Raw cell data, row-major order. Index as Data[y * Width + x].
    /// Modifying values directly bypasses InBounds checks - use the indexer for safe access.
    /// </summary>
    public byte[] Data => front;
    
    private byte[] back;
    private byte[] front;
    
    public Grid(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        
        Width = width;
        Height = height;
        front = new byte[Length];
        back = new byte[Length];
    }

    public Grid(Grid source)
    {
        Width = source.Width;
        Height = source.Height;
        front = new byte[Length];
        back = new byte[Length];
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

    public void Expand(byte color, byte intoColor, int steps, Neighbourhood mode)
    {
        var offsets = mode == Neighbourhood.Moore
            ? new[] { (-1,-1), (0,-1), (1,-1), (-1,0), (1,0), (-1,1), (0,1), (1,1) }
            : new[] { (0,-1), (-1,0), (1,0), (0,1) };
    
        while (steps-- > 0)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var index = y * Width + x;
                
                    if (front[index] != intoColor)
                    {
                        back[index] = front[index];
                        continue;
                    }
                
                    var hasColorNeighbour = false;
                    foreach (var (dx, dy) in offsets)
                    {
                        var nx = x + dx;
                        var ny = y + dy;
                        if ((uint)nx < (uint)Width && (uint)ny < (uint)Height
                                                   && front[ny * Width + nx] == color)
                        {
                            hasColorNeighbour = true;
                            break;
                        }
                    }
                
                    back[index] = hasColorNeighbour ? color : front[index];
                }
            }
            Swap();
        }
    }


    private void Swap()
    {
        (front, back) = (back, front);
    }
}