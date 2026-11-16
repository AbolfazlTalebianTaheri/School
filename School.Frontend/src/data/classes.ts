import { fieldLabel, gradeLabel } from "../utils/format";

/**
 * کلاس‌های پیش‌فرض مدرسه (پایه × رشته).
 * وقتی Entity کلاس در بک‌اند ساخته شد، این لیست باید از API خوانده شود.
 */
export type ClassDef = {
  key: string; // "پایه-رشته" مثل "1-2"
  grade: number;
  field: number;
  title: string;
  gradeLabel: string;
  fieldLabel: string;
  color: "blue" | "green" | "purple" | "amber" | "red";
};

const COLORS: ClassDef["color"][] = ["blue", "green", "purple", "amber", "red", "blue"];

export const CLASS_DEFS: ClassDef[] = [
  [1, 1], // دهم شبکه و نرم‌افزار
  [1, 2], // دهم حسابداری
  [2, 1], // یازدهم شبکه و نرم‌افزار
  [2, 2], // یازدهم حسابداری
  [3, 1], // دوازدهم شبکه و نرم‌افزار
  [3, 2] // دوازدهم حسابداری
].map(([grade, field], i) => ({
  key: `${grade}-${field}`,
  grade,
  field,
  gradeLabel: gradeLabel(grade),
  fieldLabel: fieldLabel(field),
  title: `${gradeLabel(grade)} ${fieldLabel(field)}`,
  color: COLORS[i]
}));
