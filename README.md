БД по умолчанию localdb, измените строку 4 в appsettings.json на localhost (см. комментарий) чтобы использовать встроенную БД.

Чтобы поднять БД изпользуй консольные команды:
Add-Migration InitialCreate
Update-Database

INSERT для тестовых данных находятся в файле database.sql.
