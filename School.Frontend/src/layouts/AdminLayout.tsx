import { useState } from "react";
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import {
  BarChart3, BookOpen, CalendarCheck, Clock3, GraduationCap,
  LayoutDashboard, LogOut, Menu, Moon, Settings, Sun, UserRoundX,
  Users, X, Bell, Search
} from "lucide-react";
import { useTheme } from "../theme/ThemeContext";
import { can, getSession, roleLabels, type Feature } from "../auth/auth";
import { signOut } from "../api/http";
import { longJalaliToday } from "../utils/jalali";

type NavItem = { to: string; label: string; icon: typeof Users; feature: Feature };

const navItems: NavItem[] = [
  { to: "/dashboard", label: "داشبورد", icon: LayoutDashboard, feature: "dashboard" },
  { to: "/students", label: "دانش‌آموزان", icon: Users, feature: "students" },
  { to: "/attendance", label: "ثبت حضور و غیاب", icon: CalendarCheck, feature: "attendance" },
  { to: "/delays", label: "تاخیرها", icon: Clock3, feature: "delays" },
  { to: "/absences", label: "غیبت‌ها", icon: UserRoundX, feature: "absences" },
  { to: "/classes", label: "کلاس‌ها", icon: GraduationCap, feature: "classes" },
  { to: "/lessons", label: "درس‌ها", icon: BookOpen, feature: "lessons" },
  { to: "/reports", label: "گزارش‌ها", icon: BarChart3, feature: "reports" },
  { to: "/users", label: "مدیریت کاربران", icon: Users, feature: "users" },
  { to: "/settings", label: "تنظیمات", icon: Settings, feature: "settings" }
];

export default function AdminLayout() {
  const [mobileOpen, setMobileOpen] = useState(false);
  const { theme, toggleTheme } = useTheme();
  const navigate = useNavigate();
  const [search, setSearch] = useState("");
  const session = getSession();
  const visibleNav = navItems.filter((item) => can(session?.role, item.feature));
  const displayName = session?.userName || "کاربر";
  const roleLabel = session?.role ? roleLabels[session.role] : "";

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
          {visibleNav.map(({ to, label, icon: Icon }) => (
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
            void signOut().finally(() => navigate("/login", { replace: true }));
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
              <span>{longJalaliToday()}</span>
            </div>
          </div>

          <div className="topbar-center">
            <Search size={18} />
            <input
              placeholder="جستجوی دانش‌آموز..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Enter" && search.trim()) {
                  navigate(`/students?q=${encodeURIComponent(search.trim())}`);
                  setSearch("");
                }
              }}
            />
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
              <div className="avatar">{displayName[0]}</div>
              <div>
                <strong>{displayName}</strong>
                <small>{roleLabel}</small>
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
