using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharactersHealth
{
    public void TakePills(float indexPill)
    {
        _health += indexPill;
        ChangeAwake();
    }
}
