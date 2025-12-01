using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("In game elements")] 
    [SerializeField] private TMP_Text roomNumberText;
    [SerializeField] private Slider healthBar;

    public void UpdateHealthUI(Context ctx)
    {
        if (ctx != null)
        {
            healthBar.value = (int)ctx.Data[0];
        }
    }

    public void UpdateRoomNumberUI()
    {
        roomNumberText.text = $"Room: {GameManager.Instance.level}";
    }
}
