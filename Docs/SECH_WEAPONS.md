# Оружие и Пассивные умения — Сечь

---

## Часть 1: Оружие

### Таблица оружия

| ID (Asset) | Тип | Класс | Поведение | Урон | Задержка | Скорость | Дальность | Особенности | Файлы |
|------------|-----|-------|-----------|------|----------|----------|-----------|-------------|--------|
| **Knife** | Ranged | `KnifeBehavior` | Летит вперёд по направлению игрока | ? | ? | ? | ? | Одноразовый снаряд, исчезает при ударе | `Weapons/Knife/KnifeBehavior.cs`, `Knife.asset` |
| **Sword** | Melee | `SwordBehavior` | Выпад меча + возврат (DOTween) | ? | ? | ? | ? | Анимация выпада, не исчезает при ударе | `Weapons/Sword/SwordBehavior.cs`, `Sword.asset` |
| **Serp** | Ranged | `SerpBehavior` | Летит, возвращается к игроку | ? | ? | ? | ? | Бумеранг: летит → возвращается через 0.8с | `Weapons/Serp/SerpBehavior.cs`, `Serp.asset` |
| **Mallet** | Ranged | `MalletBehavior` | Летит с силой (Rigidbody2D) | ? | ? | ? | ? | Случайное направление вверх, вращение | `Weapons/Mallet/MalletBehavior.cs`, `Mallet.asset` |
| **Circle** | Ranged | `CircleBehavior` | Орбита вокруг игрока | ? | ? | ? | ? | Вечное оружие, вращается вокруг игрока | `Weapons/Circle/CircleBehavior.cs`, `Circle.asset` |
| **Sable** | Melee | `SableBehavior` | Атака по таймеру с анимацией | ? | ? | ? | ? | Анимация + PolygonCollider, звук | `Weapons/Sable/SableBehavior.cs`, `Sable.asset` |
| **YaropolkSable** | Melee | `YaropolkSableBehavior` | Управление коллайдером вручную | ? | ? | ? | ? | Коллайдер включается/выключается отдельно | `Weapons/Yaropolk Sable/YaropolkSableBehavior.cs`, `YaropolkSable.asset` |

### Базовые параметры (WeaponData)

```
WeaponData (ScriptableObject)
├── weaponType: TypeWeapon (Melee/Ranged/Upgraid)
├── Name: string
├── Description: string
├── Icon: Sprite
├── Prefab: GameObject
├── _damage: float
├── _delay: float (перезарядка)
├── _attackRange: float
├── _speed: float
├── _level: int
├── LevelOpen: int (уровень игрока для открытия)
├── Controller: WeaponController
└── Current* (динамические значения после улучшений)
```

### Иерархия классов

```
Weapon (базовый класс)
├── KnifeBehavior      — одноразовый снаряд
├── SwordBehavior     — выпад меча (DOTween)
├── SerpBehavior      — бумеранг
├── MalletBehavior    — снаряд с физикой
├── CircleBehavior    — орбитальное оружие
├── SableBehavior     — анимация + коллайдер
└── YaropolkSableBehavior — ручное управление коллайдером
```

### Ключевые файлы

| Файл | Назначение |
|------|------------|
| `Scripts/Weapon/WeaponData.cs` | ScriptableObject с параметрами оружия |
| `Scripts/Weapon/WeaponController.cs` | Базовый контроллер, улучшения |
| `Scripts/Weapon/Weapon.cs` | Базовый класс снаряда |
| `Scripts/Weapon/GeneratorWeapon.cs` | Генерация снарядов, пул, разброс |
| `Scripts/Weapon/ManagerWeapon.cs` | Управление всем оружием игрока |

### Логика улучшений (WeaponController)

```
WeaponController.Initialize(weaponData)
        ↓
Upgraid(level) → Level2/Level3/Level4
        ↓
каждый уровень: CurrentDamage *= 1.x
                CurrentDelay /= 1.x
                CurrentSpeed *= 1.x
```

