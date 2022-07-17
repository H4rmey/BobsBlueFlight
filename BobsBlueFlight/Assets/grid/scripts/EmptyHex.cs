using Godot;
using System;

public class EmptyHex : StaticBody
{
    public Vector2 grid_pos = Vector2.Zero;
    public HexGrid hex_grid;

    public override void _Ready()
    {
        base._Ready();

        hex_grid = GetParent<HexGrid>();

    }

    public void _on_StaticBody_mouse_entered()
    {
        hex_grid.mouse_hover_tile = this;
    }
}
