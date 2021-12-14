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

	private const string ACTION_NAME = "rewarded_video";
	public UnityEvent onRewarded;
	public UnityEvent onClose;
	
	private void Start()
	{
		button = GetComponent<Button>();		
		button.onClick.AddListener(OnClick);
		button.interactable = true;
		Invoke("CallAddEvent", 3);
	}
	//Timer.Schedule(this, 5f, AddEvents);
	public void CallAddEvent()
    {
		StartCoroutine(AddEvent());
	}
	public IEnumerator AddEvent()
	{
		
		//button.interactable = false;
		yield return new WaitUntil(() => rewardedAdManager.IsReadyToShowAd());
		AddEvents();
		button.interactable = true;

	}

	private void AddEvents()
	{
		if (rewardedAdManager.IsReadyToShowAd())
		{
			rewardedAdManager.CurrentRewardedAd().OnUserEarnedReward += HandleRewardBasedVideoRewarded;
			rewardedAdManager.CurrentRewardedAd().OnAdClosed += HandleRewardedAdClosed;

		}
	}

	public void OnClick()
	{
		if(rewardedAdManager.IsReadyToShowAd())
		rewardedAdManager.ShowRewardedAd();
		AddEvents();
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		onRewarded.Invoke();
	}
	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		onClose.Invoke();
	}	

	private void OnDisable()
	{
		if (rewardedAdManager.IsReadyToShowAd())
		{
			rewardedAdManager.CurrentRewardedAd().OnUserEarnedReward -= HandleRewardBasedVideoRewarded;
			rewardedAdManager.CurrentRewardedAd().OnAdClosed -= HandleRewardedAdClosed;
		}
	}
}


