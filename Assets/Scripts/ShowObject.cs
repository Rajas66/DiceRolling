using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObject : MonoBehaviour
{
    [SerializeField]
    AnimationCurve scaleCurve;

    [SerializeField]
    float sizeMultiplier = 2f;

    bool scaleSpawn = false;

    float lerpFactor = 0;
    public float lerpSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (scaleSpawn)
        {
            lerpFactor += Time.deltaTime * lerpSpeed;
            lerpFactor = Mathf.Clamp(lerpFactor, 0, 1);

            this.transform.localScale = Vector3.one * scaleCurve.Evaluate(lerpFactor) * sizeMultiplier;

            if(lerpFactor >= 1)
                scaleSpawn = false;
        }
    }
    private void OnEnable()
    {
        lerpFactor = 0;
        scaleSpawn = true;
    }
}
