using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionRadiusScript : MonoBehaviour
{
    public Transform PlayerTransform;
    public ICharacter PlayerScript;
    public float TranslateSpeed;
    public float RangeOfError;

    void Start()
    {
        PlayerTransform = transform.parent;
        PlayerScript = PlayerTransform.gameObject.GetComponent<ICharacter>();
        TranslateSpeed = 3f;
        RangeOfError = 0.4f;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "XP Item")
        {
            Vector3 directionToPlayer = PlayerTransform.position - other.transform.position;
            other.transform.Translate(directionToPlayer.normalized * TranslateSpeed * Time.deltaTime);
            float XPValue = other.gameObject.GetComponent<IXP>().XP_Value;
            if (ApproximatelyEqual(PlayerTransform.position, other.transform.position, RangeOfError))
            {
                PlayerScript.XPGain(XPValue);
                Destroy(other.gameObject);
                Debug.Log("Object is on dead center of player");
            }
        }
        if (other.gameObject.tag == "Health")
        {
            Vector3 directionToPlayer = PlayerTransform.position - other.transform.position;
            other.transform.Translate(directionToPlayer.normalized * TranslateSpeed * Time.deltaTime);
        }
        if(other.gameObject.tag == "Credit Item")
        {
            Vector3 directionToPlayer = PlayerTransform.position - other.transform.position;
            other.transform.Translate(directionToPlayer.normalized * TranslateSpeed * Time.deltaTime);
            int CreditValue = other.gameObject.GetComponent<ICredit>().Credit_Value;
            if (ApproximatelyEqual(PlayerTransform.position, other.transform.position, RangeOfError))
            {
                SaveManager.currentCredits += CreditValue;
                Destroy(other.gameObject);
                Debug.Log("Object is on dead center of player");
            }
        }
    }

    bool ApproximatelyEqual(Vector3 a, Vector3 b, float tolerance)
    {
        float squaredTolerance = tolerance * tolerance;
        float squaredDistance = (a - b).sqrMagnitude;
        return squaredDistance <= squaredTolerance;
    }
}
