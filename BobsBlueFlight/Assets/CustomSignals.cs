using Godot;
using System;

public class CustomSignals : Node
{
    [Signal]public delegate void LevelUp(int value);
}
