using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthUI : MonoBehaviour
{
    [SerializeField] Image _healthFillImage;
    public float updateSpeed = 0.18f;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    void Reset()
    {
        _healthFillImage.fillAmount = 1;
    }

    public void ChangeHitPointUI(Damageable damageable)
    {
        float hpPercentage = damageable.CurrentHitPoints / damageable.maxHitPoints;

        StartCoroutine(UpdateHitPointSequence(hpPercentage));
    }

    IEnumerator UpdateHitPointSequence(float pct)
    {
        float preChangePct = _healthFillImage.fillAmount;
        float elapsed = 0;

        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            _healthFillImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeed);
            yield return null;
        }
        _healthFillImage.fillAmount = pct;
    }
}
