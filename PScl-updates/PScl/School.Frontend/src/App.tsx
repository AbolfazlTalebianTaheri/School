import { useEffect, useState } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { ThemeProvider } from "./theme/ThemeContext";
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import StudentsPage from "./pages/StudentsPage";
import AttendancePage from "./pages/AttendancePage";
import DelaysPage from "./pages/DelaysPage";
import AbsencesPage from "./pages/AbsencesPage";
import ClassesPage from "./pages/ClassesPage";
import ReportsPage from "./pages/ReportsPage";
import UsersPage from "./pages/UsersPage";
import LessonsPage from "./pages/LessonsPage";
import SettingsPage from "./pages/SettingsPage";
import AdminLayout from "./layouts/AdminLayout";
import RequireAuth, { HomeRedirect, PublicOnly } from "./auth/RequireAuth";
import { ensureSession } from "./api/http";

export default function App() {
  // اگر توکن منقضی شده ولی کوکی رفرش‌توکن معتبر است، قبل از نمایش صفحه‌ها بی‌صدا وارد می‌شود
  const [ready, setReady] = useState(false);

  useEffect(() => {
    void ensureSession().finally(() => setReady(true));
  }, []);

  if (!ready) {
    return (
      <ThemeProvider>
        <div className="boot-screen">در حال بارگذاری...</div>
      </ThemeProvider>
    );
  }

  return (
    <ThemeProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<PublicOnly />}>
            <Route path="/login" element={<LoginPage />} />
          </Route>

          <Route element={<RequireAuth />}>
            <Route element={<AdminLayout />}>
              <Route element={<RequireAuth feature="dashboard" />}>
                <Route path="/dashboard" element={<DashboardPage />} />
              </Route>
              <Route element={<RequireAuth feature="students" />}>
                <Route path="/students" element={<StudentsPage />} />
              </Route>
              <Route element={<RequireAuth feature="attendance" />}>
                <Route path="/attendance" element={<AttendancePage />} />
              </Route>
              <Route element={<RequireAuth feature="delays" />}>
                <Route path="/delays" element={<DelaysPage />} />
              </Route>
              <Route element={<RequireAuth feature="absences" />}>
                <Route path="/absences" element={<AbsencesPage />} />
              </Route>
              <Route element={<RequireAuth feature="classes" />}>
                <Route path="/classes" element={<ClassesPage />} />
              </Route>
              <Route element={<RequireAuth feature="reports" />}>
                <Route path="/reports" element={<ReportsPage />} />
              </Route>
              <Route element={<RequireAuth feature="users" />}>
                <Route path="/users" element={<UsersPage />} />
              </Route>
              <Route element={<RequireAuth feature="lessons" />}>
                <Route path="/lessons" element={<LessonsPage />} />
              </Route>
              <Route element={<RequireAuth feature="settings" />}>
                <Route path="/settings" element={<SettingsPage />} />
              </Route>
            </Route>
          </Route>

          <Route path="*" element={<HomeRedirect />} />
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}
