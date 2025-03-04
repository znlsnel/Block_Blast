using UnityEngine;

public class AdManager : Singleton<AdManager>
{
	public AdsInitializer adsInitializer;
	public RewardedAd rewardedAd;
	protected override void Awake()
	{
		base.Awake();

		adsInitializer = GetComponent<AdsInitializer>();
		rewardedAd = GetComponent<RewardedAd>();


	}
}
