export type Todo = {
    id: string;
    title: string;
    description: string;
    isCompleted: boolean;
    createdAt: Date;    
};

export type TodoResponse = {
    todos: Todo[];
}

export async function fetchTodos(): Promise<Todo[]> {
    const url = new URL("http://localhost:5084/todos/");

    const response = await fetch(url);

    if (!response.ok) throw new Error(`Error fetching Todos: ${response.statusText}`);

    return response.json();
}