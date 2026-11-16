import { useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { GraduationCap, Plus, Users } from "lucide-react";
import PageHeader from "../components/PageHeader";
import { getStudents } from "../api/services";
import { useAsync } from "../api/useAsync";
import { CLASS_DEFS } from "../data/classes";
import { classKeyOf, faNumber } from "../utils/format";

export default function ClassesPage() {
  const navigate = useNavigate();
  const students = useAsync(getStudents, []);

  const counts = useMemo(() => {
    const map = new Map<string, number>();
    (students.data ?? []).forEach((s) => {
      const key = classKeyOf(s);
      map.set(key, (map.get(key) ?? 0) + 1);
    });
    return map;
  }, [students.data]);

  return (
    <>
      <PageHeader
        title="کلاس‌ها"
        subtitle="مدیریت کلاس‌های مدرسه"
        action={<button className="primary-btn compact"><Plus size={18} /> کلاس جدید</button>}
      />

      <div className="class-grid">
        {CLASS_DEFS.map((c) => (
          <article className="class-card" key={c.key}>
            <div className={`class-icon ${c.color}`}><GraduationCap /></div>
            <h3>{c.title}</h3>
            <p>پایه {c.gradeLabel} · رشته {c.fieldLabel}</p>
            <div className="class-meta">
              <Users size={17} />
              <span>{students.loading ? "..." : faNumber(counts.get(c.key) ?? 0)} دانش‌آموز</span>
            </div>
            <button className="secondary-btn" onClick={() => navigate(`/students?class=${c.key}`)}>مشاهده کلاس</button>
          </article>
        ))}
      </div>
    </>
  );
}
