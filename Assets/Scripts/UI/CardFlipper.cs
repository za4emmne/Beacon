using UnityEngine;
using DG.Tweening;

public class CardFlipper : MonoBehaviour
{
    private bool _isFlipped = false;
    
    public bool IsFlipped => _isFlipped;

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space)) Flip();
    }

    public void Flip()
    {
       _isFlipped = !_isFlipped;
       transform.DORotate(new Vector3(0, _isFlipped ? 180f : 0f, 0), 0.25f);
    }

    public void Reset()
    {
        _isFlipped = false;
        transform.rotation = Quaternion.identity;
    }
}
