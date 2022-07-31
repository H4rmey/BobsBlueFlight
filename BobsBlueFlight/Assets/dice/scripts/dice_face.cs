using Godot;
using System;

public class dice_face : Area
{
    public bool is_chosen = false;

    public void OnEnter(Node other)
    {
        is_chosen = true;
        
    }

    public void OnExit(Node other)
    {
        is_chosen = false;
    }
}
