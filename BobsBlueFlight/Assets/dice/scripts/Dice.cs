using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class Dice : RigidBody
{
	[Export]
	public int nof_faces = 6;

	public List<dice_face> dice_faces = new List<dice_face>();

	[Export]
	public float force = 10;
	[Export]
	public float waittime = 1;

	public bool allow_new_value = false;
	public int last_rolled_value = 0;
	public int current_roll_value = 999;
	public float current_lowest_value = 999;


	private CustomSignals cs;
	private Timer timer;
	private Timer timer_stand_still;
	private bool timer_is_running;

    public TextureRect tile_preview;
    public List<Texture> tile_preview_options = new List<Texture>();

	public override void _Ready()
	{
		get_faces();
        get_previews();

		cs = GetNode<CustomSignals>("/root/CustomSignals");

		timer = new Timer();
		timer.Connect("timeout", this, "_on_timer_timeout");
		AddChild(timer);
		timer.Start();
		timer.WaitTime = waittime;

        PackedScene scene = ResourceLoader.Load("res://Assets/GUI.tscn") as PackedScene;
        tile_preview = (TextureRect)scene.Instance();
        AddChild(tile_preview);
	}

    public void get_previews()
    {
        for (int i = 1; i <= nof_faces; i++)
        {
            Texture scene = ResourceLoader.Load("res://Assets/GUI_elements/Prev/" + i.ToString() + ".png") as Texture;
            tile_preview_options.Add(scene);
        }
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

        current_lowest_value = 999;
		for (int i = 0; i < nof_faces; i++)
		{
			dice_face face = dice_faces[i];
			float pos_y = face.GlobalTransform.origin.y;
			if (pos_y < current_lowest_value)
			{
				current_lowest_value = pos_y; 
				current_roll_value = i+1;

                if (current_roll_value >= nof_faces)
                {
                    current_roll_value = nof_faces-1;
                }
                if (current_roll_value < 0)
                {
                    current_roll_value = 0;
                }

				if (tile_preview_options[current_roll_value] != null)
				{
                	tile_preview.Texture = tile_preview_options[current_roll_value];
				}
			}

			if (face.is_chosen && allow_new_value && is_velocity_zero())
			{
				last_rolled_value = i+1;
				timer.Start();
				face.is_chosen = false;

				if (tile_preview_options[i] != null)
				{
                	tile_preview.Texture = tile_preview_options[last_rolled_value-1];
				}
				break;
			}
		}
	}

	public bool is_velocity_zero()
	{
		return LinearVelocity.x < 0.01f && LinearVelocity.y < 0.01f && LinearVelocity.z < 0.01f;
	}

	public void _on_timer_timeout()
	{
		GD.Print("last_rolled_value = " + last_rolled_value);
		cs.EmitSignal(nameof(CustomSignals.LevelUp), last_rolled_value);
		timer.Stop();
	}

	public void apply_force(float p_force_top, float p_force_bot) 
	{
		Random rand = new Random();

		float   x = map((float)rand.NextDouble(), 0, 1, -1, 1);
		float   y = map((float)rand.NextDouble(), 0, 1, -1, 1);
		float   z = map((float)rand.NextDouble(), 0, 1, -1, 1);
		Vector3 force_position = new Vector3(x, y, z);
				x = map((float)rand.NextDouble(), 0, 1, -p_force_top, p_force_top);
				y = map((float)rand.NextDouble(), 0, 1, p_force_bot, p_force_top);
				z = map((float)rand.NextDouble(), 0, 1, -p_force_top, p_force_top);
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
				apply_force(force, 5);
				allow_new_value = true;
				timer.Stop();
			}
		}
	}
}
