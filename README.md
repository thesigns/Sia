# Sia

A C# library for procedural generation on 2D grids.

Sia operates on byte grids (0-255 values per cell) using mathematical morphology
and procedural generation algorithms. The library does not define what cell values
mean - that's up to the user. A `0` might be a wall, empty space, water, or anything
else. Sia provides the operations; semantics are yours.

## Status

Early development. API is unstable and will change.

## License

See LICENSE.txt file.