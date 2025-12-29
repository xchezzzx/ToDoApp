dapr run `
	--app-id todoapp-accessor `
	--app-port 5228 `
	--dapr-http-port 3502 `
	--resources-path ./dapr/components `
	-- dotnet run --project ToDoApp.Accessor
