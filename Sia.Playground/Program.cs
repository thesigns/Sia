using Sia;
using Raylib_cs;

const int cellSize = 8;
const int autoStepEveryNFrames = 5;

var grid = new Grid(200, 100);

List<Action> steps =
[
    () => { grid[10, 10] = 1; },
    () => { grid[42, 12] = 2; },
];






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