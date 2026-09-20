import { LucideIcon } from "lucide-react";

export default function StatCard({
  title, value, hint, icon: Icon, tone = "blue"
}: {
  title: string;
  value: string | number;
  hint: string;
  icon: LucideIcon;
  tone?: "blue" | "green" | "red" | "amber" | "purple";
}) {
  return (
    <div className={`stat-card tone-${tone}`}>
      <div className="stat-icon"><Icon size={24} /></div>
      <div>
        <span>{title}</span>
        <strong>{value}</strong>
        <small>{hint}</small>
      </div>
    </div>
  );
}
