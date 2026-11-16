import { request } from "./http";
import { getSession } from "../auth/auth";
import { todayIso } from "../utils/format";
import { monthDays } from "../utils/jalali";

// ---------- Types (مطابق Contractهای بک‌اند؛ نام‌ها camelCase هستند) ----------

export type LoginPayload = {
  userRole: number; // 0 = Parent, 1 = Admin, 2 = Teacher
  userName?: string;
  password?: string;
  /** کد ملی دانش‌آموز (ورود والدین) */
  nationalCode?: string;
  /** مرا به خاطر بسپار: کوکی رفرش‌توکن ماندگار */
  rememberMe?: boolean;
};

export type LoginResult = { userId: number; accessToken: string };

export type StudentApi = {
  id: number;
  nationalCode?: string;
  firstName: string;
  lastName: string;
  studentCode: number;
  grade_Level: number;
  field_Of_Study: number;
};

export type LessonApi = {
  id: number;
  lessonName: string;
  grade_Level: number;
  field_Of_Study: number;
};

export type NewStudent = {
  firstName: string;
  lastName: string;
  studentCode: number;
  nationalCode: string;
  grade_Level: number;
  field_Of_Study: number;
};

export type UserApi = {
  userName: string | null;
  nationalCode: string | null;
  userRole: number;
};

export type NewUser = {
  userRole: number;
  userName?: string;
  password?: string;
  /** کد ملی دانش‌آموزِ وصل‌شده (فقط برای والدین) */
  nationalCode?: string;
  studentId?: number;
};

export type LessonInput = {
  lessonName: string;
  grade_Level: number;
  field_Of_Study: number;
};

export type AttendanceLogApi = {
  studentId: number;
  student?: StudentApi | null;
  attendanceDayId: number;
  isAbsent: boolean;
};

export type AttendanceDelayApi = {
  attendanceLogId: number;
  attendanceLog?: AttendanceLogApi | null;
  class_Bell: number;
  lessonId: number;
  lesson?: { lessonName: string } | null;
  delayMinutes: number;
};

export const ROLE_ADMIN = 1;
export const ROLE_PARENT = 0;
export const ROLE_TEACHER = 2;

/** والدین فقط داده‌ی دانش‌آموزِ وصل‌شده به حساب خودشان را می‌بینند (شناسه از توکن می‌آید) */
function onlyOwnStudent<T>(items: T[], studentIdOf: (item: T) => number | undefined): T[] {
  const session = getSession();
  if (session?.role !== "Parent") return items;
  const own = new Set(session.studentIds);
  return items.filter((item) => {
    const id = studentIdOf(item);
    return id !== undefined && own.has(id);
  });
}

// ---------- Auth ----------

export const login = (payload: LoginPayload) =>
  request<LoginResult>("auth/Login", { method: "POST", body: payload, auth: false });

// ---------- Students / Users ----------

export const getStudents = () =>
  request<StudentApi[]>("students").then((r) => onlyOwnStudent(r ?? [], (s) => s.id));
export const deleteStudent = (id: number) => request<void>(`students/${id}`, { method: "DELETE" });
export const createStudent = (student: NewStudent) =>
  request<void>("students", { method: "POST", body: student });

export const getLessons = () => request<LessonApi[]>("lessons").then((r) => r ?? []);
export const createLesson = (lesson: LessonInput) =>
  request<void>("lessons", { method: "POST", body: lesson });
export const updateLesson = (id: number, lesson: LessonInput) =>
  request<void>(`lessons/${id}`, { method: "PUT", body: { id, ...lesson } });
export const deleteLesson = (id: number) => request<void>(`lessons/${id}`, { method: "DELETE" });

export const createUser = (user: NewUser) => request<void>("users", { method: "POST", body: user });
export const getUsers = () => request<UserApi[]>("users").then((r) => r ?? []);

// ---------- Attendance (date = yyyy-MM-dd) ----------

export const getAbsents = (date: string) =>
  request<AttendanceLogApi[]>(`attendances/${date}/absents`).then((r) => onlyOwnStudent(r ?? [], (l) => l.studentId));

