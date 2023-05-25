using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PopUp : MonoBehaviour
{
    float textSpeed = 5;
    TextMeshPro textMesh;
    Color textColor;
    public float disapearTimer;
    float disapearMax;
    public float fadeRate;

    static int sortingOrder = 0;

    Vector3 moveVector;
    public void setup(float damage, Vector3 originOfDamage)
    {
        textMesh.SetText(damage.ToString());
        moveVector = addSpread(originOfDamage) * textSpeed * -1;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

    }



    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        textColor = textMesh.color;
        transform.Rotate(90, 0, 0);
        disapearMax = disapearTimer;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveVector*Time.deltaTime;
        moveVector -= moveVector * 4 * Time.deltaTime;
        disapearTimer -= Time.deltaTime;
        if(disapearTimer > disapearMax * 0.5f)
        {//first half of lifetime
            transform.localScale += Vector3.one * Time.deltaTime;
        }
        else
        {//second half of lifetime
            transform.localScale -= Vector3.one * Time.deltaTime;
        }

        if (disapearTimer < 0)
        {//start disappearing
            textColor.a -= fadeRate *Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a< 0)
            {
                Destroy(gameObject);
            }
        }
    }


    Vector3 addSpread(Vector3 originalAngle)
    {
        float spreadAmount = 0.5f;
        originalAngle += new Vector3(
                Random.Range(-spreadAmount, spreadAmount),
                0,
                Random.Range(-spreadAmount, spreadAmount)
                );
        originalAngle.Normalize();

        return originalAngle;
    }
}