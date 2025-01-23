using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light _Light;
    public float _MinTime;
    public float _MaxTime;
    public float _Timer;

    // Start is called before the first frame update
    void Start()
    {
        _Timer = Random.Range(_MinTime, _MaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        FlickerLight();
    }
    public void FlickerLight()
    {
        if (_Timer > 0)
        {
            _Timer -= Time.deltaTime;
        }
        if (_Timer <= 0)
        {
            _Light.enabled = !_Light.enabled;
            _Timer = Random.Range(_MinTime, _MaxTime);
        }
    }
}
