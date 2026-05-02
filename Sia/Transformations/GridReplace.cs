namespace Sia.Transformations;

public static class GridReplace
{
    public static Grid Replace(this Grid grid, byte oldValue, byte newValue)
    {
        for (var i = 0; i < grid.Length; i++)
        {
            if (grid.Front[i] == oldValue)
            {
                grid.Front[i] = newValue;
            }
        }
        return grid;
    }
}