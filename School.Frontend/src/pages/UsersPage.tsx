import { useMemo, useState } from "react";
import { Plus, Search } from "lucide-react";
import PageHeader from "../components/PageHeader";
import StatusBadge from "../components/StatusBadge";
import TableState from "../components/TableState";
import AddUserModal from "../components/AddUserModal";
import { getStudents, getUsers } from "../api/services";
import { useAsync } from "../api/useAsync";
import { roleName } from "../utils/format";

export default function UsersPage() {
  const [version, setVersion] = useState(0);
  const users = useAsync(getUsers, [version]);
  const students = useAsync(getStudents, []);

  const [query, setQuery] = useState("");
  const [role, setRole] = useState("");
  const [status, setStatus] = useState("");
  const [adding, setAdding] = useState(false);

  const rows = useMemo(
    () =>
      (users.data ?? []).map((u, i) => ({
        key: `${u.userName ?? u.nationalCode ?? "user"}-${i}`,
        name: u.userName ?? (u.nationalCode ? `والد (کد ملی ${u.nationalCode})` : "-"),
        username: u.userName ?? "-",
        role: roleName(u.userRole),
        status: "فعال" // API فقط کاربران حذف‌نشده را برمی‌گرداند
      })),
    [users.data]
  );

  const filtered = rows.filter(
    (u) =>
      (!role || u.role === role) &&
      (!status || u.status === status) &&
      (!query.trim() || `${u.name} ${u.username}`.toLowerCase().includes(query.trim().toLowerCase()))
  );

  return (
    <>
      <PageHeader
        title="مدیریت کاربران"
        subtitle="مدیریت حساب‌های مدیر، معلم و والدین"
        action={<button className="primary-btn compact" onClick={() => setAdding(true)}><Plus size={18} /> افزودن کاربر</button>}
      />

      <section className="panel">
        <div className="filters-row">
          <div className="search-box">
            <Search size={18} />
            <input
              placeholder="جستجوی نام یا نام کاربری..."
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
          </div>
          <select value={role} onChange={(e) => setRole(e.target.value)}>
            <option value="">همه نقش‌ها</option>
            <option value="Admin">Admin</option>
            <option value="Teacher">Teacher</option>
            <option value="Parent">Parent</option>
          </select>
          <select value={status} onChange={(e) => setStatus(e.target.value)}>
            <option value="">همه وضعیت‌ها</option>
            <option value="فعال">فعال</option>
            <option value="غیرفعال">غیرفعال</option>
          </select>
        </div>

        <div className="table-wrap">
          <table>
            <thead><tr><th>#</th><th>نام</th><th>نام کاربری</th><th>نقش</th><th>وضعیت</th><th>عملیات</th></tr></thead>
            <tbody>
              {users.loading ? (
                <TableState cols={6} text="در حال بارگذاری..." />
              ) : users.error ? (
                <TableState cols={6} text={users.error} error />
              ) : filtered.length === 0 ? (
                <TableState cols={6} text="کاربری یافت نشد" />
              ) : (
                filtered.map((u, i) => (
                  <tr key={u.key}>
                    <td>{i + 1}</td><td>{u.name}</td><td>{u.username}</td><td>{u.role}</td>
                    <td><StatusBadge status="فعال" /></td>
                    <td><button className="more-btn">•••</button></td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </section>

      {adding && (
        <AddUserModal
          students={students.data ?? []}
          existingUsers={users.data ?? []}
          onClose={() => setAdding(false)}
          onCreated={() => setVersion((v) => v + 1)}
        />
      )}
    </>
  );
}
