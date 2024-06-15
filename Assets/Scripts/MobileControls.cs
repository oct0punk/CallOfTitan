using UnityEngine;

public class MobileControls : MonoBehaviour
{
    public Vector2 dir {  get; private set; }
    public int minRange = 60;
    public int maxRange = 100;
    Vector2 start;

    private void Awake()
    {
        enabled = Application.isMobilePlatform;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.touches[0];
            switch (t.phase) {
                case TouchPhase.Began:
                    start = t.position;
                    dir = Vector2.zero;
                    break;
                case TouchPhase.Moved:
                    if (Vector2.Distance(t.position, start) < minRange) break;
                    dir = t.position - start;
                    dir.Normalize();
                    if (Vector2.Distance(t.position, start) > maxRange)
                    {
                        Vector2 fromStartToPos = t.position - start;
                        start = t.position - fromStartToPos.normalized * maxRange;
                    }
                    break;
                case TouchPhase.Ended:
                    dir = Vector2.zero;
                    break;
            }
        }
    }
}
