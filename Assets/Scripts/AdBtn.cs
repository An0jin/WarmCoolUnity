using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdBtn : MonoBehaviour
{
    [SerializeField] Button btn;
    private InterstitialAd interstitialAd;

    void Start()
    {
        if (btn == null) btn = GetComponent<Button>();

        MobileAds.Initialize(_ => { Debug.Log("AdMob Initialized"); });
        // btn.interactable = false;
        LoadInterstitial();

        btn.onClick.AddListener(ShowInterstitial);
    }

    void LoadInterstitial()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        InterstitialAd.Load(Env.adMobId, new AdRequest(), (ad, loadError) =>
        {
            if (loadError != null)
            {
                Debug.LogError($"[AD] 로드 실패: {loadError}");
                // btn.interactable = false;
                // 실패 시 일정 시간 뒤 재시도하거나, 사용자 액션 때 재시도
                return;
            }

            interstitialAd = ad;
            RegisterCallbacks(interstitialAd);
            Debug.Log("[AD] 로드 성공");
            // btn.interactable = true; // 준비되면 버튼 활성화
        });
    }

    void ShowInterstitial()
    {
        Debug.Log("버튼 누름");
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("[AD] 표시");
            interstitialAd.Show();
            // Show() 이후 이 인스턴스는 재사용 금지
        }
        else
        {
            Debug.Log("[AD] 아직 준비 안 됨 → 재로드 시도");
            // btn.interactable = false;
            LoadInterstitial();
        }
    }

    void RegisterCallbacks(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("[AD] 전체화면 열림");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("[AD] 닫힘 → 폐기 후 재로드");
            ad.Destroy();
            interstitialAd = null;
            // btn.interactable = false;
            LoadInterstitial(); // 다음 노출을 위해 즉시 새로 로드
        };

        ad.OnAdFullScreenContentFailed += (AdError err) =>
        {
            Debug.LogError($"[AD] 전체화면 열기 실패: {err} → 폐기 후 재로드");
            ad.Destroy();
            interstitialAd = null;
            // btn.interactable = false;
            LoadInterstitial();
        };

        ad.OnAdImpressionRecorded += () => Debug.Log("[AD] 노출 기록");
        ad.OnAdClicked += () => Debug.Log("[AD] 클릭");
    }

    void OnDestroy()
    {
        interstitialAd?.Destroy();
        interstitialAd = null;
    }
}
