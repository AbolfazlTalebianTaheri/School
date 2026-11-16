export default function TableState({
  cols,
  text,
  error
}: {
  cols: number;
  text: string;
  error?: boolean;
}) {
  return (
    <tr>
      <td colSpan={cols} className={`table-state${error ? " error" : ""}`}>
        {text}
      </td>
    </tr>
  );
}
