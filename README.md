Для запуска необходимо:
1. установить SQL Server Express LocalDB. Ссылка на скачивание: https://go.microsoft.com/fwlink/?linkid=2215160;
2. после установки LocalDB выполнить команду "Update-Database" в Package Manager Console, которая применит созданные ранее Entity Framework Core миграции;
3. в папке meteoriteapp.client выполнить npm install.

Запуск проекта осуществляется через Debug (F5). Бэк и фронт запустится сам, ничего руками прописывать не нужно.
После запуска Debug откроется Swagger по адресу https://localhost:44305/swagger. 
Чуть позже откроется консоль, в которой будет указан адрес для перехода на фронт: https://localhost:5173/
