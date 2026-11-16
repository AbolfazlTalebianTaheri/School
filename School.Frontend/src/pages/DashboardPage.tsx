import { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { BarChart3, Clock3, Search, UserRoundX, Users } from "lucide-react";
import StatCard from "../components/StatCard";
import StatusBadge from "../components/StatusBadge";
import TableState from "../components/TableState";
import PageHeader from "../components/PageHeader";
import { getStudents, getTodayAttendance, studentStatus } from "../api/services";
import { useAsync } from "../api/useAsync";
import { getSession } from "../auth/auth";
import { faNumber, fullName, studentClass, todayJalali } from "../utils/format";

export default function DashboardPage() {
  const navigate = useNavigate();
  const students = useAsync(getStudents, []);
  const today = useAsync(getTodayAttendance, []);
  const userName = getSession()?.userName;
  const [query, setQuery] = useState("");

  const rows = useMemo(
    () =>
      (students.data ?? []).map((s) => ({
        code: s.studentCode,
        name: fullName(s),
        className: studentClass(s),
        status: studentStatus(s.studentCode, today.data)
      })),
    [students.data, today.data]
  );

  const filteredRows = rows.filter(
    (r) => !query.trim() || `${r.name} ${r.code} ${r.className}`.includes(query.trim())
  );

  const total = rows.length;
  const att = today.data;
  const registered = !!att?.registered;
  const absentCount = att?.absent.size ?? 0;
  const delayCount = att?.delayed.size ?? 0;
  const presentCount = Math.max(total - absentCount - delayCount, 0);
  const rate = registered && total > 0 ? Math.round(((total - absentCount) / total) * 100) : null;
  const todayHint = registered ? "ثبت‌شده برای امروز" : "حضور و غیاب امروز ثبت نشده";

  // حلقه‌ی نمودار وضعیت امروز (اگر داده نباشد حلقه‌ی خنثی)
  const donutStyle =
    registered && total > 0
      ? {
          background: `conic-gradient(var(--green) 0 ${(presentCount / total) * 100}%, var(--amber) ${(presentCount / total) * 100}% ${((presentCount + delayCount) / total) * 100}%, var(--red) ${((presentCount + delayCount) / total) * 100}% 100%)`
        }
      : { background: "var(--border)" };

  return (
    <>
      <PageHeader
        title={`صبح بخیر${userName ? `، ${userName}` : ""} 👋`}
        subtitle="امروز هم روز خوبی برای ساختن فردای بهتر است"
      />

      <div className="stats-grid">
        <StatCard title="کل دانش‌آموزان" value={students.loading ? "..." : faNumber(total)} hint="ثبت‌شده در سامانه" icon={Users} tone="green" />
        <StatCard title="تاخیرهای امروز" value={registered ? faNumber(delayCount) : "-"} hint={todayHint} icon={Clock3} tone="amber" />
        <StatCard title="غیبت‌های امروز" value={registered ? faNumber(absentCount) : "-"} hint={todayHint} icon={UserRoundX} tone="red" />
        <StatCard title="نرخ حضور امروز" value={rate === null ? "-" : `${faNumber(rate)}٪`} hint={todayHint} icon={BarChart3} tone="blue" />
      </div>

      <div className="dashboard-grid">
        <section className="panel">
          <div className="panel-head">
            <div>
              <h3>روند حضور و غیاب ماهانه</h3>
              <p>مقایسه حضور، تاخیر و غیبت در سال تحصیلی</p>
            </div>
          </div>
          <div className="fake-chart">
            <svg viewBox="0 0 700 230" preserveAspectRatio="none">
              <polyline className="chart-line green" points="0,160 80,140 160,118 240,90 320,105 400,80 480,110 560,95 640,120 700,100" />
              <polyline className="chart-line amber" points="0,180 80,170 160,160 240,145 320,155 400,150 480,165 560,150 640,160 700,158" />
              <polyline className="chart-line red" points="0,195 80,190 160,185 240,175 320,180 400,175 480,183 560,178 640,187 700,182" />
            </svg>
            <div className="chart-legend">
              <span><i className="dot green" /> حاضر</span>
              <span><i className="dot amber" /> تاخیر</span>
              <span><i className="dot red" /> غایب</span>
            </div>
          </div>
        </section>

        <section className="panel">
          <div className="panel-head">
            <div>
              <h3>وضعیت کلی دانش‌آموزان</h3>
              <p>خلاصه امروز</p>
            </div>
          </div>
          <div className="donut-wrap">
            <div className="donut" style={donutStyle}><div><strong>{faNumber(total)}</strong><span>دانش‌آموز</span></div></div>
            <div className="donut-legend">
              <span><i className="dot green" /> حاضر <b>{registered ? faNumber(presentCount) : "-"}</b></span>
              <span><i className="dot amber" /> تاخیر <b>{registered ? faNumber(delayCount) : "-"}</b></span>
              <span><i className="dot red" /> غایب <b>{registered ? faNumber(absentCount) : "-"}</b></span>
            </div>
          </div>
        </section>
      </div>

      <section className="panel">
        <div className="panel-head">
          <div>
            <h3>لیست دانش‌آموزان</h3>
            <p>همه‌ی دانش‌آموزان ثبت‌شده و وضعیت امروز</p>
          </div>
          <button className="secondary-btn" onClick={() => navigate("/students")}>مدیریت دانش‌آموزان</button>
        </div>

        <div className="filters-row">
          <div className="search-box">
            <Search size={18} />
            <input
              placeholder="جستجوی نام، کد یا کلاس دانش‌آموز..."
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
          </div>
        </div>

        <div className="table-wrap table-scroll">
          <table>
            <thead>
              <tr>
                <th>#</th><th>نام و نام خانوادگی</th><th>کد دانش‌آموز</th><th>کلاس</th><th>وضعیت امروز</th><th>تاریخ</th>
              </tr>
            </thead>
            <tbody>
              {students.loading ? (
                <TableState cols={6} text="در حال بارگذاری..." />
              ) : students.error ? (
                <TableState cols={6} text={students.error} error />
              ) : filteredRows.length === 0 ? (
                <TableState cols={6} text={rows.length === 0 ? "هنوز دانش‌آموزی ثبت نشده است" : "دانش‌آموزی با این مشخصات یافت نشد"} />
              ) : (
                filteredRows.map((r, i) => (
                  <tr key={r.code}>
                    <td>{i + 1}</td>
                    <td className="student-cell"><div className="tiny-avatar">{r.name[0]}</div>{r.name}</td>
                    <td>{r.code}</td>
                    <td>{r.className}</td>
                    <td>{r.status ? <StatusBadge status={r.status} /> : "-"}</td>
                    <td>{r.status ? todayJalali() : "-"}</td>
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
