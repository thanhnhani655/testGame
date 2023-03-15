using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShakeObject : MonoBehaviour
{
    public event System.Action ShakeDone = delegate { };
    [SerializeField]
    private GameObject shakeObject;

    [SerializeField]
    private Vector3 staringPos;
    [SerializeField]
    private float shake_intensity = 0.1f;
    [SerializeField]
    private float timeShake = 0.5f;
    [SerializeField]
    private float timeCount = 0;

    private void Awake()
    {
        staringPos = shakeObject.transform.localPosition;
    }

    [Button]
    public void Shake()
    {
        StartCoroutine(Shaking());
    }

    
    private IEnumerator Shaking()
    {
        while(true)
        {
            shakeObject.transform.localPosition = staringPos + Random.insideUnitSphere * shake_intensity;
            timeCount += Time.deltaTime;
            if (timeCount > timeShake)
            {
                timeCount = 0;
                shakeObject.transform.localPosition = staringPos;
                ShakeDone();
                break;
            }
            yield return null;
        }
    }

  
}
