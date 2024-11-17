# Распределённые системы

## ASP.NET Core WebApi

### [Minimal](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-9.0) WebApi 

Пример программы: https://github.com/sergey-berezin/dotnet4/tree/master/3.%20Distributed/WebApiMin

Обработчики запросов записываются в виде лямбда-выражений.

* `GET /hello?name=World` получает параметры из строки запроса
* `POST /hello` получает параметры из тела запроса. Командная строка для отправки запроса: `curl -v -X POST http://localhost:5148/hello -d @input.json --header "Content-Type: application/json"` 

Сервис предоставляет описание своего контракта.

* Человеко-читаемый формат: `/swagger`
* В формате [OpenApi](https://spec.openapis.org/oas/latest.html): `/swagger/v1/swagger.json`