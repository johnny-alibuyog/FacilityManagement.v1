import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
    clearScreen: false,
    plugins: [react()],
    server: {
        watch: {
            ignored: ["**/*.fs"] // Don't watch F# source files (Fable handles compilation)
        }
    }
})