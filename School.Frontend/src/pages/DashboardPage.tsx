import { BarChart3, Clock3, UserRoundX, Users } from "lucide-react";
import StatCard from "../components/StatCard";
import StatusBadge from "../components/StatusBadge";
import { students } from "../data/mock";
import PageHeader from "../components/PageHeader";

export default function DashboardPage() {
  return (
    <>
      <PageHeader title="صبح بخیر، آقای محمدی 👋" subtitle="امروز هم روز خوبی برای ساختن فردای بهتر است" />

      <div className="stats-grid">
        <StatCard title="کل دانش‌آموزان" value="۳۲۶" hint="↑ ۱۲٪ نسبت به ماه قبل" icon={Users} tone="green" />
        <StatCard title="تعداد تاخیرها" value="۴۳" hint="↓ ۱۸٪ کاهش نسبت به ماه قبل" icon={Clock3} tone="amber" />
        <StatCard title="تعداد غیبت‌ها" value="۱۲۸" hint="↑ ۱۲٪ افزایش نسبت به ماه قبل" icon={UserRoundX} tone="red" />
        <StatCard title="نرخ حضور کلی" value="۸۷٪" hint="↑ ۳٪ بهتر از ماه گذشته" icon={BarChart3} tone="blue" />
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
            <div className="donut"><div><strong>۳۲۶</strong><span>دانش‌آموز</span></div></div>
            <div className="donut-legend">
              <span><i className="dot green" /> حاضر <b>۲۸۳</b></span>
              <span><i className="dot amber" /> تاخیر <b>۲۵</b></span>
              <span><i className="dot red" /> غایب <b>۱۸</b></span>
            </div>
          </div>
        </section>
      </div>

      <section className="panel">
        <div className="panel-head">
          <div>
            <h3>آخرین سوابق دانش‌آموزان</h3>
            <p>آخرین وضعیت ثبت‌شده</p>
          </div>
          <button className="secondary-btn">نمایش همه</button>
        </div>

        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th>#</th><th>نام و نام خانوادگی</th><th>کلاس</th><th>حضور</th><th>تاخیر</th><th>غیبت</th><th>وضعیت</th><th>تاریخ</th>
              </tr>
            </thead>
            <tbody>
              {students.map(s => (
                <tr key={s.id}>
                  <td>{s.id}</td>
                  <td className="student-cell"><div className="tiny-avatar">{s.name[0]}</div>{s.name}</td>
                  <td>{s.className}</td>
                  <td>{s.present}</td>
                  <td>{s.delay}</td>
                  <td>{s.absence}</td>
                  <td><StatusBadge status={s.status} /></td>
                  <td>{s.date}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>
    </>
  );
}
