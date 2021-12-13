using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System.Collections;
using System;
using UnityEngine.Events;


public class RewardedAdManager : MonoBehaviour
{

    private int maxRew = 3;  
    [HideInInspector] public AdRequest[] requestRewarded;
    bool show_ad_as_index;
    private bool showAds;
    UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        StartCoroutine(TryToFetch());
        uiManager = FindObjectOfType<UIManager>();
    }
    IEnumerator TryToFetch()
    {
        yield return new WaitForSeconds(3);
        yield return new WaitUntil(() => GoogleSheetHandler.googlesheetInitilized);
        RequestRewarded();
    }
    public static void Initialize(Action<InitializationStatus> initCompleteAction) { }
    #region REWARDED
    private RewardedAd rewardedAd1;
    private RewardedAd rewardedAd2;
    private RewardedAd rewardedAd3;

    public string rewardedAd_ID1;
    public string rewardedAd_ID2;
    public string rewardedAd_ID3;
    public void RequestRewarded()
    {
        rewardedAd_ID1 = GoogleSheetHandler.g_rewarded1;
        rewardedAd1 = RequestRewardedAd(rewardedAd_ID1);
        rewardedAd_ID2 = GoogleSheetHandler.g_rewarded2;
        rewardedAd2 = RequestRewardedAd(rewardedAd_ID2);
        rewardedAd_ID3 = GoogleSheetHandler.g_rewarded3;
        rewardedAd3 = RequestRewardedAd(rewardedAd_ID3);
        show_ad_as_index = GoogleSheetHandler.show_ad_as_index;
        showAds = GoogleSheetHandler.showAds;
    }
    public RewardedAd RequestRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
        rewardedAd.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
        rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        return rewardedAd;
    }
    RewardedAd currentRewardedAd;
    public RewardedAd CurrentRewardedAd()
    {
        
        if (show_ad_as_index == false)
        {           
            if (GoogleSheetHandler.show_g_rewarded1 == true)
            {
                currentRewardedAd = rewardedAd1;
               
            }
            else if (GoogleSheetHandler.show_g_rewarded2 == true)
            {
                currentRewardedAd = rewardedAd2;
            }
            else
                 if (GoogleSheetHandler.show_g_rewarded3 == true)
            {
                currentRewardedAd = rewardedAd3;
            }
        }
        else
        {
            if (rewIndex == 0)
            {
                currentRewardedAd = rewardedAd1;

            }
            else if (rewIndex == 1)
            {
                currentRewardedAd = rewardedAd2;
            }
            else
                 if (rewIndex == 2)
            {
                currentRewardedAd = rewardedAd2;
            }
            rewIndex++;
            if (rewIndex >= maxRew)
            {
                rewIndex = 0;
            }
        }
        return currentRewardedAd;        
        
    }
    string CurrentID;
    public string CurrentRewardedAd_ID()
    {
        if (GoogleSheetHandler.show_g_rewarded1 == true)
        {
            CurrentID = GoogleSheetHandler.g_rewarded1;           
        }
        else if (GoogleSheetHandler.show_g_rewarded2 == true)
        {
            CurrentID = GoogleSheetHandler.g_rewarded2;            
        }
        else
             if (GoogleSheetHandler.show_g_rewarded3 == true)
        {
            CurrentID = GoogleSheetHandler.g_rewarded3;
            
        }
        return CurrentID;
    }
    int rewIndex;
    IEnumerator WaitAplayRewardedAd()
    {       
        while (!CurrentRewardedAd().IsLoaded())
        {
            yield return null;
        }
        CurrentRewardedAd().Show();       
    }
   
    public void ShowRewardedAd()
    {
        if (!showAds)
            return;
        StartCoroutine(WaitAplayRewardedAd());
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        //MonoBehaviour.print(
        //    "HandleRewardedAdFailedToLoad event received with message: "
        //                     + args.Message);
        RequestRewardedAd(CurrentRewardedAd_ID());
    }
    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        RequestRewardedAd(CurrentRewardedAd_ID());
        uiManager.RewardTheUser();

    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        RequestRewardedAd(CurrentRewardedAd_ID());
        uiManager.WarnAdClosed();
    }
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //  Debug.Log("Rewarded Video ad loaded successfully");

    }
    #endregion
}