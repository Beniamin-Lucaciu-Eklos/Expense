# API development sample data

Run the API from the repository root to create or top up the local development dataset:

```powershell
dotnet run --configuration Release --project src\ExpenseApp.Api\ExpenseApp.Api.csproj --launch-profile http -- --seed-sample-data --seed-count=600000
```

The command is Development-only, applies pending EF Core migrations, and uses SQL Server bulk batches. It creates a dedicated demo account and generates synthetic income and expense transactions across the previous 36 months. Re-running it is safe: existing sample rows are counted and only missing rows are added. The maximum supported count is 1,000,000.

Demo login:

- Email: `sample-data@expenseapp.local`
- Password: `SampleData-Only!2026`

The client uses the paginated endpoint (`GET /api/transactions/paged?page=1&pageSize=50`); page sizes are limited to 100. Monthly and all-time totals are available from `GET /api/transactions/summary` and `GET /api/transactions/summary/all-time`.
