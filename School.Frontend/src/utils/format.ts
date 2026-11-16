// مقادیر Enum بک‌اند (به‌صورت عدد در JSON می‌آیند)
const GRADES: Record<number, string> = { 1: "دهم", 2: "یازدهم", 3: "دوازدهم" };
const FIELDS: Record<number, string> = { 1: "شبکه و نرم‌افزار", 2: "حسابداری" };
const ROLES: Record<number, string> = { 0: "Parent", 1: "Admin", 2: "Teacher" };

type EnumValue = number | string | null | undefined;

function label(map: Record<number, string>, value: EnumValue): string {
  if (value === null || value === undefined) return "";
  const n = Number(value);
  if (!Number.isNaN(n)) return map[n] ?? String(value);
  return String(value); // اگر سرور خودش متن فرستاد
}

export const gradeLabel = (v: EnumValue) => label(GRADES, v);
export const fieldLabel = (v: EnumValue) => label(FIELDS, v);

/** کلید کلاس (پایه-رشته) مثل "1-2" برای فیلتر و گروه‌بندی */
export function classKeyOf(s: { grade_Level?: EnumValue; field_Of_Study?: EnumValue }): string {
  return `${Number(s.grade_Level)}-${Number(s.field_Of_Study)}`;
}

export const BELLS = [
  { value: 1, label: "زنگ اول" },
  { value: 2, label: "زنگ دوم" },
  { value: 3, label: "زنگ سوم" },
  { value: 4, label: "زنگ چهارم" }
];

/** ارقام فارسی/عربی را به لاتین تبدیل می‌کند (برای فیلدهای عددی) */
export function toLatinDigits(value: string): string {
  return value
    .replace(/[۰-۹]/g, (d) => String(d.charCodeAt(0) - 0x06f0))
    .replace(/[٠-٩]/g, (d) => String(d.charCodeAt(0) - 0x0660));
}

export const faNumber = (n: number) => n.toLocaleString("fa-IR");

export function roleName(value: EnumValue): string {
  return label(ROLES, value) || "-";
}

export function studentClass(s: { grade_Level?: EnumValue; field_Of_Study?: EnumValue }): string {
  return [label(GRADES, s.grade_Level), label(FIELDS, s.field_Of_Study)].filter(Boolean).join(" ") || "-";
}

export function fullName(s: { firstName?: string | null; lastName?: string | null }): string {
  return `${s.firstName ?? ""} ${s.lastName ?? ""}`.trim() || "-";
}

/** تاریخ امروز به‌صورت yyyy-MM-dd (فرمتی که API برای DateOnly می‌خواهد) */
export function todayIso(): string {
  const d = new Date();
  const p = (n: number) => String(n).padStart(2, "0");
  return `${d.getFullYear()}-${p(d.getMonth() + 1)}-${p(d.getDate())}`;
}

const jalaliFormatter = new Intl.DateTimeFormat("fa-IR-u-ca-persian", {
  year: "numeric",
  month: "2-digit",
  day: "2-digit"
});

export function todayJalali(): string {
  return jalaliFormatter.format(new Date());
}
