import { useMemo, useState } from "react";
import { Save } from "lucide-react";
import PageHeader from "../components/PageHeader";
import JalaliDatePicker from "../components/JalaliDatePicker";
import {
  getDayAttendance,
  getLessons,
  getStudents,
  saveAttendance,
  studentStatus,
  type AttendanceItem
} from "../api/services";
import { useAsync } from "../api/useAsync";
import { CLASS_DEFS } from "../data/classes";
import { BELLS, classKeyOf, fullName, studentClass, todayIso, toLatinDigits } from "../utils/format";

type DelayDraft = { bell: string; lessonId: string; minutes: string };
type Edit = { status: "حاضر" | "غایب" } | ({ status: "تاخیر" } & DelayDraft);

const EMPTY_DRAFT: DelayDraft = { bell: "", lessonId: "", minutes: "" };

export default function AttendancePage() {
  const [version, setVersion] = useState(0);
  const students = useAsync(getStudents, []);
  const lessons = useAsync(getLessons, []);
  const [date, setDate] = useState(todayIso());
  const today = useAsync(() => getDayAttendance(date), [version, date]);
  const [classKey, setClassKey] = useState(CLASS_DEFS[0].key);
  const [edits, setEdits] = useState<Record<number, Edit>>({}); // کلید: id دانش‌آموز
  const [saving, setSaving] = useState(false);
  const [message, setMessage] = useState<{ text: string; error: boolean } | null>(null);

  const rows = useMemo(
    () =>
      (students.data ?? []).map((s) => ({
        id: s.id,
        code: s.studentCode,
        name: fullName(s),
        className: studentClass(s),
        classKey: classKeyOf(s),
        grade: Number(s.grade_Level),
        field: Number(s.field_Of_Study),
        saved: studentStatus(s.studentCode, today.data)
      })),
    [students.data, today.data]
  );

  type Row = (typeof rows)[number];

  const visible = rows.filter((r) => r.classKey === classKey);
  const changedCount = Object.keys(edits).length;

  const lessonsFor = (row: Row) =>
    (lessons.data ?? []).filter((l) => Number(l.grade_Level) === row.grade && Number(l.field_Of_Study) === row.field);

  /** مقدار فیلدهای تاخیر: از ویرایش فعلی، وگرنه از تاخیر ذخیره‌شده */
  const draftOf = (row: Row): DelayDraft => {
    const e = edits[row.id];
    if (e && e.status === "تاخیر") return e;
    const saved = today.data?.delays.get(row.code);
    return saved
      ? { bell: String(saved.bell), lessonId: String(saved.lessonId), minutes: String(saved.minutes) }
      : EMPTY_DRAFT;
  };

  const setDraft = (row: Row, patch: Partial<DelayDraft>) => {
    setMessage(null);
    const base = draftOf(row);
    setEdits((prev) => ({
      ...prev,
      [row.id]: { status: "تاخیر", bell: base.bell, lessonId: base.lessonId, minutes: base.minutes, ...patch }
    }));
  };

  const choose = (row: Row, choice: "حاضر" | "غایب") => {
    setMessage(null);
    setEdits((prev) => {
      const next = { ...prev };
      if (row.saved === choice) delete next[row.id]; // برگشت به وضعیت ذخیره‌شده = بدون تغییر
      else next[row.id] = { status: choice };
      return next;
    });
  };

  const chooseDelay = (row: Row) => {
    setMessage(null);
    setEdits((prev) => {
      const next = { ...prev };
      if (row.saved === "تاخیر") delete next[row.id]; // برگشت به تاخیر ذخیره‌شده
      else if (!(next[row.id]?.status === "تاخیر")) next[row.id] = { status: "تاخیر", ...EMPTY_DRAFT };
      return next;
    });
  };

  const save = async () => {
    if (changedCount === 0 || saving) return;

    const items: AttendanceItem[] = [];
    for (const [idText, edit] of Object.entries(edits)) {
      const studentId = Number(idText);
      if (edit.status === "تاخیر") {
        const minutes = Number(edit.minutes);
        if (!edit.bell || !edit.lessonId || !(minutes >= 1 && minutes <= 240)) {
          const name = rows.find((r) => r.id === studentId)?.name ?? "";
          setMessage({
            text: `برای «${name}» زنگ، درس و دقیقه‌ی تاخیر (بین ۱ تا ۲۴۰) را کامل کنید.`,
            error: true
          });
          return;
        }
        items.push({
          studentId,
          isAbsent: false,
          delay: { classBell: Number(edit.bell), lessonId: Number(edit.lessonId), delayMinutes: minutes }
        });
      } else {
        items.push({ studentId, isAbsent: edit.status === "غایب" });
      }
    }

    setSaving(true);
    setMessage(null);
    try {
      await saveAttendance(date, items);
      setEdits({});
      setVersion((v) => v + 1);
      setMessage({ text: "حضور و غیاب با موفقیت ذخیره شد.", error: false });
    } catch (err) {
      setMessage({ text: err instanceof Error ? err.message : "ذخیره ناموفق بود.", error: true });
    } finally {
      setSaving(false);
    }
  };

  return (
    <>
      <PageHeader title="ثبت حضور و غیاب" subtitle="وضعیت دانش‌آموزان را برای تاریخ انتخاب‌شده ثبت کنید" />

      <section className="panel">
        <div className="filters-row">
          <select value={classKey} onChange={(e) => setClassKey(e.target.value)}>
            {CLASS_DEFS.map((c) => <option key={c.key} value={c.key}>{c.title}</option>)}
          </select>
          <JalaliDatePicker
            value={date}
            max={todayIso()}
            onChange={(iso) => {
              setDate(iso);
              setEdits({});
              setMessage(null);
            }}
          />
          <button className="primary-btn compact" onClick={save} disabled={saving || changedCount === 0}>
            <Save size={18} /> {saving ? "در حال ذخیره..." : "ذخیره تغییرات"}
          </button>
        </div>

        {message && (
          <div style={{ padding: "0 14px 14px" }}>
            <div className={message.error ? "form-error" : "form-success"} role="alert">{message.text}</div>
          </div>
        )}

        <div className="attendance-list">
          {students.loading ? (
            <div className="table-state">در حال بارگذاری...</div>
          ) : students.error ? (
            <div className="table-state error">{students.error}</div>
          ) : visible.length === 0 ? (
            <div className="table-state">دانش‌آموزی در این کلاس ثبت نشده است</div>
          ) : (
            visible.map((s) => {
              const shown: string | null = edits[s.id]?.status ?? s.saved;
              const draft = draftOf(s);
              const classLessons = lessonsFor(s);

              return (
                <div className="attendance-row" key={s.id}>
                  <div className="student-cell">
                    <div className="tiny-avatar">{s.name[0]}</div>
                    <div><strong>{s.name}</strong><small>{s.className}</small></div>
                  </div>
                  <div className="segmented">
                    <button className={`seg ok${shown === "حاضر" ? " active" : ""}`} onClick={() => choose(s, "حاضر")}>حاضر</button>
                    <button className={`seg warn${shown === "تاخیر" ? " active" : ""}`} onClick={() => chooseDelay(s)}>تاخیر</button>
                    <button className={`seg bad${shown === "غایب" ? " active" : ""}`} onClick={() => choose(s, "غایب")}>غایب</button>
                  </div>
                  <input
                    className="time-input"
                    inputMode="numeric"
                    placeholder={shown === "تاخیر" ? "دقیقه" : "-"}
                    disabled={shown !== "تاخیر"}
                    value={shown === "تاخیر" ? draft.minutes : ""}
                    onChange={(e) => setDraft(s, { minutes: toLatinDigits(e.target.value).replace(/\D/g, "").slice(0, 3) })}
                  />

                  {shown === "تاخیر" && (
                    <div className="delay-fields">
                      <label className="delay-field">
                        زنگ
                        <select value={draft.bell} onChange={(e) => setDraft(s, { bell: e.target.value })}>
                          <option value="">انتخاب زنگ</option>
                          {BELLS.map((b) => <option key={b.value} value={b.value}>{b.label}</option>)}
                        </select>
                      </label>
                      <label className="delay-field">
                        درس
                        <select value={draft.lessonId} onChange={(e) => setDraft(s, { lessonId: e.target.value })}>
                          <option value="">{classLessons.length ? "انتخاب درس" : "درسی برای این کلاس ثبت نشده"}</option>
                          {classLessons.map((l) => <option key={l.id} value={l.id}>{l.lessonName}</option>)}
                        </select>
                      </label>
                      <label className="delay-field">
                        دقیقه تاخیر
                        <input
                          inputMode="numeric"
                          placeholder="مثلاً 10"
                          value={draft.minutes}
                          onChange={(e) =>
                            setDraft(s, { minutes: toLatinDigits(e.target.value).replace(/\D/g, "").slice(0, 3) })
                          }
                        />
                      </label>
                    </div>
                  )}
                </div>
              );
            })
          )}
        </div>
      </section>
    </>
  );
}
