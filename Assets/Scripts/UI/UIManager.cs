using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text _ammoCount_Text;
    [SerializeField] GameObject _reloadWidget;
    [SerializeField] Image _reloadFillImage;
    [SerializeField] Image _healthFillImage;
    public float updateSpeed = 0.18f;

    CanvasGroup _reloadWidgetCG;

    private void OnEnable()
    {
        _reloadWidgetCG = _reloadWidget.GetComponent<CanvasGroup>();
        Reset();

        PlayerWeapon.OnAmmoChanged += UpdateAmmoDisplay;
        PlayerWeapon.OnReload += ReloadWeaponUI;
    }

    void Reset()
    {
        _healthFillImage.fillAmount = 1;
    }

    private void OnDisable()
    {
        PlayerWeapon.OnAmmoChanged -= UpdateAmmoDisplay;
        PlayerWeapon.OnReload -= ReloadWeaponUI;
    }

    void UpdateAmmoDisplay(int value)
    {
        _ammoCount_Text.text = value.ToString();
    }

    public void ReloadWeaponUI(float reloadTime)
    {
        StartCoroutine(UpdateReloadBar(reloadTime));
    }

    IEnumerator UpdateReloadBar(float reloadTime)
    {
        _reloadWidgetCG.alpha = 1;

        float elapsed = 0;
        while (elapsed < reloadTime)
        {
            yield return null;
            elapsed += Time.deltaTime;
            _reloadFillImage.fillAmount = 1.0f - Mathf.Clamp01(elapsed / reloadTime);
        }

        _reloadWidgetCG.alpha = 0;
    }

    #region hp ui

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

    #endregion
}
