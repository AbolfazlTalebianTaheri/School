import { Plus, Search } from "lucide-react";
import PageHeader from "../components/PageHeader";
import StatusBadge from "../components/StatusBadge";

const users = [
  { id: 1, name: "علی محمدی", username: "admin", role: "Admin", status: "فعال" as const },
  { id: 2, name: "مریم احمدی", username: "parent01", role: "Parent", status: "فعال" as const },
  { id: 3, name: "رضا کریمی", username: "parent02", role: "Parent", status: "غیرفعال" as const },
  { id: 4, name: "حسین مرادی", username: "parent03", role: "Parent", status: "فعال" as const }
];

export default function UsersPage() {
  return (
    <>
      <PageHeader
        title="مدیریت کاربران"
        subtitle="مدیریت حساب‌های مدیر و والدین"
        action={<button className="primary-btn compact"><Plus size={18} /> افزودن کاربر</button>}
      />

      <section className="panel">
        <div className="filters-row">
          <div className="search-box"><Search size={18} /><input placeholder="جستجوی نام یا نام کاربری..." /></div>
          <select><option>همه نقش‌ها</option><option>Admin</option><option>Parent</option></select>
          <select><option>همه وضعیت‌ها</option><option>فعال</option><option>غیرفعال</option></select>
        </div>

        <div className="table-wrap">
          <table>
            <thead><tr><th>#</th><th>نام</th><th>نام کاربری</th><th>نقش</th><th>وضعیت</th><th>عملیات</th></tr></thead>
            <tbody>
              {users.map(u => (
                <tr key={u.id}>
                  <td>{u.id}</td><td>{u.name}</td><td>{u.username}</td><td>{u.role}</td>
                  <td><StatusBadge status={u.status} /></td>
                  <td><button className="more-btn">•••</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>
    </>
  );
}
