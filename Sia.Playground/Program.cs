using Sia;
using Raylib_cs;

const int cellSize = 8;
const int autoStepEveryNFrames = 1;

var grid = new Grid(200, 100);

List<Action> steps = [];

grid[40, 30] = 1;
grid[160, 30] = 2;
grid[40, 70] = 3;
grid[160, 70] = 4;
grid[100, 50] = 5;

for (var i = 0; i < 100; i++)
{
    steps.Add(() => {
        grid.Expand(1, 0, 1, Grid.Neighbourhood.Moore, Grid.Topology.Bounded);
        grid.Expand(2, 0, 1, Grid.Neighbourhood.Moore, Grid.Topology.Bounded);
        grid.Expand(3, 0, 1, Grid.Neighbourhood.Moore, Grid.Topology.Bounded);
        grid.Expand(4, 0, 1, Grid.Neighbourhood.Moore, Grid.Topology.Bounded);
        grid.Expand(5, 0, 1, Grid.Neighbourhood.Moore, Grid.Topology.Bounded);
    });
}




var stepIndex = 0;

Raylib.InitWindow(grid.Width * cellSize, grid.Height * cellSize, "Sia Playground");
Raylib.SetTargetFPS(60);

bool autoMode = false;
int frameCounter = 0;

while (!Raylib.WindowShouldClose())
{
    if (Raylib.IsKeyPressed(KeyboardKey.Space) && stepIndex < steps.Count)
    {
        steps[stepIndex++]();
    }
    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
    {
        autoMode = !autoMode;
    }
    if (autoMode && stepIndex < steps.Count)
    {
        frameCounter++;
        if (frameCounter >= autoStepEveryNFrames)
        {
            steps[stepIndex++]();
            frameCounter = 0;
        }
    }
    
    Raylib.BeginDrawing();
    DrawGrid(grid);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
return;

void DrawGrid(Grid g)
{
    for (var y = 0; y < g.Height; y++)
    {
        for (var x = 0; x < g.Width; x++)
        {
            Color color = g[x, y] switch
            {
                0 => Color.Black,
                1 => Color.White,
                2 => Color.Red,
                3 => Color.Green,
                4 => Color.Blue,
                _ => Color.Yellow
            };
            Raylib.DrawRectangle(x * cellSize, y * cellSize, cellSize, cellSize, color);
        }
    }
}