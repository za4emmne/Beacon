using UnityEngine;

public struct PlayerTag { }
public struct PlayerInputComponent {
    public float Horizontal;
    public float Vertical;
    public bool isMobile;
}

public struct PlayerSpeedComponent
{
    public float Value;
}

public struct PlayerDirectionComponent
{
    public Vector2 Value;
}

public struct Rigidbody2DReference {  
    public float Rigidbody; 
}

