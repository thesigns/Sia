namespace Sia.Transformations;

public static class GridOutline
{
    public static Grid Outline(this Grid grid, byte oldValue, byte newValue, byte borderValue,
        Neighbourhood mode = Neighbourhood.Moore, Topology topology = Topology.Bounded)
    {
        
        var toChange = new List<int>();

        for (var i = 0; i < grid.Length; i++)
        {
            if (grid.Front[i] != oldValue) continue;
    
            foreach (var ni in grid.GetNeighboursIndex(i, mode, topology))
            {
                if (grid.Front[ni] == borderValue)
                {
                    toChange.Add(i);
                    break;
                }
            }
        }

        foreach (var i in toChange)
            grid.Front[i] = newValue;
        
        return grid;
    }    
}
