import { useMemo, useState } from "react";
import PageHeader from "../components/PageHeader";
import TableState from "../components/TableState";
import { CLASS_DEFS } from "../data/classes";
import { getMonthAbsents } from "../api/services";
import { useAsync } from "../api/useAsync";
import { fullName, studentClass } from "../utils/format";
import { formatJalali, jalaliToday, JALALI_MONTHS } from "../utils/jalali";

export default function Page() {
  const now = jalaliToday();
  const [month, setMonth] = useState(now.m);
  const [query, setQuery] = useState("");
  const [cls, setCls] = useState("");

  const absents = useAsync(() => getMonthAbsents(now.y, month), [month]);

  const rows = useMemo(() => {
    const list = absents.data ?? [];
    const perStudent = new Map<number, number>();
    list.forEach(({ log }) => perStudent.set(log.studentId, (perStudent.get(log.studentId) ?? 0) + 1));

    return list
      .map(({ date, log }, i) => ({
        key: `${date}-${log.studentId}-${i}`,
        date,
        name: log.student ? fullName(log.student) : "-",
        className: log.student ? studentClass(log.student) : "-",
        count: perStudent.get(log.studentId) ?? 1
      }))
      .sort((a, b) => b.date.localeCompare(a.date));
  }, [absents.data]);

  const filtered = rows.filter(
    (r) => (!cls || r.className === cls) && (!query.trim() || r.name.includes(query.trim()))
  );

  return (
    <>
      <PageHeader title="غیبت‌ها" subtitle="مشاهده سوابق غیبت دانش‌آموزان" />
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
            <thead><tr><th>#</th><th>نام دانش‌آموز</th><th>کلاس</th><th>تاریخ</th><th>غیبت در این ماه</th><th>وضعیت</th></tr></thead>
            <tbody>
              {absents.loading ? (
                <TableState cols={6} text="در حال بارگذاری..." />
              ) : absents.error ? (
                <TableState cols={6} text={absents.error} error />
              ) : filtered.length === 0 ? (
                <TableState cols={6} text={`غیبتی برای ${JALALI_MONTHS[month - 1]} ثبت نشده است`} />
              ) : (
                filtered.map((r, i) => (
                  <tr key={r.key}>
                    <td>{i + 1}</td><td>{r.name}</td><td>{r.className}</td>
                    <td>{formatJalali(r.date)}</td><td>{r.count}</td>
                    <td><span className="status bad">غیبت ثبت‌شده</span></td>
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
