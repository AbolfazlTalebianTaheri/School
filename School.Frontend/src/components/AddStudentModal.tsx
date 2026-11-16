import { useEffect, useRef, useState } from "react";
import { CheckCircle2, GraduationCap, Hash, User, UserPlus, X } from "lucide-react";
import { createStudent } from "../api/services";
import { CLASS_DEFS } from "../data/classes";
import { toLatinDigits } from "../utils/format";

type Props = {
  existingCodes: Set<number>;
  existingNationalCodes: Set<string>;
  onClose: () => void;
  onCreated: () => void;
};

type Errors = Partial<Record<"firstName" | "lastName" | "code" | "national", string>>;

export default function AddStudentModal({ existingCodes, existingNationalCodes, onClose, onCreated }: Props) {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [code, setCode] = useState("");
  const [national, setNational] = useState("");
  const [classKey, setClassKey] = useState(CLASS_DEFS[0].key);
  const [errors, setErrors] = useState<Errors>({});
  const [serverError, setServerError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [added, setAdded] = useState<string[]>([]);

  const addedCodes = useRef(new Set<number>());
  const addedNational = useRef(new Set<string>());
  const firstRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    firstRef.current?.focus();
  }, []);

  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === "Escape" && !saving) onClose();
    };
    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, [onClose, saving]);

  const validate = (): Errors => {
    const e: Errors = {};
    const f = firstName.trim();
    const l = lastName.trim();

    if (!f) e.firstName = "نام را وارد کنید.";
    else if (f.length < 3 || f.length > 20) e.firstName = "نام باید بین ۳ تا ۲۰ حرف باشد.";

    if (!l) e.lastName = "نام خانوادگی را وارد کنید.";
    else if (l.length < 3 || l.length > 20) e.lastName = "نام خانوادگی باید بین ۳ تا ۲۰ حرف باشد.";

    const n = Number(code);
    if (!code) e.code = "کد دانش‌آموزی را وارد کنید.";
    else if (code.length !== 6) e.code = "کد دانش‌آموزی باید دقیقاً ۶ رقم باشد.";
    else if (code.startsWith("0")) e.code = "کد دانش‌آموزی نباید با صفر شروع شود.";
    else if (existingCodes.has(n) || addedCodes.current.has(n)) e.code = "این کد دانش‌آموزی قبلاً ثبت شده است.";

    if (!national) e.national = "کد ملی را وارد کنید.";
    else if (national.length !== 10) e.national = "کد ملی باید دقیقاً ۱۰ رقم باشد.";
    else if (existingNationalCodes.has(national) || addedNational.current.has(national))
      e.national = "این کد ملی قبلاً برای دانش‌آموز دیگری ثبت شده است.";

    return e;
  };

  const submit = async (another: boolean) => {
    if (saving) return;
    setServerError(null);

    const found = validate();
    setErrors(found);
    if (Object.keys(found).length > 0) return;

    const def = CLASS_DEFS.find((c) => c.key === classKey) ?? CLASS_DEFS[0];
    const fullName = `${firstName.trim()} ${lastName.trim()}`;

    setSaving(true);
    try {
      await createStudent({
        firstName: firstName.trim(),
        lastName: lastName.trim(),
        studentCode: Number(code),
        nationalCode: national,
        grade_Level: def.grade,
        field_Of_Study: def.field
      });

      addedCodes.current.add(Number(code));
      addedNational.current.add(national);
      onCreated();

      if (another) {
        setAdded((prev) => [...prev, fullName]);
        setFirstName("");
        setLastName("");
        setCode("");
        setNational("");
        setErrors({});
        firstRef.current?.focus();
      } else {
        onClose();
      }
    } catch (err) {
      setServerError(err instanceof Error ? err.message : "ثبت دانش‌آموز ناموفق بود.");
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
        aria-labelledby="add-student-title"
        noValidate
        onSubmit={(e) => {
          e.preventDefault();
          void submit(false);
        }}
      >
        <div className="modal-head">
          <div className="modal-icon"><UserPlus size={22} /></div>
          <div>
            <h3 id="add-student-title">افزودن دانش‌آموز</h3>
            <p>اطلاعات دانش‌آموز جدید را وارد کنید</p>
          </div>
          <button type="button" className="icon-btn" onClick={onClose} disabled={saving} aria-label="بستن">
            <X size={18} />
          </button>
        </div>

        <div className="modal-body">
          <label className={`modal-field${errors.firstName ? " invalid" : ""}`}>
            نام
            <div>
              <User size={18} />
              <input
                ref={firstRef}
                placeholder="مثلاً علی"
                value={firstName}
                maxLength={20}
                onChange={(e) => setFirstName(e.target.value)}
              />
            </div>
            {errors.firstName && <small className="err">{errors.firstName}</small>}
          </label>

          <label className={`modal-field${errors.lastName ? " invalid" : ""}`}>
            نام خانوادگی
            <div>
              <User size={18} />
              <input
                placeholder="مثلاً رضایی"
                value={lastName}
                maxLength={20}
                onChange={(e) => setLastName(e.target.value)}
              />
            </div>
            {errors.lastName && <small className="err">{errors.lastName}</small>}
          </label>

          <label className={`modal-field${errors.code ? " invalid" : ""}`}>
            کد دانش‌آموزی
            <div>
              <Hash size={18} />
              <input
                placeholder="۶ رقم، مثلاً 402315"
                inputMode="numeric"
                value={code}
                onChange={(e) => setCode(toLatinDigits(e.target.value).replace(/\D/g, "").slice(0, 6))}
              />
            </div>
            {errors.code && <small className="err">{errors.code}</small>}
          </label>

          <label className={`modal-field${errors.national ? " invalid" : ""}`}>
            کد ملی
            <div>
              <Hash size={18} />
              <input
                placeholder="کد ملی ۱۰ رقمی"
                inputMode="numeric"
                value={national}
                onChange={(e) => setNational(toLatinDigits(e.target.value).replace(/\D/g, "").slice(0, 10))}
              />
            </div>
            {errors.national && <small className="err">{errors.national}</small>}
          </label>

          <label className="modal-field">
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
              <span>{added.length} دانش‌آموز اضافه شد: {added.join("، ")}</span>
            </div>
          )}
        </div>

        <div className="modal-foot">
          <button type="submit" className="primary-btn" disabled={saving}>
            {saving ? "در حال ثبت..." : "ثبت دانش‌آموز"}
          </button>
          <button type="button" className="secondary-btn" disabled={saving} onClick={() => void submit(true)}>
            ثبت و افزودن بعدی
          </button>
        </div>
      </form>
    </div>
  );
}
