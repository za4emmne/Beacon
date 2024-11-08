using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player singleton {  get; private set; }

    private void Awake()
    {
        singleton = this;
    }
}
