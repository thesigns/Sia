namespace Sia;

public class Grid
{
    
    private static readonly (int dx, int dy)[] MooreOffsets = 
    {
        (-1, -1), (0, -1), (1, -1),
        (-1,  0),          (1,  0),
        (-1,  1), (0,  1), (1,  1),
    };

    private static readonly (int dx, int dy)[] VonNeumannOffsets = 
    {
            (0, -1), 
      (-1, 0),    (1, 0), 
            (0,  1),
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
    
    public Random Rng { get; set; } = Random.Shared;
    
    internal byte[] Back;
    internal byte[] Front;
    internal void Swap() => (Front, Back) = (Back, Front);
    
    public Grid(int width, int height, Random rng)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        
        Width = width;
        Height = height;
        Rng = rng;
        Front = new byte[Length];
        Back = new byte[Length];
    }
    
    public Grid(int width, int height) : this(width, height, Random.Shared) { }
    

    public Grid(Grid source)
    {
        Width = source.Width;
        Height = source.Height;
        Rng = source.Rng;
        Front = new byte[Length];
        Back = new byte[Length];
        Array.Copy(source.Front, Front, Length);
    }

    public byte this[int x, int y]
    {
        get => InBounds(x, y) ? Front[y * Width + x] : (byte)0;
        set { if (InBounds(x, y)) Front[y * Width + x] = value; }
    }
    
    public bool InBounds(int x, int y) => (uint)x < (uint)Width && (uint)y < (uint)Height;

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
    
    public List<int> IndicesOf(byte value)
    {
        List<int> result = [];
        for (var i = 0; i < Length; i++)
            if (Front[i] == value) result.Add(i);
        return result;
    }

}