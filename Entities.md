# 📊 Описание сущностей проекта HabitTracker

## AppUser (Пользователь)
**Назначение:** Представляет пользователя системы  
**Наследуется от:** IdentityUser (ASP.NET Core Identity)

| Поле | Тип | Описание |
|------|-----|----------|
| Id | string | Уникальный идентификатор |
| DisplayName | string | Отображаемое имя |
| RegisteredAt | DateTime | Дата регистрации |
| TimeZone | string | Часовой пояс пользователя |
| Habits | ICollection<Habit> | Список привычек пользователя |
| Goals | ICollection<Goal> | Список целей пользователя |

---

## Habit (Привычка)
**Назначение:** Хранит информацию о привычке пользователя

| Поле | Тип | Описание |
|------|-----|----------|
| Id | Guid | Уникальный идентификатор |
| Name | string | Название привычки |
| Description | string? | Описание |
| Frequency | int | Частота (0=Daily, 1=Weekly, 2=Monthly) |
| Type | int | Тип (0=Binary, 1=Numeric, 2=Text) |
| TargetStreak | int | Целевая серия дней |
| ReminderTime | TimeSpan? | Время напоминания |
| UserId | string | Владелец привычки |
| User | AppUser? | Навигационное свойство |
| Logs | ICollection<HabitLog> | История выполнений |
| Goals | ICollection<Goal> | Связанные цели |
| CreatedAt | DateTime | Дата создания |
| ArchivedAt | DateTime? | Дата архивации |

---

## Goal (Цель)
**Назначение:** Цель, связанная с привычкой

| Поле | Тип | Описание |
|------|-----|----------|
| Id | Guid | Уникальный идентификатор |
| Name | string | Название цели |
| Description | string? | Описание |
| TargetDate | DateTime | Целевая дата достижения |
| IsCompleted | bool | Статус выполнения |
| CompletedAt | DateTime? | Дата завершения |
| HabitId | Guid | Связанная привычка |
| UserId | string | Владелец цели |

---

## HabitLog (Лог выполнения)
**Назначение:** История выполнения привычки

| Поле | Тип | Описание |
|------|-----|----------|
| Id | Guid | Уникальный идентификатор |
| HabitId | Guid | Связанная привычка |
| UserId | string | Владелец |
| Date | DateTime | Дата записи |
| IsCompleted | bool | Выполнено или нет |
| Note | string? | Заметка |
| Value | decimal? | Числовое значение |
| Mood | string? | Настроение |
| EnergyLevel | int? | Уровень энергии |
| LoggedAt | DateTime | Дата создания записи |

---

## PredictionModel (Модель прогноза)
**Назначение:** Данные для прогнозирования выполнения привычек

| Поле | Тип | Описание |
|------|-----|----------|
| Id | Guid | Уникальный идентификатор |
| HabitId | Guid | Связанная привычка |
| PredictedDate | DateTime | Дата прогноза |
| SuccessProbability | decimal | Вероятность успеха |
| Factors | string? | Влияющие факторы |


