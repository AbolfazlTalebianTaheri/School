import { GraduationCap, Plus, Users } from "lucide-react";
import PageHeader from "../components/PageHeader";
import { classes } from "../data/mock";

export default function ClassesPage() {
  return (
    <>
      <PageHeader
        title="کلاس‌ها"
        subtitle="مدیریت کلاس‌های مدرسه"
        action={<button className="primary-btn compact"><Plus size={18} /> کلاس جدید</button>}
      />

      <div className="class-grid">
        {classes.map(c => (
          <article className="class-card" key={c.id}>
            <div className={`class-icon ${c.color}`}><GraduationCap /></div>
            <h3>{c.title}</h3>
            <p>{c.teacher}</p>
            <div className="class-meta"><Users size={17} /><span>{c.students} دانش‌آموز</span></div>
            <button className="secondary-btn">مشاهده کلاس</button>
          </article>
        ))}
      </div>
    </>
  );
}
