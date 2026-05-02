namespace Sia.Transformations;

public static class GridExpand
{
    public static Grid Expand(this Grid grid, byte oldValue, byte newValue, int steps = 1, 
        Neighbourhood mode = Neighbourhood.Moore, Topology topology = Topology.Bounded)
    {
        while (steps-- > 0)
        {
            for (var i = 0; i < grid.Length; i++)
            {
                if (grid.Front[i] != oldValue)
                {
                    grid.Back[i] = grid.Front[i];
                    continue;
                }
                var hasMatchingNeighbour = false;
                foreach (var ni in grid.GetNeighboursIndex(i, mode, topology))
                {
                    if (grid.Front[ni] == newValue)
                    {
                        hasMatchingNeighbour = true;
                        break;
                    }
                }
                grid.Back[i] = hasMatchingNeighbour ? newValue : grid.Front[i];
            }
            grid.Swap();
        }
        return grid;
    }
}