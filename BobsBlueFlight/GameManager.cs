using Godot;
using System;

public class GameManager : Node
{
    public Camera camera;

    public override void _Ready()
    {
        camera = GetNode<Camera>("Camera");
    }

}
