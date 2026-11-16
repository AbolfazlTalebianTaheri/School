export type Role = "Admin" | "Parent" | "Teacher";

export type Session = {
  token: string;
  userId: string | null;
  userName: string;
  role: Role | null;
  /** دانش‌آموزان وصل‌شده به حساب والد (از توکن) */
  studentIds: number[];
};

export type Feature =
  | "dashboard" | "students" | "attendance" | "delays" | "absences"
  | "classes" | "reports" | "lessons" | "users" | "settings";

const ALL: Feature[] = [
  "dashboard", "students", "attendance", "delays", "absences",
  "classes", "reports", "lessons", "users", "settings"
];

// والدین فقط اطلاعات دانش‌آموز خودشان را می‌بینند؛ تنظیمات، گزارش‌ها، کاربران و کلاس‌ها بسته است
const ACCESS: Record<Role, Feature[]> = {
  Admin: ALL,
  Teacher: ["dashboard", "students", "attendance", "delays", "absences", "classes", "reports"],
  Parent: ["dashboard", "students", "delays", "absences"]
};

export const can = (role: Role | null | undefined, feature: Feature) => !!role && ACCESS[role].includes(feature);
export const canAddStudent = (role: Role | null | undefined) => role === "Admin";
export const canDeleteStudent = (role: Role | null | undefined) => role === "Admin";

const TOKEN_KEY = "accessToken";
// بک‌اند Claim نقش را با همین کلید داخل JWT می‌گذارد (ClaimTypes.Role)
const ROLE_CLAIM = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

export const roleLabels: Record<Role, string> = {
  Admin: "مدیر مدرسه",
  Parent: "والدین",
  Teacher: "معلم"
};

export function getToken(): string | null {
  return sessionStorage.getItem(TOKEN_KEY);
}

export function clearToken() {
  sessionStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(TOKEN_KEY); // توکن‌های قدیمیِ نسخه‌های قبل
}

/** access token کوتاه‌عمر است؛ ورود طولانی‌مدت با کوکی رفرش‌توکن (httpOnly) انجام می‌شود */
export function setToken(token: string) {
  clearToken();
  sessionStorage.setItem(TOKEN_KEY, token);
}

/** ثانیه‌ی باقی‌مانده تا انقضای access token (۰ اگر نامعتبر باشد) */
export function secondsUntilExpiry(): number {
  const token = getToken();
  const payload = token ? decodePayload(token) : null;
  if (!payload || typeof payload.exp !== "number") return 0;
  return Math.max(0, payload.exp - Math.floor(Date.now() / 1000));
}

function decodePayload(token: string): Record<string, unknown> | null {
  try {
    const part = token.split(".")[1];
    if (!part) return null;
    const b64 = part.replace(/-/g, "+").replace(/_/g, "/");
    const padded = b64 + "=".repeat((4 - (b64.length % 4)) % 4);
    const bytes = Uint8Array.from(atob(padded), (c) => c.charCodeAt(0));
    return JSON.parse(new TextDecoder().decode(bytes));
  } catch {
    return null;
  }
}

function toRole(value: unknown): Role | null {
  const v = Array.isArray(value) ? value[0] : value;
  return v === "Admin" || v === "Parent" || v === "Teacher" ? v : null;
}

/** نشست فعلی را از روی توکن می‌خواند؛ اگر توکن نامعتبر یا منقضی باشد null برمی‌گرداند. */
export function getSession(): Session | null {
  const token = getToken();
  if (!token) return null;

  const payload = decodePayload(token);
  if (!payload) {
    clearToken();
    return null;
  }

  const exp = typeof payload.exp === "number" ? payload.exp : null;
  if (exp !== null && exp * 1000 <= Date.now()) {
    clearToken();
    return null;
  }

  return {
    token,
    userId: typeof payload.sub === "string" ? payload.sub : null,
    userName: typeof payload.unique_name === "string" ? payload.unique_name : "",
    role: toRole(payload[ROLE_CLAIM] ?? payload.role),
    studentIds: [payload.student_id].flat().map(Number).filter((n) => n > 0)
  };
}

export function isAdmin(): boolean {
  return getSession()?.role === "Admin";
}
