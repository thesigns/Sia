using Sia;
using Raylib_cs;
using Sia.Transformations;

const int cellSize = 8;
const int autoStepEveryNFrames = 1;

var grid = new Grid(200, 100);

grid
    .Scatter(0, 1, 100)
    .Expand(0, 1, 4, Neighbourhood.VonNeumann)
    .Scatter(0, 2, 100)
    .Expand(0, 2, 8, Neighbourhood.Moore, Topology.Torus)
    .Replace(1, 0);

List<Action> steps = [];

var stepIndex = 0;

Raylib.InitWindow(grid.Width * cellSize, grid.Height * cellSize, "Sia Playground");
Raylib.SetTargetFPS(30);

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
    grid.Scatter(2, 0, 20);
    
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
                0 => new Color(0, 0, 0),
                1 => new Color(255, 255, 255),
                2 => new Color(255, 0, 0),
                3 => new Color(0, 255, 0),
                4 => new Color(0, 0, 255),
                5 => new Color(255, 255, 0),
                6 => new Color(255, 0, 255),
                7 => new Color(0, 255, 255),
                _ => new Color(127, 127, 127),
            };
            Raylib.DrawRectangle(x * cellSize, y * cellSize, cellSize, cellSize, color);
        }
    }
}