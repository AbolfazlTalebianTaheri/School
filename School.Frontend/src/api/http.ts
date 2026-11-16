import { clearToken, getSession, getToken, secondsUntilExpiry, setToken } from "../auth/auth";

const BASE_URL = (import.meta.env.VITE_API_BASE_URL as string | undefined) ?? "/api/v1";

export class ApiError extends Error {
  status: number;

  constructor(message: string, status: number) {
    super(message);
    this.name = "ApiError";
    this.status = status;
  }
}

type Query = Record<string, string | number | boolean | null | undefined>;

type RequestOptions = {
  method?: "GET" | "POST" | "PUT" | "DELETE";
  query?: Query;
  body?: unknown;
  /** برای login: توکن ارسال نشود و روی 401 خروج خودکار انجام نشود */
  auth?: boolean;
};

function buildUrl(path: string, query?: Query) {
  const url = `${BASE_URL.replace(/\/$/, "")}/${path.replace(/^\//, "")}`;
  if (!query) return url;

  const params = new URLSearchParams();
  Object.entries(query).forEach(([key, value]) => {
    if (value !== undefined && value !== null && value !== "") params.append(key, String(value));
  });
  const qs = params.toString();
  return qs ? `${url}?${qs}` : url;
}

function firstString(value: unknown): string | null {
  if (typeof value === "string") return value;
  if (Array.isArray(value)) return firstString(value[0]);
  if (value && typeof value === "object") return firstString(Object.values(value)[0]);
  return null;
}

/** پیام خطا را از فرمت‌های مختلف بک‌اند (ApiResult، ProblemDetails، خطای اعتبارسنجی) بیرون می‌کشد. */
function extractMessage(body: unknown, status: number): string {
  if (status >= 500) return "خطایی در سرور رخ داد. دوباره تلاش کنید.";

  if (body && typeof body === "object") {
    const b = body as Record<string, unknown>;
    const err = b.error as Record<string, unknown> | string | null | undefined;

    const candidates: unknown[] = [
      b.errors,
      err && typeof err === "object" ? err.errors : null,
      err && typeof err === "object" ? err.message : null,
      err && typeof err === "object" ? err.detail : null,
      typeof err === "string" ? err : null,
      b.detail,
      b.message,
      b.title
    ];

    for (const c of candidates) {
      const msg = firstString(c);
      if (msg) return msg;
    }
  }

  if (status === 401) return "نام کاربری یا رمز عبور نادرست است.";
  if (status === 403) return "شما به این بخش دسترسی ندارید.";
  if (status === 404) return "مورد درخواستی پیدا نشد.";
  if (status === 409) return "این مورد قبلاً ثبت شده است.";
  return "درخواست ناموفق بود.";
}

/** اگر پاسخ در قالب ApiResult بود، فقط data را برمی‌گرداند؛ در غیر این صورت خود پاسخ را. */
function unwrap<T>(body: unknown): T {
  if (body && typeof body === "object" && "success" in body && "status" in body) {
    return (body as { data?: T }).data as T;
  }
  return body as T;
}

let refreshing: Promise<string | null> | null = null;

/** با کوکی رفرش‌توکن، access token تازه می‌گیرد. هم‌زمان فقط یک درخواست رفرش می‌رود. */
export function refreshAccessToken(): Promise<string | null> {
  if (!refreshing) {
    refreshing = (async () => {
      try {
        const response = await fetch(buildUrl("auth/refresh"), {
          method: "POST",
          credentials: "include",
          headers: { Accept: "application/json" }
        });
        if (!response.ok) return null;

        const data = unwrap<{ accessToken?: string }>(await response.json());
        if (!data?.accessToken) return null;

        setToken(data.accessToken);
        return data.accessToken;
      } catch {
        return null;
      }
    })().finally(() => {
      refreshing = null;
    });
  }
  return refreshing;
}

/** اگر توکن معتبر نیست ولی کوکی رفرش هست، بی‌صدا وارد می‌شود. */
export async function ensureSession(): Promise<boolean> {
  if (getSession()) return true;
  return (await refreshAccessToken()) !== null;
}

/** خروج: رفرش‌توکن در سرور باطل و کوکی پاک می‌شود. */
export async function signOut(): Promise<void> {
  try {
    await fetch(buildUrl("auth/logout"), { method: "POST", credentials: "include" });
  } catch {
    // حتی اگر سرور در دسترس نبود، نشست محلی پاک می‌شود
  }
  clearToken();
}

export async function request<T>(path: string, options: RequestOptions = {}): Promise<T> {
  const { method = "GET", query, body, auth = true } = options;

  if (auth && getToken() && secondsUntilExpiry() < 20) await refreshAccessToken();

  const send = async (): Promise<Response> => {
    const headers: Record<string, string> = { Accept: "application/json" };
    const token = auth ? getToken() : null;
    if (token) headers.Authorization = `Bearer ${token}`;
    if (body !== undefined) headers["Content-Type"] = "application/json";

    try {
      return await fetch(buildUrl(path, query), {
        method,
        headers,
        credentials: "include",
        body: body !== undefined ? JSON.stringify(body) : undefined
      });
    } catch {
      throw new ApiError("ارتباط با سرور برقرار نشد. مطمئن شوید API در حال اجراست.", 0);
    }
  };

  let response = await send();
  const hadToken = auth && !!getToken();

  // توکن منقضی شده؟ یک بار با رفرش‌توکن تمدید و دوباره تلاش می‌کنیم
  if (response.status === 401 && hadToken && (await refreshAccessToken())) {
    response = await send();
  }

  const text = await response.text();
  let parsed: unknown = null;
  if (text) {
    try {
      parsed = JSON.parse(text);
    } catch {
      parsed = text;
    }
  }

  if (!response.ok) {
    if (response.status === 401 && hadToken) {
      clearToken();
      window.location.assign("/login");
    }
    throw new ApiError(extractMessage(parsed, response.status), response.status);
  }

  return unwrap<T>(parsed);
}
