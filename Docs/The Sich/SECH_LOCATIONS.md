# Система локаций — Сечь

## Overview

Добавлена система выбора и покупки локаций из главного меню на основе существующего **BiomeData**.

---

## Компоненты

### BiomeData (расширенный ScriptableObject)

**Путь:** `Assets/Scripts/Chunks/BiomeData.cs`

Используется как локация. Добавленные поля:
- `biomeId` — уникальный ID
- `displayName` — ключ локализации названия
- `description` — ключ локализации описания
- `icon` — иконка для магазина
- `preview` — превью для магазина
- `price` — цена в монетах
- `isDefault` — доступна с начала
- `backgroundMusic` — фоновая музыка

### GameDataManager (расширен)

Новые методы:
- `GetLocation(id)` — получить локацию по ID
- `IsLocationUnlocked(id)` — проверка, куплена ли локация
- `CanAffordLocation(location)` — хватает ли монет
- `PurchaseLocation(location)` — купить локацию
- `SelectLocation(location)` — выбрать локацию
- `GetCurrentSelectedLocation()` — получить текущую выбранную
- `GetDefaultLocation()` — получить локацию по умолчанию

### SavesYG (расширен)

Добавлены поля:
```csharp
public List<string> unlockedLocations = new List<string>();
public string selectedLocation;
```

### UI компоненты

- **LocationCardUI** — карточка одной локации
- **LocationShopUI** — панель магазина локаций

---

## Пример использования

### Создание локации

1. Создать `BiomeData` через Unity: правый клик → Create → Game → Biome Data
2. Заполнить поля:
   - `biomeId` — уникальный ID (например "forest", "desert")
   - `displayName` — ключ для локализации (например "location_forest_name")
   - `description` — ключ описания
   - `price` — цена в монетах
   - `isDefault` — галочка для бесплатной стартовой
   - `tiles`, `decorations` — настройки мира

3. Добавить все BiomeData в GameDataManager.Locations

### Добавление ключей локализации

В `ru.json`:
```json
{
  "location_shop_title": "Локации",
  "location_forest_name": "Лес",
  "location_forest_desc": "Тёмный лес с призраками",
  "location_desert_name": "Пустыня",
  "location_desert_desc": "Знойная пустыня"
}
```

---

## Архитектура

### Поток покупки

```
User clicks Buy
    ↓
LocationCardUI.OnActionButtonClick()
    ↓
GameDataManager.PurchaseLocation()
    ↓
YG2.saves.unlockedLocations.Add()
YG2.saves.coins -= price
YG2.SaveProgress()
    ↓
Refresh UI
```

### Поток выбора

```
User clicks Select
    ↓
LocationCardUI.OnActionButtonClick()
    ↓
GameDataManager.SelectLocation()
    ↓
YG2.saves.selectedLocation = biomeId
```

### Поток старта игры

```
UIMenuManager.StartGame()
    ↓
SceneManager.LoadScene("Game")
    ↓
GameManager.InitializeGame()
    ↓
TilemapChunkManager.SetLocation(currentLocation)
TilemapChunkManager.Init()
    ↓
Использует BiomeData для генерации мира
```

---

## Пример данных

| biomeId | Название | Цена | По умолчанию |
|---------|----------|------|---------------|
| forest | Лес | 0 | Да |
| desert | Пустыня | 500 | Нет |
| volcano | Вулкан | 1000 | Нет |

---

*Обновлено: Март 2026*
