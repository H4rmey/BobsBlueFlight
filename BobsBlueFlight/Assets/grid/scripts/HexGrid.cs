using Godot;
using System;
using System.Collections.Generic;

public class HexGrid : Spatial
{
    public PackedScene temp_hex_tile = ResourceLoader.Load("res://Assets/grid/HexTile.tscn") as PackedScene;
    public List<PackedScene> placable_tiles = new List<PackedScene>();
    [Export]
    public float tile_size = 1.1f;
    [Export]
    public int grid_size_x = 15;
    [Export]
    public int grid_size_z = 10;

    [Export]
    public float tile_scale = 0.035f;
    [Export]
    public float rotation_offset = 30.0f;
    [Export]
    public float hight_offset = 7.0f;

    [Export]
    public int nof_different_tiles = 18;
    [Export]
    public List<int> skippables = new List<int>();

    public bool allow_click = true;

    public EmptyHex mouse_hover_tile;
    private CustomSignals cs;
    [Export]
    public Dice dice;

    public int level = 1;
    [Export]
    public int level_max = 6;

    public override void _Ready()
    {
        base._Ready();

        get_placable_tiles();  
        create_grid();

        get_dice(level);

        cs = GetNode<CustomSignals>("/root/CustomSignals");
        cs.Connect("LevelUp", this, "level_up");
    }

    public void level_up(int x)
    {
        if (dice.last_rolled_value + 1 == dice.nof_faces)
        {
            level++;
            get_dice(level, false);
        }

        if (level > level_max)
        {
            level = level_max;
        }
    }

    public void set_dice_to_center()
    {
        Vector3 pos = new Vector3(grid_size_x/2, 10, grid_size_z/2);
        dice.Translate(pos);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (dice.GlobalTransform.origin.y < -10)
        {
            get_dice(level);
        }

        if (level > level_max)
        {
            level = level_max;
        }
    }


    public void get_dice(int level, bool reset_position = true)
    {
        Vector3 old_pos = new Vector3(grid_size_x/2, 10, grid_size_z/2);
        if (!reset_position && dice != null)
        {
            old_pos = dice.GlobalTransform.origin + new Vector3(0, 2, 0);
        }

        if (dice != null)
        {
            RemoveChild(dice);
        }

        PackedScene scene = ResourceLoader.Load("res://Assets/dice/dice_" + level + ".tscn") as PackedScene;

        dice = (Dice)scene.Instance();
        AddChild(dice);


        if (reset_position)
        {
            set_dice_to_center();
        }
        else
        {
            dice.Translate(old_pos);
        }
    }

    public void get_placable_tiles()
    {
        for (int i = 1; i <= nof_different_tiles; i++)
        {
            placable_tiles.Add(new PackedScene());

            if (!skippables.Contains(i))
            {
                PackedScene hex_tile = ResourceLoader.Load("res://Assets/grid/tiles/tile_" + i.ToString() + ".tscn") as PackedScene;

                placable_tiles[i-1] = hex_tile;
            }
        }
    }

    public void create_grid()
    {
        for (int x = 0; x < grid_size_x; x++)
        {
            Vector3 tile_coordinates = Vector3.Zero;
            for (int z = 0; z < grid_size_z; z++)
            {
                EmptyHex current_hex_tile = (EmptyHex)temp_hex_tile.Instance();
                AddChild(current_hex_tile);
                current_hex_tile.grid_pos = new Vector2(x, z);

                float temp_x = x * tile_size * Mathf.Cos(Mathf.Pi * 30 / 180); 
                float temp_z = z * tile_size + ((x % 2 == 0) ? 0 : tile_size/2);
                Vector3 pos = new Vector3(temp_x, 0, temp_z);

                current_hex_tile.Translate(pos);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("left_mouse") && allow_click)
        {
            if (dice.last_rolled_value == -1)
            {
                return;
            }

            allow_click = false;

            Spatial tile = (Spatial)placable_tiles[dice.last_rolled_value].Instance();

            Spatial spatial = tile.GetChild(0) as Spatial; 
            
            spatial.Scale = Vector3.One * tile_scale;
            spatial.Translate(new Vector3(0, hight_offset, 0));
            spatial.Rotate(new Vector3(0,1,0), Mathf.Pi * rotation_offset/180);

            mouse_hover_tile.AddChild(tile);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Space)
            {
                allow_click = true;
                dice.last_rolled_value = -1;
            }
        }
    }
}
