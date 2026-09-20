import { Bell, LockKeyhole, Save, ShieldCheck, SlidersHorizontal } from "lucide-react";
import PageHeader from "../components/PageHeader";

export default function SettingsPage() {
  return (
    <>
      <PageHeader title="تنظیمات" subtitle="تنظیمات عمومی سامانه" />

      <div className="settings-grid">
        <section className="panel settings-card">
          <div className="settings-title"><SlidersHorizontal /><div><h3>تنظیمات مدرسه</h3><p>اطلاعات عمومی مدرسه</p></div></div>
          <label>نام مدرسه<input defaultValue="مدرسه فردا" /></label>
          <label>سال تحصیلی<select><option>۱۴۰۳ - ۱۴۰۴</option></select></label>
          <label>ساعت شروع مدرسه<input defaultValue="07:30" /></label>
        </section>

        <section className="panel settings-card">
          <div className="settings-title"><Bell /><div><h3>اعلان‌ها</h3><p>تنظیم ارسال اعلان‌ها</p></div></div>
          <label className="toggle-row"><span>اعلان غیبت دانش‌آموز</span><input type="checkbox" defaultChecked /></label>
          <label className="toggle-row"><span>اعلان تاخیر</span><input type="checkbox" defaultChecked /></label>
          <label className="toggle-row"><span>ارسال پیامک به والدین</span><input type="checkbox" defaultChecked /></label>
        </section>

        <section className="panel settings-card">
          <div className="settings-title"><ShieldCheck /><div><h3>امنیت</h3><p>تنظیمات احراز هویت</p></div></div>
          <label className="toggle-row"><span>ورود با OTP والدین</span><input type="checkbox" defaultChecked /></label>
          <label className="toggle-row"><span>قفل حساب بعد از چند تلاش</span><input type="checkbox" defaultChecked /></label>
          <label>حداکثر تلاش ناموفق<input defaultValue="5" /></label>
        </section>

        <section className="panel settings-card">
          <div className="settings-title"><LockKeyhole /><div><h3>رمز عبور مدیر</h3><p>تغییر رمز عبور</p></div></div>
          <label>رمز فعلی<input type="password" /></label>
          <label>رمز جدید<input type="password" /></label>
          <label>تکرار رمز<input type="password" /></label>
        </section>
      </div>

      <button className="primary-btn settings-save"><Save size={18} /> ذخیره تنظیمات</button>
    </>
  );
}
