using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private Cinemachine.CinemachineVirtualCamera cinemachineCamera;

    [SerializeField]
    [NotNull]
    private Camera uiCamera;

    [SerializeField]
    [NotNull(IgnorePrefab = true)]
    private Transform followedObject;

    [SerializeField]
    [Min(0f)]
    [NotNull(IgnorePrefab = true)]
    private GameInputManager gameInput;

    [SerializeField]
    private float zoomScaleFactor = 5f;

    [SerializeField]
    private float minOrthoSize = 1f;

    [SerializeField]
    private float maxOrthoSize = 10f;

    [SerializeField]
    private float initialOrthoSize = 3f;

    private void Awake()
    {
        cinemachineCamera.Follow = followedObject;
    }

    private void Start()
    {
        cinemachineCamera.m_Lens.OrthographicSize = initialOrthoSize;
        uiCamera.orthographicSize = initialOrthoSize;
    }

    private void Update()
    {
        var zoomDelta = gameInput.GetZoom();
        if (zoomDelta != 0f)
        {
            var delta = zoomDelta * Time.deltaTime * -zoomScaleFactor;
            cinemachineCamera.m_Lens.OrthographicSize += delta;
            uiCamera.orthographicSize += delta;
        }

        // Prevent ortho size from becoming too small or large.
        var orthoSizeClamped = Mathf.Clamp(cinemachineCamera.m_Lens.OrthographicSize, minOrthoSize, maxOrthoSize);
        cinemachineCamera.m_Lens.OrthographicSize = orthoSizeClamped;
        uiCamera.orthographicSize = orthoSizeClamped;
    }
}
