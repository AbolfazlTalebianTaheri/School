import { FormEvent, useState } from "react";
import { BookOpen, Eye, EyeOff, LockKeyhole, Moon, ShieldCheck, Sun, User, Users } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useTheme } from "../theme/ThemeContext";

export default function LoginPage() {
  const [role, setRole] = useState<"admin" | "parent">("admin");
  const [showPassword, setShowPassword] = useState(false);
  const [otpSent, setOtpSent] = useState(false);
  const { theme, toggleTheme } = useTheme();
  const navigate = useNavigate();

  const submit = (e: FormEvent) => {
    e.preventDefault();
    localStorage.setItem("accessToken", "demo-token");
    navigate("/dashboard");
  };

  return (
    <div className="login-screen">
      <section className="login-visual">
        <div className="login-visual-overlay" />
        <div className="login-visual-content">
          <div className="big-logo"><BookOpen size={74} /></div>
          <h1>مدرسه <span>فردا</span></h1>
          <p>سامانه ثبت حضور، غیاب و تاخیر</p>
          <div className="login-slogan">آینده روشن<br />با نظم امروز</div>
        </div>
      </section>

      <section className="login-side">
        <button className="theme-fab" onClick={toggleTheme}>
          {theme === "dark" ? <Sun /> : <Moon />}
        </button>

        <div className="login-card">
          <div className="login-brand">
            <BookOpen size={52} />
            <h2>مدرسه فردا</h2>
            <span>سامانه حضور و غیاب دانش‌آموزان</span>
          </div>

          <div className="login-head">
            <h1>ورود به حساب کاربری</h1>
            <p>برای ادامه، اطلاعات خود را وارد کنید</p>
          </div>

          <div className="role-tabs">
            <button className={role === "admin" ? "active" : ""} onClick={() => setRole("admin")}>
              <User size={18} /> مدیر
            </button>
            <button className={role === "parent" ? "active" : ""} onClick={() => setRole("parent")}>
              <Users size={18} /> والدین
            </button>
          </div>

          {role === "admin" ? (
            <form className="login-form" onSubmit={submit}>
              <label className="field">
                <span>نام کاربری</span>
                <div><User size={19} /><input placeholder="نام کاربری خود را وارد کنید" /></div>
              </label>

              <label className="field">
                <span>رمز عبور</span>
                <div>
                  <LockKeyhole size={19} />
                  <input type={showPassword ? "text" : "password"} placeholder="رمز عبور خود را وارد کنید" />
                  <button type="button" onClick={() => setShowPassword(v => !v)} className="field-action">
                    {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
              </label>

              <div className="login-options">
                <label><input type="checkbox" defaultChecked /> مرا به خاطر بسپار</label>
                <a href="#">فراموشی رمز عبور؟</a>
              </div>

              <button className="primary-btn" type="submit">ورود به سامانه</button>
            </form>
          ) : (
            <form className="login-form" onSubmit={submit}>
              <label className="field">
                <span>شماره موبایل</span>
                <div><Users size={19} /><input placeholder="09121234567" inputMode="tel" /></div>
              </label>

              {otpSent && (
                <label className="field">
                  <span>کد تایید</span>
                  <div><ShieldCheck size={19} /><input placeholder="------" inputMode="numeric" /></div>
                </label>
              )}

              {!otpSent ? (
                <button className="primary-btn" type="button" onClick={() => setOtpSent(true)}>ارسال کد تایید</button>
              ) : (
                <button className="primary-btn" type="submit">تایید و ورود</button>
              )}
            </form>
          )}
        </div>
      </section>
    </div>
  );
}
