namespace Sia.Transformations;

public static class GridScatter
{
    public static Grid Scatter(this Grid grid, byte oldValue, byte newValue, int count)
    {
        List<int> scatterPool = [];
        for (var i = 0; i < grid.Length; i++)
        {
            if (grid.Front[i] == oldValue)
                scatterPool.Add(i);
        }
        
        while (scatterPool.Count > 0 && count > 0)
        {
            var scatterIndex = grid.Rng.Next(scatterPool.Count);
            var gridIndex = scatterPool[scatterIndex];
            scatterPool[scatterIndex] = scatterPool[^1];
            scatterPool.RemoveAt(scatterPool.Count - 1);
            grid.Front[gridIndex] = newValue;
            count--;
        }
        
        return grid;
    }
}