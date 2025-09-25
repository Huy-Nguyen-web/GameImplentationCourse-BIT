using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;


    public void UpdateHealthUI(Context ctx)
    {
        if (ctx != null)
        {
            Debug.Log($"player health: {(int)ctx.Data[0]}");
            healthBar.value = (int)ctx.Data[0];
        }
    } 
}
