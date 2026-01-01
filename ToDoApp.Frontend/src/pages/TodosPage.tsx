import { useQuery } from "@tanstack/react-query";
import { fetchTodos } from "../api/todos";
import { Link } from "react-router-dom";

export default function TodosPage() {
  const q = useQuery({
    queryKey: ['todos', {}],
    queryFn: () => fetchTodos()
  })

  const todos = q.data

  return (
    <div>
      <Link to="/">← Back</Link>

      {todos?.map(todo =>
      <div key={todo.id} style={{borderColor: 'red', borderWidth: '1px', marginBottom: '1rem'}}>
        <ul>{todo.title}</ul>
        <small>{todo.description}</small>
      </div>
      )}
    </div>
  )
}