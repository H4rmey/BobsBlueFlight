using Godot;
using System;
using System.Collections.Generic;

public class HexGrid : Spatial
{
    [Export]
    public PackedScene temp_hex_tile = ResourceLoader.Load("res://Assets/grid/HexTile.tscn") as PackedScene;
    [Export]
    public List<PackedScene> hex_tiles = new List<PackedScene>();

    [Export]
    public float tile_size = 1.1f;
    [Export]
    public int grid_size_x = 15;
    [Export]
    public int grid_size_z = 10;
    [Export]
    public int nof_different_tiles = 18;
    [Export]
    public List<int> skippables = new List<int>();

    public override void _Ready()
    {
        base._Ready();

        for (int i = 1; i <= nof_different_tiles; i++)
        {
            hex_tiles.Add(new PackedScene());

            if (!skippables.Contains(i))
            {
                PackedScene hex_tile = ResourceLoader.Load("res://Assets/grid/hexagons/tile_" + i.ToString() + ".glb") as PackedScene;

                hex_tiles[i-1] = hex_tile;
            }
        }

        for (int x = 0; x < grid_size_x; x++)
        {
            Vector3 tile_coordinates = Vector3.Zero;
            for (int z = 0; z < grid_size_z; z++)
            {
                Spatial current_hex_tile = (Spatial)temp_hex_tile.Instance();
                AddChild(current_hex_tile);

                float temp_x = x * tile_size * Mathf.Cos(Mathf.Pi * 30 / 180); 
                float temp_z = z * tile_size + ((x % 2 == 0) ? 0 : tile_size/2);
                Vector3 pos = new Vector3(temp_x, 0, temp_z);

                current_hex_tile.Translate(pos);
            }
            
        }
    }
}
