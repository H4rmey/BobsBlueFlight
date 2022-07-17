using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq; // Import the C# collection query api

public class Dice : RigidBody
{
    [Export]
    public int nof_faces = 6;

    public List<dice_face> dice_faces = new List<dice_face>();

    [Export]
    public float force = 10;

    public int last_rolled_value = 0;

    private CustomSignals cs;

    public override void _Ready()
    {
        get_faces();

        cs = GetNode<CustomSignals>("/root/CustomSignals");
    }

    public void get_faces()
    {
        dice_faces.Clear();

        for (int i = 1; i <= nof_faces; i++)
        {
            dice_faces.Add(GetNode<dice_face>( "nr_" + i.ToString() ));
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        for (int i = 0; i < nof_faces; i++)
        {
            dice_face face = dice_faces[i];
            if (face.is_chosen)
            {
                last_rolled_value = i;
                cs.EmitSignal(nameof(CustomSignals.LevelUp), i);
            }
        }
    }

    public void apply_force() 
    {
        Random rand = new Random();

        float   x = map((float)rand.NextDouble(), 0, 1, -1, 1);
        float   y = map((float)rand.NextDouble(), 0, 1, -1, 1);
        float   z = map((float)rand.NextDouble(), 0, 1, -1, 1);
        Vector3 force_position = new Vector3(x, y, z);
                x = map((float)rand.NextDouble(), 0, 1, -force, force);
                y = map((float)rand.NextDouble(), 0, 1, 5, force);
                z = map((float)rand.NextDouble(), 0, 1, -force, force);
        Vector3 force_impulse = new Vector3(x, y, z);
        ApplyImpulse(force_position, force_impulse);
    }

    public float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Space)
            {
                apply_force();
            }
        }
    }
}
