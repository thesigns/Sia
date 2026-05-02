namespace Sia.Transformations;

public static class GridFill
{
    public static Grid Fill(this Grid grid, byte newValue)
    {
        Array.Fill(grid.Front, newValue);
        return grid;
    }
}