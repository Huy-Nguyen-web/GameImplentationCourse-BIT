using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;


    public void UpdateHealthUI(Context ctx)
    {
        if (ctx != null)
        {
            healthBar.value = (int)ctx.Data[0];
        }
    } 
}