export const getDelays = (date: string) =>
  request<AttendanceDelayApi[]>(`attendances/date/${date}/delay`).then((r) =>
    onlyOwnStudent(r ?? [], (d) => d.attendanceLog?.studentId)
  );

export type AttendanceItem = {
  studentId: number;
  isAbsent: boolean;
  delay?: { classBell: number; lessonId: number; delayMinutes: number };
};

export const saveAttendance = (date: string, items: AttendanceItem[]) =>
  request<void>(`attendances/${date}/save`, { method: "POST", body: { items } });

export const getDay = (date: string) =>
  request<{ id: number; date: string } | null>(`attendances/date/${date}/day`);

// ---------- وضعیت امروز ----------

export type TodayStatus = "حاضر" | "تاخیر" | "غایب";

export type SavedDelay = { bell: number; lessonId: number; minutes: number };

export type TodayAttendance = {
  /** آیا برای امروز حضور و غیاب ثبت شده است؟ */
  registered: boolean;
  absent: Set<number>;
  delayed: Set<number>;
  /** جزئیات تاخیر ثبت‌شده‌ی هر دانش‌آموز (کلید: کد دانش‌آموزی) */
  delays: Map<number, SavedDelay>;
};

export const getTodayAttendance = () => getDayAttendance(todayIso());

/** وضعیت حضور و غیاب یک تاریخ دلخواه (yyyy-MM-dd) */
export async function getDayAttendance(date: string): Promise<TodayAttendance> {
  const [day, absents, delays] = await Promise.all([getDay(date), getAbsents(date), getDelays(date)]);

  const codes = (items: (number | undefined)[]) =>
    new Set(items.filter((c): c is number => typeof c === "number"));

  const delayInfo = new Map<number, SavedDelay>();
  delays.forEach((d) => {
    const code = d.attendanceLog?.student?.studentCode;
    if (typeof code === "number" && !delayInfo.has(code)) {
      delayInfo.set(code, { bell: d.class_Bell, lessonId: d.lessonId, minutes: d.delayMinutes });
    }
  });

  return {
    registered: !!day,
    absent: codes(absents.map((l) => l.student?.studentCode)),
    delayed: new Set(delayInfo.keys()),
    delays: delayInfo
  };
}

/** اگر امروز روزی ثبت نشده باشد وضعیتی وجود ندارد (null). */
export function studentStatus(studentCode: number, today: TodayAttendance | null): TodayStatus | null {
  if (!today || !today.registered) return null;
  if (today.absent.has(studentCode)) return "غایب";
  if (today.delayed.has(studentCode)) return "تاخیر";
  return "حاضر";
}

// ---------- گزارش ماهانه (روزبه‌روز از API می‌خواند) ----------

const daysUpToToday = (y: number, m: number) => monthDays(y, m).filter((d) => d <= todayIso());

export type DatedAbsent = { date: string; log: AttendanceLogApi };
export type DatedDelay = { date: string; delay: AttendanceDelayApi };

export async function getMonthAbsents(y: number, m: number): Promise<DatedAbsent[]> {
  const days = daysUpToToday(y, m);
  const lists = await Promise.all(days.map(async (date) => ({ date, items: await getAbsents(date) })));
  return lists.flatMap(({ date, items }) => items.map((log) => ({ date, log })));
}

export async function getMonthDelays(y: number, m: number): Promise<DatedDelay[]> {
  const days = daysUpToToday(y, m);
  const lists = await Promise.all(days.map(async (date) => ({ date, items: await getDelays(date) })));
  return lists.flatMap(({ date, items }) => items.map((delay) => ({ date, delay })));
}

export async function getMonthReport(y: number, m: number) {
  const days = daysUpToToday(y, m);
  const perDay = await Promise.all(
    days.map(async (date) => {
      const [day, absents, delays] = await Promise.all([getDay(date), getAbsents(date), getDelays(date)]);
      return { date, registered: !!day, absents, delays };
    })
  );

  return {
    registeredDays: perDay.filter((d) => d.registered).map((d) => d.date),
    absents: perDay.flatMap((d) => d.absents.map((log) => ({ date: d.date, log }))),
    delays: perDay.flatMap((d) => d.delays.map((delay) => ({ date: d.date, delay })))
  };
}
