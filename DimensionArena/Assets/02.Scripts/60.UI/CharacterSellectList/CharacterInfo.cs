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
                skillNameTxt.text = "�ƿ��";
                infoText.text = "ũ��Ƽ�þƿ��� �� �ҳ� �ƿ��� �޾ �ο��̶�� �� �ȵ�� �ҿ��� ���� ���̾�����, ����Ϸ� �ܷõ� ��ü ���п� ���ſ� ����� ���� �ֵθ��� �� �����ε� ���� �Ʒ����� �ٸ� �����ڵ��� ��ġ�� �� �ֽ��ϴ�.";
                break;
            case "JiJooHyeock":
                skillNameTxt.text = "������";
                infoText.text = "���� ����� ���ѹα� û�� �������� �󱼿� �� ���͸� ����� �ʹٴ� �ҿ��� �̷����, ���� �Ʒ����� �����߽��ϴ�. ";
                break;
            case "Securitas":
                skillNameTxt.text = "��ť��Ÿ��";
                infoText.text = "���� �Ʒ����� ��� �κ��̾��� ��ť��Ÿ����, �����ڵ��� �������� ���� ������ ǥ������ ����������, �� �� 1���� ��ť��Ÿ���� �����ڵ鿡�� �����ϱ� ���� ���� �Ʒ����� �����߽��ϴ�.";
                break;
            case "Ravagebell":
                skillNameTxt.text = "��������";
                infoText.text = "���ø� �༺ ��Ÿ�Ͼƿ��� �� �� ������ ���� ����������, �� ���ְ� ���簡 �ִ� ��ɰ��� ã�� ���� �Ʒ����� �����߽��ϴ�. ";
                break;
            case "Secilia":
                skillNameTxt.text = "���Ǹ���";
                infoText.text = "�༺ ���� �ֹ����� �ڽ��� �༺�� �¾���� ���� �ູ�� �޾� �ɷ��� ��ȭ�ϴ� ����ü���� ������ �Ѿ���� ���̻� �ູ�� ���� ���ϰ� �Ǿ� �ɷ��� �����ϰ� ������ ���Ƚ��ϴ�. �׳డ �ູ�� ���� ���ϰ� �Ǿ��ٴ� ǥ������ �ݹ��� £�� ��߷� �ٲ�����ϴ�.";
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
                skillNameTxt.text = "������";
                infoText.text = "�̵� �� �ð��� ����Ͽ� �̵��ӵ��� ������ �����մϴ�. ������ �ϰų� ���ݹ��� ��� �ش� ȿ���� ���� ������ϴ�.";
                break;
            case "JiJooHyeock":
                skillNameTxt.text = "��ȭ ���";
                infoText.text = "�� ��° ���ݸ��� �⺻ ������ ��ȭ�Ǿ� �� 9���� źȯ�� �߻��մϴ�. ";
                break;
            case "Securitas":
                skillNameTxt.text = "����Ŭ��";
                infoText.text = "����Ŭ�� ���°� �Ǿ� ���� �⺻ ���� 1ȸ�� 3���� ���ظ� �ݴϴ�. �� ȿ���� 5�ʿ� �� ���� �ߵ��Ǹ� ��ø���� �ʽ��ϴ�.";
                break;
            case "Ravagebell":
                skillNameTxt.text = "�͵� ������";
                infoText.text = "���� �߻�� �͵� ��ȭ ��� �� ���� ������ �ʴ� ���ظ� ������ �͵� �����̸� �����մϴ�.";
                break;
            case "Secilia":
                skillNameTxt.text = "�ű�����";
                infoText.text = "�� ���� ���� �� ü���� ���������� ȸ���մϴ�. ���� �� ü���� ���� ���Ϸ� �������� �� �ִ� ü���� �Ϻθ�ŭ 10�ʰ� ��ȣ���� �����մϴ�. �� ȿ���� 30�ʿ� �� ������ �ߵ��˴ϴ�. ";
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
                skillNameTxt.text = "�̾� !";
                infoText.text = "������ ����� �ſ� ������ �ֵѷ� �ٶ��� Į���� �����ϴ�. �ٶ��� Į���� ���̳� ��ֹ��� �ε����� ������ϴ�.";
                break;
            case "JiJooHyeock":
                skillNameTxt.text = "���� ���";
                infoText.text = "��������� �����Ͽ� 6���� źȯ�� �߻��� ���ظ� �����ϴ�.";
                break;
            case "Securitas":
                skillNameTxt.text = "�̿���";
                infoText.text = "������ źȯ�� 1�� �߻��Ͽ� ���ظ� �����ϴ�. ";
                break;
            case "Ravagebell":
                skillNameTxt.text = "���� �߻�";
                infoText.text = "������ �߻��Ͽ� ���� ������ �͵� �����̸� �����մϴ�.";
                break;
            case "Secilia":
                skillNameTxt.text = "���Ƿ� �Ҳ� ���� ��ġ";
                infoText.text = "������ ���� ���� 3��Ÿ ��ġ�� �����ϴ�.";
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
                skillNameTxt.text = "���í !!";
                infoText.text = "������ �� �� ���� ���� ���� ����� �ٴڿ� ���ϰ� ����Ĩ�ϴ�. �ƿ���� ���濡 ��ä�� ������� ����ĸ� ������ ���� ���ظ� �ְ� ���ĳ��ϴ�. ";
                break;
            case "JiJooHyeock":
                skillNameTxt.text = "����ź ��ô";
                infoText.text = "���� ������ ���� ���ظ� �ְ� ���ĳ��� ����ź�� ��ô�մϴ�. ";
                break;
            case "Securitas":
                skillNameTxt.text = "��� Ż��";
                infoText.text = "���� ��ġ�� ���߹��� ����߸��� �����Ͽ� ���� ��ġ�� �̵��մϴ�. ���߹��� ���鿡 ������ �� 1�ʰ� ������ �����մϴ�. ";
                break;
            case "Ravagebell":
                skillNameTxt.text = "�͵� ��ȭ";
                infoText.text = "���� �߻縦 3ȸ �������� �����Ͽ� 3���� �͵� �����̸� �����մϴ�.";
                break;
            case "Secilia":
                skillNameTxt.text = "���Ƿ� �ʼ� �̵�";
                infoText.text = "���� �� ����� ��󿡰� ���������� �̵��մϴ�. ";
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
