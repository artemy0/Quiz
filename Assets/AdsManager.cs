using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    public Text interAdText;
    public Text rewardedAdText;

    void Awake()
    {
        if (!RuntimeManager.IsInitialized())
            RuntimeManager.Init();
    }

    private void Update()
    {
        //check if the Interstital Ad is ready
        if (Advertising.IsInterstitialAdReady())
        {
            interAdText.text = "Interstital Ad is Ready!";
            interAdText.color = Color.green;
        }
        else
        {
            interAdText.text = "Interstital Ad is Not Ready!";
            interAdText.color = Color.red;
        }

        //check if the Rewarded Ad is ready
        if (Advertising.IsRewardedAdReady())
        {
            rewardedAdText.text = "Rewarded Ads is Ready!";
            rewardedAdText.color = Color.green;
        }
        else
        {
            rewardedAdText.text = "Rewarded Ads is Not Ready!";
            rewardedAdText.color = Color.red;
        }
        
    }

    //show banner ad
    public void ShowBannerAds()
    {
        Advertising.ShowBannerAd(BannerAdPosition.Bottom);
    }

    //show interstital ad
    public void ShowInterstitalAds()
    {
        if (Advertising.IsInterstitialAdReady())
            Advertising.ShowInterstitialAd();
    }

    //show rewarded ad
    public void ShowRewardedAds()
    {
        if (Advertising.IsRewardedAdReady())
            Advertising.ShowRewardedAd();
    }

}
