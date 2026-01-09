using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum RuleType { Color, Pattern, Size, Dots }

public class Tile : MonoBehaviour, IPointerClickHandler

{
    [Header("Visual Components")]
    public Image tileImage;
    public Image[] dotImages = new Image[4];

    [Header("Tile Slate")]
    public bool isOddTile = false;  // NEW: Track odd status
    public int dotCount = 2;

    private float fadeTimer = 0f;
    private bool fading = false;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    void Start()
    {
        if (tileImage == null) tileImage = GetComponent<Image>();
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
        ResetTile();
    }

    public void ResetTile()
    {
        fading = false;
        fadeTimer = 0f;
        transform.localScale = originalScale;
        transform.rotation = originalRotation;
        gameObject.SetActive(true);
        SetupDots();
    }
    
    public void SetupDots()
    {
        for (int i = 0; i < dotImages.Length; i++)
        {
            if (dotImages[i] != null)
                dotImages[i].gameObject.SetActive(i < dotCount);
        }
    }
    
    void Update()
    {
        if (fading)
        {
            fadeTimer += Time.deltaTime;
            if (fadeTimer >= 5f)  // YOUR 5s timeout
            {
                gameObject.SetActive(false);
            }
            else if (tileImage != null)
            {
                float alpha = Mathf.Lerp(1f, 0f, fadeTimer / 5f);
                Color color = tileImage.color;
                tileImage.color = new Color(color.r, color.g, color.b, alpha);
            }
        }
    }
    
    public void StartFade()
    {
        fading = true;
        fadeTimer = 0f;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager gm = FindAnyObjectByType<GameManager>();
        if (gm != null)
        {
            gm.OnTileClicked(this);  // Pass THIS tile
        }
    }
    
    /*public void SetOddTile(bool isOdd)
    {
        isOddTile = isOdd;
        tileImage.color = isOdd ? Color.red : Color.blue;  // Visual debug
    }*/
}

