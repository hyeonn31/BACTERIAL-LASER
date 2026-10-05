# 🔫 BACTERIAL LASER

[![YouTube](https://img.shields.io/badge/YouTube-플레이_영상-FF0000?style=flat-square&logo=youtube&logoColor=white)](https://www.youtube.com/watch?v=rqN3LNkh5fo)
![Unity](https://img.shields.io/badge/Unity-2021.2-000000?style=flat-square&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![Meta Quest](https://img.shields.io/badge/Meta_Quest-VR-0467DF?style=flat-square&logo=meta&logoColor=white)

**VR 코어 방어 서바이벌 슈팅 게임**입니다.
사방에서 끝없이 몰려오는 몬스터로부터 중앙의 코어를 지키는 게임으로, 플레이어는 VR 컨트롤러로 총을 잡아 쏘고 폭탄을 던지며 최대한 오래 버팁니다.

Unity **XR Interaction Toolkit**과 **Oculus(Meta Quest)** 환경에서 개발했습니다.

## 🎬 플레이 영상

<p align="center">
  <a href="https://www.youtube.com/watch?v=rqN3LNkh5fo">
    <img src="https://img.youtube.com/vi/rqN3LNkh5fo/hqdefault.jpg" width="720" alt="BACTERIAL LASER 플레이 영상">
  </a>
  <br>
  <sub>▶ 이미지를 클릭하면 유튜브에서 플레이 영상을 볼 수 있습니다</sub>
</p>

---

## 🎮 게임 플레이

| 요소 | 설명 |
|---|---|
| **코어 방어** | 몬스터가 코어에 닿을 때마다 코어 HP가 1씩 줄고, HP가 0이 되면 게임 오버 |
| **점점 거세지는 웨이브** | 일정 간격으로 몬스터 무리가 생성되며, 웨이브마다 생성 수가 점점 증가 |
| **레이저 총** | 컨트롤러로 직접 총을 잡고 트리거를 당기면 레이저가 발사되어 명중 지점에 이펙트 표시 |
| **탄창 & 재장전** | 탄약이 떨어지면 총을 **무기 거치대(Weapon Stand)에 꽂아** 재장전 |
| **폭탄** | 잡아서 던지면 착지 지점 반경의 몬스터를 한 번에 처치 |
| **이동** | 텔레포트 + 스냅 회전으로 VR 멀미를 줄인 로코모션 |
| **UI** | 생존 시간, 처치/생존/생성 몬스터 수, 코어 HP를 VR 공간 UI로 표시 |
| **피드백** | 피격 시 화면 플래시, 사운드, 컨트롤러 햅틱 진동 |

---

## 🛠️ 구현 포인트

### 1. 이벤트 기반 설계
`Core`, `Mob`, `MobManager`, `Shooter`, `Bomb` 등 핵심 컴포넌트가 상태 변화를 `UnityEvent`로 알립니다.
사운드·이펙트·UI·햅틱은 이 이벤트를 **에디터에서 연결**만 하면 되므로 게임 로직과 연출이 느슨하게 분리됩니다.

### 2. 싱글톤 매니저
`Core.Instance`, `MobManager.Instance`로 어디서든 코어와 몬스터 목록에 접근합니다. `MobManager`는 살아 있는 몬스터를 리스트로 관리해 처치 수 집계와 게임 오버 시 일괄 제거(`DestroyAll`)를 담당합니다.

### 3. 코루틴 기반 난이도 곡선
`Spawner`는 `startFactor`에서 시작해 웨이브마다 `additiveFactor`만큼 계수를 올리며 `factor ~ factor×2` 범위의 몬스터를 생성합니다. 시간이 지날수록 자연스럽게 난이도가 오릅니다.

### 4. 인터페이스로 재장전 추상화
`IReloadable` 인터페이스를 `Magazine`이 구현하고, `WeaponStand`는 소켓에 꽂힌 물체가 무엇이든 `IReloadable`이면 재장전을 시작합니다. 새 무기를 추가해도 거치대 코드는 바꿀 필요가 없습니다.

### 5. NavMesh 몬스터 AI
몬스터는 NavMeshAgent로 코어를 향해 이동하며, 개체마다 속도 비율을 무작위로 부여해 움직임이 단조롭지 않습니다.

---

## 📂 프로젝트 구조

```
Assets/
├── Tutorial/
│   ├── Scenes/Tutorial.unity        # ▶ 메인 게임 씬
│   ├── Scripts/
│   │   ├── Core.cs                  # 코어 HP · 피격 · 파괴
│   │   ├── Mob/                     # Mob · MobManager · Spawner · Hittable
│   │   ├── Weapon/
│   │   │   ├── Gun/                 # Gun · Shooter · Magazine · RayVisualizer
│   │   │   ├── Bomb/Bomb.cs         # 투척 · 폭발 범위 판정
│   │   │   ├── WeaponStand.cs       # 소켓 거치 → 재장전
│   │   │   └── IReloadable.cs
│   │   ├── UI/                      # 생존 시간 · 처치 수 · 시선 활성화 UI
│   │   ├── Effect/                  # 플래시 · 색상 · SFX · 햅틱 · NavMesh 목적지
│   │   ├── TeleportActionHandler.cs
│   │   └── EventBridge.cs
│   ├── Prefabs/ · Models/ · Materials/ · Audios/ · Fonts/
├── Settings/                        # URP 렌더 파이프라인 설정
├── XR/ · XRI/                       # XR 플러그인 · 입력 설정
└── (외부 무료 에셋)                   # RPG Monster DUO, Supercyan Character Pack
```

---

## ⚙️ 개발 환경

| 항목 | 버전 |
|---|---|
| Unity | **2021.2.13f1** |
| Render Pipeline | URP 12.1.4 |
| XR Interaction Toolkit | 2.3.2 |
| XR Plugin Management | 4.2.1 |
| Oculus XR Plugin | 1.11.2 |
| 언어 | C# |

## 🚀 실행 방법

1. Unity Hub에서 **Unity 2021.2.13f1**로 이 폴더를 엽니다.
2. `Assets/Tutorial/Scenes/Tutorial.unity`를 엽니다.
3. **Meta Quest에서 실행**
   - File → Build Settings → **Android**로 플랫폼 전환
   - Project Settings → XR Plug-in Management → Android 탭에서 **Oculus** 체크
   - 기기를 USB로 연결하고 **Build And Run**
4. **PC VR(Quest Link)로 테스트**: PC 탭에서 Oculus를 켜고 에디터에서 Play

> 헤드셋이 없다면 `Assets/Samples/XR Interaction Toolkit/2.3.2/XR Device Simulator`의 시뮬레이터 프리팹을 씬에 추가해 키보드·마우스로 테스트할 수 있습니다.

---

## 🎨 사용 에셋

- RPG Monster DUO PBR Polyart — 몬스터 모델
- Supercyan Character Pack Free Sample — 캐릭터 모델
- XR Interaction Toolkit Starter Assets / XR Device Simulator (Unity 공식 샘플)
