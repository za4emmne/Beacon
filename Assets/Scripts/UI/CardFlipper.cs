using UnityEngine;
using DG.Tweening;

public class CardFlipper : MonoBehaviour
{
   private bool _isFlipped = false;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Space)) Flip();
      
   }

   private void Flip()
   {
      _isFlipped = !_isFlipped;
      transform.DORotate(new(0, _isFlipped ? 0f : 180, 0), 0.25f);
   }
}
