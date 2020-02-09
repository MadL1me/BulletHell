using Unity;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [SerializeField] public Image[] playerHP;
    [SerializeField] public GameObject roulette;
    [SerializeField] public Image[] bulletsPlaces;

    [SerializeField] private Sprite stdBullet;
    [SerializeField] private Sprite hellBullet;
    [SerializeField] private Sprite silverBullet;
    [SerializeField] private Sprite bloodBullet;

    [SerializeField] private GameObject Hellamicon;
    [SerializeField] private Text bulletComboText;


    [SerializeField] private GameObject _prologueUI;
    [SerializeField] private GameObject _mail;
    [SerializeField] private Image BlackImage;
    [SerializeField] private GameObject mailButton;
    [SerializeField] private Text[] LocationInfo;


    [SerializeField] private Sprite _ricoshet;
    [SerializeField] private Sprite _double;
    [SerializeField] private Sprite _ghost;
    [SerializeField] private Sprite _vampire;
    [SerializeField] private Sprite _shotgun;
    [SerializeField] private Sprite _demonic;


    [SerializeField] private GameObject _bossHealthBarObj;
    [SerializeField] private Slider _bossHealthBarSlider;

    public float rotatingSpeed = 1; 

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(OpenHellamicon());
    }

    private bool isOpened = false;

    public IEnumerator OpenHellamicon()
    {
        AudioManager.Instance.PlayBookOpenSound();
        Debug.Log(Time.timeScale);
        if (!isOpened)
        {
            Hellamicon.SetActive(true);
            Time.timeScale = 0;
            isOpened = true;
            GameManager.isPause = true;
        }
        else
        {
            Hellamicon.SetActive(false);
            Time.timeScale = 1;
            isOpened = false;
            GameManager.isPause = false;
        }
        yield return new WaitForFixedUpdate();
    }

    public void ChangeComboBullets(ComboType type)
    {
        switch (type)
        {
            case ComboType.Double:
                foreach (Image img in bulletsPlaces)
                    img.sprite = _double;
                break;

            case ComboType.Ricoshet:
                foreach (Image img in bulletsPlaces)
                    img.sprite = _ricoshet;
                break;

            case ComboType.Demonic:
                foreach (Image img in bulletsPlaces)
                    img.sprite = _demonic;
                break;

            case ComboType.Ghost:
                foreach (Image img in bulletsPlaces)
                    img.sprite = _ghost;
                break;

            case ComboType.Vampire:
                foreach (Image img in bulletsPlaces)
                    img.sprite = _vampire;
                break;

            case ComboType.Shotgun:
                foreach (Image img in bulletsPlaces)
                    img.sprite = _shotgun;
                break;
        }
    }

    public void ChangeComboText(string text)
    {
        bulletComboText.text = text;

        if (!PlayerInfo.findedComboDict.Contains(text) && text != "No combo")
        {
            PlayerInfo.findedComboDict.Add(text);
            NewBulletComboEffect();
        }
        if (PlayerInfo.findedComboDict.Contains(text))
        {
            AudioManager.Instance.PlayBulletComboSound();
        }
    }

    private void NewBulletComboEffect()
    {
        AudioManager.Instance.PlayNewComboSound();
    }

    public void PressBtn() => isPressed = true;

    public bool isPressed = false;

    public IEnumerator IntroToGame()
    {
        GameManager.isPause = true;
        var code = KeyCode.Space;
        var speed = 0.01f;

        yield return new WaitForSecondsRealtime(1);

        for (float i = 1; i >= 0; i -= speed)
        {
            BlackImage.color = new Color(BlackImage.color.r, BlackImage.color.g, BlackImage.color.b, i);
            yield return new WaitForFixedUpdate();
        }

        while (!isPressed)
        {
            yield return new WaitForFixedUpdate();
        }

        isPressed = false;

        for (float i = 0; i <= 1; i += speed)
        {
            BlackImage.color = new Color(BlackImage.color.r, BlackImage.color.g, BlackImage.color.b, i);
            yield return new WaitForFixedUpdate();
        }

        _mail.SetActive(false);


        for (float i = 0; i <= 1; i += speed)
        {
            foreach (Text txt in LocationInfo)
                txt.color = new Color(1, 1, 1, i);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSecondsRealtime(2);

        GameManager.isPause = false;

        for (float i = 1; i >= 0; i -= speed)
        {
            BlackImage.color = new Color(BlackImage.color.r, BlackImage.color.g, BlackImage.color.b, i);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSecondsRealtime(1);

        for (float i = 1; i >= 0; i -= speed)
        {
            foreach (Text txt in LocationInfo)
                txt.color = new Color(1, 1, 1, i);
            yield return new WaitForFixedUpdate();
        }

    }
    public IEnumerator LearnToCrafts()
    {
        yield break;
    }

    public IEnumerator LearnToShoot()
    {
        yield break;
    }

    public IEnumerator Introduce()
    {
        yield break;
    }

    public IEnumerator BossHealthBar()
    {
        _bossHealthBarObj.SetActive(true);
        _bossHealthBarSlider.value = 0;

        for (int i = 0; i<30; i++)
        {
            _bossHealthBarSlider.value += 1000 / 30;
            yield return new WaitForFixedUpdate();
        }

    }

    public void BossHealthBarChange(float value) => _bossHealthBarSlider.value = value;


    public IEnumerator LearnToDash()
    {
        //Learn to Reload and shoot
        var code = KeyCode.R;
        yield return new WaitUntil(isPressedButton);

        code = KeyCode.V;
        yield return new WaitUntil(isPressedButton);
        yield return new WaitUntil(isPressedButton);
        yield return new WaitUntil(isPressedButton);
        yield return new WaitUntil(isPressedButton);
        yield return new WaitUntil(isPressedButton);
        yield return new WaitUntil(isPressedButton);

        code = KeyCode.R;
        yield return new WaitUntil(isPressedButton);

        // Learn to Dash
        code = KeyCode.Space;
        yield return new WaitUntil(isPressedButton);

        // Learn to Combos & Hellamicon


        yield return new WaitForFixedUpdate();

        GameManager.isPause = false;

        bool isPressedButton() => Input.GetKeyDown(code);
        bool isPressedAnyButton() => Input.anyKeyDown;
    }



    public IEnumerator InitBossBar()
    {
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator RotateRevolverRoll(bool toDefaultRotation = false, bool inserBullet = true)
    {
        var delta = 72 / (100 / rotatingSpeed);
        if (toDefaultRotation)
            delta = roulette.GetComponent<RectTransform>().rotation.eulerAngles.z / (100 / rotatingSpeed);

        if (!inserBullet)
            delta *= -1;

        for (float i = 0; i<100; i+=rotatingSpeed)
        {
            roulette.gameObject.transform.Rotate(new Vector3(0,0, -delta));
            yield return new WaitForFixedUpdate();
        }
    }

    public void AddBullet(BulletTypes bulletTypes, List<BulletTypes> types)
    {
        var col = stdBullet;
        if (bulletTypes == BulletTypes.Silver)
            col = silverBullet;
        else if (bulletTypes == BulletTypes.Blood)
            col = bloodBullet;
        else if (bulletTypes == BulletTypes.Hellfire)
            col = hellBullet;
        StartCoroutine(RotateRevolverRoll());
        bulletsPlaces[types.Count - 1].color = Color.white;
        bulletsPlaces[types.Count - 1].sprite = col;
    }

    public void ShootBullet(int bulletsInRound, int startBulletCount)
    {
        bulletsPlaces[startBulletCount - bulletsInRound].color = Color.black;
    }

    public void ChangeHeartsCount(int value, int capacity)
    {
        if (value > capacity)
            value = capacity;
        for (int i = 0; i < value; i++)
            playerHP[i].color = Color.white;
        for (int i = value; i < capacity; i++)
            playerHP[i].color = Color.black;
    }

    public void ChangeHeartsCapacity(int value)
    {
        for (int i = 0; i < value; i++)
            playerHP[i].enabled = true;
        for (int i = value; i < playerHP.Length; i++)
            playerHP[i].enabled = false;
    }
}