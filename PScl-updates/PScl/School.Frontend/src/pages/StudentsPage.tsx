import { useMemo, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { Plus, Search, Trash2 } from "lucide-react";
import PageHeader from "../components/PageHeader";
import StatusBadge from "../components/StatusBadge";
import TableState from "../components/TableState";
import AddStudentModal from "../components/AddStudentModal";
import ConfirmModal from "../components/ConfirmModal";
import { CLASS_DEFS } from "../data/classes";
import { deleteStudent, getStudents, getTodayAttendance, studentStatus } from "../api/services";
import { useAsync } from "../api/useAsync";
import { canAddStudent, canDeleteStudent, getSession } from "../auth/auth";
import { classKeyOf, fullName, studentClass, todayJalali } from "../utils/format";

type Row = {
  id: number;
  code: number;
  name: string;
  className: string;
  classKey: string;
  status: ReturnType<typeof studentStatus>;
};

export default function StudentsPage() {
  const role = getSession()?.role;
  const canAdd = canAddStudent(role);
  const canDelete = canDeleteStudent(role);
  const cols = canDelete ? 7 : 6;

  const [version, setVersion] = useState(0);
  const students = useAsync(getStudents, [version]);
  const today = useAsync(getTodayAttendance, [version]); // اگر خطا بدهد فقط وضعیت‌ها "-" می‌شوند

  const [params] = useSearchParams();
  const [query, setQuery] = useState(params.get("q") ?? "");
  const [cls, setCls] = useState(params.get("class") ?? "");
  const [status, setStatus] = useState("");
  const [adding, setAdding] = useState(false);
  const [removing, setRemoving] = useState<Row | null>(null);
  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState<string | null>(null);

  const rows: Row[] = useMemo(
    () =>
      (students.data ?? []).map((s) => ({
        id: s.id,
        code: s.studentCode,
        name: fullName(s),
        className: studentClass(s),
        classKey: classKeyOf(s),
        status: studentStatus(s.studentCode, today.data)
      })),
    [students.data, today.data]
  );

  const existingCodes = useMemo(() => new Set(rows.map((r) => r.code)), [rows]);
  const existingNationalCodes = useMemo(
    () => new Set((students.data ?? []).map((s) => s.nationalCode ?? "").filter(Boolean)),
    [students.data]
  );

  const filtered = rows.filter(
    (r) =>
      (!cls || r.classKey === cls) &&
      (!status || r.status === status) &&
      (!query.trim() || `${r.name} ${r.code} ${r.className}`.includes(query.trim()))
  );

  const confirmDelete = async () => {
    if (!removing || deleting) return;
    setDeleting(true);
    setDeleteError(null);
    try {
      await deleteStudent(removing.id);
      setRemoving(null);
      setVersion((v) => v + 1);
    } catch (err) {
      setDeleteError(err instanceof Error ? err.message : "حذف دانش‌آموز ناموفق بود.");
    } finally {
      setDeleting(false);
    }
  };

  return (
    <>
      <PageHeader
        title="دانش‌آموزان"
        subtitle="مدیریت و مشاهده اطلاعات دانش‌آموزان"
        action={
          canAdd ? (
            <button className="primary-btn compact" onClick={() => setAdding(true)}>
              <Plus size={18} /> افزودن دانش‌آموز
            </button>
          ) : undefined
        }
      />

      <section className="panel">
        <div className="filters-row">
          <div className="search-box">
            <Search size={18} />
            <input
              placeholder="جستجوی نام، کد یا کلاس..."
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
          </div>
          <select value={cls} onChange={(e) => setCls(e.target.value)}>
            <option value="">همه کلاس‌ها</option>
            {CLASS_DEFS.map((c) => <option key={c.key} value={c.key}>{c.title}</option>)}
          </select>
          <select value={status} onChange={(e) => setStatus(e.target.value)}>
            <option value="">همه وضعیت‌ها</option>
            <option value="حاضر">حاضر</option>
            <option value="تاخیر">تاخیر</option>
            <option value="غایب">غایب</option>
          </select>
        </div>

        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th>#</th><th>نام</th><th>کد دانش‌آموز</th><th>کلاس</th><th>وضعیت</th><th>آخرین تاریخ</th>
                {canDelete && <th>عملیات</th>}
              </tr>
            </thead>
            <tbody>
              {students.loading ? (
                <TableState cols={cols} text="در حال بارگذاری..." />
              ) : students.error ? (
                <TableState cols={cols} text={students.error} error />
              ) : filtered.length === 0 ? (
                <TableState cols={cols} text="دانش‌آموزی یافت نشد" />
              ) : (
                filtered.map((s, i) => (
                  <tr key={s.id}>
                    <td>{i + 1}</td>
                    <td className="student-cell"><div className="tiny-avatar">{s.name[0]}</div>{s.name}</td>
                    <td>{s.code}</td>
                    <td>{s.className}</td>
                    <td>{s.status ? <StatusBadge status={s.status} /> : "-"}</td>
                    <td>{s.status ? todayJalali() : "-"}</td>
                    {canDelete && (
                      <td>
                        <div className="row-actions">
                          <button
                            className="mini-btn danger"
                            title="حذف دانش‌آموز"
                            onClick={() => {
                              setDeleteError(null);
                              setRemoving(s);
                            }}
                          >
                            <Trash2 size={16} />
                          </button>
                        </div>
                      </td>
                    )}
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </section>

      {adding && (
        <AddStudentModal
          existingCodes={existingCodes}
          existingNationalCodes={existingNationalCodes}
          onClose={() => setAdding(false)}
          onCreated={() => setVersion((v) => v + 1)}
        />
      )}

      {removing && (
        <ConfirmModal
          title="حذف دانش‌آموز"
          text={`دانش‌آموز «${removing.name}» با کد ${removing.code} (${removing.className}) از لیست حذف شود؟`}
          confirmLabel="حذف دانش‌آموز"
          busy={deleting}
          error={deleteError}
          onConfirm={confirmDelete}
          onClose={() => setRemoving(null)}
        />
      )}
    </>
  );
}
