import { Download, FileBarChart } from "lucide-react";
import PageHeader from "../components/PageHeader";
import StatCard from "../components/StatCard";
import { BarChart3, Clock3, UserRoundX, Users } from "lucide-react";

export default function ReportsPage() {
  return (
    <>
      <PageHeader
        title="گزارش‌ها"
        subtitle="گزارش‌های تحلیلی حضور، غیبت و تاخیر"
        action={<button className="primary-btn compact"><Download size={18} /> خروجی Excel</button>}
      />

      <div className="filters-row panel">
        <select><option>سال تحصیلی ۱۴۰۳ - ۱۴۰۴</option></select>
        <select><option>آبان</option><option>مهر</option></select>
        <select><option>همه کلاس‌ها</option></select>
      </div>

      <div className="stats-grid">
        <StatCard title="میانگین حضور" value="۸۷٪" hint="نسبت به ماه قبل +۳٪" icon={Users} tone="green" />
        <StatCard title="کل تاخیرها" value="۴۳" hint="در ماه جاری" icon={Clock3} tone="amber" />
        <StatCard title="کل غیبت‌ها" value="۱۲۸" hint="در ماه جاری" icon={UserRoundX} tone="red" />
        <StatCard title="تعداد گزارش‌ها" value="۱۲" hint="گزارش ذخیره‌شده" icon={BarChart3} tone="blue" />
      </div>

      <div className="dashboard-grid">
        <section className="panel">
          <div className="panel-head"><div><h3>مقایسه کلاس‌ها</h3><p>حضور، تاخیر و غیبت</p></div></div>
          <div className="bar-chart">
            {[72, 60, 81, 66, 74, 59].map((h, i) => (
              <div className="bar-group" key={i}>
                <div className="bar green" style={{height: `${h}%`}} />
                <div className="bar amber" style={{height: `${Math.max(12, 40 - i*3)}%`}} />
                <div className="bar red" style={{height: `${Math.max(8, 25 - i*2)}%`}} />
              </div>
            ))}
          </div>
        </section>

        <section className="panel report-summary">
          <FileBarChart size={44} />
          <h3>خلاصه گزارش</h3>
          <p>بهترین نرخ حضور: دهم الف</p>
          <p>بیشترین تاخیر: یازدهم ب</p>
          <p>بیشترین غیبت: دوازدهم الف</p>
        </section>
      </div>
    </>
  );
}
