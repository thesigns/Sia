namespace Sia;

public class Grid
{
    public enum Neighbourhood
    {
        Moore, // 8
        VonNeumann, // 4
    }

    public enum Topology
    {
        Bounded, 
        Torus, 
        HorizontalCylinder, 
        VerticalCylinder
    }
    
    private static readonly (int dx, int dy)[] MooreOffsets = 
    {
        (-1, -1), (0, -1), (1, -1),
        (-1,  0),          (1,  0),
        (-1,  1), (0,  1), (1,  1)
    };

    private static readonly (int dx, int dy)[] VonNeumannOffsets = 
    {
            (0, -1), 
        (-1, 0), (1, 0), 
            (0, 1)
    };
    
    private static (int dx, int dy)[] GetOffsets(Neighbourhood mode) => mode switch
    {
        Neighbourhood.Moore => MooreOffsets,
        Neighbourhood.VonNeumann => VonNeumannOffsets,
        _ => throw new ArgumentOutOfRangeException(nameof(mode))
    };
    
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
        while (steps-- > 0)
        {
            for (var i = 0; i < Length; i++)
            {
                if (front[i] != intoColor)
                {
                    back[i] = front[i];
                    continue;
                }
                var hasColorNeighbour = false;
                foreach (var ni in GetNeighboursIndex(i, mode))
                {
                    if (front[ni] == color)
                    {
                        hasColorNeighbour = true;
                        break;
                    }
                }
                back[i] = hasColorNeighbour ? color : front[i];
            }
            Swap();
        }
    }

    public IEnumerable<(int x, int y)> GetNeighbours(int x, int y, 
        Neighbourhood mode = Neighbourhood.Moore, Topology topology = Topology.Bounded)
    {
        var offsets = GetOffsets(mode);
        foreach (var (dx, dy) in offsets)
        {
            var nx = x + dx;
            var ny = y + dy;

            if (topology is Topology.Torus or Topology.HorizontalCylinder)
            {
                if (nx < 0) nx += Width;
                else if (nx >= Width) nx -= Width;
            }
            else
            {
                if (nx < 0 || nx >= Width) continue;                
            }
            
            if (topology is Topology.Torus or Topology.VerticalCylinder)
            {
                if (ny < 0) ny += Height;
                else if (ny >= Height) ny -= Height;
            }
            else
            {
                if (ny < 0 || ny >= Height) continue;
            }
            
            yield return (nx, ny);
        }
    }

    public IEnumerable<int> GetNeighboursIndex(int index, 
        Neighbourhood mode = Neighbourhood.Moore, Topology topology = Topology.Bounded)
    {
        var x = index % Width;
        var y = index / Width;
        foreach (var (nx, ny) in GetNeighbours(x, y, mode, topology))
            yield return ny * Width + nx;
    }
    

    private void Swap()
    {
        (front, back) = (back, front);
    }
}