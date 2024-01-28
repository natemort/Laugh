using Godot;
using Laugh.code;

public partial class Lazer : CharacterBody2D, Killable
{
	[Export] public int Bounces = 10;
	private double _spawnTime;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnTime = Time.GetTicksMsec();
	}

	public override void _PhysicsProcess(double delta)
	{
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			GD.Print("Collision: " + collision.GetCollider());
			if (collision.GetCollider() is Killable k)
			{
				k.Kill();
				Kill();
			} else if (Bounces-- > 0)
			{
				Velocity = Velocity.Bounce(collision.GetNormal());
				Rotation = Velocity.Angle();
			}
			else
			{
				Kill();
			}
		}
	}

	public void Kill()
	{
		this.QueueFree();
	}
}
