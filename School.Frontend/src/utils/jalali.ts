// تاریخ شمسی بر پایه‌ی Intl مرورگر (بدون وابستگی خارجی)

export const JALALI_MONTHS = [
  "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
  "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
];

export type JalaliDate = { y: number; m: number; d: number };

const partsFormatter = new Intl.DateTimeFormat("en-US-u-ca-persian-nu-latn", {
  year: "numeric",
  month: "numeric",
  day: "numeric"
});

const displayFormatter = new Intl.DateTimeFormat("fa-IR-u-ca-persian", {
  year: "numeric",
  month: "2-digit",
  day: "2-digit"
});

const longFormatter = new Intl.DateTimeFormat("fa-IR-u-ca-persian", {
  weekday: "long",
  day: "numeric",
  month: "long",
  year: "numeric"
});

const pad = (n: number) => String(n).padStart(2, "0");

export const faDigits = (n: number) => n.toLocaleString("fa-IR", { useGrouping: false });

export function isoOf(d: Date): string {
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;
}

function dateOfIso(iso: string): Date {
  const [y, m, d] = iso.split("-").map(Number);
  return new Date(y, m - 1, d, 12); // ظهر، تا مشکل تغییر ساعت رسمی پیش نیاید
}

function toJalali(d: Date): JalaliDate {
  const parts = partsFormatter.formatToParts(d);
  const get = (type: string) => Number(parts.find((p) => p.type === type)?.value);
  return { y: get("year"), m: get("month"), d: get("day") };
}

export const jalaliToday = (): JalaliDate => toJalali(new Date());
export const isoToJalali = (iso: string): JalaliDate => toJalali(dateOfIso(iso));

/** تاریخ میلادی (yyyy-MM-dd) → نمایش شمسی مثل ۱۴۰۵/۰۷/۲۷ */
export const formatJalali = (iso: string): string => displayFormatter.format(dateOfIso(iso));

/** مثل: سه‌شنبه، ۲۷ مهر ۱۴۰۵ */
export function longJalaliToday(): string {
  const parts = longFormatter.formatToParts(new Date());
  const get = (type: string) => parts.find((p) => p.type === type)?.value ?? "";
  return `${get("weekday")}، ${get("day")} ${get("month")} ${get("year")}`;
}

const yearCache = new Map<number, string[][]>();

function buildYear(y: number): string[][] {
  const months: string[][] = Array.from({ length: 12 }, () => []);
  // سال شمسی y حدود ۲۰ مارس سال میلادی y+621 شروع می‌شود
  for (let i = 0; i < 385; i++) {
    const dt = new Date(y + 621, 2, 15 + i, 12);
    const j = toJalali(dt);
    if (j.y === y) months[j.m - 1].push(isoOf(dt));
    else if (j.y > y) break;
  }
  return months;
}

/** همه‌ی روزهای یک ماه شمسی به‌صورت تاریخ میلادی yyyy-MM-dd */
export function monthDays(y: number, m: number): string[] {
  if (!yearCache.has(y)) yearCache.set(y, buildYear(y));
  return yearCache.get(y)![m - 1] ?? [];
}

/** سال تحصیلی جاری، مثل «۱۴۰۵ - ۱۴۰۶» (سال تحصیلی از مهر شروع می‌شود) */
export function academicYearLabel(): string {
  const t = jalaliToday();
  const start = t.m >= 7 ? t.y : t.y - 1;
  return `${faDigits(start)} - ${faDigits(start + 1)}`;
}
