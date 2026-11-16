import { useEffect, useRef, useState } from "react";
import { BookOpen, CheckCircle2, GraduationCap, X } from "lucide-react";
import { createLesson, updateLesson, type LessonApi } from "../api/services";
import { CLASS_DEFS } from "../data/classes";
import { classKeyOf } from "../utils/format";

type Props = {
  /** اگر داده شود، حالت ویرایش است */
  lesson?: LessonApi;
  existing: LessonApi[];
  defaultClassKey?: string;
  onClose: () => void;
  onSaved: () => void;
};

const normalize = (s: string) => s.replace(/\s+/g, " ").trim();

export default function LessonModal({ lesson, existing, defaultClassKey, onClose, onSaved }: Props) {
  const editing = !!lesson;
  const [name, setName] = useState(lesson?.lessonName ?? "");
  const [classKey, setClassKey] = useState(
    lesson ? classKeyOf(lesson) : defaultClassKey || CLASS_DEFS[0].key
  );
  const [error, setError] = useState<string | null>(null);
  const [serverError, setServerError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [added, setAdded] = useState<string[]>([]);
  const nameRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    nameRef.current?.focus();
  }, []);

  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === "Escape" && !saving) onClose();
    };
    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, [onClose, saving]);

  const submit = async (another: boolean) => {
    if (saving) return;
    setServerError(null);

    const clean = normalize(name);
    if (!clean) return setError("نام درس را وارد کنید.");
    if (clean.length < 2 || clean.length > 50) return setError("نام درس باید بین ۲ تا ۵۰ حرف باشد.");

    const def = CLASS_DEFS.find((c) => c.key === classKey) ?? CLASS_DEFS[0];
    const duplicate = existing.some(
      (l) =>
        l.id !== lesson?.id &&
        classKeyOf(l) === def.key &&
        normalize(l.lessonName) === clean
    );
    if (duplicate) return setError("این درس قبلاً برای این کلاس ثبت شده است.");
    setError(null);

    const payload = { lessonName: clean, grade_Level: def.grade, field_Of_Study: def.field };

    setSaving(true);
    try {
      if (lesson) await updateLesson(lesson.id, payload);
      else await createLesson(payload);

      onSaved();
      if (another && !editing) {
        setAdded((prev) => [...prev, `${clean} (${def.title})`]);
        setName("");
        nameRef.current?.focus();
      } else {
        onClose();
      }
    } catch (err) {
      setServerError(err instanceof Error ? err.message : "ثبت درس ناموفق بود.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div
      className="modal-backdrop"
      onMouseDown={(e) => {
        if (e.target === e.currentTarget && !saving) onClose();
      }}
    >
      <form
        className="modal"
        role="dialog"
        aria-modal="true"
        aria-labelledby="lesson-title"
        noValidate
        onSubmit={(e) => {
          e.preventDefault();
          void submit(false);
        }}
      >
        <div className="modal-head">
          <div className="modal-icon"><BookOpen size={22} /></div>
          <div>
            <h3 id="lesson-title">{editing ? "ویرایش درس" : "افزودن درس"}</h3>
            <p>{editing ? "اطلاعات درس را ویرایش کنید" : "درس جدید را برای یک کلاس ثبت کنید"}</p>
          </div>
          <button type="button" className="icon-btn" onClick={onClose} disabled={saving} aria-label="بستن">
            <X size={18} />
          </button>
        </div>

        <div className="modal-body">
          <label className={`modal-field modal-full${error ? " invalid" : ""}`}>
            نام درس
            <div>
              <BookOpen size={18} />
              <input
                ref={nameRef}
                placeholder="مثلاً ریاضی ۱"
                value={name}
                maxLength={50}
                onChange={(e) => setName(e.target.value)}
              />
            </div>
            {error && <small className="err">{error}</small>}
          </label>

          <label className="modal-field modal-full">
            کلاس
            <div>
              <GraduationCap size={18} />
              <select value={classKey} onChange={(e) => setClassKey(e.target.value)}>
                {CLASS_DEFS.map((c) => <option key={c.key} value={c.key}>{c.title}</option>)}
              </select>
            </div>
          </label>

          {serverError && <div className="form-error modal-full" role="alert">{serverError}</div>}

          {added.length > 0 && (
            <div className="form-success modal-full modal-added">
              <CheckCircle2 size={16} />
              <span>{added.length} درس اضافه شد: {added.join("، ")}</span>
            </div>
          )}
        </div>

        <div className="modal-foot">
          <button type="submit" className="primary-btn" disabled={saving}>
            {saving ? "در حال ثبت..." : editing ? "ذخیره تغییرات" : "ثبت درس"}
          </button>
          {!editing && (
            <button type="button" className="secondary-btn" disabled={saving} onClick={() => void submit(true)}>
              ثبت و افزودن بعدی
            </button>
          )}
        </div>
      </form>
    </div>
  );
}
