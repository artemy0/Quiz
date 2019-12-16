﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    void Awake()
    {
        if (!RuntimeManager.IsInitialized())
            RuntimeManager.Init();
    }

    //show banner ad
    public void ShowBannerAds()
    {
        Advertising.ShowBannerAd(BannerAdPosition.Bottom);
    }

    public void HideBannerAd()
    {
        Advertising.HideBannerAd();
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