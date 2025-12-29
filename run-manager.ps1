dapr run `
	--app-id todoapp-manager `
	--app-port 5084 `
	--dapr-http-port 3501 `
	--resources-path ./dapr/components `
	-- dotnet run --project ToDoApp.Manager
