using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public Slider energySlider;
    public Image fillImage;
    public float maxEnergy = 100f;
    public float currentEnergy;

    private void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
    }

    public void DecreaseEnergy(float amount)
    {
        currentEnergy -= amount;

        currentEnergy = Mathf.Max(currentEnergy, 0f);

        UpdateEnergyBar();
    }

    public void IncreaseEnergy(float amount)
    {
        currentEnergy += amount;

        currentEnergy = Mathf.Min(currentEnergy, maxEnergy);

        UpdateEnergyBar();
    }

    private void UpdateEnergyBar()
    {
        energySlider.value = currentEnergy / maxEnergy;

        if (fillImage != null)
        {
            fillImage.color = Color.Lerp(Color.red, Color.green, currentEnergy / maxEnergy);
        }
    }
}
