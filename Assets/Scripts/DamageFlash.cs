using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{

    [SerializeField]private Material flashMaterial;
    [SerializeField]private Material defaultMaterial;
    [SerializeField]private float flashDuration;

    [SerializeField]private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

    public void Flash()
    {
        if(spriteRenderer != null)
        {
            if(flashCoroutine  != null)
            {
                StopCoroutine(Flash_Coroutine());
            }

            flashCoroutine = StartCoroutine(Flash_Coroutine());
        }
    }
    public IEnumerator Flash_Coroutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);  
        spriteRenderer.material = defaultMaterial;
    }
    
}
