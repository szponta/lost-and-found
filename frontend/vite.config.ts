import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import path from "path";

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      src: path.resolve(__dirname, "src/"),
    },
  },
  server: {
    port: 8080,
    proxy: {
      "/api": {
        target: "http://backend:3000",
        changeOrigin: true,
      },
    },
    watch: {
      usePolling: true,
      interval: 100,
    },
    host: true,
    strictPort: true,
  },
});
