using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TutorialOverlay : MonoBehaviour
{
    public RectTransform holeTarget; // UI element to highlight
    private Material runtimeMat;
    private Image img;
    private Canvas rootCanvas;

    void Awake()
    {
        img = GetComponent<Image>();
        runtimeMat = new Material(img.material);
        img.material = runtimeMat;

        rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
    }

    void Update()
    {
        if (holeTarget == null) return;

        // Get world corners of the target
        Vector3[] corners = new Vector3[4];
        holeTarget.GetWorldCorners(corners);

        // Convert to viewport space (0–1 relative to screen)
        Vector3 bottomLeft = RectTransformUtility.WorldToScreenPoint(rootCanvas.worldCamera, corners[0]);
        Vector3 topRight = RectTransformUtility.WorldToScreenPoint(rootCanvas.worldCamera, corners[2]);

        float xMin = bottomLeft.x / Screen.width;
        float yMin = bottomLeft.y / Screen.height;
        float xMax = topRight.x / Screen.width;
        float yMax = topRight.y / Screen.height;

        Vector2 center = new Vector2((xMin + xMax) * 0.5f, (yMin + yMax) * 0.5f);
        Vector2 size = new Vector2(xMax - xMin, yMax - yMin);

        runtimeMat.SetVector("_HoleCenter", center);
        runtimeMat.SetVector("_HoleSize", size);
    }
}
