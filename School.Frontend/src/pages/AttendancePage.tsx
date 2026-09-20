import { Save } from "lucide-react";
import PageHeader from "../components/PageHeader";
import { students } from "../data/mock";

export default function AttendancePage() {
  return (
    <>
      <PageHeader title="ثبت حضور و غیاب" subtitle="وضعیت امروز دانش‌آموزان را ثبت کنید" />

      <section className="panel">
        <div className="filters-row">
          <select><option>دهم الف</option><option>دهم ب</option><option>یازدهم الف</option></select>
          <input className="date-input" value="۱۴۰۳/۰۸/۱۵" readOnly />
          <button className="primary-btn compact"><Save size={18} /> ذخیره تغییرات</button>
        </div>

        <div className="attendance-list">
          {students.map(s => (
            <div className="attendance-row" key={s.id}>
              <div className="student-cell"><div className="tiny-avatar">{s.name[0]}</div><div><strong>{s.name}</strong><small>{s.className}</small></div></div>
              <div className="segmented">
                <button className="seg ok">حاضر</button>
                <button className="seg warn">تاخیر</button>
                <button className="seg bad">غایب</button>
              </div>
              <input className="time-input" value={s.time} readOnly />
            </div>
          ))}
        </div>
      </section>
    </>
  );
}
