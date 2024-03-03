# GameOfLife
Basic C# console implementation of Conway's Game Of Life.
## Usage
### With randomized initializtion
```C#
GameOfLife gol = new GameOfLife(200, 50, 4);
gol.Simulate(70, false);
```
### With a prewritten pattern
```C#
GameOfLife gol = new GameOfLife(200, 50, 4, "gosper_glider_gun.txt");
gol.Simulate(70, false);
```
#### Example pattern (Gosper glider gun)
```

                         1
                       1 1
             11      11            11
            1   1    11            11
 11        1     1   11
 11        1   1 11    1 1
           1     1       1
            1   1
             11
```
## Showcase
### Randomized initialization
![]()
### Gosper glider gun (prewritten pattern)
![]()
