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

### Протокол HTTP

* Методы HTTP
    * GET
    * HEAD
    * PUT
    * PATCH
    * POST
    * DELETE
    * OPTIONS
    * TRACE
    * CONNECT
* [Коты состояния](https://http.cat/)
    * 1xx - Информационный
    * 2xx - Успех
    * 3xx - Перенаправление
    * 4xx - Ошибка клиента. Например, неправильные параметры запроса
    * 5xx - Ошибка сервера
* [REST](https://en.wikipedia.org/wiki/REST): REpresentational State Transfer
  * Ресурсы и URI
  * Архитектурные ограничения:
    * Client/Server
    * Stateless
    * Cache
    * Uniform inteface
      * URI
      * Resource representation
      * Self-descriptive messages
      * Hyperlinks
    * Code on demand
  * [CRUD](): Created, Read, Update and Delete
    * `POST /books` добавляет книгу. Возвращает в том числе `id` книги
    * `GET /books` возвращает список книг
    * `GET /books/id` возвращает расширенную информацию о книге
    * `PUT /books/id` изменяет информацю о книге
    * `DELETE /books/id` удаляет информацию о книге
  
## Контроллеры WebApi

Действия над одним ресурсом группируются в классе-контроллере.
* Потомок класса `ControllerBase`
* Атрибуты `[Route]` и `[ApiController]`