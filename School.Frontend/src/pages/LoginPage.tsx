import { FormEvent, useState } from "react";
import { BookOpen, Eye, EyeOff, Hash, LockKeyhole, Moon, Sun, User, UserCog, Users } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useTheme } from "../theme/ThemeContext";
import { login, ROLE_ADMIN, ROLE_PARENT, ROLE_TEACHER, type LoginPayload } from "../api/services";
import { setToken } from "../auth/auth";
import { toLatinDigits } from "../utils/format";

type LoginRole = "admin" | "teacher" | "parent";

const ROLE_CODE: Record<LoginRole, number> = {
  admin: ROLE_ADMIN,
  teacher: ROLE_TEACHER,
  parent: ROLE_PARENT
};

export default function LoginPage() {
  const [role, setRole] = useState<LoginRole>("admin");
  const [showPassword, setShowPassword] = useState(false);
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [nationalCode, setNationalCode] = useState("");
  const [remember, setRemember] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { theme, toggleTheme } = useTheme();
  const navigate = useNavigate();

  const changeRole = (next: LoginRole) => {
    setRole(next);
    setError(null);
  };

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);

    let payload: LoginPayload;
    if (role === "parent") {
      if (nationalCode.length !== 10) {
        setError("کد ملی دانش‌آموز باید دقیقاً ۱۰ رقم باشد.");
        return;
      }
      payload = { userRole: ROLE_PARENT, nationalCode, rememberMe: remember };
    } else {
      if (!userName.trim() || !password) {
        setError("نام کاربری و رمز عبور را وارد کنید.");
        return;
      }
      payload = { userRole: ROLE_CODE[role], userName: userName.trim(), password, rememberMe: remember };
    }

    setLoading(true);
    try {
      const result = await login(payload);
      setToken(result.accessToken);
      navigate("/dashboard", { replace: true });
    } catch (err) {
      setError(err instanceof Error ? err.message : "ورود ناموفق بود.");
    } finally {
      setLoading(false);
    }
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
            <button type="button" className={role === "admin" ? "active" : ""} onClick={() => changeRole("admin")}>
              <User size={18} /> مدیر
            </button>
            <button type="button" className={role === "teacher" ? "active" : ""} onClick={() => changeRole("teacher")}>
              <UserCog size={18} /> معلم
            </button>
            <button type="button" className={role === "parent" ? "active" : ""} onClick={() => changeRole("parent")}>
              <Users size={18} /> والدین
            </button>
          </div>

          <form className="login-form" onSubmit={submit}>
            {role === "parent" ? (
              <label className="field">
                <span>کد ملی دانش‌آموز</span>
                <div>
                  <Hash size={19} />
                  <input
                    placeholder="کد ملی ۱۰ رقمی"
                    inputMode="numeric"
                    value={nationalCode}
                    onChange={(e) => setNationalCode(toLatinDigits(e.target.value).replace(/\D/g, "").slice(0, 10))}
                    autoComplete="off"
                  />
                </div>
              </label>
            ) : (
              <>
                <label className="field">
                  <span>نام کاربری</span>
                  <div>
                    <User size={19} />
                    <input
                      placeholder="نام کاربری خود را وارد کنید"
                      value={userName}
                      onChange={(e) => setUserName(e.target.value)}
                      autoComplete="username"
                    />
                  </div>
                </label>

                <label className="field">
                  <span>رمز عبور</span>
                  <div>
                    <LockKeyhole size={19} />
                    <input
                      type={showPassword ? "text" : "password"}
                      placeholder="رمز عبور خود را وارد کنید"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      autoComplete="current-password"
                    />
                    <button type="button" onClick={() => setShowPassword((v) => !v)} className="field-action">
                      {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                    </button>
                  </div>
                </label>
              </>
            )}

            <div className="login-options">
              <label>
                <input type="checkbox" checked={remember} onChange={(e) => setRemember(e.target.checked)} /> مرا به خاطر بسپار
              </label>
              {role !== "parent" && <a href="#">فراموشی رمز عبور؟</a>}
            </div>

            {error && <div className="form-error" role="alert">{error}</div>}

            <button className="primary-btn" type="submit" disabled={loading}>
              {loading ? "در حال ورود..." : "ورود به سامانه"}
            </button>
          </form>
        </div>
      </section>
    </div>
  );
}
