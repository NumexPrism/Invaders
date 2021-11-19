using UnityEngine;

namespace UI
{
  public class RotatingImage : MonoBehaviour
  {
    private const float RpmToDegPerSecond = 360.0f / 60.0f;
    public float RPM = 30;
    public bool Clockwise = true;

    private void Update()
    {
      var rotation = transform.rotation.eulerAngles;
      rotation.z += RPM * Time.unscaledDeltaTime * RpmToDegPerSecond * (Clockwise?-1:1);
      transform.rotation = Quaternion.Euler(rotation);
    }

  }
}
