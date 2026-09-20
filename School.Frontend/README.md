# School.Frontend

رابط کاربری React + TypeScript برای سامانه حضور، غیبت و تاخیر دانش‌آموزان.

## امکانات
- Login مدیر با Username/Password
- Login والدین با OTP (UI)
- داشبورد
- دانش‌آموزان
- ثبت حضور و غیاب
- تاخیرها
- غیبت‌ها
- کلاس‌ها
- گزارش‌ها
- مدیریت کاربران
- تنظیمات
- Dark / Light mode
- Responsive برای Desktop / Tablet / Mobile
- منوی RTL در سمت راست دسکتاپ
- Drawer موبایل

## اجرا

```bash
npm install
npm run dev
```

سپس آدرسی که Vite نشان می‌دهد را باز کنید.

## اتصال به API
فعلاً داده‌ها Mock هستند.
در مرحله بعد می‌توانید سرویس‌های API را به این مسیرها وصل کنید:

- POST /api/auth/admin/login
- POST /api/auth/send-otp
- POST /api/auth/verify-otp
- GET /api/students
- GET /api/attendance
- GET /api/delays
- GET /api/absences
- GET /api/classes
- GET /api/reports
- GET /api/users
