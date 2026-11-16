import { useMemo, useState } from "react";
import { BarChart3, Clock3, Download, FileBarChart, UserRoundX, Users } from "lucide-react";
import PageHeader from "../components/PageHeader";
import StatCard from "../components/StatCard";
import { CLASS_DEFS } from "../data/classes";
import { getMonthReport, getStudents } from "../api/services";
import { useAsync } from "../api/useAsync";
import { classKeyOf, faNumber } from "../utils/format";
import { faDigits, jalaliToday, JALALI_MONTHS } from "../utils/jalali";

export default function ReportsPage() {
  const now = jalaliToday();
  const [year, setYear] = useState(now.y);
  const [month, setMonth] = useState(now.m);
  const [clsKey, setClsKey] = useState("");

  const students = useAsync(getStudents, []);
  const report = useAsync(() => getMonthReport(year, month), [year, month]);

  // آمار هر کلاس برای ماه انتخاب‌شده
  const perClass = useMemo(() => {
    const days = report.data?.registeredDays.length ?? 0;
    const counts = new Map<string, number>();
    (students.data ?? []).forEach((s) => counts.set(classKeyOf(s), (counts.get(classKeyOf(s)) ?? 0) + 1));

    return CLASS_DEFS.map((c) => {
      const slots = (counts.get(c.key) ?? 0) * days;
      const absent = (report.data?.absents ?? []).filter(({ log }) => log.student && classKeyOf(log.student) === c.key).length;
      const delayed = (report.data?.delays ?? []).filter(
        ({ delay }) => delay.attendanceLog?.student && classKeyOf(delay.attendanceLog.student) === c.key
      ).length;
      const present = Math.max(slots - absent - delayed, 0);
      return { def: c, slots, absent, delayed, present };
    });
  }, [students.data, report.data]);

  const selected = clsKey ? perClass.filter((c) => c.def.key === clsKey) : perClass;
  const totalSlots = selected.reduce((sum, c) => sum + c.slots, 0);
  const totalAbsent = selected.reduce((sum, c) => sum + c.absent, 0);
  const totalDelayed = selected.reduce((sum, c) => sum + c.delayed, 0);
  const days = report.data?.registeredDays.length ?? 0;
  const rate = totalSlots > 0 ? Math.round(((totalSlots - totalAbsent) / totalSlots) * 100) : null;
  const hasData = days > 0 && totalSlots > 0;

  const withData = perClass.filter((c) => c.slots > 0);
  const best = [...withData].sort((a, b) => (b.slots - b.absent) / b.slots - (a.slots - a.absent) / a.slots)[0];
  const mostDelays = [...perClass].sort((a, b) => b.delayed - a.delayed)[0];
  const mostAbsent = [...perClass].sort((a, b) => b.absent - a.absent)[0];

  const years = [now.y, now.y - 1, now.y - 2];
  const loading = report.loading || students.loading;
  const pct = (value: number, total: number) => (total > 0 ? Math.round((value / total) * 100) : 0);

  return (
    <>
      <PageHeader
        title="گزارش‌ها"
        subtitle="گزارش‌های تحلیلی حضور، غیبت و تاخیر"
        action={<button className="primary-btn compact"><Download size={18} /> خروجی Excel</button>}
      />

      <div className="filters-row panel">
        <select value={year} onChange={(e) => setYear(Number(e.target.value))}>
          {years.map((y) => <option key={y} value={y}>سال {faDigits(y)}</option>)}
        </select>
        <select value={month} onChange={(e) => setMonth(Number(e.target.value))}>
          {JALALI_MONTHS.map((name, i) => <option key={name} value={i + 1}>{name}</option>)}
        </select>
        <select value={clsKey} onChange={(e) => setClsKey(e.target.value)}>
          <option value="">همه کلاس‌ها</option>
          {CLASS_DEFS.map((c) => <option key={c.key} value={c.key}>{c.title}</option>)}
        </select>
      </div>

      <div className="stats-grid">
        <StatCard title="میانگین حضور" value={loading || rate === null ? "-" : `${faNumber(rate)}٪`} hint="در ماه انتخاب‌شده" icon={Users} tone="green" />
        <StatCard title="کل تاخیرها" value={loading ? "-" : faNumber(totalDelayed)} hint="در ماه انتخاب‌شده" icon={Clock3} tone="amber" />
        <StatCard title="کل غیبت‌ها" value={loading ? "-" : faNumber(totalAbsent)} hint="در ماه انتخاب‌شده" icon={UserRoundX} tone="red" />
        <StatCard title="روزهای ثبت‌شده" value={loading ? "-" : faNumber(days)} hint="روزهای دارای حضور و غیاب" icon={BarChart3} tone="blue" />
      </div>

      <div className="dashboard-grid">
        <section className="panel">
          <div className="panel-head"><div><h3>مقایسه کلاس‌ها</h3><p>حضور، تاخیر و غیبت در {JALALI_MONTHS[month - 1]}</p></div></div>

          {report.error ? (
            <div className="table-state error">{report.error}</div>
          ) : loading ? (
            <div className="table-state">در حال بارگذاری...</div>
          ) : !hasData ? (
            <div className="table-state">برای این ماه حضور و غیابی ثبت نشده است</div>
          ) : (
            <div className="bar-chart labeled">
              {perClass.map((c) => (
                <div className="bar-col" key={c.def.key}>
                  <div className="bar-group">
                    <div className="bar green" style={{ height: `${pct(c.present, c.slots)}%` }} title={`حاضر: ${pct(c.present, c.slots)}٪`} />
                    <div className="bar amber" style={{ height: `${pct(c.delayed, c.slots)}%` }} title={`تاخیر: ${c.delayed}`} />
                    <div className="bar red" style={{ height: `${pct(c.absent, c.slots)}%` }} title={`غیبت: ${c.absent}`} />
                  </div>
                  <span className="bar-label">{c.def.title}</span>
                </div>
              ))}
            </div>
          )}

          <div className="chart-legend chart-note">
            <span><i className="dot green" /> حاضر</span>
            <span><i className="dot amber" /> تاخیر</span>
            <span><i className="dot red" /> غایب</span>
          </div>
        </section>

        <section className="panel report-summary">
          <FileBarChart size={44} />
          <h3>خلاصه گزارش</h3>
          <p>بهترین نرخ حضور: {hasData && best ? best.def.title : "-"}</p>
          <p>بیشترین تاخیر: {hasData && mostDelays.delayed > 0 ? mostDelays.def.title : "-"}</p>
          <p>بیشترین غیبت: {hasData && mostAbsent.absent > 0 ? mostAbsent.def.title : "-"}</p>
        </section>
      </div>
    </>
  );
}
