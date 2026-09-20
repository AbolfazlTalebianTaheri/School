import PageHeader from "../components/PageHeader";
import { students } from "../data/mock";

export default function Page() {
  return (
    <>
      <PageHeader title="غیبت‌ها" subtitle="مشاهده سوابق غیبت دانش‌آموزان" />
      <section className="panel">
        <div className="filters-row">
          <select><option>همه کلاس‌ها</option><option>دهم الف</option><option>دهم ب</option></select>
          <select><option>همه ماه‌ها</option><option>آبان</option><option>مهر</option></select>
          <input placeholder="جستجوی دانش‌آموز..." />
        </div>
        <div className="table-wrap">
          <table>
            <thead><tr><th>#</th><th>نام دانش‌آموز</th><th>کلاس</th><th>تاریخ</th><th>تعداد غیبت</th><th>وضعیت</th></tr></thead>
            <tbody>
        {students.map(s => <tr key={s.id}><td>{s.id}</td><td>{s.name}</td><td>{s.className}</td><td>{s.date}</td><td>{s.absence}</td><td><span className="status bad">غیبت ثبت‌شده</span></td></tr>)}
        </tbody>
          </table>
        </div>
      </section>
    </>
  );
}
