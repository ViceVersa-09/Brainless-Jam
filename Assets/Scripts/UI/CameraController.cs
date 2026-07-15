using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] public GameObject target;
    [SerializeField] float smoothing;
    [SerializeField] public Vector3 offset;

    [Header("Tracking")]
    [SerializeField] bool trackX;
    [SerializeField] bool trackY;
    [SerializeField] Vector2 minBounds;
    [SerializeField] Vector2 maxBounds;

    [Header("Testing")]
    [SerializeField] public float testShakeDuration;
    [SerializeField] public float testShakeMagnitude;

    float timeElapsed;
    bool shakeActive;

    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (!shakeActive && target != null)
        {
            MoveCamera();
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x), Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y), -10);
    }

    void MoveCamera()
    {
        transform.position = Vector3.SmoothDamp(transform.position, GetTargetPosition(), ref velocity, smoothing);
    }

    public void PlayCameraShakeRoutine(float shakeDuration, float shakeMagnitude)
    {
        StartCoroutine(ShakeCameraRoutine(shakeDuration, shakeMagnitude));
    }

    Vector3 GetTargetPosition()
    {
        if (trackX && trackY) // Track both axis
        {
            if (target != null)
            {
                targetPosition = new Vector3
                (
                Mathf.Clamp(target.transform.position.x + transform.localScale.x * offset.x, minBounds.x, maxBounds.x),
                target.transform.position.y + offset.y,
                transform.position.z + offset.z
                );
            }
        }
        else if (trackX && !trackY) // Track X But not Y Axis
        {
            if (target != null)
            {
                targetPosition = new Vector3
                (
                Mathf.Clamp(target.transform.position.x + transform.localScale.x * offset.x, minBounds.x, maxBounds.x),
                0,
                transform.position.z + offset.z
                );
            }
        }
        else if (!trackX && trackY) // Track Y But not X Axis
        {
            if (target != null)
            {
                targetPosition = new Vector3
                (
                0,
                target.transform.position.y,
                transform.position.z
                ) + new Vector3(offset.x * target.transform.localScale.x, offset.y, offset.z);
            }
        }
        return targetPosition;
    }

    IEnumerator ShakeCameraRoutine(float shakeDuration, float shakeMagnitude)
    {
        shakeActive = true;
        timeElapsed = 0;
        while (timeElapsed < shakeDuration)
        {
            Vector3 initialPosition = transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, GetTargetPosition(), ref velocity, smoothing) + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            transform.position = initialPosition;
            transform.position = Vector3.SmoothDamp(transform.position, GetTargetPosition(), ref velocity, smoothing);
        }
        shakeActive = false;
    }

    public IEnumerator Transition(int level)
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Out");
        AudioManager.instance.PlaySFX(AudioManager.instance.swooshClip);

        yield return new WaitForSeconds(1);

        if (SceneManager.sceneCountInBuildSettings >= level)
        {
            SceneManager.LoadScene(level);
        }
    }
}