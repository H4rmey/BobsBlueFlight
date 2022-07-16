using Godot;
using System;

public class dice_face : Area
{
    public void OnEnter(Node other)
    {
        GD.Print(Name);
    }
}
