using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    Gradient gradient;
    [SerializeField]
    private Image fill;
    
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHealth(int health)
    {
        fill.color = gradient.Evaluate(1f);
        slider.maxValue = health;
        slider.value = health;
    }
}
