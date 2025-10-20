using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [Header("Display Settings")] public Color textColor = Color.green;
    [Range(10, 80)] public int fontSize = 26;
    public Vector2 offset = new Vector2(10, 10);

    [Header("Statistics")] public float refreshInterval = 0.5f;
    private float deltaTime;
    private float timer;
    private int frameCount;

    private float fpsCurrent;
    private float fpsAverage;
    private float fpsMin = float.MaxValue;
    private float fpsMax = float.MinValue;

    // 👇 tạo style một lần để fontSize ăn chắc
    private GUIStyle style;

    private void Awake()
    {
        style = new GUIStyle
        {
            fontSize = fontSize,
            normal = { textColor = textColor },
            alignment = TextAnchor.UpperLeft
        };
    }

    private void Update()
    {
        float dt = Time.unscaledDeltaTime;
        deltaTime += dt;
        frameCount++;
        timer += dt;

        if (timer >= refreshInterval)
        {
            fpsCurrent = 1f / (deltaTime / frameCount);
            fpsAverage = (fpsAverage + fpsCurrent) * 0.5f;
            fpsMin = Mathf.Min(fpsMin, fpsCurrent);
            fpsMax = Mathf.Max(fpsMax, fpsCurrent);

            timer = 0f;
            frameCount = 0;
            deltaTime = 0f;
        }

        // cập nhật dynamic màu theo FPS
        if (fpsCurrent >= 60) style.normal.textColor = Color.green;
        else if (fpsCurrent >= 30) style.normal.textColor = Color.yellow;
        else style.normal.textColor = Color.red;
    }

    private void OnGUI()
    {
        // ⚙️ scale GUI theo màn hình (bắt buộc cho cỡ chữ ăn đúng)
        float scale = Screen.height / 1080f; // ví dụ scale theo 1080p
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1));

        Rect rect = new Rect(offset.x, offset.y, 350, 100);

        GUI.BeginGroup(rect);
        GUI.Label(new Rect(0, 0, rect.width, 25), $"FPS: {fpsCurrent:0.0}", style);
        GUI.Label(new Rect(0, 25, rect.width, 25), $"Avg: {fpsAverage:0.0}", style);
        GUI.Label(new Rect(0, 50, rect.width, 25), $"Min: {fpsMin:0.0}   Max: {fpsMax:0.0}", style);
        GUI.EndGroup();
    }

    private void OnValidate()
    {
        if (style != null) style.fontSize = fontSize;
    }

    private void OnEnable()
    {
        fpsMin = float.MaxValue;
        fpsMax = float.MinValue;
        fpsAverage = 0;
    }
}