# GameOfLife
Basic C# console implementation of Conway's Game Of Life.
## Usage
### With randomized initializtion
```C#
Life gol = new Life(950, 250, 4);
gol.Simulate(0, false);
```
### With a prewritten pattern
```C#
Life gol = new Life(200, 100, 4, "gosper_glider_gun.txt");
gol.Simulate(30, false);
```
#### Example pattern (Gosper glider gun)
(1 represents a living cell, anything else a dead one)
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
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/randomizedinit2.gif)
### Gosper glider gun (prewritten pattern)
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/gosperglidergun.gif)
