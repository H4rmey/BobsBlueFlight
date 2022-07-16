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
    public List<Vector3> dice_mapping;

    [Export]
    public float wait_time;
    [Export]
    public float correction_threshold;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Random rand = new Random();

        float   x = (float)rand.NextDouble();
        float   y = (float)rand.NextDouble();
        float   z = (float)rand.NextDouble();
        Vector3 force_position = new Vector3(x, y, z);
                x = ((float)rand.NextDouble() * 5);
                y = ((float)rand.NextDouble() * 5);
                z = ((float)rand.NextDouble() * 5);
        Vector3 force_impulse = new Vector3(x, y, z);
        ApplyImpulse(force_position, force_impulse);

        for (int i = 1; i <= nof_faces; i++)
        {
            dice_rotations.Add(GetNode<Area>("nr_" + i.ToString() ));
        } 
    }

    public override void _Process(float delta) {


    }

    public override void _PhysicsProcess(float delta)
    {
    }

    public Vector3 normalize_rotation(Vector3 rot, float clamp)
    {
        Vector3 ret_vec;
        ret_vec.x = normalize(rot.x, clamp);
        ret_vec.y = normalize(rot.y, clamp);
        ret_vec.z = normalize(rot.z, clamp);
        return ret_vec;
    }

    public float normalize(float value, float clamp)
    {
        value = value % clamp;

        if (value < 0)
        {
            value += clamp;
        }

        return value;
    }
}
