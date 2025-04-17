using Godot;

public class Quest
{
    private Vector2 location;
    public Vector2 Location
    {
        get
        {
            return location;
        }
    }

    public Quest(Vector2 location){
        this.location = location;
    }
}