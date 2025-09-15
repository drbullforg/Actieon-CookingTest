using System;
using TMPro;
using UnityEngine;

public class CookingTimer: MonoBehaviour
{
    public static CookingTimer instance;

    private DateTime endTime;
    private bool isCooking = false;

    public float cookingDuration = 10f;
    public TextMeshProUGUI timer;

    private void Awake() {
        instance = this;

        if (PlayerPrefs.HasKey("CookingEndTime")) {
            long binaryTime = Convert.ToInt64(PlayerPrefs.GetString("CookingEndTime"));
            endTime = DateTime.FromBinary(binaryTime);
            isCooking = true;
        }
    }

    void Update() {
        if (isCooking) {
            TimeSpan remaining = endTime - DateTime.UtcNow;

            if (remaining.TotalSeconds > 0) {
                UpdateTime(remaining);
            } else {
                Debug.Log("อาหารเสร็จแล้ว!");
                isCooking = false;
                GameSystemManager.instance.CookFinished();
            }
        }
    }

    public void UpdateTime(TimeSpan time) {
        timer.text = time.Minutes + ":" + time.Seconds.ToString("00");
    }

    public void StartCooking() {
        endTime = DateTime.UtcNow.AddSeconds(cookingDuration);
        PlayerPrefs.SetString("CookingEndTime", endTime.ToBinary().ToString());
        PlayerPrefs.Save();
        isCooking = true;
    }
}
