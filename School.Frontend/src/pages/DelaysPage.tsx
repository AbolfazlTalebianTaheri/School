import PageHeader from "../components/PageHeader";
import { students } from "../data/mock";

export default function Page() {
  return (
    <>
      <PageHeader title="تاخیرها" subtitle="مشاهده و مدیریت تاخیرهای ثبت‌شده" />
      <section className="panel">
        <div className="filters-row">
          <select><option>همه کلاس‌ها</option><option>دهم الف</option><option>دهم ب</option></select>
          <select><option>همه ماه‌ها</option><option>آبان</option><option>مهر</option></select>
          <input placeholder="جستجوی دانش‌آموز..." />
        </div>
        <div className="table-wrap">
          <table>
            <thead><tr><th>#</th><th>نام دانش‌آموز</th><th>کلاس</th><th>تاریخ</th><th>زمان ورود</th><th>دقایق تاخیر</th></tr></thead>
            <tbody>
        {students.map(s => <tr key={s.id}><td>{s.id}</td><td>{s.name}</td><td>{s.className}</td><td>{s.date}</td><td>{s.time}</td><td>{s.delay * 5}</td></tr>)}
        </tbody>
          </table>
        </div>
      </section>
    </>
  );
}
