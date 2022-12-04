using UnityEngine;

public class RestorePurchases : MonoBehaviour
{

    private void Awake()
    {
#if UNITY_IOS
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }

    void Start()
    {
       
        
    }

    public void ClickRestorePurchaseButton()
    {
        IAPManager.instance.RestorePurchases();
    }
}
