using System.Collections;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{

    public IEnumerator Appear(GameObject icon, float duration)
    {
        float time = 0f;
        icon.transform.localScale = Vector3.zero;


        while (time < duration)


        {
            float t = time / duration;
            icon.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            time += Time.deltaTime;
            yield return null;
        }


        icon.transform.localScale = Vector3.one;
    }




    public IEnumerator Disappear(GameObject icon, float duration)
    {

        float time = 0f;
        Vector3 startScale = icon.transform.localScale;



        while (time < duration)
        {

            float t = time / duration;
            icon.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            time += Time.deltaTime;
            yield return null;
        }


        Destroy(icon);
    }
}
