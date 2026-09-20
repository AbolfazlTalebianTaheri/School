export default function StatusBadge({ status }: { status: "حاضر" | "تاخیر" | "غایب" | "فعال" | "غیرفعال" }) {
  const cls =
    status === "حاضر" || status === "فعال" ? "ok" :
    status === "تاخیر" ? "warn" : "bad";

  return <span className={`status ${cls}`}>{status}</span>;
}
