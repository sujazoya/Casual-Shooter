using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System.Collections;
using System;
using UnityEngine.Events;


public class RewardedAdManager : MonoBehaviour
{
    
    [HideInInspector] public AdRequest[] requestRewarded;
    bool show_ad_as_index;
    private bool showAds;    
    bool show_rewarded;
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        StartCoroutine(TryToFetch());       
    }
    IEnumerator TryToFetch()
    {
        yield return new WaitForSeconds(3);
        yield return new WaitUntil(() => GoogleSheetHandler.googlesheetInitilized);
        show_rewarded = GoogleSheetHandler.show_rewarded;
        show_ad_as_index = GoogleSheetHandler.show_ad_as_index;
        showAds = GoogleSheetHandler.showAds;

        if (show_rewarded == true)
        {
            RequestRewarded();
        }
        
    }
    public static void Initialize(Action<InitializationStatus> initCompleteAction) { }
    #region REWARDED
    private RewardedAd Rewarded_1;
    private RewardedAd Rewarded_2;
    private RewardedAd Rewarded_3;
    private RewardedAd Rewarded_4;
    private RewardedAd Rewarded_5;
    private RewardedAd Rewarded_6;
    private RewardedAd Rewarded_7;
    private RewardedAd Rewarded_8;
    private RewardedAd Rewarded_9;
    private RewardedAd Rewarded_10;



    string rewardedAd_ID1;
    string rewardedAd_ID2;
    string rewardedAd_ID3;

    int totallRewarded = 10;   
    public void RequestRewarded()
    {
        rewardedAd_ID1 = GoogleSheetHandler.g_rewarded1;
        rewardedAd_ID2 = GoogleSheetHandler.g_rewarded2;
        rewardedAd_ID3 = GoogleSheetHandler.g_rewarded3;

        if (show_ad_as_index == true)
        {
            Rewarded_1 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_2= RequestRewardedAd(rewardedAd_ID2);
            Rewarded_3= RequestRewardedAd(rewardedAd_ID3);
            Rewarded_4= RequestRewardedAd(rewardedAd_ID1);
            Rewarded_5= RequestRewardedAd(rewardedAd_ID2);
            Rewarded_6= RequestRewardedAd(rewardedAd_ID3);
            Rewarded_7= RequestRewardedAd(rewardedAd_ID1);
            Rewarded_8= RequestRewardedAd(rewardedAd_ID2);
            Rewarded_9 = RequestRewardedAd(rewardedAd_ID3);
            Rewarded_10 = RequestRewardedAd(rewardedAd_ID1); 
        }
        else
        {
            Rewarded_1 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_2 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_3 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_4 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_5 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_6 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_7 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_8 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_9 = RequestRewardedAd(rewardedAd_ID1);
            Rewarded_10 = RequestRewardedAd(rewardedAd_ID1);
        }      

       
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
    public bool IsReadyToShowAd()
    {
        if (CurrentRewardedAd().IsLoaded())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    RewardedAd currentRewardedAd;
    /*
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
    */

    public void ShowRewardedAd()
    {
       
        if (!showAds)
            return;
        StartCoroutine(WaitAplayRewardedAd());
       
    }
    int gIndex;
    RewardedAd cr;
    public  RewardedAd CurrentRewardedAd()
    {       
        if (gIndex == 0)
        {
            cr = Rewarded_1;
        }
        else
         if (gIndex == 1)
        {
            cr = Rewarded_2;
        }
        else
             if (gIndex == 2)
        {
            cr = Rewarded_3;
        }
        else
             if (gIndex == 3)
        {
            cr = Rewarded_4;
        }
        else
             if (gIndex == 4)
        {
            cr = Rewarded_5;
        }
        else
             if (gIndex == 5)
        {
            cr = Rewarded_6;
        }
        else
             if (gIndex == 6)
        {
            cr = Rewarded_7;
        }
        else
             if (gIndex == 7)
        {
            cr = Rewarded_8;
        }
        else
             if (gIndex == 8)
        {
            cr = Rewarded_9;
        }
        else
             if (gIndex == 9)
        {
            cr = Rewarded_10;
        }
        return cr;
    }
    void Gindex()
    {
        gIndex++;
        if (gIndex >= totallRewarded)
        {
            gIndex = 0;
        }
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
    IEnumerator WaitAplayRewardedAd()
    {       
       
        CurrentRewardedAd().Show();
        RewardedAd currentGameoverRewardedAd = CurrentRewardedAd();
        Gindex();
        while (!currentGameoverRewardedAd.IsLoaded())
        {
            yield return null;
        }        
        currentGameoverRewardedAd.Show();        
    }
   
    //public void ShowRewardedAd()
    //{
       
    //}

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        //MonoBehaviour.print(
        //    "HandleRewardedAdFailedToLoad event received with message: "
        //                     + args.Message);
        //RequestRewardedAd(CurrentRewardedAd_ID());
    }
    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        //RequestRewardedAd(CurrentRewardedAd_ID());
        //uiManager.RewardTheUser();

    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        //RequestRewardedAd(CurrentRewardedAd_ID());
        //uiManager.WarnAdClosed();
    }
   
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //  Debug.Log("Rewarded Video ad loaded successfully");

    }
    #endregion
}