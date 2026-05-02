namespace Sia.Transformations;

public static class GridDrunkard
{
    public static Grid Drunkard(this Grid grid, byte oldValue, byte newValue, int steps,
        Neighbourhood mode = Neighbourhood.Moore, Topology topology = Topology.Bounded)
    {
        var startPool = grid.IndicesOf(oldValue);
        if (startPool.Count == 0) return grid;
        
        var poolIndex = grid.Rng.Next(startPool.Count);
        var gridIndex = startPool[poolIndex];

        grid.Front[gridIndex] = newValue;

        while (steps-- > 0)
        {
            var valid = grid.GetNeighboursIndex(gridIndex, mode, topology)
                .Where(n => grid.Front[n] == oldValue || grid.Front[n] == newValue)
                .ToList();
            
            if (valid.Count == 0) break;
            
            var randomValid = valid[grid.Rng.Next(valid.Count)];

            grid.Front[randomValid] = newValue;
            gridIndex = randomValid;
        }

        return grid;
    }
}