import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, ".", "");

  return {
    plugins: [react()],
    server: {
      proxy: {
        // درخواست‌های /api در حالت dev به پروژه .NET فرستاده می‌شوند (بدون مشکل CORS و گواهی https)
        "/api": {
          target: env.VITE_API_PROXY_TARGET || "https://localhost:7037",
          changeOrigin: true,
          secure: false
        }
      }
    }
  };
});
