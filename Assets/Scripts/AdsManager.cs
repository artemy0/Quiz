using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    private void Awake()
    {
        if (!RuntimeManager.IsInitialized())
            RuntimeManager.Init();
    }

    public void ShowBannerAds()
    {
        Advertising.ShowBannerAd(BannerAdPosition.Bottom);
    }

    public void HideBannerAd()
    {
        Advertising.HideBannerAd();
    }

    public void ShowInterstitalAds()
    {
        if (Advertising.IsInterstitialAdReady())
            Advertising.ShowInterstitialAd();
    }

    public void ShowRewardedAds()
    {
        if (Advertising.IsRewardedAdReady())
            Advertising.ShowRewardedAd();
    }

}
