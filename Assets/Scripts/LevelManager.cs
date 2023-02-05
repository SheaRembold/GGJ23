using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    PortalController portal;
    [SerializeField]
    CharacterController character;
    [SerializeField]
    Slider waterProgress;
    [SerializeField]
    int waterRequired = 1;

    public static LevelManager Instance;

    public bool IsCutscene { get; private set; }

    int waterCollected;

    private void Awake()
    {
        Instance = this;

        waterProgress.value = 0f;
        waterProgress.maxValue = waterRequired;

        StartCoroutine(EnterLevel());
    }

    IEnumerator EnterLevel()
    {
        IsCutscene = true;

        character.gameObject.SetActive(false);
        portal.Open();

        yield return new WaitWhile(() => !portal.IsOpen);

        character.gameObject.SetActive(true);

        yield return new WaitWhile(() => character.IsLanding);

        portal.Close();

        yield return new WaitWhile(() => portal.IsOpen);

        IsCutscene = false;
    }

    public void CollectWater()
    {
        waterCollected++;
        waterProgress.value = (float)waterCollected / waterRequired;
        if (waterCollected >= waterRequired)
        {
            portal.Open();
        }
    }

    public void EnterPortal()
    {
        StartCoroutine(ExitLevel());
    }

    IEnumerator ExitLevel()
    {
        IsCutscene = true;

        character.Leave();

        yield return new WaitWhile(() => character.IsLeaving);

        character.gameObject.SetActive(false);
        portal.Close();

        yield return new WaitWhile(() => portal.IsOpen);

        IsCutscene = false;
    }
}
