using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text txt;
    [SerializeField] private Transform player;

    [SerializeField] private Image[] SkillImage;

    [SerializeField] private Sprite[] AuraSkill;
    [SerializeField] private Sprite[] JiJooHyeockSkill;
    [SerializeField] private Sprite[] SecuritasSkill;
    [SerializeField] private Sprite[] RavagebellSkill;
    [SerializeField] private Sprite[] SeciliaSkill;

    [SerializeField] private TMP_Text skillNameTxt;
    [SerializeField] private TMP_Text infoText;

    [SerializeField] private GameObject textInfo_Box;
    [SerializeField] private GameObject imgInfo_Box;

    [SerializeField] private Slider atkSlider;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider spdSlider;

    public void CharacterImageInfo_Change()
    {
        textInfo_Box.SetActive(false);
        imgInfo_Box.SetActive(true);

        switch (txt.text)
        {
            case "Aura":
                atkSlider.value = 0.8f;
                hpSlider.value = 0.6f;
                spdSlider.value = 0.4f;
                break;
            case "JiJooHyeock":
                atkSlider.value = 0.6f;
                hpSlider.value = 0.6f;
                spdSlider.value = 0.6f;
                break;
            case "Securitas":
                atkSlider.value = 0.8f;
                hpSlider.value = 0.6f;
                spdSlider.value = 0.2f;
                break;
            case "Ravagebell":
                atkSlider.value = 1f;
                hpSlider.value = 0.2f;
                spdSlider.value = 0.2f;
                break;
            case "Secilia":
                atkSlider.value = 0.4f;
                hpSlider.value = 0.8f;
                spdSlider.value = 0.8f;
                break;
            default:
                break;
        }
    }

    public void CharacterSelect_TxtChange()
    {
        textInfo_Box.SetActive(true);
        imgInfo_Box.SetActive(false);
        switch (txt.text)
        {
            case "Aura":
                skillNameTxt.text = "아우라";
                infoText.text = "크리티시아에서 온 소녀 아우라는 겪어본 싸움이라곤 말 안듣는 소와의 씨름 뿐이었지만, 농삿일로 단련된 신체 덕분에 무거운 대검을 대충 휘두르는 것 만으로도 디멘션 아레나의 다른 참가자들을 해치울 수 있습니다.";
                break;
            case "JiJooHyeock":
                skillNameTxt.text = "지주혁";
                infoText.text = "지구 출신의 대한민국 청년 지주혁은 얼굴에 난 흉터를 지우고 싶다는 소원을 이루고자, 디멘션 아레나에 참가했습니다. ";
                break;
            case "Securitas":
                skillNameTxt.text = "시큐리타스";
                infoText.text = "디멘션 아레나의 경비 로봇이었던 시큐리타스는, 참가자들의 공격으로 인해 연습용 표적으로 전락했지만, 그 중 1기의 시큐리타스가 참가자들에게 복수하기 위해 디멘션 아레나에 참가했습니다.";
                break;
            case "Ravagebell":
                skillNameTxt.text = "레비지벨";
                infoText.text = "원시림 행성 보타니아에서 온 꽃 형태의 괴물 래비지벨은, 더 맛있고 영양가 있는 사냥감을 찾아 디멘션 아레나에 참가했습니다. ";
                break;
            case "Secilia":
                skillNameTxt.text = "세실리아";
                infoText.text = "행성 라스의 주민으로 자신의 행성에 태양신의 빛의 축복을 받아 능력을 강화하는 생명체지만 차원을 넘어오며 더이상 축복을 받지 못하게 되어 능력이 현저하게 떨어져 버렸습니다. 그녀가 축복을 받지 못하게 되었다는 표식으로 금발이 짙은 흑발로 바뀌었습니다.";
                break;
            default:
                skillNameTxt.text = "";
                infoText.text = "";
                break;
        }
    }

    public void PassiveSelect_TxtChange()
    {
        textInfo_Box.SetActive(true);
        imgInfo_Box.SetActive(false);
        switch (txt.text)
        {
            case "Aura":
                skillNameTxt.text = "룰루랄라";
                infoText.text = "이동 중 시간에 비례하여 이동속도가 서서히 증가합니다. 공격을 하거나 공격받을 경우 해당 효과는 전부 사라집니다.";
                break;
            case "JiJooHyeock":
                skillNameTxt.text = "강화 사격";
                infoText.text = "세 번째 공격마다 기본 공격이 강화되어 총 9발의 탄환을 발사합니다. ";
                break;
            case "Securitas":
                skillNameTxt.text = "오버클럭";
                infoText.text = "오버클럭 상태가 되어 다음 기본 공격 1회가 3배의 피해를 줍니다. 이 효과는 5초에 한 번만 발동되며 중첩되지 않습니다.";
                break;
            case "Ravagebell":
                skillNameTxt.text = "맹독 웅덩이";
                infoText.text = "독액 발사와 맹독 포화 사용 시 지정 지점에 초당 피해를 입히는 맹독 웅덩이를 생성합니다.";
                break;
            case "Secilia":
                skillNameTxt.text = "신기조식";
                infoText.text = "비 전투 상태 시 체력을 지속적으로 회복합니다. 전투 중 체력이 일정 이하로 떨어졌을 때 최대 체력의 일부만큼 10초간 보호막을 생성합니다. 이 효과는 30초에 한 번씩만 발동됩니다. ";
                break;
            default:
                break;
        }
    }

    public void AtkSelect_TxtChange()
    {
        textInfo_Box.SetActive(true);
        imgInfo_Box.SetActive(false);
        switch (txt.text)
        {
            case "Aura":
                skillNameTxt.text = "이얍 !";
                infoText.text = "육중한 대검을 매우 빠르게 휘둘러 바람의 칼날을 날립니다. 바람의 칼날은 적이나 장애물과 부딪히면 사라집니다.";
                break;
            case "JiJooHyeock":
                skillNameTxt.text = "지향 사격";
                infoText.text = "기관단총을 난사하여 6발의 탄환을 발사해 피해를 입힙니다.";
                break;
            case "Securitas":
                skillNameTxt.text = "이온포";
                infoText.text = "에너지 탄환을 1발 발사하여 피해를 입힙니다. ";
                break;
            case "Ravagebell":
                skillNameTxt.text = "독액 발사";
                infoText.text = "독액을 발사하여 지정 지점에 맹독 웅덩이를 생성합니다.";
                break;
            case "Secilia":
                skillNameTxt.text = "세실류 불꽃 연속 펀치";
                infoText.text = "전방을 향해 빠른 3연타 펀치를 날립니다.";
                break;
            default:
                break;
        }
    }

    public void SkillSelect_TxtChange()
    {
        textInfo_Box.SetActive(true);
        imgInfo_Box.SetActive(false);
        switch (txt.text)
        {
            case "Aura":
                skillNameTxt.text = "흐랏챠 !!";
                infoText.text = "도약한 후 온 몸의 힘을 실은 대검을 바닥에 강하게 내려칩니다. 아우라의 전방에 부채꼴 모양으로 충격파를 일으켜 범위 피해를 주고 밀쳐냅니다. ";
                break;
            case "JiJooHyeock":
                skillNameTxt.text = "수류탄 투척";
                infoText.text = "원형 범위에 폭발 피해를 주고 밀쳐내는 수류탄을 투척합니다. ";
                break;
            case "Securitas":
                skillNameTxt.text = "비상 탈출";
                infoText.text = "현재 위치에 폭발물을 떨어뜨리고 점프하여 지정 위치로 이동합니다. 폭발물은 지면에 떨어진 후 1초가 지나면 폭발합니다. ";
                break;
            case "Ravagebell":
                skillNameTxt.text = "맹독 포화";
                infoText.text = "독액 발사를 3회 연속으로 실행하여 3개의 맹독 웅덩이를 생성합니다.";
                break;
            case "Secilia":
                skillNameTxt.text = "세실류 초속 이동";
                infoText.text = "범위 내 가까운 대상에게 순간적으로 이동합니다. ";
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        txt.text = SelectedCharacter.Instance.nextCharacterName;
        Sprite[] tempSprite;
        switch (txt.text)
        {
            case "Aura":
                tempSprite = AuraSkill;
                break;
            case "JiJooHyeock":
                tempSprite = JiJooHyeockSkill;
                break;
            case "Securitas":
                tempSprite = SecuritasSkill;
                break;
            case "Ravagebell":
                tempSprite = RavagebellSkill;
                break;
            case "Secilia":
                tempSprite = SeciliaSkill;
                break;
            default:
                tempSprite = null;
                break;
        }

        if (tempSprite == null)
            return;

        for (int i = 0; i < SkillImage.Length; ++i)
        {
            SkillImage[i].sprite = tempSprite[i];
        }
    }

    private void OnDisable()
    {
        player.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void BackSpace()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        SelectedCharacter.Instance.CharacterSelected(false);
    }

    public void CharacterSelected()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");

        SelectedCharacter.Instance.ChangeCharacterInfo();
    }
}
