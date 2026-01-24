using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Text _damageText;
    private GeneratorDamageText generator;

    private void Awake()
    {
        _damageText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        DamageAnimationText();
    }

    public void InitGenerator(GeneratorDamageText gen)
    {
        generator = gen;
    }

    public void SetDamage(float damage)
    {
        _damageText.text = damage.ToString();
    }

    private void DamageAnimationText()
    {
        transform.DOMoveY(transform.position.y + 0.5f, 2)
            .SetEase(Ease.OutQuad);
        _damageText.DOFade(0, 2).OnComplete(() =>
        {
            _damageText.DOFade(1, 0).OnComplete(() =>
            {
                generator.PutObject(this);
            });
        });
    }
}
