import { useState } from "react";
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import {
  BarChart3, BookOpen, CalendarCheck, Clock3, GraduationCap,
  LayoutDashboard, LogOut, Menu, Moon, Settings, Sun, UserRoundX,
  Users, X, Bell, Search
} from "lucide-react";
import { useTheme } from "../theme/ThemeContext";

const navItems = [
  { to: "/dashboard", label: "داشبورد", icon: LayoutDashboard },
  { to: "/students", label: "دانش‌آموزان", icon: Users },
  { to: "/attendance", label: "ثبت حضور و غیاب", icon: CalendarCheck },
  { to: "/delays", label: "تاخیرها", icon: Clock3 },
  { to: "/absences", label: "غیبت‌ها", icon: UserRoundX },
  { to: "/classes", label: "کلاس‌ها", icon: GraduationCap },
  { to: "/reports", label: "گزارش‌ها", icon: BarChart3 },
  { to: "/users", label: "مدیریت کاربران", icon: Users },
  { to: "/settings", label: "تنظیمات", icon: Settings }
];

export default function AdminLayout() {
  const [mobileOpen, setMobileOpen] = useState(false);
  const { theme, toggleTheme } = useTheme();
  const navigate = useNavigate();

  const close = () => setMobileOpen(false);

  return (
    <div className="shell">
      {mobileOpen && <button className="backdrop" onClick={close} aria-label="بستن منو" />}

      <aside className={`sidebar ${mobileOpen ? "open" : ""}`}>
        <div className="brand">
          <div className="brand-mark"><BookOpen size={32} /></div>
          <div>
            <h2>مدرسه فردا</h2>
            <span>سامانه مدیریت مدرسه</span>
          </div>
          <button className="icon-btn sidebar-close" onClick={close}><X size={20} /></button>
        </div>

        <nav className="nav">
          {navItems.map(({ to, label, icon: Icon }) => (
            <NavLink
              key={to}
              to={to}
              onClick={close}
              className={({ isActive }) => `nav-link ${isActive ? "active" : ""}`}
            >
              <Icon size={20} />
              <span>{label}</span>
            </NavLink>
          ))}
        </nav>

        <div className="sidebar-quote">
          <GraduationCap size={34} />
          <strong>دانش، آغاز آینده‌ای بهتر است</strong>
        </div>

        <button
          className="logout"
          onClick={() => {
            localStorage.removeItem("accessToken");
            navigate("/login");
          }}
        >
          <LogOut size={20} />
          خروج از سامانه
        </button>
      </aside>

      <div className="app-main">
        <header className="topbar">
          <div className="topbar-right">
            <button className="icon-btn mobile-menu" onClick={() => setMobileOpen(true)}>
              <Menu size={22} />
            </button>
            <div className="date-box">
              <CalendarCheck size={18} />
              <span>سه‌شنبه، ۱۵ آبان ۱۴۰۳</span>
            </div>
          </div>

          <div className="topbar-center">
            <Search size={18} />
            <input placeholder="جستجوی دانش‌آموز، کلاس، گزارش..." />
          </div>

          <div className="topbar-left">
            <button className="icon-btn" onClick={toggleTheme} title="تغییر تم">
              {theme === "dark" ? <Sun size={20} /> : <Moon size={20} />}
            </button>

            <button className="icon-btn notification">
              <Bell size={20} />
              <span>3</span>
            </button>

            <div className="profile">
              <div className="avatar">ع</div>
              <div>
                <strong>علی محمدی</strong>
                <small>مدیر مدرسه</small>
              </div>
            </div>
          </div>
        </header>

        <main className="page-wrap">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
