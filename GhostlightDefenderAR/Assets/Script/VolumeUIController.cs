using UnityEngine;
using UnityEngine.UI;

public class VolumeUIController : MonoBehaviour
{
    [Header("Background Volume")]
    public Slider backgroundSlider;
    public Image backgroundIcon;
    public Sprite bgOnIcon;
    public Sprite bgOffIcon;

    [Header("SFX Volume")]
    public Slider sfxSlider;
    public Image sfxIcon;
    public Sprite sfxOnIcon;
    public Sprite sfxOffIcon;

    private bool isChangingFromCode = false;

    private void Start()
    {
        InitUI();
        AddListeners();
    }

    private void InitUI()
    {
        float bgVolume = SoundManager.Instance.GetVolume(SoundCategory.Background);
        float sfxVolume = SoundManager.Instance.GetVolume(SoundCategory.SFX);

        isChangingFromCode = true;

        backgroundSlider.value = bgVolume;
        sfxSlider.value = sfxVolume;

        UpdateIcons();

        isChangingFromCode = false;
    }

    private void AddListeners()
    {
        backgroundSlider.onValueChanged.AddListener(val => {
            if (isChangingFromCode) return;
            UpdateBackgroundVolume(val);
        });

        sfxSlider.onValueChanged.AddListener(val => {
            if (isChangingFromCode) return;
            UpdateSFXVolume(val);
        });
    }

    private void UpdateBackgroundVolume(float value)
    {
        SoundManager.Instance.SetVolume(SoundCategory.Background, value);
        UpdateIcons();
    }

    private void UpdateSFXVolume(float value)
    {
        SoundManager.Instance.SetVolume(SoundCategory.SFX, value);
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        backgroundIcon.sprite = backgroundSlider.value > 0 ? bgOnIcon : bgOffIcon;
        sfxIcon.sprite = sfxSlider.value > 0 ? sfxOnIcon : sfxOffIcon;
    }

    public void ToggleBackgroundMute()
    {
        float newVal = backgroundSlider.value > 0 ? 0f : 1f;
        isChangingFromCode = true;
        backgroundSlider.value = newVal;
        UpdateBackgroundVolume(newVal);
        isChangingFromCode = false;
    }

    public void ToggleSFXMute()
    {
        float newVal = sfxSlider.value > 0 ? 0f : 1f;
        isChangingFromCode = true;
        sfxSlider.value = newVal;
        UpdateSFXVolume(newVal);
        isChangingFromCode = false;
    }
}
