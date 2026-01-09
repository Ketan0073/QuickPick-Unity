using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Tile : MonoBehaviour, IPointerClickHandler

{
    public Image tileImage;
    public bool isOddTile = false;  // NEW: Track odd status
    
    void Start()
    {
        if (tileImage == null)
            tileImage = GetComponent<Image>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager gm = FindAnyObjectByType<GameManager>();
        if (gm != null)
        {
            gm.OnTileClicked(this);  // Pass THIS tile
        }
    }
    
    public void SetOddTile(bool isOdd)
    {
        isOddTile = isOdd;
        tileImage.color = isOdd ? Color.red : Color.blue;  // Visual debug
    }
}

