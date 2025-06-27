using System;
using UnityEngine;
using DG.Tweening;

public class WheelSpinner : MonoBehaviour
{
    [SerializeField] private int sliceCount = 8;
    [SerializeField] private float markerLandingAngle = -245f;
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private int fullSpins = 4;
    private RectTransform rectTransform;
    private float currentZ;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        AssertUtil.IsNotNull(rectTransform);
    }

    public void Spin(int selectedIndex, bool isWinningSpin, Action onComplete = null)
    {
        float sliceAngle = 360f / sliceCount;

        // Current rot
        currentZ = rectTransform.localEulerAngles.z;
        if (currentZ > 180f) currentZ -= 360f;

        float targetStopZ;

        if (isWinningSpin)
        {
            // land at marker
            targetStopZ = markerLandingAngle + (selectedIndex * sliceAngle);
        }
        else
        {
            int fakeIndex = (selectedIndex + UnityEngine.Random.Range(1, sliceCount)) % sliceCount;
            targetStopZ = markerLandingAngle + (fakeIndex * sliceAngle);
        }

        // Calculations
        float totalSpinDegrees = fullSpins * 360f;
        float finalZ = currentZ - totalSpinDegrees;
        float offsetToTarget = Mathf.DeltaAngle(finalZ, targetStopZ);
        finalZ += offsetToTarget;

        rectTransform
            .DORotate(new Vector3(0, 0, finalZ), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                if (isWinningSpin)
                {
                    rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 6, 0.8f);
                }
                onComplete?.Invoke();
            });
    }
}
