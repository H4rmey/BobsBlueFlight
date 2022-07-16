using Godot;
using System;

public class EmptyHex : StaticBody
{
    public void _on_StaticBody_mouse_entered(Node other)
    {
        GD.Print(Name);
    }
}
