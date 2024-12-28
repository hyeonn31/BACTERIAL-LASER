using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Magazine 클래스는 탄창 시스템을 구현하며, IReloadable 인터페이스를 상속받습니다.
public class Magazine : MonoBehaviour, IReloadable
{
    // 최대 탄약 수
    public int maxBullets = 20;

    // 재장전 시간 (초 단위)
    public float chargingTime = 2f;

    // 현재 탄약 수를 저장하는 내부 변수
    private int currentBullets;

    // 현재 탄약 수를 관리하는 프로퍼티
    private int CurrentBullets
    {
        get => currentBullets; // 현재 탄약 수를 반환
        set
        {
            // 현재 탄약 수가 0보다 작으면 0으로 설정
            if (value < 0)
                currentBullets = 0;
            // 현재 탄약 수가 최대 탄약 수를 초과하면 최대치로 설정
            else if (value > maxBullets)
                currentBullets = maxBullets;
            // 유효한 범위 내에서는 그대로 설정
            else
                currentBullets = value;

            // 탄약 변경 이벤트를 호출
            OnBulletsChanged?.Invoke(currentBullets);
            // 충전 상태 변경 이벤트를 호출 (비율로 계산)
            OnChargeChanged?.Invoke((float)currentBullets / maxBullets);
        }
    }

    // 재장전 시작 이벤트
    public UnityEvent OnReloadStart;
    // 재장전 종료 이벤트
    public UnityEvent OnReloadEnd;

    // 탄약 수 변경 이벤트 (int형으로 전달)
    public UnityEvent<int> OnBulletsChanged;
    // 충전 상태 변경 이벤트 (float형으로 비율 전달)
    public UnityEvent<float> OnChargeChanged;

    // 시작 시 탄약을 최대치로 설정
    private void Start()
    {
        CurrentBullets = maxBullets; // 현재 탄약을 최대 탄약으로 초기화
    }

    // 탄약을 사용할 때 호출되는 메서드
    public bool Use(int amount = 1)
    {
        // 사용할 만큼의 탄약이 있는지 확인
        if (CurrentBullets >= amount)
        {
            CurrentBullets -= amount; // 탄약을 차감
            return true; // 사용 가능
        }
        else
        {
            return false; // 사용 불가능
        }
    }

    // 재장전을 시작하는 메서드
    public void StartReload()
    {
        // 탄약이 이미 최대치이면 재장전 중단
        if (currentBullets == maxBullets)
            return;

        // 기존에 실행 중인 모든 코루틴을 중단
        StopAllCoroutines();
        // 새로운 재장전 코루틴을 시작
        StartCoroutine(ReloadProcess());
    }

    // 재장전을 중단하는 메서드
    public void StopReload()
    {
        // 실행 중인 모든 코루틴을 중단
        //StopAllCoroutines();
    }

    // 재장전 과정을 처리하는 코루틴
    private IEnumerator ReloadProcess()
    {
        // 재장전 시작 이벤트 호출
        OnReloadStart?.Invoke();

        // 재장전 시작 시간을 기록
        var beginTime = Time.time;

        // 재장전 시작 시점의 탄약 수
        var beginBullets = currentBullets;

        // 부족한 탄약 비율 계산
        var enoughPercent = 1f - ((float)currentBullets / maxBullets);

        // 부족한 비율에 따른 필요한 재장전 시간 계산
        var enoughChargingTime = chargingTime * enoughPercent;

        // 재장전 진행
        while (true)
        {
            // 현재 재장전 경과 시간을 비율로 계산
            var t = (Time.time - beginTime) / enoughChargingTime;

            // 재장전이 완료되었으면 루프 종료
            if (t >= 1f)
                break;

            // 현재 탄약 수를 선형 보간하여 업데이트
            CurrentBullets = (int)Mathf.Lerp(beginBullets, maxBullets, t);

            // 다음 프레임까지 대기
            yield return null;
        }

        // 탄약을 최대치로 설정
        CurrentBullets = maxBullets;

        // 재장전 종료 이벤트 호출
        OnReloadEnd?.Invoke();
    }
}
