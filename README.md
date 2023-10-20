<h1>⚙️ Minimal REST-API</h1>

Было созданно минимальное REST-API с помощью компонентов middleware, которые соответсвовали CRUD операциями. <br>

✳️ `GetUsersMiddleware` - обрабатывает запрос по методу GET <br>
✳️ `PostUsersMiddleware` - обрабатывает запрос по методу POST <br>
✳️ `PutUsersMiddleware` - обрабатывает запрос по методу PUT <br>
✳️ `DeleteUsersMiddleware` - обрабатывает запрос по методу DELETE <br>

Этим компонентам соответствуют такие конечные точки:

📚 Описание:<br>
base_url - ваш URL, который задан в приложении. Н.П: `http://localhost:5159`<br>

📌GET `{base_url}/api/v0.0.1/users` возвращает пользователей по данному пути 
<br>
  ```json
  [
    {
        "id": "1",
        "name": "Stas",
        "surname": "ggggas",
        "age": 19
    },
    {
        "id": "2",
        "name": "Slava",
        "surname": "Sdawd",
        "age": 19
    },
    {
        "id": "3",
        "name": "Andrei",
        "surname": "Fasd1",
        "age": 19
    },
    {
        "id": "4",
        "name": "Vania",
        "surname": "1rsdfs",
        "age": 19
    }
]
```

<br>

📌 GET `{base_url}/api/v0.0.1/users?_limit=2` возвращает ограниченное количество пользователей по параметру запроса _limit=2
  ```json
 [
    {
        "id": "1",
        "name": "Stas",
        "surname": "ggggas",
        "age": 19
    },
    {
        "id": "2",
        "name": "Slava",
        "surname": "Sdawd",
        "age": 19
    }
]
```
📌 GET `{base_url}/api/v0.0.1/users?id=2` возвращает пользователя по параметру запроса id=2
```json   
    {
        "id": "2",
        "name": "Slava",
        "surname": "Sdawd",
        "age": 19
    }
```
Если пользователей не найден по такому id, эму приходит ответ об ошибке, что такого пользователя не существует
```json   
    {
      "type": "Eror",
      "statusCode": 200,
      "serverMessage": "User by id: 7 not found",
      "time": "2023-10-20T11:36:23.1720672+03:00"
    }
```
📌 POST `/api/v0.0.1/users/add` создаем пользователя, отправляя его серверу в формате JSON

```json   
    {
      "Name": "Kostia",
      "Surname": "Mmss",
      "Age": 19
    }
```
И получаем сразу ответ в формате JSON про статус операции
```json   
{
    "operationId": 31,
    "status": 100,
    "title": "Entity created and added",
    "jsonBody": "{\r\n  \"Id\": \"5\",\r\n  \"Name\": \"Kostia\",\r\n  \"Surname\": \"Mmss\",\r\n  \"Age\": 19\r\n}"
}
```
📌PUT - `/api/v0.0.1/users/update` частично изменяет пользователя. Например, изменим у пользователя фамилию на Test с Id 1
```json   
    {
        "id": "1",
        "name": "Stas",
        "surname": "Test",
        "age": 19
    }
```
В ответ об статусе операции, сервер нам отправляет JSON
```json   
   {
      "operationId": 28,
      "status": 300,
      "title": "Entity has been updating",
      "jsonBody": null
   }
```
📌 DELETE - `/api/v0.0.1/users/remove` удаляет пользователя по id 4
```json
[
  {
    "id": "4"
  }
]
```
Об успешном удалении пользователя, нам придёт ответ 
```json
[
    {
        "operationId": 78,
        "status": 200,
        "title": "Entity has been deleted",
        "jsonBody": null
    }
]
```
