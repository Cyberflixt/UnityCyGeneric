using System.Collections;
using UnityEngine;

public class PopupSystem : MonoBehaviour
{
    [SerializeField] private Transform popupsHolder;
    [SerializeField] private float popupLifetime = 5;

    [Header("Start")]
    [SerializeField] private MinMaxFloat delay;
    [SerializeField] private Transform[] startPopups;
    [SerializeField] private bool startRandomOrder = false;
    [SerializeField] private MinMaxFloat startRate = new(0.05f, 0.07f);

    [Header("Ongoing")]
    [SerializeField] private Transform[] ongoingPopups;
    [SerializeField] private MinMaxFloat ongoingRate = new(1f, 10f);
    [SerializeField] private MinMaxInt ongoingBurstCount = new(1, 1);
    [SerializeField] private int ongoingCountMax = 0;
    [SerializeField] private int ongoingDurationMax = 0;
    [SerializeField] private bool ongoingRandomOrder = true;

    private int ongoingCount = 0;
    private bool ongoing = false;
    private float startTime;

    // Fisher-Yates Shuffle Implementation
    void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            // Random index
            int randomIndex = Random.Range(0, i + 1);

            // Swap array[i] with the element at the random index
            (array[randomIndex], array[i]) = (array[i], array[randomIndex]);
        }
    }

    public void CreatePopup(Transform popupRef)
    {
        Transform popup = Instantiate(popupRef, popupsHolder);
        popup.gameObject.SetActive(true);
        Destroy(popup.gameObject, popupLifetime);
    }

    IEnumerator Start()
    {
        // Delay
        if (delay.max > 0)
            yield return new WaitForSeconds(delay.GetRandom());

        if (startPopups.Length > 0)
        {
            if (startRandomOrder)
                ShuffleArray(startPopups);

            foreach (Transform v in startPopups)
            {
                CreatePopup(v);

                if (startRate.min > 0)
                    yield return new WaitForSeconds(startRate.GetRandom());
            }
        }

        if (ongoingPopups.Length > 0 && ongoingBurstCount.max > 0 && ongoingRate.max > 0)
        {
            ongoing = true;
            startTime = Time.time;
            ongoingCooldown = ongoingRate.GetRandom();
        }
    }

    float ongoingCooldown = 0;

    void Update()
    {
        if (ongoing)
        {
            ongoingCooldown -= Time.deltaTime;
            if (ongoingCooldown < 0)
            {
                ongoingCooldown += ongoingRate.GetRandom();


                for (int burstCount = ongoingBurstCount.GetRandom(); burstCount > 0; burstCount--)
                {
                    int index;
                    if (ongoingRandomOrder)
                        index = Random.Range(0, ongoingPopups.Length);
                    else
                        index = ongoingCount;
                    CreatePopup(ongoingPopups[index]);
                    ongoingCount++;
                }

                // Max duration
                if (ongoingDurationMax > 0 && (Time.time - startTime > ongoingDurationMax))
                    ongoing = false;

                // Max count
                if (ongoingCountMax > 0 && ongoingCount > ongoingCountMax)
                    ongoing = false;
            }
        }
    }
}
