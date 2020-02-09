# multitenancy-sample
Multi-tenant ASP.NET Core Application with Dotnettency


# Run DB Migrations
```
dotnet fm migrate -p SqlServer -c "Data Source=.\sqlexpress;Initial Catalog=todo-fabrikam;Integrated Security=True" -a .\bin\Debug\netstandard2.0\TodoApp.Data.dll up -t 100
```
