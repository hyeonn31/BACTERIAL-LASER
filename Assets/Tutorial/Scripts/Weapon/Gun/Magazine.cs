using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Magazine Ŭ������ źâ �ý����� �����ϸ�, IReloadable �������̽��� ��ӹ޽��ϴ�.
public class Magazine : MonoBehaviour, IReloadable
{
    // �ִ� ź�� ��
    public int maxBullets = 20;

    // ������ �ð� (�� ����)
    public float chargingTime = 2f;

    // ���� ź�� ���� �����ϴ� ���� ����
    private int currentBullets;

    // ���� ź�� ���� �����ϴ� ������Ƽ
    private int CurrentBullets
    {
        get => currentBullets; // ���� ź�� ���� ��ȯ
        set
        {
            // ���� ź�� ���� 0���� ������ 0���� ����
            if (value < 0)
                currentBullets = 0;
            // ���� ź�� ���� �ִ� ź�� ���� �ʰ��ϸ� �ִ�ġ�� ����
            else if (value > maxBullets)
                currentBullets = maxBullets;
            // ��ȿ�� ���� �������� �״�� ����
            else
                currentBullets = value;

            // ź�� ���� �̺�Ʈ�� ȣ��
            OnBulletsChanged?.Invoke(currentBullets);
            // ���� ���� ���� �̺�Ʈ�� ȣ�� (������ ���)
            OnChargeChanged?.Invoke((float)currentBullets / maxBullets);
        }
    }

    // ������ ���� �̺�Ʈ
    public UnityEvent OnReloadStart;
    // ������ ���� �̺�Ʈ
    public UnityEvent OnReloadEnd;

    // ź�� �� ���� �̺�Ʈ (int������ ����)
    public UnityEvent<int> OnBulletsChanged;
    // ���� ���� ���� �̺�Ʈ (float������ ���� ����)
    public UnityEvent<float> OnChargeChanged;

    // ���� �� ź���� �ִ�ġ�� ����
    private void Start()
    {
        CurrentBullets = maxBullets; // ���� ź���� �ִ� ź������ �ʱ�ȭ
    }

    // ź���� ����� �� ȣ��Ǵ� �޼���
    public bool Use(int amount = 1)
    {
        // ����� ��ŭ�� ź���� �ִ��� Ȯ��
        if (CurrentBullets >= amount)
        {
            CurrentBullets -= amount; // ź���� ����
            return true; // ��� ����
        }
        else
        {
            return false; // ��� �Ұ���
        }
    }

    // �������� �����ϴ� �޼���
    public void StartReload()
    {
        // ź���� �̹� �ִ�ġ�̸� ������ �ߴ�
        if (currentBullets == maxBullets)
            return;

        // ������ ���� ���� ��� �ڷ�ƾ�� �ߴ�
        StopAllCoroutines();
        // ���ο� ������ �ڷ�ƾ�� ����
        StartCoroutine(ReloadProcess());
    }

    // �������� �ߴ��ϴ� �޼���
    public void StopReload()
    {
        // ���� ���� ��� �ڷ�ƾ�� �ߴ�
        //StopAllCoroutines();
    }

    // ������ ������ ó���ϴ� �ڷ�ƾ
    private IEnumerator ReloadProcess()
    {
        // ������ ���� �̺�Ʈ ȣ��
        OnReloadStart?.Invoke();

        // ������ ���� �ð��� ���
        var beginTime = Time.time;

        // ������ ���� ������ ź�� ��
        var beginBullets = currentBullets;

        // ������ ź�� ���� ���
        var enoughPercent = 1f - ((float)currentBullets / maxBullets);

        // ������ ������ ���� �ʿ��� ������ �ð� ���
        var enoughChargingTime = chargingTime * enoughPercent;

        // ������ ����
        while (true)
        {
            // ���� ������ ��� �ð��� ������ ���
            var t = (Time.time - beginTime) / enoughChargingTime;

            // �������� �Ϸ�Ǿ����� ���� ����
            if (t >= 1f)
                break;

            // ���� ź�� ���� ���� �����Ͽ� ������Ʈ
            CurrentBullets = (int)Mathf.Lerp(beginBullets, maxBullets, t);

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ź���� �ִ�ġ�� ����
        CurrentBullets = maxBullets;

        // ������ ���� �̺�Ʈ ȣ��
        OnReloadEnd?.Invoke();
    }
}
