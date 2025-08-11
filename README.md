# UrlShortener

Веб-приложение для создания коротких ссылок (URL-сокращатель) на ASP.NET Core 8 с использованием SQLite.

---

## Описание

Приложение предоставляет REST API для создания коротких ссылок и перенаправления по ним.  

Основные возможности:

▪ Создание короткой ссылки из длинной URL  
▪ Перенаправление с короткой ссылки на оригинальный URL  
▪ Валидация URL  
▪ Обработка ошибок с корректными HTTP статусами  
▪ Swagger UI для удобного тестирования API  

---

## Технологии

▪ ASP.NET Core 8  
▪ Entity Framework Core с SQLite  
▪ xUnit и Moq для тестирования  
▪ Swagger для документации API  

---

## Запуск проекта

1. Клонировать репозиторий:

```git clone https://github.com/Grv25/UrlShortener```

```cd UrlShortener/UrlShortener```

2. Настроить строку подключения в appsettings.json (по умолчанию используется SQLite файл urls.db):

```json
"ConnectionStrings": {
 "DefaultConnection": "Data Source=urls.db"
}
```
   
3. Запустить миграции и приложение:

```dotnet ef database update```

```dotnet run```

4. Открыть Swagger UI для тестирования API по адресу:

```https://localhost:7294/swagger```

---

## API

• POST /api/urlapi — Создание короткой ссылки  
   Request body:
   ```json
   {
      "longUrl": "https://example.com/very/long/url"
   }
```
   Response:
   ```json
   {
      "shortUrl": "https://localhost/abc12345"
   }
```
   
• GET /{shortCode} — Перенаправление на оригинальный URL
   
---

## Тесты

▪ Тесты написаны с использованием xUnit и Moq  
▪ Для тестов сервисов используется In-Memory база данных для изоляции  
▪ Чтобы запустить тесты:  
```dotnet test```

---

## Структура проекта

▪ Controllers/ — контроллеры API и редиректа  
▪ Services/ — бизнес-логика работы с URL  
▪ Models/ — модели данных и DTO  
▪ Migrations/ — миграции базы данных  
▪ Tests/ — Unit-тесты  

---

## Особенности и решения

В качестве базы данных выбран SQLite, так как он проще в использовании и не требует установки сервера.

Метод GET протестирован следующим образом:   
▪ Вставка в строку браузера короткого URL (полученного в методе POST)  
▪ С помощью Postman.  
    
