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

	//  �޼���� ���� �ε��ϱ� ���� ȣ��˴ϴ�.
	public void LoadAd()
	{
		Debug.Log("Loading Ad: " + _adUnitId);
		Advertisement.Load(_adUnitId, this);
	}

	// ���� ���������� �ε�Ǹ� ȣ��˴ϴ�.
	public void OnUnityAdsAdLoaded(string adUnitId)
	{
		Debug.Log("Ad Loaded: " + adUnitId);
		_isAdLoaded = true;
	}

	// ��ư�� Ŭ���Ǹ� ȣ��˴ϴ�.
	public void ShowAd(Action action = null)
	{
		if(!_isAdLoaded)
		{
			action?.Invoke();
			return;
		}

		if (action != null)
			onCompleteAd.AddListener(()=>action?.Invoke());

		// ���� ��û 
		_isAdLoaded = false;  
		Advertisement.Show(_adUnitId, this);
	} 
	 
	// ���� ������ ��û�Ǹ� ȣ��˴ϴ�.
	public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
	{
		if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
		{
			onCompleteAd?.Invoke(); 
			LoadAd();
			Debug.Log("Unity Ads Rewarded Ad Completed");
		}
	}

	// ���� �ε� �Ǵ� ǥ�� �߿� ������ �߻��ϸ� ȣ��˴ϴ�.:
	public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
	{
		Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
	}
	public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
	{
		Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
	}


	// ���� ���۵ǰų� Ŭ���� �� ȣ��˴ϴ�. 
	public void OnUnityAdsShowStart(string adUnitId) { }
	public void OnUnityAdsShowClick(string adUnitId) { }


	void OnDestroy() 
	{
	
	}
}
