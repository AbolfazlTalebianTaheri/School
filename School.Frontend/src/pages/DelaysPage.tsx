import { useMemo, useState } from "react";
import PageHeader from "../components/PageHeader";
import TableState from "../components/TableState";
import { CLASS_DEFS } from "../data/classes";
import { getMonthDelays } from "../api/services";
import { useAsync } from "../api/useAsync";
import { BELLS, fullName, studentClass } from "../utils/format";
import { formatJalali, jalaliToday, JALALI_MONTHS } from "../utils/jalali";

export default function Page() {
  const now = jalaliToday();
  const [month, setMonth] = useState(now.m);
  const [query, setQuery] = useState("");
  const [cls, setCls] = useState("");

  const delays = useAsync(() => getMonthDelays(now.y, month), [month]);

  const rows = useMemo(
    () =>
      (delays.data ?? [])
        .map(({ date, delay: d }, i) => {
          const s = d.attendanceLog?.student;
          const bell = BELLS.find((b) => b.value === Number(d.class_Bell))?.label ?? "-";
          return {
            key: `${date}-${d.attendanceLogId}-${d.class_Bell}-${i}`,
            date,
            name: s ? fullName(s) : "-",
            className: s ? studentClass(s) : "-",
            bellLesson: d.lesson?.lessonName ? `${bell} · ${d.lesson.lessonName}` : bell,
            minutes: d.delayMinutes
          };
        })
        .sort((a, b) => b.date.localeCompare(a.date)),
    [delays.data]
  );

  const filtered = rows.filter(
    (r) => (!cls || r.className === cls) && (!query.trim() || r.name.includes(query.trim()))
  );

  return (
    <>
      <PageHeader title="تاخیرها" subtitle="مشاهده و مدیریت تاخیرهای ثبت‌شده" />
      <section className="panel">
        <div className="filters-row">
          <select value={cls} onChange={(e) => setCls(e.target.value)}>
            <option value="">همه کلاس‌ها</option>
            {CLASS_DEFS.map((c) => <option key={c.key} value={c.title}>{c.title}</option>)}
          </select>
          <select value={month} onChange={(e) => setMonth(Number(e.target.value))}>
            {JALALI_MONTHS.map((name, i) => <option key={name} value={i + 1}>{name}</option>)}
          </select>
          <input
            placeholder="جستجوی دانش‌آموز..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
        </div>
        <div className="table-wrap">
          <table>
            <thead><tr><th>#</th><th>نام دانش‌آموز</th><th>کلاس</th><th>تاریخ</th><th>زنگ / درس</th><th>دقایق تاخیر</th></tr></thead>
            <tbody>
              {delays.loading ? (
                <TableState cols={6} text="در حال بارگذاری..." />
              ) : delays.error ? (
                <TableState cols={6} text={delays.error} error />
              ) : filtered.length === 0 ? (
                <TableState cols={6} text={`تاخیری برای ${JALALI_MONTHS[month - 1]} ثبت نشده است`} />
              ) : (
                filtered.map((r, i) => (
                  <tr key={r.key}>
                    <td>{i + 1}</td><td>{r.name}</td><td>{r.className}</td>
                    <td>{formatJalali(r.date)}</td><td>{r.bellLesson}</td><td>{r.minutes}</td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </section>
    </>
  );
}
