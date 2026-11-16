import { Navigate, Outlet } from "react-router-dom";
import { can, getSession, type Feature } from "./auth";

/**
 * - بدون ورود → هدایت به /login
 * - اگر feature داده شود و نقش کاربر به آن دسترسی نداشته باشد → هدایت به /dashboard
 */
export default function RequireAuth({ feature }: { feature?: Feature }) {
  const session = getSession();

  if (!session) return <Navigate to="/login" replace />;
  if (feature && !can(session.role, feature)) return <Navigate to="/dashboard" replace />;

  return <Outlet />;
}

/** صفحه‌هایی مثل /login که با توکن معتبر قابل دیدن نیستند */
export function PublicOnly() {
  return getSession() ? <Navigate to="/dashboard" replace /> : <Outlet />;
}

/** آدرس / و آدرس‌های ناشناس: با توکن معتبر مستقیم داخل برنامه، وگرنه صفحه ورود */
export function HomeRedirect() {
  return <Navigate to={getSession() ? "/dashboard" : "/login"} replace />;
}
