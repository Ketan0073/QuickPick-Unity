using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Tile : MonoBehaviour, IPointerClickHandler

{
    public Image tileImage;
    
    void Start()
    {
        // Auto-assign if not set
        if (tileImage == null)
            tileImage = GetComponent<Image>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
{
    tileImage.color = Color.green;
    FindAnyObjectByType<GameManager>().OnTileClicked();  // Calls GameManager
    Debug.Log("Tile clicked!");
}

}