---

## Часть 2: Пассивные умения (Upgrades)

### Таблица пассивок

| ID | Название | Эффект | Формула | Файлы |
|----|----------|--------|---------|--------|
| **SpeedUpgraid** | Ускорение | Увеличивает скорость игрока | `_speed += (_speed / count)` где count = 10 | `Scripts/Upgraid/SpeedUpgraid/SpeedUpgraidController.cs`, `SpeedUpgraid.asset` |
| **HealthUpgraid** | Здоровье | Увеличивает максимальное HP | `_maxHealth += (_maxHealth / count)` где count = 10 | `Scripts/Upgraid/UpgraidHealth/HealthUpgraidController.cs`, `HealthUpgraid.asset` |
| **Cooldown** | Перезарядка | Уменьшает задержку оружия | `CurrentDelay *= 1.1f` (10% уменьшение) | `Scripts/Upgraid/Cooldown/CooldownController.cs`, `Cooldown.asset` |

### Иерархия пассивок

```
WeaponController (базовый класс)
├── SpeedUpgraidController
│   └── PlayerMovement.UpgradeSpeed(_count) — увеличение скорости
├── HealthUpgraidController
│   └── PlayerHealth.UpgraidMaxHealth(_count) — увеличение HP
└── CooldownController
    └── PlayerWeapons.StartWeapon.CurrentDelay — уменьшение задержки
```

### Как работают пассивки

1. При Level Up открывается выбор из 3 опций (ManagerWeapon.GetRandomChoices())
2. Если выбрана пассивка — создаётся новый GameObject с соответствующим Controller
3. При Initialize() вызывается логика улучшения
4. При повторном выборе (улучшение) — вызывается Level2/Level3

### Логика выбора при Level Up

```
Слот 1: Оружие игрока (улучшение)
        ↓
        ├── Если есть оружие в арсенале → случайное оружие игрока
        ├── Иначе → случайное новое оружие
        └── Иначе → случайная пассивка

Слот 2: Новое оружие
        ↓
        ├── Новое оружие (которого нет в арсенале)
        └── Если нет → любое доступное оружие/пассивка

Слот 3: Пассивка
        ↓
        ├── Любая доступная пассивка
        └── Если нет → любое доступное оружие
```

### Ключевые файлы

| Файл | Назначение |
|------|------------|
| `Scripts/Upgraid/SpeedUpgraid/SpeedUpgraidController.cs` | Ускорение игрока |
| `Scripts/Upgraid/UpgraidHealth/HealthUpgraidController.cs` | Увеличение HP |
| `Scripts/Upgraid/Cooldown/CooldownController.cs` | Уменьшение перезарядки |

---

## Часть 3: Как добавить новое оружие

### Шаги

1. **Создать ScriptableObject** (`WeaponData`)
   - Путь: `Assets/Weapons/НовоеОружие/НовоеОружие.asset`
   - Заполнить: Name, Icon, Prefab, параметры

2. **Создать Behavior** (наследник `Weapon`)
   - Путь: `Assets/Weapons/НовоеОружие/НовоеОружиеBehavior.cs`
   - Переопределить: `Initialize()`, `OnTriggerEnter2D()`

3. **Добавить в ManagerWeapon**
   - Добавить `WeaponData` в список `_allWeapons`

4. **Настроить префаб**
   - Добавить компонент GeneratorWeapon
   - Настроить параметры пула

---

## Часть 4: Как добавить новую пассивку

### Шаги

1. **Создать ScriptableObject** (`WeaponData`)
   - Тип: `Upgraid`
   - Заполнить параметры

2. **Создать Controller** (наследник `WeaponController`)
   - Путь: `Scripts/Upgraid/НоваяПассивка/НоваяПассивкаController.cs`
   - Реализовать логику в `Initialize()` и `Level2/Level3`

3. **Добавить в ManagerWeapon**
   - Добавить `WeaponData` в список `_allWeapons`

---

*Обновлено: Февраль 2026*
