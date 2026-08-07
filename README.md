# EnemyWaves

Мобильный top-down шутер-выживалка: игрок двигается джойстиком,
оружие стреляет автоматически по ближайшей цели,
волны врагов спавнятся вокруг игрока и не заканчиваются, пока он жив.

- **Unity:** 6000.3.10f1 (URP)
- **DI:** Zenject
- **Reactive:** UniRx
- **Ввод:** Joystick Pack + Input System

## Быстрый старт

1. Открыть проект в Unity 6000.3.10f1.
2. Запустить сцену `Assets/Scenes/Loading.unity` — она через `LoadingController` грузит `Gameplay.unity`.

## Конфиги

Все игровые настройки — это `ScriptableObject`-ассеты, лежащие в **`Assets/GameData/`**. Менять баланс нужно там, в коде цифр нет.

| Ассет | Что настраивает |
| --- | --- |
| `Assets/GameData/ApplicationConfig.asset` | Target FPS, VSync, запрет затухания экрана |
| `Assets/GameData/PlayerConfig.asset` | Здоровье и скорость игрока |
| `Assets/GameData/WeaponConfig.asset` | Скорострельность, урон, дальность, скорость и префаб снаряда |
| `Assets/GameData/WaveConfig.asset` | Минимум живых врагов, радиус спавна, интервал проверки |
| `Assets/GameData/EnemyDatabase.asset` | Список типов врагов для взвешенного случайного спавна |
| `Assets/GameData/Enemies/*.asset` | Отдельные враги (`Grunt`, `Runner`, `Brute`): HP, скорость, урон, интервал и радиус атаки, вес спавна |
| `Assets/GameData/VfxConfig.asset` | Префабы эффектов попадания/смерти и запас времени перед возвратом в пул |
| `Assets/GameData/Player/PlayerAnimator.controller` | Аниматор игрока + маска верхней части тела |

Новые конфиги создаются через `Create → EnemyWaves → Configs → ...`.

### Куда конфиги подключаются

- `ApplicationConfig` — в `ProjectInstaller` на префабе `Assets/Resources/ProjectContext.prefab` (живёт весь запуск).
- Остальные — в `GameplayInstaller` на сцене `Gameplay.unity`, поля в инспекторе под заголовком `Configs`.

Прочие настройки проекта: `ProjectSettings/` (Unity), `Packages/manifest.json` (зависимости и OpenUPM-реестр), `Assets/Settings/` (URP), `Assets/InputSystem_Actions.inputactions` (ввод).

## Структура кода

`Assets/Scripts/`

```
Bootstrap/    LoadingController — переход из Loading в Gameplay
Configs/      ScriptableObject-классы для ассетов из GameData
Core/         IDamageable, ITargetProvider
Installers/   ProjectInstaller (глобальный), GameplayInstaller (сцена)
Gameplay/
  Player/     PlayerModel/Presenter, PlayerMotor, PlayerHealth, PlayerAnimatorDriver
  Enemies/    EnemyController/Model/View, EnemyFactory, EnemyHopAnimator
  Waves/      EnemySpawnService — добивает популяцию до MinAliveEnemies
  Weapon/     WeaponController, Projectile (пул Zenject)
Services/     ввод, загрузка сцен, состояние игры, реестр целей, VFX, настройки приложения
UI/           HudPresenter/View, GameOverPresenter/View, FillBarAnimator, SafeArea
```

Модели и презентеры — обычные C#-классы, MonoBehaviour остаются только там, где нужна сцена (View, Motor, Health, ввод). Связи и время жизни задаются в инсталлерах.

## Ассеты

`Assets/Prefabs/` — враги (`Grunt`, `Runner`, `Brute`) и `Projectile`.
`Assets/Art/`, `Assets/Textures/`, `Assets/Materials/` и папки сторонних паков — графика и VFX.


https://github.com/user-attachments/assets/efdf8efd-fa92-4567-a7f3-6b165bb3da87
