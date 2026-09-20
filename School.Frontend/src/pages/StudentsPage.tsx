import { Plus, Search } from "lucide-react";
import PageHeader from "../components/PageHeader";
import StatusBadge from "../components/StatusBadge";
import { students } from "../data/mock";

export default function StudentsPage() {
  return (
    <>
      <PageHeader
        title="دانش‌آموزان"
        subtitle="مدیریت و مشاهده اطلاعات دانش‌آموزان"
        action={<button className="primary-btn compact"><Plus size={18} /> افزودن دانش‌آموز</button>}
      />

      <section className="panel">
        <div className="filters-row">
          <div className="search-box"><Search size={18} /><input placeholder="جستجوی نام، کد یا کلاس..." /></div>
          <select><option>همه کلاس‌ها</option><option>دهم الف</option><option>دهم ب</option></select>
          <select><option>همه وضعیت‌ها</option><option>حاضر</option><option>تاخیر</option><option>غایب</option></select>
        </div>

        <div className="table-wrap">
          <table>
            <thead>
              <tr><th>#</th><th>نام</th><th>کد دانش‌آموز</th><th>کلاس</th><th>وضعیت</th><th>آخرین تاریخ</th><th>عملیات</th></tr>
            </thead>
            <tbody>
              {students.map(s => (
                <tr key={s.id}>
                  <td>{s.id}</td>
                  <td className="student-cell"><div className="tiny-avatar">{s.name[0]}</div>{s.name}</td>
                  <td>{s.code}</td>
                  <td>{s.className}</td>
                  <td><StatusBadge status={s.status} /></td>
                  <td>{s.date}</td>
                  <td><button className="more-btn">•••</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>
    </>
  );
}
