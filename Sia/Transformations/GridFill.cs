namespace Sia.Transformations;

public static class GridFill
{
    public static Grid Fill(this Grid grid, byte value)
    {
        Array.Fill(grid.Front, value);
        return grid;
    }
}