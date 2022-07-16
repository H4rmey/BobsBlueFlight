using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq; // Import the C# collection query api

public class Dice : RigidBody
{
    public int nof_faces = 6;

    public List<Area> dice_rotations = new List<Area>();

    [Export]
    public float force = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Random rand = new Random();

        float   x = (float)rand.NextDouble();
        float   y = (float)rand.NextDouble();
        float   z = (float)rand.NextDouble();
        Vector3 force_position = new Vector3(x, y, z);
                x = ((float)rand.NextDouble() * force);
                y = ((float)rand.NextDouble() * force);
                z = ((float)rand.NextDouble() * force);
        Vector3 force_impulse = new Vector3(x, y, z);
        ApplyImpulse(force_position, force_impulse);

    }

    public override void _Process(float delta) {
    }

    public override void _PhysicsProcess(float delta)
    {
    }
}
