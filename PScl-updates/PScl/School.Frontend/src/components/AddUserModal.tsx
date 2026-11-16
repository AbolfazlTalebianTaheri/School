import { useEffect, useRef, useState } from "react";
import { CheckCircle2, Eye, EyeOff, Hash, LockKeyhole, ShieldCheck, User, UserPlus, X } from "lucide-react";
import { createUser, type StudentApi, type UserApi } from "../api/services";
import { fullName, studentClass, toLatinDigits } from "../utils/format";

type Props = {
  students: StudentApi[];
  existingUsers: UserApi[];
  onClose: () => void;
  onCreated: () => void;
};

type Errors = Partial<Record<"userName" | "password" | "student", string>>;

// مقدارهای UserRole در بک‌اند: Parent = 0, Admin = 1, Teacher = 2
const ROLE_OPTIONS = [
  { value: 1, label: "مدیر (Admin)" },
  { value: 2, label: "معلم (Teacher)" },
  { value: 0, label: "والدین (Parent)" }
];

export default function AddUserModal({ students, existingUsers, onClose, onCreated }: Props) {
  const [role, setRole] = useState(1);
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [studentCode, setStudentCode] = useState("");
  const [errors, setErrors] = useState<Errors>({});
  const [serverError, setServerError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const firstRef = useRef<HTMLInputElement>(null);

  const isParent = role === 0;
  const matchedStudent = students.find((s) => String(s.studentCode) === studentCode);

  useEffect(() => {
    firstRef.current?.focus();
  }, [isParent]);

  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === "Escape" && !saving) onClose();
    };
    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, [onClose, saving]);

  const validate = (): Errors => {
    const e: Errors = {};

    if (isParent) {
      if (!studentCode) e.student = "کد دانش‌آموزی را وارد کنید.";
      else if (studentCode.length !== 6) e.student = "کد دانش‌آموزی باید دقیقاً ۶ رقم باشد.";
      else if (!matchedStudent) e.student = "دانش‌آموزی با این کد پیدا نشد.";
      else if (!matchedStudent.nationalCode) e.student = "برای این دانش‌آموز کد ملی ثبت نشده است.";
      else if (existingUsers.some((u) => u.userRole !== undefined && u.nationalCode === matchedStudent.nationalCode))
        e.student = "برای این دانش‌آموز قبلاً حساب والد ساخته شده است.";
    } else {
      const name = userName.trim();
      if (!name) e.userName = "نام کاربری را وارد کنید.";
      else if (name.length < 3 || name.length > 50) e.userName = "نام کاربری باید بین ۳ تا ۵۰ حرف باشد.";
      else if (existingUsers.some((u) => (u.userName ?? "").toLowerCase() === name.toLowerCase()))
        e.userName = "این نام کاربری قبلاً ثبت شده است.";

      if (!password) e.password = "رمز عبور را وارد کنید.";
      else if (password.length < 8 || password.length > 50) e.password = "رمز عبور باید بین ۸ تا ۵۰ کاراکتر باشد.";
      else if (!/[A-Z]/.test(password)) e.password = "رمز عبور باید حداقل یک حرف بزرگ انگلیسی داشته باشد.";
      else if (!/[0-9]/.test(password)) e.password = "رمز عبور باید حداقل یک عدد داشته باشد.";
    }

    return e;
  };

  const submit = async () => {
    if (saving) return;
    setServerError(null);

    const found = validate();
    setErrors(found);
    if (Object.keys(found).length > 0) return;

    setSaving(true);
    try {
      await createUser(
        isParent
          ? { userRole: 0, nationalCode: matchedStudent!.nationalCode, studentId: matchedStudent!.id }
          : { userRole: role, userName: userName.trim(), password }
      );
      onCreated();
      onClose();
    } catch (err) {
      setServerError(err instanceof Error ? err.message : "ثبت کاربر ناموفق بود.");
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
        aria-labelledby="add-user-title"
        noValidate
        onSubmit={(e) => {
          e.preventDefault();
          void submit();
        }}
      >
        <div className="modal-head">
          <div className="modal-icon"><UserPlus size={22} /></div>
          <div>
            <h3 id="add-user-title">افزودن کاربر</h3>
            <p>{isParent ? "والد با کد ملی دانش‌آموز وارد می‌شود" : "ورود با نام کاربری و رمز عبور"}</p>
          </div>
          <button type="button" className="icon-btn" onClick={onClose} disabled={saving} aria-label="بستن">
            <X size={18} />
          </button>
        </div>

        <div className="modal-body">
          <label className="modal-field modal-full">
            نقش کاربر
            <div>
              <ShieldCheck size={18} />
              <select
                value={role}
                onChange={(e) => {
                  setRole(Number(e.target.value));
                  setErrors({});
                  setServerError(null);
                }}
              >
                {ROLE_OPTIONS.map((r) => <option key={r.value} value={r.value}>{r.label}</option>)}
              </select>
            </div>
          </label>

          {isParent ? (
            <>
              <label className={`modal-field${errors.student ? " invalid" : ""}`}>
                کد دانش‌آموزی
                <div>
                  <Hash size={18} />
                  <input
                    placeholder="۶ رقم، مثلاً 402315"
                    inputMode="numeric"
                    value={studentCode}
                    onChange={(e) => setStudentCode(toLatinDigits(e.target.value).replace(/\D/g, "").slice(0, 6))}
                  />
                </div>
                {errors.student && <small className="err">{errors.student}</small>}
                {!errors.student && matchedStudent && (
                  <small className="ok">
                    <CheckCircle2 size={13} /> {fullName(matchedStudent)} — {studentClass(matchedStudent)}
                  </small>
                )}
              </label>
            </>
          ) : (
            <>
              <label className={`modal-field${errors.userName ? " invalid" : ""}`}>
                نام کاربری
                <div>
                  <User size={18} />
                  <input
                    ref={firstRef}
                    placeholder="مثلاً admin"
                    value={userName}
                    maxLength={50}
                    autoComplete="off"
                    onChange={(e) => setUserName(e.target.value)}
                  />
                </div>
                {errors.userName && <small className="err">{errors.userName}</small>}
              </label>

              <label className={`modal-field${errors.password ? " invalid" : ""}`}>
                رمز عبور
                <div>
                  <LockKeyhole size={18} />
                  <input
                    type={showPassword ? "text" : "password"}
                    placeholder="حداقل ۸ کاراکتر، یک حرف بزرگ و یک عدد"
                    value={password}
                    maxLength={50}
                    autoComplete="new-password"
                    onChange={(e) => setPassword(e.target.value)}
                  />
                  <button
                    type="button"
                    className="eye-btn"
                    onClick={() => setShowPassword((v) => !v)}
                    aria-label="نمایش رمز"
                  >
                    {showPassword ? <EyeOff size={17} /> : <Eye size={17} />}
                  </button>
                </div>
                {errors.password && <small className="err">{errors.password}</small>}
              </label>
            </>
          )}

          {serverError && <div className="form-error modal-full" role="alert">{serverError}</div>}
        </div>

        <div className="modal-foot">
          <button type="submit" className="primary-btn" disabled={saving}>
            {saving ? "در حال ثبت..." : "ثبت کاربر"}
          </button>
          <button type="button" className="secondary-btn" onClick={onClose} disabled={saving}>انصراف</button>
        </div>
      </form>
    </div>
  );
}
