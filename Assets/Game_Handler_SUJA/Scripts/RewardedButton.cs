using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;
[RequireComponent(typeof(Button))]
public class RewardedButton : MonoBehaviour
{
    Button button;
    [SerializeField]  RewardedAdManager rewardedAdManager;	
	public UnityEvent onRewarded;
	public UnityEvent onClose;
	[Space]
	public UnityEvent OnInHouseAdComplete;
	public UnityEvent OnInHouseAdClosed;
	private InHouse_Ad_Handler inHouse_Ad;

	private bool show_rewarded;
	private bool show_inhouse_ad;
	int show_rewarded_onrequest_count;

	private void OnEnable()
	{
		button = GetComponent<Button>();		
		button.onClick.AddListener(OnClick);
		button.interactable = true;
		StartCoroutine("AddEvent", 3);		
		inHouse_Ad = FindObjectOfType<InHouse_Ad_Handler>();
		InHouseAdManager.onAdCompleted += OnAdCompleted;
		InHouseAdManager.onAdClosed += OnAdClosed;
	}     
    
	void ShowInHouseAd()
	{
		inHouse_Ad.ShowInHouseAd();
	}
	void OnAdCompleted()
	{
			OnInHouseAdComplete.Invoke();
	}
	void OnAdClosed()
	{
			OnInHouseAdClosed.Invoke();
	}
	//Timer.Schedule(this, 5f, AddEvents);
	//public void CallAddEvent()
	//   {
	//	StartCoroutine(AddEvent());
	//}
    public IEnumerator AddEvent()
	{		
		//button.interactable = false;
		yield return new WaitUntil(() => rewardedAdManager.IsReadyToShowAd()&&gameObject.activeSelf);		
		button.interactable = true;

	}
	RewardedAd rewardedAd()
    {
		return rewardedAdManager.CurrentRewardedAd();     

	}
	private void AddEvents()
	{
		if (rewardedAdManager.IsReadyToShowAd())
		{
			rewardedAd().OnUserEarnedReward += HandleRewardBasedVideoRewarded;
			rewardedAd().OnAdClosed += HandleRewardedAdClosed;

		}
	}

	public void OnClick()
	{
		if(rewardedAdManager.IsReadyToShowAd())
		AddEvents();
		rewardedAd().Show();
		
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		onRewarded.Invoke();
		//rewardedAdManager.RequestRewardedAd_Shop(rewardedAdManager.CurrentRewardedAd_ID());

	}
	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		onClose.Invoke();
		//rewardedAdManager.RequestRewardedAd_Shop(rewardedAdManager.CurrentRewardedAd_ID());
	}
	private void OnDisable()
	{
		if (rewardedAdManager.IsReadyToShowAd())
		{
			rewardedAd().OnUserEarnedReward -= HandleRewardBasedVideoRewarded;
			rewardedAd().OnAdClosed -= HandleRewardedAdClosed;
		}
	}
}


