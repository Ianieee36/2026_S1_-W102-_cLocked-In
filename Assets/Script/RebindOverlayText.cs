using UnityEngine;
using UnityEngine.UI;
public class RebindOverlayText : MonoBehaviour
{
    public Text overlayText;

    public void SetCustomText()
    {
        overlayText.text = "Press a new key\nESC to cancel";
    }
}
