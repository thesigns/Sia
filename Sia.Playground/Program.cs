using Sia;
using Raylib_cs;
using Sia.Transformations;

const int cellSize = 8;
const int autoStepEveryNFrames = 1;

List<Action> steps = [];
var stepIndex = 0;


var grid = new Grid(200, 100);

const byte water = 4;
const byte land = 3;
const byte depression = 11;
const byte shallowWater = 10;

steps.Add(() => grid.Fill(water));
steps.Add(() => grid.Drunkard(water, land, 20000, Neighbourhood.VonNeumann, Topology.Torus));
steps.Add(() => grid.Expand(water, land, 1, Neighbourhood.VonNeumann, Topology.Torus));

for (var i = 0; i < 20; i++)
{
    steps.Add(() => grid.Drunkard(land, depression, 500, Neighbourhood.Moore, Topology.Torus));
}

steps.Add(() => grid.Expand(land, depression, 1, Neighbourhood.VonNeumann, Topology.Torus));
steps.Add(() => grid.Replace(depression, water));

steps.Add(() => grid.Outline(water, shallowWater, land, Neighbourhood.Moore, Topology.Torus));

steps.Add(() => grid.Replace(land, water));
steps.Add(() => grid.Outline(water, land, shallowWater, Neighbourhood.VonNeumann, Topology.Torus));
steps.Add(() => grid.Replace(shallowWater, water));

for (var i = 0; i < 25; i++)
{
    steps.Add(() => grid.Outline(water, shallowWater, land, Neighbourhood.VonNeumann, Topology.Torus));
    steps.Add(() => grid.Outline(water, land, shallowWater, Neighbourhood.Moore, Topology.Torus));
}

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
                0 => new Color(0, 0, 0),
                1 => new Color(255, 255, 255),
                2 => new Color(255, 0, 0),
                3 => new Color(0, 255, 0),
                4 => new Color(0, 0, 255),
                5 => new Color(255, 255, 0),
                6 => new Color(255, 0, 255),
                7 => new Color(0, 255, 255),
                8 => new Color(160, 0, 0),
                9 => new Color(0, 160, 0),
                10 => new Color(0, 0, 160),
                11 => new Color(120, 120, 0),
                12 => new Color(120, 0, 120),
                13 => new Color(0, 120, 120),
                _ => new Color(127, 127, 127),
            };
            Raylib.DrawRectangle(x * cellSize, y * cellSize, cellSize, cellSize, color);
        }
    }
}