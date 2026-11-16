import { useMemo, useState } from "react";
import { Pencil, Plus, Search, Trash2 } from "lucide-react";
import PageHeader from "../components/PageHeader";
import TableState from "../components/TableState";
import LessonModal from "../components/LessonModal";
import ConfirmModal from "../components/ConfirmModal";
import { deleteLesson, getLessons, type LessonApi } from "../api/services";
import { useAsync } from "../api/useAsync";
import { CLASS_DEFS } from "../data/classes";
import { classKeyOf, studentClass } from "../utils/format";

export default function LessonsPage() {
  const [version, setVersion] = useState(0);
  const lessons = useAsync(getLessons, [version]);

  const [query, setQuery] = useState("");
  const [cls, setCls] = useState("");
  const [modal, setModal] = useState<{ lesson?: LessonApi } | null>(null);
  const [removing, setRemoving] = useState<LessonApi | null>(null);
  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState<string | null>(null);

  const all = lessons.data ?? [];

  const filtered = useMemo(
    () =>
      all
        .filter(
          (l) =>
            (!cls || classKeyOf(l) === cls) &&
            (!query.trim() || l.lessonName.includes(query.trim()))
        )
        .sort((a, b) => classKeyOf(a).localeCompare(classKeyOf(b)) || a.lessonName.localeCompare(b.lessonName, "fa")),
    [all, cls, query]
  );

  const confirmDelete = async () => {
    if (!removing || deleting) return;
    setDeleting(true);
    setDeleteError(null);
    try {
      await deleteLesson(removing.id);
      setRemoving(null);
      setVersion((v) => v + 1);
    } catch (err) {
      setDeleteError(err instanceof Error ? err.message : "حذف درس ناموفق بود.");
    } finally {
      setDeleting(false);
    }
  };

  return (
    <>
      <PageHeader
        title="مدیریت درس‌ها"
        subtitle="افزودن و ویرایش درس‌های هر کلاس (در ثبت تاخیر استفاده می‌شود)"
        action={<button className="primary-btn compact" onClick={() => setModal({})}><Plus size={18} /> افزودن درس</button>}
      />

      <section className="panel">
        <div className="filters-row">
          <div className="search-box">
            <Search size={18} />
            <input
              placeholder="جستجوی نام درس..."
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
          </div>
          <select value={cls} onChange={(e) => setCls(e.target.value)}>
            <option value="">همه کلاس‌ها</option>
            {CLASS_DEFS.map((c) => <option key={c.key} value={c.key}>{c.title}</option>)}
          </select>
        </div>

        <div className="table-wrap">
          <table>
            <thead>
              <tr><th>#</th><th>نام درس</th><th>کلاس</th><th>عملیات</th></tr>
            </thead>
            <tbody>
              {lessons.loading ? (
                <TableState cols={4} text="در حال بارگذاری..." />
              ) : lessons.error ? (
                <TableState cols={4} text={lessons.error} error />
              ) : filtered.length === 0 ? (
                <TableState cols={4} text={all.length === 0 ? "هنوز درسی ثبت نشده است. با «افزودن درس» شروع کنید." : "درسی یافت نشد"} />
              ) : (
                filtered.map((l, i) => (
                  <tr key={l.id}>
                    <td>{i + 1}</td>
                    <td>{l.lessonName}</td>
                    <td>{studentClass(l)}</td>
                    <td>
                      <div className="row-actions">
                        <button className="mini-btn" title="ویرایش" onClick={() => setModal({ lesson: l })}>
                          <Pencil size={16} />
                        </button>
                        <button
                          className="mini-btn danger"
                          title="حذف"
                          onClick={() => {
                            setDeleteError(null);
                            setRemoving(l);
                          }}
                        >
                          <Trash2 size={16} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </section>

      {modal && (
        <LessonModal
          lesson={modal.lesson}
          existing={all}
          defaultClassKey={cls || undefined}
          onClose={() => setModal(null)}
          onSaved={() => setVersion((v) => v + 1)}
        />
      )}

      {removing && (
        <ConfirmModal
          title="حذف درس"
          text={`درس «${removing.lessonName}» (${studentClass(removing)}) حذف شود؟`}
          confirmLabel="حذف درس"
          busy={deleting}
          error={deleteError}
          onConfirm={confirmDelete}
          onClose={() => setRemoving(null)}
        />
      )}
    </>
  );
}
