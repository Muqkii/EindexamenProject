using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class MinimapOutput : MonoBehaviour
{
    [Header("Output")]
    [Tooltip("RawImage on your Canvas that will show the minimap.")]
    public RawImage targetRawImage;

    [Tooltip("Optional. If not assigned, one will be created at runtime.")]
    public RenderTexture renderTexture;

    [Header("RenderTexture Settings (if created at runtime)")]
    public int textureWidth = 256;
    public int textureHeight = 256;
    public int depthBuffer = 16;

    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();

        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(textureWidth, textureHeight, depthBuffer, RenderTextureFormat.ARGB32);
            renderTexture.name = "Minimap_RT";
            renderTexture.Create();
        }

        _cam.targetTexture = renderTexture;

        if (targetRawImage != null)
        {
            targetRawImage.texture = renderTexture;
        }
        else
        {
            Debug.LogWarning("[MinimapOutput] No RawImage assigned. The RenderTexture is still created and attached to the camera.");
        }
    }

    void OnDestroy()
    {
        if (_cam != null) _cam.targetTexture = null;
        if (renderTexture != null)
        {
            if (renderTexture.IsCreated()) renderTexture.Release();
            Destroy(renderTexture);
        }
    }
}
