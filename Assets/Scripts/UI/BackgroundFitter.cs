using UnityEngine;

[ExecuteAlways]
public class BackgroundFitter : MonoBehaviour
{
    public Camera targetCamera;
    public float distanceFromCamera;
    public bool matchHeight = true;
    public bool keepAspectRatio;

    [Header("Original Plane Size")]
    public Vector2 originalPlaneSize = new Vector2(10f, 10f);

    private void LateUpdate()
    {
        Fit();
    }


    private void Fit()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null) return;

        transform.position = cam.transform.position + cam.transform.forward * distanceFromCamera;
        transform.rotation = cam.transform.rotation * Quaternion.Euler(90f, -180f, 0f);

        float width, height;


        float frustumHeight = 2.0f * distanceFromCamera * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        height = frustumHeight;
        width = frustumHeight * cam.aspect;

        Vector3 scale = transform.localScale;

        if (keepAspectRatio)
        {
            // keep original aspect
            float planeAspect = originalPlaneSize.x / originalPlaneSize.y;

            if (matchHeight)
            {
                scale.y = height / originalPlaneSize.y;
                scale.x = scale.y * planeAspect;
            }
            else
            {
                scale.x = width / originalPlaneSize.x;
                scale.y = scale.x / planeAspect;
            }
        }
        else
        {
            scale.x = width / originalPlaneSize.x;
            scale.y = height / originalPlaneSize.y;
        }

        scale.z = 1f;
        transform.localScale = scale;
    }
}
