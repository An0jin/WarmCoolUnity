using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdBtn : MonoBehaviour
{
    Button btn;
    // [SerializeField] Text msg;
    private InterstitialAd interstitialAd;

    void Start()
    {
        btn = GetComponent<Button>();

        MobileAds.Initialize(_ => { Debug.Log("AdMob Initialized"); });
        btn.gameObject.SetActive(false);
        LoadInterstitial();

        btn.onClick.AddListener(ShowInterstitial);
    }
    // void SetMsg(string msg){
    //     msg.color = new Color(1, 0, 0);
    //     msg.text = msg;
    // }

    void LoadInterstitial()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        InterstitialAd.Load(Env.interstitialId, new AdRequest(), (ad, loadError) =>
        {
            if (loadError != null)
            {
                Debug.LogError($"[AD] 로드 실패: {loadError}");
                btn.gameObject.SetActive(false);
                // SetMsg($"[AD] 로드 실패: {loadError}");
                // 실패 시 일정 시간 뒤 재시도하거나, 사용자 액션 때 재시도
                return;
            }

            interstitialAd = ad;
            RegisterCallbacks(interstitialAd);
            Debug.Log("[AD] 로드 성공");
            btn.gameObject.SetActive(true);
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
            btn.gameObject.SetActive(false);
            // SetMsg("[AD] 아직 준비 안 됨 → 재로드 시도");
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
            btn.gameObject.SetActive(false);
            LoadInterstitial(); // 다음 노출을 위해 즉시 새로 로드
        };

        ad.OnAdFullScreenContentFailed += (AdError err) =>
        {
            Debug.LogError($"[AD] 전체화면 열기 실패: {err} → 폐기 후 재로드");
            ad.Destroy();
            interstitialAd = null;
            btn.gameObject.SetActive(false);
            // SetMsg($"[AD] 전체화면 열기 실패: {err} → 폐기 후 재로드");
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
