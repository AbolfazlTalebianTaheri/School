import { faDigits, isoToJalali, jalaliToday, JALALI_MONTHS, monthDays } from "../utils/jalali";

type Props = {
  value: string; // yyyy-MM-dd (میلادی)
  onChange: (iso: string) => void;
  max?: string;
};

/** انتخاب تاریخ شمسی با سه لیست (روز، ماه، سال) هماهنگ با فیلترهای برنامه */
export default function JalaliDatePicker({ value, onChange, max }: Props) {
  const cur = isoToJalali(value);
  const today = jalaliToday();
  const years = Array.from(new Set([today.y, today.y - 1, today.y - 2, cur.y])).sort((a, b) => b - a);
  const dayCount = monthDays(cur.y, cur.m).length;

  const change = (y: number, m: number, d: number) => {
    const list = monthDays(y, m);
    if (list.length === 0) return;
    let iso = list[Math.min(d, list.length) - 1];
    if (max && iso > max) iso = max;
    onChange(iso);
  };

  return (
    <div className="date-picker">
      <select value={cur.d} onChange={(e) => change(cur.y, cur.m, Number(e.target.value))} aria-label="روز">
        {Array.from({ length: dayCount }, (_, i) => (
          <option key={i + 1} value={i + 1}>{faDigits(i + 1)}</option>
        ))}
      </select>
      <select value={cur.m} onChange={(e) => change(cur.y, Number(e.target.value), cur.d)} aria-label="ماه">
        {JALALI_MONTHS.map((name, i) => <option key={name} value={i + 1}>{name}</option>)}
      </select>
      <select value={cur.y} onChange={(e) => change(Number(e.target.value), cur.m, cur.d)} aria-label="سال">
        {years.map((y) => <option key={y} value={y}>{faDigits(y)}</option>)}
      </select>
    </div>
  );
}
