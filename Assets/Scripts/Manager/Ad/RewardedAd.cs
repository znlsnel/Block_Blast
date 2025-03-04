using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardedAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
	[SerializeField] string _androidAdUnitId = "Rewarded_Android";
	[SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
	string _adUnitId = null; // This will remain null for unsupported platforms
	UnityEvent onCompleteAd = new UnityEvent();

	private bool _isAdLoaded = false;
	void Awake()
	{
		// Get the Ad Unit ID for the current platform:
#if UNITY_IOS
		_adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

		 
		// Disable the button until the ad is ready to show:
	}

	//  메서드는 광고를 로드하기 위해 호출됩니다.
	public void LoadAd()
	{
		Debug.Log("Loading Ad: " + _adUnitId);
		Advertisement.Load(_adUnitId, this);
	}

	// 광고가 성공적으로 로드되면 호출됩니다.
	public void OnUnityAdsAdLoaded(string adUnitId)
	{
		Debug.Log("Ad Loaded: " + adUnitId);
		_isAdLoaded = true;
	}

	// 버튼이 클릭되면 호출됩니다.
	public void ShowAd(Action action = null)
	{
		if(!_isAdLoaded)
		{
			action?.Invoke();
			return;
		}

		if (action != null)
			onCompleteAd.AddListener(()=>action?.Invoke());

		// 광고 시청 
		_isAdLoaded = false;  
		Advertisement.Show(_adUnitId, this);
	} 
	 
	// 광고가 완전히 시청되면 호출됩니다.
	public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
	{
		if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
		{
			onCompleteAd?.Invoke(); 
			LoadAd();
			Debug.Log("Unity Ads Rewarded Ad Completed");
		}
	}

	// 광고 로드 또는 표시 중에 오류가 발생하면 호출됩니다.:
	public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
	{
		Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
	}
	public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
	{
		Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
	}


	// 광고가 시작되거나 클릭될 때 호출됩니다. 
	public void OnUnityAdsShowStart(string adUnitId) { }
	public void OnUnityAdsShowClick(string adUnitId) { }


	void OnDestroy() 
	{
	
	}
}
