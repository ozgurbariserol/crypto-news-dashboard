import { fileURLToPath, URL } from 'node:url'

import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')
  return {
    plugins: [vue()],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url))
      }
    },
    // The source reads the API address as process.env['CONNECTIONSTRINGS__BACKEND']
    // (the name the original orchestration tool injected). Only that single value is
    // exposed to the bundle; the whole build environment is NOT embedded.
    define: {
      'process.env': {
        CONNECTIONSTRINGS__BACKEND:
          env.VITE_BACKEND_URL || env.CONNECTIONSTRINGS__BACKEND || 'http://localhost:5000'
      }
    }
  }
})
